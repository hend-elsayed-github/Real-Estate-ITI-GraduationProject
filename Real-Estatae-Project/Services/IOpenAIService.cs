namespace Real_Estatae_Project.Services
{
    public interface IOpenAIService
    {
        Task<float[]> GenerateEmbeddingAsync(string input);
    }
}
