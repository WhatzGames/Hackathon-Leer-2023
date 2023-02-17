// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using JaKleingartenParadies.Dto;
using SocketIOClient;
using SocketIOClient.Transport;

const string Secret = "2d376eb7-ead4-4b7c-99c0-3a21515e8cd5";
bool gotDisconnected = false;

var client = new SocketIO("https://games.uhno.de", new SocketIOOptions() {Transport = TransportProtocol.WebSocket});

client.On("connect",
    async (e) =>
    {
        await client.EmitAsync("authenticate",
            (success) => { Console.WriteLine($"Authentication successful: {success}"); }, Secret);
    });

client.On("disconnect", (e) =>
{
    Console.WriteLine("Disconnected");
    gotDisconnected = true;
});


await client.ConnectAsync();

client.On("data", (response) =>
{
    var data = JsonSerializer.Deserialize<BotDto>(response.GetValue()
                                                          .GetString());
    switch (data.type)
    {
        case "INIT":
            Init(data);

            break;
        case "RESULT":
            Result(data);

            break;

        case "SET":
            Set(data, response);
            break;
        case "ROUND":
            Round(data, response);
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
    throw new NotImplementedException();
}


void Result(BotDto botDto)
{
    throw new NotImplementedException();
}
void Set(BotDto botDto, SocketIOResponse socketIoResponse)
{
    throw new NotImplementedException();
}


void Round(BotDto botDto, SocketIOResponse socketIoResponse)
{
    throw new NotImplementedException();
}
