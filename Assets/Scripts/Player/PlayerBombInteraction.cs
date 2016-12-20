using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class PlayerBombInteraction : NetworkBehaviour {

    private Transform myTransform;

    //Prefabs
    public GameObject bombPrefab;

    private bool calledServer;

    // Use this for initialization
    void Start() {
        myTransform = transform;
        calledServer = false;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("DropBomb") || CrossPlatformInputManager.GetButtonDown("DropBomb")) {
            if (GetComponent<Player>().CanDropBombs && GetComponent<Player>().BombCount < GetComponent<Player>().MaxBombs) {
                new Vector2(Mathf.RoundToInt(myTransform.position.x), Mathf.RoundToInt(myTransform.position.z));
                DropBomb();
            }
        }
    }

    /// <summary>
    /// Drops a bomb beneath the player
    /// </summary>
    [Client]
    private void DropBomb() {
        foreach (GameObject bomb in GameObject.FindGameObjectsWithTag("Bomb")) {
            if (bomb.transform.position.x == Mathf.RoundToInt(myTransform.position.x) && bomb.transform.position.z == Mathf.RoundToInt(myTransform.position.z))
                return;
        }
        GetComponent<Player>().BombCount++;
        CmdDropBomb(myTransform.position, GetComponent<Player>().Id);
    }

    [Command]
    private void CmdDropBomb(Vector3 position, int id) {
        RpcDropBomb(position, id);
    }

    [ClientRpc]
    private void RpcDropBomb(Vector3 position, int id) {
        if (bombPrefab) { //Check if bomb prefab is assigned first
            // Create new bomb and snap it to a tile
            GameObject goBomb = Instantiate(bombPrefab,
                new Vector3(Mathf.RoundToInt(position.x), bombPrefab.transform.position.y, Mathf.RoundToInt(position.z)),
                bombPrefab.transform.rotation);

            goBomb.name = "(" + id.ToString() + ")" + "Bomb";

            Collider[] bombColliders = goBomb.GetComponentsInChildren<Collider>();
            for (int i = 0; i < bombColliders.Length; i++) {
                if (bombColliders[i].isTrigger == false)
                    Physics.IgnoreCollision(bombColliders[i], PlayerManager.GetPlayer(id).GetComponent<Collider>());
            }
        }
    }
}
