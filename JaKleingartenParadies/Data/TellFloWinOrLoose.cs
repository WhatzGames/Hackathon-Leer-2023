using System.Net.Http.Json;
using JaKleingartenParadies.Dto;

namespace JaKleingartenParadies.Data;

public class TellFloWinOrLoose
{
    private readonly TellFloWinOrLooseDto _tellFloWinOrLooseDto;
    private readonly string ConnectionString;
    private HttpClient _client;
    
    public TellFloWinOrLoose()
    {
        _tellFloWinOrLooseDto = new TellFloWinOrLooseDto();
        ConnectionString = "https://schiffeversenkenbot-njgyfl6nqq-ey.a.run.app";
        _client = new HttpClient();
    }

    public async Task SentFloWin()
    {
        _tellFloWinOrLooseDto.status = "success";
        _tellFloWinOrLooseDto.message = "kurt rocks!";
        _tellFloWinOrLooseDto.title = "win";

        await _client.PostAsJsonAsync(ConnectionString, _tellFloWinOrLooseDto);
    }

    public async Task SentFloLoose()
    {
        _tellFloWinOrLooseDto.status = "failure";
        _tellFloWinOrLooseDto.message = "flo rocks not!";
        _tellFloWinOrLooseDto.title = "loss";
        
        await _client.PostAsJsonAsync(ConnectionString, _tellFloWinOrLooseDto);
    }
}