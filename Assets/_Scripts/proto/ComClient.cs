using UnityEngine;
using Grpc.Core;
using static NetworkService.NetworkService;
using NetworkService;

public class ComClient {

    private Channel channel;
    private NetworkServiceClient client;
    private string ipaddr;
    private string port;
    private string userName;

    // TODO: Bypass network stack for local client

    public ComClient(string ipaddr, string port, string userName) {
        this.ipaddr = ipaddr;
        this.port = port;
        this.userName = userName;
    }

    ~ComClient() {
        // Not sure if this is actually necessary
        channel.ShutdownAsync().Wait();
    }

    private Channel GetChannel() {
        if (channel == null) {
            channel = new Channel(ipaddr + ":" + port, ChannelCredentials.Insecure);
        }
        return channel;
    }

    private NetworkServiceClient GetClient() {
        GetChannel();
        if (client == null) {
            client = new NetworkServiceClient(channel);
        }
        return client;
    }

    public bool PingService() {
        GetClient();
        var reply = client.PingService(new PingRequest { Name = userName });
        Debug.Log("PingService REPLY: " + reply.ResCode);
        if (reply.ResCode == PingReplyStatus.Success)
            return true;
        return false;
    }

    public bool JoinGame() {
        GetClient();
        var reply = client.JoinGame(new JoinGameRequest { PlayerName = userName });
        Debug.Log("JoinGame REPLY: " + reply.ResCode);
        if (reply.ResCode == JoinGameReplyStatus.Success)
            return true;
        return false;
    }
}
