using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour {

	public static Dictionary<int, Vector3> spawnPoints = new Dictionary<int, Vector3>();

    public void Start() {
        spawnPoints.Add(1, new Vector3(1, 0.5f, 7));
        spawnPoints.Add(2, new Vector3(8, 0.5f, 1));
        spawnPoints.Add(3, new Vector3(8, 0.5f, 7));
        spawnPoints.Add(4, new Vector3(1, 0.5f, 1));
    }
}
