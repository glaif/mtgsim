using UnityEngine;
using System.Threading.Tasks;
using Grpc.Core;
using static NetworkService.NetworkService;
using NetworkService;

public class ComServiceParent : NetworkServiceBase {

    public override Task<PingReply> PingService(PingRequest request, ServerCallContext context) {
        return Task.FromResult(new PingReply { ResCode = PingReplyStatus.Success });
    }

}
