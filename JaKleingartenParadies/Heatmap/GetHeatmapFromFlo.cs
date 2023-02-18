using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using JaKleingartenParadies.Dto;

namespace JaKleingartenParadies.Heatmap;

//schicke: das grid so wie ist gerade ist, verbleibende schiffe deren schiffslänge
public class GetHeatmapFromFlo
{
    private readonly HttpClient _client;
    
    public GetHeatmapFromFlo()
    {
        _client = new HttpClient();
    }
    
    //von flo kommt eine list<list<float>>mit der heatmap
    //post request
    //json format
    public async Task<List<List<double>>> GetHeatmap(string spielerId, List<List<string>> grid, IEnumerable<int> ships)
    {
        
        //todo: anzahl der schiffe mitschicken
        string route = "https://probability-calculator-njgyfl6nqq-ey.a.run.app/";

        //response = probability map
        //10 * 10 f list
        var value = JsonSerializer.Serialize(new
        {
            Board = grid,
            RemainingShips = ships
        }, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        //File.AppendAllText($"Requests_{spielerId}.json",value);
        var response = await _client.PostAsync(route, new StringContent(value, Encoding.UTF8, MediaTypeNames.Application.Json));

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return null;
        }
        
        var responseObject = await response.Content.ReadFromJsonAsync<FlosResponseDto>();

        return responseObject.probabilities;
    }
}