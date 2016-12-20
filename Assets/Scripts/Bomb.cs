using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine.Networking;

public class Bomb : NetworkBehaviour {
    public AudioClip explosionSound;
    public GameObject explosionPrefab;
    public LayerMask levelMask; // This LayerMask makes sure the rays cast to check for free spaces only hits the blocks in the level
    private bool exploded = false;

    // Use this for initialization
    void Start() {
        Invoke("Explode", 3f); //Call Explode in 3 seconds
    }

    void Explode() {
        //Explosion sound
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);

        

        //Create a first explosion at the bomb position
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        //For every direction, start a chain of explosions
        StartCoroutine(CreateExplosions(Vector3.forward));
        StartCoroutine(CreateExplosions(Vector3.right));
        StartCoroutine(CreateExplosions(Vector3.back));
        StartCoroutine(CreateExplosions(Vector3.left));

        GetComponent<MeshRenderer>().enabled = false; //Disable mesh
        exploded = true;
        transform.FindChild("Collider").gameObject.SetActive(false); //Disable the trigger
        Destroy(gameObject, .3f); //Destroy the actual bomb in 0.3 seconds, after all coroutines have finished
    }

    void OnDestroy() {
        foreach (Player player in PlayerManager.GetAllPlayers()) {
            player.UpdateBombCount();
        }
        /*if (transform.name.Contains("(1)")) {
            PlayerManager.GetPlayer(1).UpdateBombCount();
        }
        if (transform.name.Contains("(2)")) {
            PlayerManager.GetPlayer(2).UpdateBombCount();
        }
        if (transform.name.Contains("(3)")) {
            PlayerManager.GetPlayer(3).UpdateBombCount();
        }
        if (transform.name.Contains("(4)")) {
            PlayerManager.GetPlayer(4).UpdateBombCount();
        }*/
    }

    public void OnTriggerEnter(Collider other) {
        if (!exploded && other.CompareTag("Explosion")) { //If not exploded yet and this bomb is hit by an explosion...
            CancelInvoke("Explode"); //Cancel the already called Explode, else the bomb might explode twice 
            Explode(); //Finally, explode!
        }
    }

    private IEnumerator CreateExplosions(Vector3 direction) {
        for (int i = 1; i < 3; i++) { //The 3 here dictates how far the raycasts will check, in this case 3 tiles far
            RaycastHit hit; //Holds all information about what the raycast hits

            Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction, out hit, i, levelMask); //Raycast in the specified direction at i distance, because of the layer mask it'll only hit blocks, not players or bombs

            if (!hit.collider) { // Free space, make a new explosion
                Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
            }
            else { //Hit a block, stop spawning in this direction
                break;
            }

            yield return new WaitForSeconds(.05f); //Wait 50 milliseconds before checking the next location
        }

    }
}
