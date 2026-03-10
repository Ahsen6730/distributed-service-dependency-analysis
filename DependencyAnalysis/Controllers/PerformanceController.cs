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

        // SENARYO 1: Bağımsız (Baseline)
        [HttpGet("independent")]
        public IActionResult GetIndependent() => Ok("Baseline Success");

        // SENARYO 2: Tekil Bağımlılık (Level 1) - Servisi Kullanıyor!
        [HttpGet("level1")]
        public async Task<IActionResult> GetLevel1()
        {
            var data = await _externalService.GetAndProcessDataAsync();
            return Ok(new { Dependency = "Level 1", ProcessedDataCount = data.Count });
        }

        // SENARYO 3: Hibrit - Ödeme Simülasyonu (CPU + I/O)
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

        [HttpGet("resilience-check")]
        public async Task<IActionResult> GetResilienceTest()
        {
            var client = _clientFactory.CreateClient("ExternalApi");

            var response = await client.GetAsync("api/ExternalData/unstable-data");

            return Ok(new
            {
                IsSuccess = response.IsSuccessStatusCode,
                StatusCode = response.StatusCode
            });
        }

        private string ComputeHash(string rawData)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return Convert.ToHexString(bytes);
        }
    }
}