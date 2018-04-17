
public class PubSubClient {
    private ComClient comCli;

    public PubSubClient(string ipaddr, string port, string userName) {
        comCli = new ComClient(ipaddr, port, userName);
    }

    public bool ClientConnect() {
        return comCli.PingService();
    }

    public bool Subscribe() {
        return comCli.Subscribe();
    }

    public bool Unsubscribe() {
        return true;
    }

    public bool SendClientStateUpdate() {
        return true;
    }
}
