using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateDeathCounters : MonoBehaviour {

    public static void ValueChanged() {

        Text player1Text = GameObject.Find("Player1Deaths").GetComponent<Text>();
        Text player2Text = GameObject.Find("Player2Deaths").GetComponent<Text>();
        Text player3Text = GameObject.Find("Player3Deaths").GetComponent<Text>();
        Text player4Text = GameObject.Find("Player4Deaths").GetComponent<Text>();

        player1Text.text = string.Empty;
        player2Text.text = string.Empty;
        player3Text.text = string.Empty;
        player4Text.text = string.Empty;

        foreach (Player player in PlayerManager.GetAllPlayers()) {
            switch (player.Id) {
                case 1:
                    player1Text.text = "Player1 - " + PlayerManager.GetPlayer(1).DeathCounter;
                    break;
                case 2:
                    player2Text.text = "Player2 - " + PlayerManager.GetPlayer(2).DeathCounter;
                    break;
                case 3:
                    player3Text.text = "Player3 - " + PlayerManager.GetPlayer(3).DeathCounter;
                    break;
                case 4:
                    player4Text.text = "Player4 - " + PlayerManager.GetPlayer(4).DeathCounter;
                    break;
            }
        }
    }
}
