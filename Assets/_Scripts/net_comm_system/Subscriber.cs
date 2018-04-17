
public class Subscriber {
    private string URI;
    private string playerName;
    private CallbackClient cbc;

    public Subscriber(string URI, string playerName) {
        this.URI = URI;
        this.playerName = playerName;
        cbc = new CallbackClient(URI, playerName);

    }

    public bool CallbackClientConnect() {
        cbc.PingService()

        }
    }
    bool PublishToClient();

}
