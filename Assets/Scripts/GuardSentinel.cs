using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class GuardSentinel : Guard
{
    [SerializeField] UnityEvent callGuardsEvent;
    
    private float waitTime = 8.7f;
    private float waitTimer = 0;

    private float callingTime = 1f;
    private float lastCallTime = 0f;

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
                break;
            case State.Attack:
                Attack();
                break;
            case State.Wait:
                Wait();
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
            currentState = State.Wait;
        }
        if (LookForObjective())
        {
            lastCallTime = Time.time;
            currentState = State.Attack;
        }
    }

    void Attack()
    {
        Vector3 lookObjective = new Vector3(objective.position.x, 0, objective.position.z);
        transform.LookAt(lookObjective);

        agent.speed = 0;
        animator.SetBool("IsAttacking", true);

        if (Time.time >= lastCallTime + callingTime && !LookForObjective())
        {
            callGuardsEvent?.Invoke();

            animator.SetBool("IsAttacking", false);
            currentState = State.Patrol;
        }
    }

    void Wait()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTime)
        {
            wpIndex++;
            wpIndex %= wayPoints.Length;
            agent.SetDestination(wayPoints[wpIndex].position);
            currentState = State.Patrol;
            waitTimer = 0;
        }

        if (LookForObjective())
        {
            lastCallTime = Time.time;
            currentState = State.Attack;
        }

    }
}