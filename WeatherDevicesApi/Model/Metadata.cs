namespace WeatherDevicesApi.Model
{
    public class Metadata : Dictionary<string, List<string>>
    {
        
    }
    public static class MetadataExtensions
    {
        public static void AddOrUpdate(this Metadata metadata, string deviceId, string sensorType)
        {
            if (!metadata.TryGetValue(deviceId, out var sensorList))
                metadata.Add(deviceId, new List<string>() { sensorType });
            else
            {
                if(!sensorList.Where(s => s == sensorType).Any())
                    sensorList.Add(sensorType);
                metadata[deviceId] = sensorList;
            }
        }
    }
}
