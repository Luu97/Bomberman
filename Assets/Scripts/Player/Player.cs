using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Player : NetworkBehaviour {

    public float MoveSpeed = 5f;
    public bool CanMove = true; //Can the player move?
    public bool CanDropBombs = true; //Can the player drop bombs?
    public int MaxBombs = 2; //Amount of bombs the player has left to drop, Sgets decreased as the player drops a bomb, increases as an owned bomb explodes
    [SyncVar]
    private bool dead = false;
    public bool Dead {
        get { return dead; }
        set {
            dead = value;
            if (value == true) {
                Die();
            }
        }
    }

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    public void Setup () {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++) {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    void SetDefaults() {
        CanMove = true;
        CanDropBombs = true;
        MaxBombs = 2;
        MoveSpeed = 5f;
        Dead = false;

        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;
    }

    void Die() {
        transform.position = new Vector3(0, -50, 0);

        for (int i = 0; i < disableOnDeath.Length; i++) {
            disableOnDeath[i].enabled = false;
        }

        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = false;

        Respawn();
    }

    public void Respawn() {
        StartCoroutine(RespawnDelayed());
    }

    private IEnumerator RespawnDelayed() {
        yield return new WaitForSeconds(3f);

        SetDefaults();
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
    }
}
