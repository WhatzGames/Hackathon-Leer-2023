// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using JaKleingartenParadies.Data;
using JaKleingartenParadies.Dto;
using JaKleingartenParadies.Game;
using SocketIOClient;
using SocketIOClient.Transport;

Dictionary<string, GameRunner> games = new Dictionary<string, GameRunner>();

const string Secret = "2d376eb7-ead4-4b7c-99c0-3a21515e8cd5";
bool gotDisconnected = false;

var client = new SocketIO("https://games.uhno.de", new SocketIOOptions() {Transport = TransportProtocol.WebSocket});

client.On("connect",
    async (e) =>
    {
        Console.WriteLine("trying to Authenticate");
        await client.EmitAsync("authenticate",
            (success) => { Console.WriteLine($"Authentication successful: {success}"); }, Secret);
    });

client.On("disconnect", (e) =>
{
    Console.WriteLine("Disconnected");
    gotDisconnected = true;
});

client.OnConnected += (sender, e) =>
{
    Console.WriteLine("Connected");
};

await client.ConnectAsync();
Console.WriteLine("trying to Authenticate");
await client.EmitAsync("authenticate",
    (success) => { Console.WriteLine($"Authentication successful: {success}"); }, Secret);

client.On("data", async (response) =>
{
    BotDto data = response.GetValue<BotDto>();

    switch (data.type)
    {
        case "INIT":
            Console.WriteLine("Init");
            Init(data);

            break;
        case "RESULT":
            Result(data);
            Console.WriteLine("Result");
            break;

        case "SET":
            await Set(data, response);
            Console.WriteLine("Set");
            break;
        case "ROUND":
            await Round(data, response);
            Console.WriteLine("Round");
            break;
    }
});


while (!gotDisconnected)
{
    await Task.Delay(10000);
    Console.WriteLine("Still alive!");
}


void Init(BotDto botDto)
{
    if (!games.ContainsKey(botDto.id))
    {
        games[botDto.id] = new GameRunner( botDto.self );
    }
}


async Task Result(BotDto botDto)
{
    TellFloWinOrLoose tellFloWinOrLoose = new TellFloWinOrLoose();
    
    foreach (var player in botDto.players)
    {
        if (player.id.Equals(games[botDto.id].SpielerId))
        {
            if (player.score == 0)
            {
                await tellFloWinOrLoose.SentFloLoose();
                Console.WriteLine("we lost :-(");
                File.WriteAllText($"{botDto.id}_lost.json",JsonSerializer.Serialize(botDto.log));
            }
            else
            {
                await tellFloWinOrLoose.SentFloWin();
                Console.WriteLine("We won! ;-)");
                File.WriteAllText($"{botDto.id}_won.json",JsonSerializer.Serialize(botDto.log));
            }

            break;
        }
    }
    
    games.Remove(botDto.id);
    
}

async Task Set(BotDto botDto, SocketIOResponse socketIoResponse)
{
    var resultShips = await games[botDto.id].Set();
    var resultShipsString = JsonSerializer.Serialize(resultShips);
    await socketIoResponse.CallbackAsync(resultShipsString);
}


async Task Round(BotDto botDto, SocketIOResponse socketIoResponse)
{
    var playerIndex = Array.FindIndex(botDto.players, players => players.id == botDto.self);
    //TODO: Write index into class in init?
    var shoot = await games[botDto.id].Round(botDto.boards[playerIndex]);
    //todo: check ob response string oder array sein muss
    var shootString = JsonSerializer.Serialize(shoot);
    await socketIoResponse.CallbackAsync(shoot);
}
