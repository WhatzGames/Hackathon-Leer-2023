using JaKleingartenParadies.Dto;
using SocketIOClient;

namespace JaKleingartenParadies.Game;

public class JaTasticInversBot : Bot
{
    public JaTasticInversBot(string secret) : base(secret)
    {
    }

    public override AimingMode AimingMode { get => AimingMode.Invers; }
}