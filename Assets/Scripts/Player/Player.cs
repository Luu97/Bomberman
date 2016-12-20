using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

    public float MoveSpeed = 5f;
    public bool CanMove = true; //Can the player move?
    public bool CanDropBombs = true; //Can the player drop bombs?
    public int MaxBombs = 2; //Amount of bombs the player has left to drop, Sgets decreased as the player drops a bomb, increases as an owned bomb explodes
    public Text[] deathCounters;

    [SyncVar]
    private int id;

    public int Id {
        get { return id; }
        set { id = value; }
    }

    [SyncVar]
    private int deathCounter;

    public int DeathCounter {
        get { return deathCounter; }
        set {
            deathCounter = value;
            UpdateDeathCounters.ValueChanged();
        }
    }


    [SyncVar]
    private int bombCount;

    public int BombCount {
        get { return bombCount; }
        set { bombCount = value; }
    }


    [SyncVar]
    private bool dead = false;
    public bool Dead {
        get { return dead; }
        protected set {
            dead = value;
        }
    }

    public void Setup() {
        SetDefaults();
        DeathCounter = 0;
    }

    void SetDefaults() {
        CanMove = true;
        CanDropBombs = true;
        MaxBombs = 2;
        MoveSpeed = 5f;
        Dead = false;

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;

        Transform playerModel = transform.FindChild("PlayerModel");
        Transform playerCube = playerModel.FindChild("Player_Cube.001");
        SkinnedMeshRenderer renderer = playerCube.GetComponent<SkinnedMeshRenderer>();
        renderer.enabled = true;
    }

    public void UpdateBombCount() {
        Invoke("DelayedUpdateBombCount", 0.1f);
    }

    private void DelayedUpdateBombCount() {
        BombCount = 0;
        foreach (GameObject bomb in GameObject.FindGameObjectsWithTag("Bomb")) {
            if (bomb.name == "(" + Id + ")Bomb") {
                BombCount++;
            }
        }
    }

    public void Die() {

        transform.position = SpawnPoints.spawnPoints[Id];

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        Dead = true;
        CanDropBombs = false;
        CanMove = false;
        Transform playerModel = transform.FindChild("PlayerModel");
        Transform playerCube = playerModel.FindChild("Player_Cube.001");
        SkinnedMeshRenderer renderer = playerCube.GetComponent<SkinnedMeshRenderer>();
        renderer.enabled = false;
    }

    public void Respawn() {
        StartCoroutine(RespawnDelayed());
    }

    private IEnumerator RespawnDelayed() {
        yield return new WaitForSeconds(3f);

        SetDefaults();
    }
}
