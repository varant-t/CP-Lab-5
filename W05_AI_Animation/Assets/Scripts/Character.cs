using UnityEngine;
using System.Collections;

// The GameObject is made to bounce using the space key.
// Also the GameOject can be moved forward/backward and left/right.
// Add a Quad to the scene so this GameObject can collider with a floor.

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    public float speed;
    public float jumpSpeed;
    public float gravity;

    Vector3 moveDirection = Vector3.zero;
    public CharacterController controller;

    public Transform thingToLookFrom;
    public float lookDistance;

    public Rigidbody projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileForce;

    Animator animator;

    Enemy en;
    void Start()
    {
        en = GetComponent<Enemy>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (speed <= 0)
        {
            speed = 6.0f;

            Debug.Log("Speed not set on " + name + ". Defaulting to " + speed);
        }

        if (jumpSpeed <= 0)
        {
            jumpSpeed = 8.0f;

            Debug.Log("JumpSpeed not set on " + name + ". Defaulting to " + jumpSpeed);
        }

        if (gravity <= 0)
        {
            gravity = 20.0f;

            Debug.Log("Gravity not set on " + name + ". Defaulting to " + gravity);
        }

        if (lookDistance <= 0)
        {
            lookDistance = 10.0f;

            Debug.Log("LookDistance not set on " + name + ". Defaulting to " + lookDistance);
        }

        if (projectileForce <= 0)
        {
            projectileForce = 10.0f;

            Debug.Log("ProjectileForce not set on " + name + ". Defaulting to " + projectileForce);
        }

        // let the gameObject fall down
        //gameObject.transform.position = new Vector3(0, 5, 0);
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection = moveDirection * speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);

        // Used for the Raycast to store information about what it collides with
        RaycastHit hit;

        // If there is no thingToLookFrom, default to Character
        if(!thingToLookFrom)
        {
            Debug.DrawRay(transform.position, transform.forward * lookDistance, Color.red);

            if(Physics.Raycast(transform.position, transform.forward, out hit, lookDistance))
            {
                Debug.Log("Raycast: " + hit.collider.gameObject.name);      
            }
        }
        else
        {
            Debug.DrawRay(thingToLookFrom.position, thingToLookFrom.forward * lookDistance, Color.red);

            if (Physics.Raycast(thingToLookFrom.position, thingToLookFrom.forward, out hit, lookDistance))
            {
                Debug.Log("Raycast: " + hit.collider.gameObject.name);
            }
        }

        if(Input.GetButtonDown("Fire1"))
        {
            //fire();

            // Use Animation Event to create projectile
            animator.SetTrigger("Attack");
            
           
        }

        animator.SetFloat("Speed", transform.TransformDirection(controller.velocity).z);
        animator.SetBool("IsGrounded", controller.isGrounded);
    }

    void fire()
    {
        Debug.Log("Pew Pew");

        Rigidbody temp = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

        temp.AddForce(projectileSpawnPoint.forward * projectileForce, ForceMode.Impulse);
    }

    // Usage Rules:
    // - Both GameObjects must have Colliders
    // - One or both GameObjects must have a Rigidbody
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter: " + collision.gameObject.name);
    }

    void OnCollisionExit(Collision collision)
    {
        Debug.Log("OnCollisionExit: " + collision.gameObject.name);
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("OnCollisionStay: " + collision.gameObject.name);
    }

    // Usage Rules:
    // - GameObject must have CharacterController
    // - Other GameObject must have a Collider
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("OnControllerColliderHit: " + hit.gameObject.name);
    }

    // Usage Rules:
    // - GameObject must have Collider marked as "IsTrigger"
    // - One or both GameObjects must have a Collider
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other.gameObject.name);
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit: " + other.gameObject.name);
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay: " + other.gameObject.name);
    }

    public void HitAnimation()
    {
        
    }

}