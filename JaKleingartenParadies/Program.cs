// See https://aka.ms/new-console-template for more information

using System.Runtime.Loader;
using SocketIOClient;
using SocketIOClient.Transport;

const string Secret = "";

var client = new SocketIO("https://games.uhno.de", new SocketIOOptions() {Transport = TransportProtocol.WebSocket});

client.OnConnected += async (sender, e) =>
{
    await client.EmitAsync("authenticate", (success) =>
    {

    }, Secret);
};

client.On("disconnect", (e) => { });




Console.WriteLine("Hello, World!");

