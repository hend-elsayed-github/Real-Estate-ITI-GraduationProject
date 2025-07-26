using Newtonsoft.Json;
using Real_Estatae_Project.DTO.ChatBot;
using System.Net.Http.Headers;
using System.Text;

namespace Real_Estatae_Project.Services
{
    public class OpenAIService: IOpenAIService
    {
        private readonly string _apiKey;

        public OpenAIService(IConfiguration config)
        {
            _apiKey = config["OpenAI:ApiKey"];
        }

        public async Task<float[]> GenerateEmbeddingAsync(string input)
        {
            using var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var body = new
            {
                input = input,
                model = "text-embedding-ada-002"
            };

            var response = await http.PostAsync("https://api.openai.com/v1/embeddings",
                new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new Exception("OpenAI request failed");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<EmbeddingResponse>(json);
            return result.data[0].embedding;

        }
    }
}
