using System.Collections;

public interface IPlayerCom {

    void SendReady();

    void SendPrepStart();

    void SendStartGame(int cardCount);

}
