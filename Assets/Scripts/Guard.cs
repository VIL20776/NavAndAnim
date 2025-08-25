using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Guard : MonoBehaviour
{
    private enum State { Patrol, Chase, Attack };

    [SerializeField] private Transform objective;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float viewAngle = 60.0f;
    [SerializeField] private float viewRadius = 10.0f;
    [SerializeField] private float attackDistance = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float loseSightTime = 1f;
    [SerializeField] private State currentState = State.Patrol;
    private float loseSightTimer = 0;
    private float lastAttackTime = 0;
    private int wpIndex = 0;
    private Animator animator => GetComponentInChildren<Animator>();
    private NavMeshAgent agent => GetComponent<NavMeshAgent>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (wayPoints.Length > 0)
        {
            agent.SetDestination(wayPoints[wpIndex].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Velocity", agent.velocity.magnitude);
        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
            default:
                break;
        }
    }

    void Patrol()
    {
        agent.speed = 2.5f;
        agent.stoppingDistance = 0;
        if (agent.remainingDistance < 0.5f)
        {
            wpIndex++;
            wpIndex %= wayPoints.Length;
            agent.SetDestination(wayPoints[wpIndex].position);
        }
        if (LookForObjective())
            currentState = State.Chase;
    }

    void Chase()
    {
        agent.speed = 5f;
        agent.SetDestination(objective.position);
        agent.stoppingDistance = 0;

        float distance = Vector3.Distance(transform.position, objective.position);
        if (distance < attackDistance)
            currentState = State.Attack;

        if (!LookForObjective())
        {
            loseSightTimer += Time.deltaTime;
            if (loseSightTimer >= loseSightTime)
            {
                currentState = State.Patrol;
                loseSightTimer = 0;
            }
        }
        else
        {
            loseSightTimer = 0;
        }
    }

    void Attack()
    {
        Vector3 lookObjective = new Vector3(objective.position.x, 0, objective.position.z);
        transform.LookAt(lookObjective);
        agent.stoppingDistance = 2f;

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetBool("IsAttacking", true);
            lastAttackTime = Time.time;

            //TODO

            float distance = Vector3.Distance(transform.position, objective.position);
            if (distance >= attackDistance)
            {
                animator.SetBool("IsAttacking", false);
                currentState = State.Chase;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = LookForObjective() ? Color.green : Color.red;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * transform.forward;

        Gizmos.DrawRay(transform.position + Vector3.up, leftBoundary * viewRadius);
        Gizmos.DrawRay(transform.position + Vector3.up, rightBoundary * viewRadius);
    }

    bool LookForObjective()
    {
        if (objective == null)
            return false;

        Vector3 dir = objective.position - transform.position;
        if (dir.magnitude > viewRadius)
            return false;

        float angleToPlayer = Vector3.Angle(transform.forward, dir.normalized);
        if (angleToPlayer > viewAngle / 2)
            return false;

        if (Physics.Raycast(transform.position + Vector3.up, dir.normalized, out RaycastHit hit, viewRadius))
        {
            if (hit.transform == objective)
                return true;
        }

        return false;
    }
}
