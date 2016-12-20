using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour {

    //Cached components
    private Rigidbody rigidBody;
    private Transform myTransform;
    private Animator animator;
    private float moveSpeed;
    private float rotationY;

    // Use this for initialization
    void Start() {
        //Cache the attached components for better performance and less typing
        rigidBody = GetComponent<Rigidbody>();
        myTransform = transform;
        animator = myTransform.FindChild("PlayerModel").GetComponent<Animator>();
        moveSpeed = GetComponent<Player>().MoveSpeed;

    }

    // Update is called once per frame
    void Update() {
        UpdateMovement();
    }

    private void UpdateMovement() {
        animator.SetBool("Walking", false); //Resets walking animation to idle

        if (!GetComponent<Player>().CanMove) { //Return if player can't move
            return;
        }

        UpdatePlayerMovement();
    }

    /// <summary>
    /// Updates Player 1's movement and facing rotation using the WASD keys and drops bombs using Space
    /// </summary>
    private void UpdatePlayerMovement() {

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            animator.SetBool("Walking", true);
            rigidBody.velocity = new Vector3(moveSpeed * Input.GetAxis("Horizontal"), rigidBody.velocity.y, moveSpeed * Input.GetAxis("Vertical"));
            myTransform.rotation = Quaternion.LookRotation(rigidBody.velocity);

        }
        else if (CrossPlatformInputManager.GetAxis("Horizontal") != 0 || CrossPlatformInputManager.GetAxis("Vertical") != 0) {
            animator.SetBool("Walking", true);
            rigidBody.velocity = new Vector3(moveSpeed * CrossPlatformInputManager.GetAxis("Horizontal"), rigidBody.velocity.y, moveSpeed * CrossPlatformInputManager.GetAxis("Vertical"));
            myTransform.rotation = Quaternion.LookRotation(rigidBody.velocity);
        }
    }
}
