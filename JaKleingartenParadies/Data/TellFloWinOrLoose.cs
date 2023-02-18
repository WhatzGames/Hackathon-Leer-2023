using System.Net.Http.Json;
using JaKleingartenParadies.Dto;

namespace JaKleingartenParadies.Data;

public class TellFloWinOrLoose
{
    private readonly TellFloWinOrLooseDto _tellFloWinOrLooseDto;
    private readonly string ConnectionString;
    private readonly HttpClient _client;
    
    public TellFloWinOrLoose()
    {
        _tellFloWinOrLooseDto = new TellFloWinOrLooseDto();
        ConnectionString = "https://schiffeversenkenbot-njgyfl6nqq-ey.a.run.app";
        _client = new HttpClient();
    }

    public async Task Send(int wins, int losses)
    {
        string winrateString;
        winrateString = wins == 0 
            ? "can not calculate winrate: wins = 0" 
            : ((double) wins / (wins + losses)).ToString("P");
        
        _tellFloWinOrLooseDto.message = $"wins: {wins}\n\r"
                                      + $"losses: {losses}\n\r"
                                      + $"winrate: {winrateString}";
        await _client.PostAsJsonAsync(ConnectionString, _tellFloWinOrLooseDto);
    }
}