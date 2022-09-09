using WeatherDevicesApi.Model;

namespace WeatherDevicesApi.Services
{
    public interface IMetadataServices
    {
        public Metadata GetMetadata();
        public IEnumerable<string> GetMetadata(string deviceId);
    }
}
