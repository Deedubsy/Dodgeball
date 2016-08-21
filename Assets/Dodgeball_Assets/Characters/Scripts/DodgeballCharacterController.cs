using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DodgeballCharacterController : MonoBehaviour
{

    public float movementSpeed = 2.0f;
    public float range = 100.0f;
    public float gravity = 7.8F;
    public Rigidbody ball1;
    public Rigidbody ball2;
    public Rigidbody ball3;
    public Camera camera;


    private Vector3 movementDirection = Vector3.zero;
    CharacterController character;



    //public GameObject ball1;
    //public GameObject ball2;
    //public GameObject ball3;
    //List<GameObject> lstOfBalls;
    //GameObject ballHeld;

    List<Rigidbody> lstOfBalls;


    Rigidbody ballHeld;

    GameObject closestObj = null;

    bool testThrow = false;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<CharacterController>();

        ballHeld = null;

        lstOfBalls = new List<Rigidbody>();
        lstOfBalls.Add(ball1);
        lstOfBalls.Add(ball2);
        lstOfBalls.Add(ball3);
    }

    // Update is called once per frame
    void Update()
    {
        if (character.isGrounded)
        {
            movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            movementDirection = transform.TransformDirection(movementDirection);
            movementDirection *= movementSpeed;
            //if (Input.GetButton("Jump"))
            //    moveDirection.y = jumpSpeed;

        }
        movementDirection.y -= gravity * Time.deltaTime;
        character.Move(movementDirection * Time.deltaTime);

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
            ballHeld.transform.position = character.transform.position + new Vector3(0, 0, 1.5f);
        }

        
        Debug.DrawLine(Vector3.zero, new Vector3(0.0f, 5.0f, 0.0f), Color.red);
    }

    private Rigidbody GetNearBalls()
    {
        Rigidbody closestObj = null;
        List<Rigidbody> lstOfTempObjects = new List<Rigidbody>();

        Collider[] colliders = Physics.OverlapSphere(character.transform.position, 100, 1);
        foreach (var collisions in colliders)
        {
            foreach (Rigidbody ball in lstOfBalls)
            {
                if (collisions.gameObject == ball.gameObject)
                {
                    lstOfTempObjects.Add(ball);
                }
            }
        }

        float closestDistance = 0.0f;

        for (int i = 0; i < lstOfTempObjects.Count; i++)
        {
            float diff = Vector3.Distance(lstOfTempObjects[i].transform.position, character.transform.position);

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
        //Vector3 force = camera.ScreenToWorldPoint(Input.mousePosition) - controller.transform.position;




        //force.Normalize();

        //ballHeld.AddForce(force);

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawLine(character.transform.position, ray.origin);

        if (Physics.Raycast(ray, out hit, 1000))
        {
            Debug.DrawLine(character.transform.position, hit.point);
        }
    }
}
