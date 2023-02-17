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
    public async Task GetHeatmap(List<List<string>> grid)
    {
        string route = "";
        var gridJson = JsonSerializer.Serialize(grid);

        //response = probability map
        //10 * 10 f list
        var response = await _client.PostAsJsonAsync(route, gridJson);
    }
}