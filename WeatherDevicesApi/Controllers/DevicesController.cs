using Microsoft.AspNetCore.Mvc;
using WeatherDevicesApi.Model;
using WeatherDevicesApi.Services;

namespace WeatherDevicesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DevicesController : ControllerBase
    {
        private readonly ILogger<DevicesController> _logger;
        private readonly IMeasurementServices _measurementService;

        public DevicesController(ILogger<DevicesController> logger, IMeasurementServices measurementService)
        {
            _logger = logger;
            _measurementService = measurementService;
        }

        [HttpGet("{deviceId}/data/{date}/{sensorType}")]
        public ActionResult<IEnumerable<Measurement>> GetSpecific(string deviceId, string date, string sensorType)
        {
            var sensorData = _measurementService.GetSpecificSensor(deviceId, date, sensorType);
            
            if (sensorData is null)
                return NotFound();
            
            if (!sensorData.Any())
                return NoContent();

            return Ok(sensorData);
        }

        [HttpGet("{deviceId}/data/{date}")]
        public ActionResult<IDictionary<string, IEnumerable<Measurement>>> GetDeviceData(string deviceId, string date)
        {
            var deviceData = _measurementService.GetDevice(deviceId, date);
            
            if (deviceData is null)
                return NotFound();

            if (!deviceData.Any())
                return NoContent();

            return Ok(deviceData);
        }
    }
}