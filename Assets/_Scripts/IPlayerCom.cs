using System.Collections;

public interface IPlayerCom {
    void SetNewState(MainGameScript.GameState state);

    bool IsMasterClient();

    void SetOppDeckSize(int cardCount);
}
