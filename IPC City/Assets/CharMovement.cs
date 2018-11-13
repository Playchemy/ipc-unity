using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CharMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Vector3 targetLoc;

    public TextMesh text;
    Animator anim;

    bool arrived;

    public bool manualControl = false;

    public float newPositionDelay = 2f;
    bool canFindNewPos = true;

    public float pathfindRadius = 25f;

    void Start()
    {
        anim = GetComponent<Animator>();
        FindNewTarget();
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }

    public void FindNewTarget()
    {
        if (!canFindNewPos)
            return;

        //targetLoc = Random.insideUnitSphere * 35 + transform.position;
        targetLoc = RandomNavmeshLocation(pathfindRadius);
        targetLoc = new Vector3(targetLoc.x, transform.position.y, targetLoc.z);

        anim.SetBool("hasStopped", false);


        arrived = false;
        NavMeshHit hit;
        NavMesh.SamplePosition(targetLoc, out hit, 10f, 1);
        Vector3 destination = hit.position;
        agent.SetDestination(destination);

        targetLoc = destination;

        canFindNewPos = false;
        Invoke("ResetFindCooldown", newPositionDelay);

    }


    //public void FindNewTarget()
    //{
    //    if (!canFindNewPos)
    //        return;



    //    float randX = Random.Range(-110, 285);
    //    float randY = Random.Range(100, -235);

    //    targetLoc = new Vector3(randX, 1, randY);
    //    //target.transform.position = targetLoc;

    //    anim.SetBool("hasStopped", false);

    //    arrived = false;

    //    NavMeshHit hit;
    //    NavMesh.SamplePosition(targetLoc, out hit, 10f, 1);

    //    Vector3 destination = hit.position;
    //    agent.SetDestination(destination);

    //    targetLoc = destination;


    //    canFindNewPos = false;
    //    Invoke("ResetFindCooldown", newPositionDelay);
    //}

    void ResetFindCooldown()
    {
        canFindNewPos = true;
    }

    public void GoToTarget(Vector3 dest)
    {
        agent.ResetPath();

        targetLoc = new Vector3(dest.x, 1, dest.z);
        //target.transform.position = targetLoc;

        anim.SetBool("hasStopped", false); 

        arrived = false;

        NavMeshHit hit;
        NavMesh.SamplePosition(targetLoc, out hit, 10, 1);

        Vector3 destination = hit.position;

        NavMeshPath newPath = new NavMeshPath(); 
        agent.CalculatePath(destination, newPath);
        agent.SetPath(newPath);
        //agent.SetDestination(destination);
    }

    void Update()
    {
        float dist = agent.remainingDistance;
        if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < 2 && arrived == false)
        {
            arrived = true;
            anim.SetBool("hasStopped", true);

            if (!manualControl)
            {
                Invoke("FindNewTarget", Random.Range(2, 6));
            }
        }

        Vector2 vel = new Vector2(agent.velocity.x, agent.velocity.z).normalized;
        //text.text = "(" + vel.x + ", " + vel.y + ")";

        if(vel.x > 0.5f)
        {
            anim.Play("WalkRight");
        }
        else if(vel.x < -0.5f)
        {
            anim.Play("WalkLeft");
        }
        else if (vel.y < -0.5f)
        {
            anim.Play("WalkDown");
        }
        else if (vel.y > 0.5f)
        {
            anim.Play("WalkUp");
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(targetLoc, 5);
    }
}
