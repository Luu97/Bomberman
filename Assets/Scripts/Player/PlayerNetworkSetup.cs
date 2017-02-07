using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

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


        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        int id = int.Parse(_netID);
        Player _player = GetComponent<Player>();
        _player.Id = id;
        PlayerManager.RegisterPlayer(id, _player);
    }

    void OnDisable() {
        PlayerManager.UnRegisterPlayer(GetComponent<Player>().Id);
    }
}
