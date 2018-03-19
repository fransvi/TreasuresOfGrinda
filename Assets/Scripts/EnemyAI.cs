using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour
{
    //Variables START
    //Enums
    public enum BehaviourState
    {
        notSpawned,
        dead,
        patrol,
        idle,
        aggressive,
        inspect
    }

    /*public enum Action
    {
        moveToPoint,
        attackStrategy //Initiate attacking strategy towards a target
    }*/

    //Connections
    public Transform target;
    public Path path; //The calculated path
    private Seeker seeker;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    //Attributes
    public float speed; //The AI's speed per second
    public ForceMode2D fMode;
    public BehaviourState currentBehaviourState;
    private int currentWaypoint = 0;//The waypoint we are currently moving towards

    //Apumuuttujat
    [HideInInspector]
    public bool pathIsEnded = false;
    private bool searchingForPlayer = false;

    //Tweaks
    public float pathUpdateRate = 2f; //How many times each second AI will update its path
    public float playerSearchRate = 2f; //How frequently AI will search for player GameObject (when player is dead etc.)
    public float waypointDistanceThreshold = 3f;//The max distance from AI to waypoint to change waypoint
    public float aggroDistance;
    //public float hitDistance;

    //Fixed values
    private readonly float minPathUpdateRate = 2f;
    private readonly float maxPathUpdateRate = 10f;
    private readonly float deltaPathUpdateRate = -1.3f; //How quickly pathUpdateRate changes when distance grows (1 Update per second/1 world distance unit)
    [SerializeField]
    public float idleAggroDistance = 5f;
    public readonly float aggressiveAggroDistance = 10f;
    //private readonly float idleHitDistance;
    //private readonly float aggressiveHitDistance;

    private readonly float idleMoveSpeed = 0f;
    [SerializeField]
    private float aggressiveMoveSpeed = 5f;

    //Variables END


    //Functions START
    //Initialization
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        StartIdle();
        
        StartCoroutine(SearchForPlayer());
        StartCoroutine(BehaviourStateMachine());
    }
    //Behaviours

    private void StartIdle()
    {
        //muuttujat
        speed = idleMoveSpeed;
        aggroDistance = idleAggroDistance;

        //Animaattori
        animator.SetBool("BatMove", false);

        //Coroutine

        //State
        currentBehaviourState = BehaviourState.idle;

    }

    private void StartAggressive()
    {
        //muuttujat
        speed = aggressiveMoveSpeed;
        aggroDistance = aggressiveAggroDistance;

        //Animaattori
        animator.SetBool("BatMove", true);

        //State
        currentBehaviourState = BehaviourState.aggressive;

        //Coroutine
        StartCoroutine(UpdatePath());

    }

    private void AggroCheck()
    {
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.position) < aggroDistance)
                StartAggressive();
        }
    }

    //Pathfinding
    private float CalculateUpdateRate()
    {
        //y = -1.3x + 10
        float rate = deltaPathUpdateRate*(Vector2.Distance(transform.position, target.position)) + maxPathUpdateRate;
        if (rate < minPathUpdateRate)
            rate = minPathUpdateRate;
        else if (rate > maxPathUpdateRate)
            rate = maxPathUpdateRate;
        return rate;
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("We got a path. Did it have an error? " + p.error);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    //Loops
    private IEnumerator BehaviourStateMachine()
    {
        while (true)
        {
            switch (currentBehaviourState)
            {
                case BehaviourState.notSpawned:
                    break;
                //case BehaviourState.patrol:
                //    Patrol();
                //    break;
                case BehaviourState.idle:
                    AggroCheck();
                    break;
                case BehaviourState.aggressive:
                    break;
                //case BehaviourState.inspect:
                //    Inspect();
                //    break;
                case BehaviourState.dead:
                    break;
            }
            yield return null;
        }
    }

    IEnumerator SearchForPlayer()
    {
        //TODO: tähän järkevä ehto
        while (true)
        {
            //Tsekataan intervallin välein onko target olemassa
            if (target == null && !searchingForPlayer)
            {
                //Jos ei, etsitään pelaajaobjekti
                searchingForPlayer = true;
                GameObject sResult = null;
                sResult = GameObject.FindGameObjectWithTag("Player");
                //Jos löytyi
                if (sResult != null)
                {
                    target = sResult.transform;
                    searchingForPlayer = false;
                }
            }
            yield return new WaitForSeconds(1f / playerSearchRate);
        }
    }

    IEnumerator UpdatePath()
    {
        while (currentBehaviourState == BehaviourState.aggressive)
        {
            if (target != null)
            {
                //Start a new path to the target position, return the result to the OnPathComplete method
                seeker.StartPath(transform.position, target.position, OnPathComplete);

                pathUpdateRate = CalculateUpdateRate();
            }
            yield return new WaitForSeconds(1f / pathUpdateRate);
        }
    }

    void FixedUpdate()
    {
        if (target != null && path != null) {
            if (currentWaypoint >= path.vectorPath.Count)
            {
                if (pathIsEnded)
                    return;

                Debug.Log("End of path reached.");
                pathIsEnded = true;
                return;
            }
            
            pathIsEnded = false;

            //Direction to the next waypoint
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

            //Flip
            sr.flipX = (dir.x > 0);

            //Move the AI
            //rb.AddForce(dir * speed * Time.fixedDeltaTime, fMode);

            //rb.velocity += (Vector2)dir * speed * Time.fixedDeltaTime;
            rb.velocity = (Vector2)dir * speed;
            if (rb.velocity.magnitude > speed)
                Debug.Log("Jouduttiin pienentää nopeutta");
                rb.velocity = rb.velocity.normalized * speed;
            Debug.Log("dir magnitude: " + dir.magnitude);
            Debug.Log("velocity magnitude: " + rb.velocity.magnitude);

            float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (dist < waypointDistanceThreshold)
            {
                currentWaypoint++;
            }
        }
    }
    //Functions END
}