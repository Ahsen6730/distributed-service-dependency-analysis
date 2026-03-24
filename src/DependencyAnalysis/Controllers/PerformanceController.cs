using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using DependencyAnalysis.Services;
namespace DependencyAnalysis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PerformanceController : ControllerBase
    {
        private readonly IExternalServiceClient _externalService;

        public PerformanceController(IExternalServiceClient externalService)
            => _externalService = externalService;

        //TC-01 SENARYO 1: Bağımsız (Baseline)
        [HttpGet("independent")]
        public IActionResult GetIndependent() => Ok("Baseline Success");

        // TC-02 SENARYO 2: Tekil Bağımlılık (Level 1)
        [HttpGet("level1")]
        public async Task<IActionResult> GetLevel1()
        {
            var data = await _externalService.GetAndProcessDataAsync();
            return Ok(new { Dependency = "Level 1", ProcessedDataCount = data.Count });
        }

        //TC-03 SENARYO 3: Hibrit - Ödeme Simülasyonu (CPU + I/O) 
        [HttpGet("secure-checkout")]
        public async Task<IActionResult> GetSecureCheckout()
        {
            var processedData = await _externalService.GetAndProcessDataAsync();

            string dataToHash = "Order_Ahsen_" + DateTime.Now.Ticks;
            for (int i = 0; i < 5000; i++)
            {
                dataToHash = ComputeHash(dataToHash);
            }

            return Ok(new
            {
                Status = "Payment Securely Processed",
                Security = "SHA256",
                ProcessedItems = processedData.Count
            });
        }

        //TC-04
        [HttpGet("resilience-check")]
        public async Task<IActionResult> GetResilienceTest()
        {
            var response = await _externalService.CheckResilienceAsync(); 
            return Ok(new { IsSuccess = response.IsSuccessStatusCode, StatusCode = response.StatusCode });
        }

        private string ComputeHash(string rawData)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return Convert.ToHexString(bytes);
        }

        // TC-05: Service Down / Connection Refused Senaryosu
        [HttpGet("connection-failure")]
        public async Task<IActionResult> GetConnectionFailure()
        {
            try
            {
                await _externalService.CheckConnectionFailureAsync();
                return Ok("Service is running normally.");
            }
            catch (Exception ex)
            {
                return StatusCode(503, new
                {
                    Scenario = "TC-05",
                    Error = "External Service Unreachable",
                    Detail = ex.Message
                });
            }
        }

        // TC-06:Large Payload
        [HttpGet("payload-test")]
        public async Task<IActionResult> GetPayloadTest()
        {
            var data = await _externalService.GetLargePayloadAsync();
            return Ok(new { Message = "Payload Received", DataLength = data.Length });
        }

        // TC-07: Connection Pool Exhaustion
        // Not: Bu testi Program.cs'de  kısıtlı client ile ölçeceğiz
        [HttpGet("pool-test")]
        public async Task<IActionResult> GetPoolTest()
        {
            var data = await _externalService.GetAndProcessDataAsync();
            return Ok(new { Info = "Pool Test Execution", Count = data.Count });
        }

        // TC-08:(Error Burst)
        [HttpGet("error-burst-test")]
        public async Task<IActionResult> GetErrorBurstTest()
        {
            var response = await _externalService.GetErrorBurstAsync();
            return StatusCode((int)response.StatusCode, "External Service Error Handled Fast");
        }
    }
}