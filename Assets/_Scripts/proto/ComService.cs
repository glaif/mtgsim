using UnityEngine;
using System.Threading.Tasks;
using Grpc.Core;
using static NetworkService.NetworkService;
using NetworkService;
using System;

public class ComService {

    class ServiceServer : NetworkServiceBase {

        private MainNetworkScript netSC;
        private bool isMaster = false;

        public void SetNetSC(MainNetworkScript netSC) {
            this.netSC = netSC;
        }

        public void SetMaster(bool val) {
            isMaster = val;
        }

        public override Task<PingReply> PingService(PingRequest request, ServerCallContext context) {
            return Task.FromResult(new PingReply { ResCode = PingReplyStatus.Success });
        }

        public override Task<JoinGameReply> JoinGame(JoinGameRequest request, ServerCallContext context) {
            JoinGameReply rep = new JoinGameReply();

            if (isMaster == false) {
                rep.ResCode = JoinGameReplyStatus.Failure;
                return Task.FromResult(rep);
            }

            if (netSC.JoinGame(request.PlayerName, context.Peer) == true) {
                rep.ResCode = JoinGameReplyStatus.Success;
            }
            return Task.FromResult(rep);
        }

    }

    private int servicePort;
    private Server serviceServer = null;
    private ServiceServer service = null;
    private bool running = false;

    public void StartService(bool isMaster, string port="50051") {
        servicePort = Int32.Parse(port);
        service = new ServiceServer();
        service.SetMaster(isMaster);
        serviceServer = new Server {
            Services = { BindService(new ServiceServer()) },
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
