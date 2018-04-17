using UnityEngine;
using System.Threading.Tasks;
using Grpc.Core;
using static NetworkService.NetworkService;
using NetworkService;
using System;

public class ComServiceServer {

    class ComService : ComServiceParent {
        private PubSubService pss;

        public void SetPubSubService(PubSubService pss) {
            this.pss = pss;
        }

        public override Task<SubscribeReply> Subscribe(SubscribeRequest request, ServerCallContext context) {
            SubscribeReply rep = new SubscribeReply();

            if (pss.Subscribe(request.PlayerName, context.Peer, request.Channel) == true) {
                rep.ResCode = SubscribeReplyStatus.Success;
            }
            return Task.FromResult(rep);
        }

    }

    private int servicePort;
    private Server serviceServer = null;
    private ComService service = null;
    private bool running = false;

    public void StartService(string port="50051") {
        servicePort = Int32.Parse(port);
        service = new ComService();
        serviceServer = new Server {
            Services = { BindService(new ComService()) },
            Ports = { new ServerPort("localhost", servicePort, ServerCredentials.Insecure) }
        };
        serviceServer.Start();
        running = true;
        Debug.Log("ComService server listening on port " + servicePort);
    }

    public void StopService() {
        if (service != null) {
            Debug.Log("Stopping the ComService...");
            serviceServer.ShutdownAsync().Wait();
            serviceServer = null;
        }
        Debug.Log("ComService stopped.");
    }

    public bool IsRunning() {
        return running;
    }
}
