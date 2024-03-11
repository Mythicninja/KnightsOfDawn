using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// this class is used to animate a sword after the attack button is pressed
public class Sword : MonoBehaviour
{
    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private Vector2 attackDirection;
    private Vector2 lastMoveDirection;
    public string targetSortingLayer = "YourTargetSortingLayer"; // what layer do you want to send it to
    public int newOrderInLayer = 0; // Adjust this value as needed
    private int originalOrderInLayer;
    private bool isActionInProgress = false;
    private bool isAttacking = false;
    public float returnTime;   // Time to return to the original order
    

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
        originalOrderInLayer = GetComponent<SpriteRenderer>().sortingOrder;
    }
    private void OnEnable() {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    private void Update(){
        attackDirection = playerControls.Movement.move.ReadValue<Vector2>(); // read player movement
        
        if (attackDirection.magnitude > 0) { 

            lastMoveDirection = attackDirection; // remember the last direction before idle 

            if (!isAttacking) {
                if ((lastMoveDirection.x > 0 && lastMoveDirection.y == 0) || 
                    (lastMoveDirection.x == 0 && lastMoveDirection.y > 0)) { // changes order of layer based on player direction
                    isActionInProgress = true;
                    ChangeOrderInLayer(newOrderInLayer);
                }
                else if (isActionInProgress || (lastMoveDirection.x > 0 && lastMoveDirection.y < 0)) { // if not facing those directions but flag was set, reset flag
                    StartCoroutine(ReturnOrginalOrder(returnTime));
                    isActionInProgress = false;
                }
            }
        }
        // attack based on the current direction or the direction in idle
        if (playerControls.Combat.Attack.triggered) {
            OnAttack(lastMoveDirection);
        }
    }
   public void OnAttack(Vector2 direction) {
        isAttacking = true;
        myAnimator.SetInteger("Attack", 2);
        myAnimator.SetFloat("Direction_x", direction.x);
        myAnimator.SetFloat("Direction_y", direction.y); 

        // the blocks below invert the order layering portion for the up and down attacks
        if (direction.y > 0) {
            StartCoroutine(ReturnOrginalOrder(returnTime));
            ChangeOrderInLayer(newOrderInLayer);
        }
        else if (direction.y < 0 || (direction.y == 0 && direction.x < 0)) {
            ChangeOrderInLayer(newOrderInLayer);
            StartCoroutine(ReturnOrginalOrder(myAnimator.GetCurrentAnimatorStateInfo(0).length));
        }

        StartCoroutine(ResetAttackAnimation());
   }

   private IEnumerator ResetAttackAnimation()
    {
        yield return new WaitForSeconds(myAnimator.GetCurrentAnimatorStateInfo(0).length);
        myAnimator.SetInteger("Attack", -2);
        isAttacking = false;
    }

     private void ChangeOrderInLayer(int newOrder)
    {
        GetComponent<SpriteRenderer>().sortingLayerName = targetSortingLayer;  
        GetComponent<SpriteRenderer>().sortingOrder = newOrder; 
    }

    private IEnumerator ReturnOrginalOrder(float delay) {
        yield return new WaitForSeconds(delay);
        ChangeOrderInLayer(originalOrderInLayer);
        isActionInProgress = false;
    }
}
