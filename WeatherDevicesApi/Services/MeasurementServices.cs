using System.Globalization;
using WeatherDevicesApi.Model;

namespace WeatherDevicesApi.Services
{
    public class MeasurementServices : IMeasurementServices
    {
        private readonly IMetadataServices _metadata;
        private readonly ILogger<MeasurementServices> _logger;

        public MeasurementServices(IMetadataServices metadata, ILogger<MeasurementServices> logger)
        {
            _metadata = metadata;
            _logger = logger;
        }

        public IDictionary<string, IEnumerable<Measurement>> GetDevice(string deviceId, string date)
        {
            var deviceSensorList = _metadata.GetMetadata(deviceId);
            if (!deviceSensorList.Any())
            {
                _logger.LogInformation("Measurement Data Not Found: File {deviceId} - {date}", deviceId, date);
                return null;
            }

            var deviceData = new Dictionary<string, IEnumerable<Measurement>>();
            foreach (var sensor in deviceSensorList)
            {
                var measurement = GetSpecificSensor(deviceId, date, sensor);
                if (measurement != null)
                    deviceData.Add(sensor, measurement);
            }

            return deviceData;
        }

        public IEnumerable<Measurement> GetSpecificSensor(string deviceId, string date, string sensorType)
        {
            string filename = $"Data/{deviceId}/{sensorType}/{date}.csv";
            if (!File.Exists(filename))
            {
                _logger.LogInformation("Not Found: File {filename}", filename);
                return null;
            }
            using var reader = new StreamReader(filename);
            var result = new List<Measurement>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line != null)
                    result.Add(ParseSingleMeasurement(line));
            }
            return result;
        }

        private Measurement ParseSingleMeasurement(string line)
        {
            var measurementValues = line.Split(';');
            var date = measurementValues[0];
            var value = measurementValues[1];

            var measurement = new Measurement()
            {
                Date = DateTime.Parse(date),
                Value = Convert.ToDouble(value, new CultureInfo("pt-BR"))
            };
            return measurement;
        }
    }
}
