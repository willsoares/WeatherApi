using WeatherDevicesApi.Model;

namespace WeatherDevicesApi.Services
{
    public interface IMeasurementServices
    {
        public IDictionary<string, IEnumerable<Measurement>> GetDevice(string deviceId, string date);
        public IEnumerable<Measurement> GetSpecificSensor(string deviceId, string date, string sensorType);
    }
}
