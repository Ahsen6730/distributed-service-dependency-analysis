
namespace DependencyAnalysis.Services
{
    public class ExternalDataResponse
    {
        public List<int> RawData { get; set; }
    }

    public interface IExternalServiceClient
    {
        Task<List<int>> GetAndProcessDataAsync();
        Task<HttpResponseMessage> CheckResilienceAsync();
    }
    public class ExternalServiceClient : IExternalServiceClient
    {
        private readonly HttpClient _httpClient;
        public ExternalServiceClient(HttpClient httpClient) => _httpClient = httpClient;
        public async Task<HttpResponseMessage> CheckResilienceAsync()
        {
            return await _httpClient.GetAsync("api/ExternalData/unstable-data");
        }
        public async Task<List<int>> GetAndProcessDataAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<ExternalDataResponse>("api/ExternalData/data-stream");

            if (response?.RawData != null)
            {
                response.RawData.Sort();
                return response.RawData;
            }

            return new List<int>();
        }
    }
}