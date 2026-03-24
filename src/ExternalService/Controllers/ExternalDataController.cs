using Microsoft.AspNetCore.Mvc;

namespace ExternalService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalDataController : ControllerBase
    {
        [HttpGet("news")]
        public IActionResult GetNews() => Ok(new { Title = "Haber Verisi", Status = "Success" });

        [HttpGet("download-image")]
        public IActionResult DownloadImage()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "thumb.jpg");
            if (!System.IO.File.Exists(path)) return NotFound("thumb.jpg dosyasını wwwroot içine koymalısın!");
            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "image/jpeg", "thumb.jpg");
        }

        [HttpGet("unstable-data")]
        public async Task<IActionResult> GetUnstableData()
        {
            var random = new Random();
            if (random.Next(1, 10) <= 3)
            {
                await Task.Delay(5000);
            }
            return Ok(new { Data = "Unstable Service Response", Status = "Late but Success" });
        }

        [HttpGet("data-stream")]
        public IActionResult GetDataStream()
        {
            var mockData = new
            {
                RawData = Enumerable.Range(1, 100).Select(_ => Random.Shared.Next(1, 1000)).ToList()
            };

            return Ok(mockData);
        }

        // TC-08: Sürekli Hata (500 Error)
        [HttpGet("error-burst")]
        public IActionResult GetError()
        {
            return StatusCode(500, "Simulated Internal Server Error");
        }

        // TC-06: Veri Boyutu Yükü (Large Payload)
        [HttpGet("large-payload")]
        public IActionResult GetLargePayload()
        {
            var data = new string('A', 10 * 1024 * 1024);
            return Ok(new { content = data });
        }
    }
}