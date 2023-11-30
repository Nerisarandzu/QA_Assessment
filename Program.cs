
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        Dictionary<string, object> dogBreeds = await GetDogBreeds();

        var jsonResult = await GetDogBreeds(dogBreeds);
        Console.WriteLine(jsonResult);

        VerifyRetrieverIsInList(dogBreeds, jsonResult);

        var randomImageLink = await GetRandomSubBreedImage("golden");
        Console.WriteLine($"Random image for the following dog sub breed: {randomImageLink}");
    }

    private static void VerifyRetrieverIsInList(Dictionary<string, object> dogBreeds, string jsonResult)
    {
        if (jsonResult.Contains("message"))
        {
            JObject data = JObject.Parse(jsonResult);
            if (data["message"] != null && data["message"]["retriever"] != null)
            {
                Console.WriteLine("Retriever is within list \n");

                List<string> retrieverBreeds = data["message"]["retriever"].ToObject<List<string>>();

                Console.WriteLine("Retriever Breeds:");
                foreach (string breed in retrieverBreeds)
                {
                    Console.WriteLine($"breed: {breed}");
                }
            }
        }
        Console.WriteLine("\n==================================================================================================================================================== \n");
    }

    private static async Task<string> GetDogBreeds(Dictionary<string, object> dogBreeds)
    {

        return JsonConvert.SerializeObject(dogBreeds, Formatting.Indented);
    }

    private static async Task<Dictionary<string, object>> GetDogBreeds()
    {
        string endpoint = "https://dog.ceo/api/breeds/list/all";

        string responseData = await GetHttpResponse(endpoint);

        return JsonConvert.DeserializeObject<Dictionary<string, object>>(responseData.ToString());
    }

    private static async Task<string> GetRandomSubBreedImage(string subBreed)
    {
        string endpoint = $"https://dog.ceo/api/breed/retriever/{subBreed}/images/random";
        return await GetHttpResponse(endpoint);
    }

    private static async Task<string> GetHttpResponse(string endpoint)
    {
        using HttpClient client = new HttpClient();

        HttpResponseMessage response = await client.GetAsync(endpoint);
        string responseData = string.Empty;

        if (response.IsSuccessStatusCode)
        {
            responseData = await response.Content.ReadAsStringAsync();
            return responseData;
        }

        return responseData;
    }
}