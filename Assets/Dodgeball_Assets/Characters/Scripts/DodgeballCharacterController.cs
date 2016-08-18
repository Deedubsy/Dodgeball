using UnityEngine;
using System.Collections;

public class DodgeballCharacterController : MonoBehaviour
{

    public float movementSpeed = 2.0f;
    public float gravity = 7.8F;
    private Vector3 movementDirection = Vector3.zero;
    CharacterController controller;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded)
        {
            movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            movementDirection = transform.TransformDirection(movementDirection);
            movementDirection *= movementSpeed;
            //if (Input.GetButton("Jump"))
            //    moveDirection.y = jumpSpeed;

        }
        movementDirection.y -= gravity * Time.deltaTime;
        controller.Move(movementDirection * Time.deltaTime);
    }
}
