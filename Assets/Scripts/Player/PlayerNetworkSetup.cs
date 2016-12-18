using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerNetworkSetup : NetworkBehaviour {

    public Behaviour[] ComponentsToDisable;

	// Use this for initialization
	void Start () {
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
        Player _player = GetComponent<Player>();

        GlobalStateManager.RegisterPlayer(_netID, _player);
    }

    void OnDisable() {
        GlobalStateManager.UnRegisterPlayer(transform.name);
    }
}
