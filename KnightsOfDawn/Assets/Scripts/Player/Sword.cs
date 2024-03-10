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
    private float delay = 2f;

    private void Awake() {
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
    }
    private void OnEnable() {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    private void Update(){
        playerControls.Combat.Attack.performed += ctx => OnAttack();
    }
   public void OnAttack(){
        myAnimator.SetInteger("Attack", 2);
   }
}
