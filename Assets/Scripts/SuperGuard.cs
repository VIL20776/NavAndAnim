using UnityEngine;
using UnityEngine.AI;

public class SuperGuard : Guard
{
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
            case State.Wait:
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
        {
            lastAttackTime = Time.time;
            currentState = State.Attack;
        }

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

        agent.speed = 0;
        animator.SetBool("IsAttacking", true);

        if (Time.time >= lastAttackTime + attackCooldown && !LookForObjective())
        {
            float distance = Vector3.Distance(transform.position, objective.position);
            if (distance < attackDistance)
            {
                gameOverEvent?.Invoke();
            }
            else
            {

                animator.SetBool("IsAttacking", false);
                currentState = State.Chase;
            }
        }
    }

}