using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DodgeballCharacterController : MonoBehaviour
{
    public Plane plane;
    public float movementSpeed = 2.0f;
    public float range = 100.0f;
    public float throwSpeed = 5.0f;
    public float gravity = 7.8F;
    public Rigidbody ball1;
    public Rigidbody ball2;
    public Rigidbody ball3;

    private Vector3 movementDirection = Vector3.zero;
    private RaycastHit hit;
    private float rayCastLength = 500;
    private CharacterController character;
    private List<Rigidbody> lstOfBalls;
    private Rigidbody ballHeld;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<CharacterController>();

        ballHeld = null;

        lstOfBalls = new List<Rigidbody>();
        lstOfBalls.Add(ball1);
        lstOfBalls.Add(ball2);
        lstOfBalls.Add(ball3);

        //plane = GetComponent<Plane>();
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

#if DEBUG 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * rayCastLength, Color.yellow);
#else

#endif

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distFromPlane;

        //if (Physics.Raycast(ray, out hit, rayCastLength))
        if (plane.Raycast(ray, out distFromPlane))
        {
            Vector3 point = ray.GetPoint(distFromPlane);
            //Vector3 direction = character.transform.position - hit.point;
            Vector3 direction = character.transform.position - point;
            ballHeld.AddForce(direction.normalized * throwSpeed);
        }
    }
}
