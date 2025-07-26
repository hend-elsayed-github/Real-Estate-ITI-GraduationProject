using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Real_Estatae_Project.Services;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IImportFile _importFile;

        public FileController(IImportFile importFile)
        {
            _importFile = importFile;
        }
        [HttpPost("ImportKnowledgeBase")]
        public async Task<IActionResult> ImportKnowledgeBase()
        {
            try
            {
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Services", "qa.txt");
                await _importFile.ImportKnowledgeBaseAsync(filePath);
                return Ok("Knowledge base imported successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
