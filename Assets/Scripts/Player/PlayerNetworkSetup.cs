using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

[RequireComponent(typeof(Player))]
public class PlayerNetworkSetup : NetworkBehaviour {

    public Behaviour[] ComponentsToDisable;

    // Use this for initialization
    void Start() {
        if (!isLocalPlayer) {
            DisableComponents();
        }

        GetComponent<Player>().Setup();
    }

    void DisableComponents() {
        foreach (Behaviour component in ComponentsToDisable) {
            component.enabled = false;
        }
    }

    public override void OnStartClient() {
        base.OnStartClient();

        int id;

        id = FindFreeId();

        if (id == 0)
            return;

        //string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();
        _player.Id = id;
        PlayerManager.RegisterPlayer(id, _player);
    }


    int FindFreeId() {
        List<Player> allPlayers = PlayerManager.GetAllPlayers();

        if (allPlayers.Count == 0) {
            return 1;
        }
        else if (allPlayers.Count == 1) {
            if (allPlayers[0].Id == 1)
                return 2;
            else
                return 1;
        }
        else if (allPlayers.Count == 4) {
            Destroy(gameObject);
            return 0;
        }
        else {
            bool[] belegt = new bool[4];

            for (int i = 0; i < allPlayers.Count; i++) {
                foreach (Player player in allPlayers) {
                    if (player.Id - 1 == i) {
                        belegt[i] = true;
                    }
                }
            }

            for (int i = 0; i < belegt.Length; i++) {
                if (belegt[i] == false)
                    return i + 1;
            }
            return 0;
        }
    }


    void OnDisable() {
        PlayerManager.UnRegisterPlayer(GetComponent<Player>().Id);
    }
}
