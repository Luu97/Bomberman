using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Player))]
public class PlayerBombInteraction : NetworkBehaviour {

    private Transform myTransform;
    private int maxBombs;
    private bool canDropBombs;

    private int bombCount;

    //Prefabs
    public GameObject bombPrefab;


    // Use this for initialization
    void Start() {
        myTransform = transform;
        maxBombs = GetComponent<Player>().MaxBombs;
        canDropBombs = GetComponent<Player>().CanDropBombs;
    }

    // Update is called once per frame
    void Update() {
        if (canDropBombs && Input.GetKeyDown(KeyCode.Space) && bombCount < maxBombs) { //Drop bomb
            DropBomb();
        }
    }

    /// <summary>
    /// Drops a bomb beneath the player
    /// </summary>
    [Client]
    private void DropBomb() {
        bombCount++;
        Invoke("BombExploded", 3f);
        CmdDropBomb(myTransform.position);
    }

    void BombExploded() {
        if (bombCount > 0)
            bombCount--;
    }

    [Command]
    private void CmdDropBomb(Vector3 position) {
        RpcDropBomb(position);
    }

    [ClientRpc]
    private void RpcDropBomb(Vector3 position) {
        if (bombPrefab) { //Check if bomb prefab is assigned first
            // Create new bomb and snap it to a tile
            Instantiate(bombPrefab,
                new Vector3(Mathf.RoundToInt(position.x), bombPrefab.transform.position.y, Mathf.RoundToInt(position.z)),
                bombPrefab.transform.rotation);
        }
    }

    [Client]
    public void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Explosion")) { //Not dead & hit by explosion
            GetComponent<Player>().Dead = true;
            CmdPlayerDied(transform.name);
        }
    }

    [Command]
    void CmdPlayerDied(string playerID) {
        Debug.Log(playerID + " just died!");
        List<Player> alivePlayers = GlobalStateManager.GetAlivePlayers();
        if (alivePlayers.Count == 0) {
            Debug.Log("Game ended in a Draw");
            //RpcRespawn(null);
        }
        else if (alivePlayers.Count == 1) {
            Debug.Log(alivePlayers[0].transform.name + " won the game!");
            //RpcRespawn(alivePlayers[0].transform.name);
        }
    }

    [ClientRpc]
    void RpcRespawn(string playerID) {
        if (playerID != null)
            GlobalStateManager.GetPlayer(playerID).Dead = true;
        List<Player> allPlayers = GlobalStateManager.GetAllPlayers();
        foreach (Player player in allPlayers) {
            player.Respawn();
        }
    }
}
