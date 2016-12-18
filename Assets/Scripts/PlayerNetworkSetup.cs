using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetworkSetup : NetworkBehaviour {

    public Behaviour[] ComponentsToDisable;

	// Use this for initialization
	void Start () {
		if (!isLocalPlayer) {
            foreach (Behaviour component in ComponentsToDisable) {
                component.enabled = false;
            }
        }
	}
}
