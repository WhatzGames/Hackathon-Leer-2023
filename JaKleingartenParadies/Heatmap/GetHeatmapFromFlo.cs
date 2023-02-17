using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

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
    public async Task<List<List<decimal>>> GetHeatmap(List<List<string>> grid, IEnumerable<int> ships)
    {
        
        //todo: anzahl der schiffe mitschicken
        string route = "https://probability-calculator-njgyfl6nqq-ey.a.run.app/";

        //response = probability map
        //10 * 10 f list
        var response = await _client.PostAsJsonAsync(route, new
        {
            Board = grid,
            RemainingShips = ships
        }, new JsonSerializerOptions(JsonSerializerDefaults.Web));

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return null;
        }

        var responseObject = await response.Content.ReadFromJsonAsync<List<List<decimal>>>();

        return responseObject;
    }
}