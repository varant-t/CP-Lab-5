using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))] 
public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    Rigidbody rigidbody;

    public Item itemPrefab;
    public Transform itemSpawnPoint;
    
    
    public GameObject target;

    public bool autoGenPath;
    public string pathName;

    public GameObject[] path;
    public int pathIndex;

    enum EnemyState{Chase, Patrol}; //lets us choose the state of enemy
    [SerializeField] EnemyState state;

    enum PatrolType { DistanceBased, TriggerBased};
    [SerializeField] PatrolType patrolType;

    public float distanceToNextNode;

     Character character;

    // Start is called before the first frame update
    void Start()
    {
       
       character = GetComponent<Character>();
       
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        animator.applyRootMotion = false;
        rigidbody.isKinematic = true;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // checks for contionus motion , if ai stops moving use this

        if(string.IsNullOrEmpty(pathName))
        {
            pathName = "PathNode";
        }

        if(autoGenPath)
        {
            path = GameObject.FindGameObjectsWithTag(pathName); // is autogenpath is selected, puts them all in the array for you.
        }

        if(distanceToNextNode <= 0)
        {
            distanceToNextNode = 1.0f;
        }

        if(state == EnemyState.Chase)
        {
            target = GameObject.FindWithTag("Player");
           
        }
        else if (state == EnemyState.Patrol)
        {
            if (path.Length > 0)
            {
                target = path[pathIndex];
                
            }
        }

        if(target)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(state == EnemyState.Patrol && patrolType == PatrolType.DistanceBased)
        {
            Debug.DrawLine(transform.position, target.transform.position, Color.green);
            // if (Vector3.Distance(transform.position, target.transform.position) < distanceToNextNode) (1 way of finding pathing)
            //if (agent.remainingDistance < distanceToNextNode) (2nd way of finding pathing) 
            if (Vector3.Distance(transform.position, target.transform.position) < distanceToNextNode)
            {
                if(path.Length >0)
                {
                    pathIndex++;
                    pathIndex %= path.Length;
                    target = path[pathIndex];
                }
            }
        }

        if (target)
        {
            agent.SetDestination(target.transform.position);
           
        }

        animator.SetBool("IsGrounded", !agent.isOnOffMeshLink); // means were jumping somewhere
        animator.SetFloat("Speed", transform.InverseTransformDirection(agent.velocity).z);
    }

    //Add COllider to Enemy
    // Add Rigidbody to Enemy
    // - Rigidbody shoudl be set to IsKiematic

    private void OnTriggerEnter(Collider other)
    {
      
        //Instantiate(itemPrefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
        if (other.gameObject.tag == "Player")
        {
          
           animator.SetBool("IsDead", true);
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
       
    }
}
