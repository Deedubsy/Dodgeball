using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DodgeballCharacterController : MonoBehaviour
{

    public float movementSpeed = 2.0f;
    public float gravity = 7.8F;
    private Vector3 movementDirection = Vector3.zero;
    CharacterController controller;
    public GameObject ball1;
    public GameObject ball2;
    public GameObject ball3;

    GameObject ballHeld;

    GameObject closestObj = null;

    List<GameObject> lstOfBalls;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();

        ballHeld = null;

        lstOfBalls = new List<GameObject>();
        lstOfBalls.Add(ball1);
        lstOfBalls.Add(ball2);
        lstOfBalls.Add(ball3);
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

        if (Input.GetKeyDown(KeyCode.F))
            ballHeld = GetNearBalls();

        if (Input.GetKeyDown(KeyCode.Mouse0) && ballHeld != null)
        {
            //Throw ball
            ThrowBall();

            //Reset held ball
            ballHeld = null;
        }

        if (ballHeld != null)
        {
            ballHeld.transform.position = controller.transform.position + new Vector3(0, 0, 1.5f);
        }
    }

    private GameObject GetNearBalls()
    {
        GameObject closestObj = null;
        List<GameObject> lstOfTempObjects = new List<GameObject>();

        Collider[] colliders = Physics.OverlapSphere(controller.transform.position, 100, 1);
        foreach (var collisions in colliders)
        {
            foreach (GameObject ball in lstOfBalls)
            {
                if (collisions.gameObject == ball)
                {
                    lstOfTempObjects.Add(ball);
                }
            }
        }

        if (lstOfTempObjects != null && lstOfTempObjects.Count > 0) closestObj = new GameObject();

        float closestDistance = 0.0f;

        for (int i = 0; i < lstOfTempObjects.Count; i++)
        {
            float diff = Vector3.Distance(lstOfTempObjects[i].transform.position, controller.transform.position);

            if (diff < closestDistance || closestDistance == 0.0f)
            {
                closestObj = lstOfTempObjects[i];
                closestDistance = diff;
            }
        }

        return closestObj;
    }

    private void ThrowBall()
    {
        Vector3 mousePos = Input.mousePosition;

        //if()
    }
}
