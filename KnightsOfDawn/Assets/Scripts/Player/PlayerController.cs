using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    // used for movement
    private static float moveSpeed = 5f; // sprite speed
    private float runSpeed = moveSpeed * 1.5f;
    private float speed;

    private Vector2 movement;   // determines the movement from the input
    private Rigidbody2D rb; // attaches movement to sprite
    private PlayerControls playerControls; // Action Map - sets up the input for the player
    // used for animation
    private Animator myAnimator;

    protected override void Awake() {
        base.Awake();
        // set up playerControls action map and rigidbody component
        playerControls = new PlayerControls();  
        rb = GetComponent<Rigidbody2D>();
        // sets up animator component
        myAnimator = GetComponent<Animator>();
    }

    private void OnEnable() {
        playerControls.Enable(); // enables the action map
    }
    private void Update() {
        PlayerInput();
   }

   private void FixedUpdate() {
        Move();
   }

    private void PlayerInput() {
        // This set of code is for movement
        // reads player controls action map that set up earlier 
        movement = playerControls.Movement.move.ReadValue<Vector2>(); 
        // assigns any new input to the x and y values of the animator control
        myAnimator.SetFloat("moveX", movement.x); 
        myAnimator.SetFloat("moveY", movement.y);
        myAnimator.SetFloat("speed", movement.sqrMagnitude); // if it detects movement from the above, it will trigger the animation(s) as needed
         // sets the new idle position based on the direction the character is going
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1) { 
            myAnimator.SetFloat("lastX", Input.GetAxisRaw("Horizontal"));
            myAnimator.SetFloat("lastY", Input.GetAxisRaw("Vertical"));  
        }
    }

    private void Move() { 
        float speed = run() ? moveSpeed * 2f : moveSpeed;
        // actually moves the sprite using the inputs
        rb.MovePosition(rb.position + movement * (speed * Time.fixedDeltaTime));
    }

    private bool run() {
        if (Input.GetKeyDown(KeyCode.Space)) {
           return true;
        }
        else if (Input.GetKeyUp(KeyCode.Space)) {
            return false;
        }
        // Return the current state of the space key (running or not)
        return Input.GetKey(KeyCode.Space);
    }

}
