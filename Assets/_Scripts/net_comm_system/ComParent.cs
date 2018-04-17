using UnityEngine;
using Grpc.Core;
using static NetworkService.NetworkService;
using NetworkService;

public class ComParent {
    protected Channel Channel { get; set; }
    protected NetworkServiceClient Client { get; set; }
    protected string IPaddr { get; set; }
    protected string Port { get; set; }
    protected string UserName { get; set; }

    public ComParent(string ipaddr=null, string port=null, string userName=null) {
        this.IPaddr = ipaddr;
        this.Port = port;
        this.UserName = userName;
    }

    ~ComParent() {
        // Not sure if this is actually necessary
        Channel.ShutdownAsync().Wait();
    }

    protected void SetNetData(string ipaddr, string port, string userName) {
        this.IPaddr = ipaddr;
        this.Port = port;
        this.UserName = userName;
    }

    protected Channel GetChannel() {
        if (Channel == null) {
            Channel = new Channel(IPaddr + ":" + Port, ChannelCredentials.Insecure);
        }
        return Channel;
    }

    protected NetworkServiceClient GetClient() {
        GetChannel();
        if (Client == null) {
            Client = new NetworkServiceClient(Channel);
        }
        return Client;
    }

    public bool PingService() {
        GetClient();
        var reply = Client.PingService(new PingRequest { Name = UserName });
        Debug.Log("PingService REPLY: " + reply.ResCode);
        if (reply.ResCode == PingReplyStatus.Success)
            return true;
        return false;
    }

}
