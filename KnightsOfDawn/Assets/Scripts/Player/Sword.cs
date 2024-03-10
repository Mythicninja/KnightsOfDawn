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

    private void Update() {
        if (Input.GetKey(KeyCode.K))
        {
            Attack();
        }
    }
    private void Attack() {
        myAnimator.SetFloat("attack", 1);
    }
}
