using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerCom {

    bool IsMasterClient();

    void SetMasterClient();

    void SetPlayerName(string name);

    string GetPlayerName();

    void SetNewState(MainGameScript.GameState state);

    void SetOppDeckSize(int cardCount);
}
