using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerController playerController;
    public Animator animator;

    public float runSpeed = 40f;

    private float horizontalMove = 0f;
    private bool jump = false;
    private bool crouch = false;

    private const string HORIZONTAL = "Horizontal";
    private const string JUMP = "Jump";
    private const string CROUCH = "Crouch";

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw(HORIZONTAL) * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown(JUMP))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }
    }


    void FixedUpdate()
    {
        // Move our character
        //playerController.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        //jump = false;
    }
}
