using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// this class is used to animate a sword after the attack button is pressed
public class Sword : MonoBehaviour
{
    public Transform weaponColliderX;
    public Transform weaponColliderY;
    private PlayerControls playerControls;
    private Animator myAnimator;
    private Vector2 attackDirection;
    private Vector2 lastMoveDirection;
    public string targetSortingLayer = "YourTargetSortingLayer"; // what layer do you want to send it to
    public int newOrderInLayer = 0; // Adjust this value as needed
    private int originalOrderInLayer;
    private bool isActionInProgress = false;
    private bool isAttacking = false;
    public float returnTime;   // Time to return to the original order
    public float attackRate = 2f; // Basically a cool down for the attacks to avoid spam
    private float nextAttackTime = 0f;
    

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
        originalOrderInLayer = GetComponent<SpriteRenderer>().sortingOrder;
        weaponColliderX.gameObject.SetActive(false);
        weaponColliderY.gameObject.SetActive(false);
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
        if (Time.time >= nextAttackTime) {
            if (playerControls.Combat.Attack.triggered) {
                OnAttack(lastMoveDirection);
                nextAttackTime = Time.time + 1f / attackRate; // cooldown for attack
            }
        }
    }
   public void OnAttack(Vector2 direction) {
        isAttacking = true;
        myAnimator.SetInteger("Attack", 2);
        
        // This section makes it so it only attacks in one of four directions (to avoid diagonals)
        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);
        if (absX > absY) {
            // Attack along the horizontal axis
            myAnimator.SetFloat("Direction_x", Mathf.Sign(direction.x));
            myAnimator.SetFloat("Direction_y", 0f);
            // Determine the rotation based on the attack direction
            weaponColliderX.gameObject.SetActive(true);
            float rotationY = direction.x > 0 ? 0f : -180f;
            weaponColliderX.transform.rotation = Quaternion.Euler(0, rotationY, 0);
        }
        else {
            // Attack along the vertical axis
            myAnimator.SetFloat("Direction_x", 0f);
            myAnimator.SetFloat("Direction_y", Mathf.Sign(direction.y));
            // Determine the rotation based on the attack direction
            weaponColliderY.gameObject.SetActive(true);
            float rotationX = direction.y > 0 ? 0f : -180f;
            weaponColliderY.transform.rotation = Quaternion.Euler(rotationX, 0, 0);
        }

        // the blocks below invert the order layering portion for the up and down attacks
        if (direction.y > 0) {
            StartCoroutine(ReturnOrginalOrder(returnTime));
            ChangeOrderInLayer(newOrderInLayer);
        }
        else if (direction.y < 0 || (direction.y == 0 && direction.x < 0)) {
            ChangeOrderInLayer(newOrderInLayer);
            StartCoroutine(ReturnOrginalOrder(myAnimator.GetCurrentAnimatorStateInfo(0).length));
        }

        StartCoroutine(ResetAttack());
   }

   private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(myAnimator.GetCurrentAnimatorStateInfo(0).length);
        myAnimator.SetInteger("Attack", -2);
        if (weaponColliderX.gameObject.activeSelf) {
            weaponColliderX.gameObject.SetActive(false);
        }
        else {
            weaponColliderY.gameObject.SetActive(false);
        }
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
