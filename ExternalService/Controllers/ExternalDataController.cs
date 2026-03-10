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
    }
}