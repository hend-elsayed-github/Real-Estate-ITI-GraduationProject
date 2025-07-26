using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Real_Estatae_Project.Services;
using Real_Estate_Project.Models;

namespace Real_Estatae_Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {

        private readonly ProjectContext _dbContext;
        private readonly IOpenAIService _openAIService;

        private float CosineSimilarity(float[] a, float[] b)
        {
            float dot = 0, magA = 0, magB = 0;
            for (int i = 0; i < a.Length; i++)
            {
                dot += a[i] * b[i];
                magA += a[i] * a[i];
                magB += b[i] * b[i];
            }
            return dot / (float)(Math.Sqrt(magA) * Math.Sqrt(magB));
        }
        public ChatController(ProjectContext dbContext, IOpenAIService openAIService)
        {
            _dbContext = dbContext;
            _openAIService = openAIService;
        }




        [HttpPost("Chat")]
        public async Task<IActionResult> Chat([FromBody] string userQuestion)
        {
            var userEmbedding = await _openAIService.GenerateEmbeddingAsync(userQuestion);
            var entries = await _dbContext.KnowledgeEntries.ToListAsync();
            var ranked = entries.Select(e => new
            {
                e.Question,
                e.Answer,
                Score = CosineSimilarity(userEmbedding, e.Embedding)
            })
                                .OrderByDescending(r => r.Score)
                                .FirstOrDefault();

            return Ok(new
            {
                answer = ranked?.Answer ?? "Sorry, I couldn’t find an answer."
            });
        }


       
    }
}
