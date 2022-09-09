using WeatherDevicesApi.Model;

namespace WeatherDevicesApi.Services
{
    public class MetadataServices : IMetadataServices
    {
        private readonly ILogger<MetadataServices> _logger;

        public MetadataServices(ILogger<MetadataServices> logger)
        {
            _logger = logger;
        }

        public IEnumerable<string> GetMetadata(string deviceId)
        {
            var metadata = ParseMetadataFile();
            if (!metadata.TryGetValue(deviceId, out var attachedSensors))
                return Enumerable.Empty<string>();

            return attachedSensors;
        }

        public Metadata GetMetadata()
        {
            return ParseMetadataFile();
        }

        private Metadata ParseMetadataFile()
        {
            string filename = $"Data/metadata.csv";
            if (!File.Exists(filename))
                throw new FileNotFoundException(filename);

            using var reader = new StreamReader(filename);
            var metadata = new Metadata();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line is not null)
                {
                    var splitted = line.Split(';');
                    var deviceId = splitted[0];
                    var sensorType = splitted[1];
                    
                    metadata.AddOrUpdate(deviceId, sensorType);
                }

            }
            return metadata;
        }

    }
}
