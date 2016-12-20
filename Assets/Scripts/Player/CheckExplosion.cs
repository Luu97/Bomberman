using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class CheckExplosion : NetworkBehaviour {

    public void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Explosion") && isServer) { //hit by explosion
            StopAllCoroutines();
            StartCoroutine(DelayedPlayerDied(GetComponent<Player>().Id));
        }
    }

    IEnumerator DelayedPlayerDied(int _id) {
        yield return new WaitForSeconds(0.1f);

        RpcKillPlayer(_id);

        Invoke("CheckAlivePlayers", 0.1f);
    }

    void CheckAlivePlayers() {
        List<Player> alivePlayers = PlayerManager.GetAlivePlayers();
        if (alivePlayers.Count == 0) {
            Debug.Log("Game ended in a Draw");
            RpcRespawn();
        }
        else if (alivePlayers.Count == 1) {
            Debug.Log(alivePlayers[0].transform.name + " won the game!");
            RpcRespawn();
        }
    }

    [ClientRpc]
    void RpcKillPlayer(int _id) {
        Player player = PlayerManager.GetPlayer(_id);
        player.Die();
        PlayerManager.GetPlayer(_id).DeathCounter++;
        Debug.Log("Player " + GetComponent<Player>().Id + " just died! (" + PlayerManager.GetPlayer(_id).DeathCounter + ")");
    }

    [ClientRpc]
    void RpcRespawn() {
        List<Player> allPlayers = PlayerManager.GetAllPlayers();
        foreach (Player player in allPlayers) {
            player.Die();
            player.Respawn();
        }
    }

}
