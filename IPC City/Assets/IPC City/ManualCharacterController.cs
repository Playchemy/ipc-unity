using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ManualCharacterController : MonoBehaviour
{
    public bool onManualControl = true;
    public float walkSpeed = 3f;

    private bool arrived = true;
    private bool doingAction = false;
    private Animator anim;
    private Vector2 prevPosition;
    public Vector2 targetLoc;
    private TextMesh textMesh;
    private Vector2 chanceToPerformAction = new Vector2(10, 20);

    private void Start()
    {
        anim = GetComponent<Animator>();
        textMesh = transform.GetChild(0).GetComponent<TextMesh>();

        if (!onManualControl)
        {
            //StartCoroutine("DoAction");
            //StartCoroutine("Run");
        }
    }

    public void SetNewTarget(Vector2 newPosition)
    {
        doingAction = false;
        targetLoc = newPosition;
        arrived = false;
    }

    private void Update()
    {
        if (doingAction == false)
        {
            if(!arrived)
            {
                if ((Vector2)transform.position != targetLoc)
                {
                    if (targetLoc.x > transform.position.x && targetLoc.x > transform.position.x)
                    {
                        transform.position = new Vector2(Mathf.MoveTowards(transform.position.x, targetLoc.x, walkSpeed * Time.deltaTime), transform.position.y);
                        anim.Play("WalkRight");
                        anim.SetBool("hasStopped", false);
                    }
                    else if (targetLoc.x < transform.position.x && targetLoc.x < transform.position.x)
                    {
                        transform.position = new Vector2(Mathf.MoveTowards(transform.position.x, targetLoc.x, walkSpeed * Time.deltaTime), transform.position.y);
                        anim.Play("WalkLeft");
                        anim.SetBool("hasStopped", false);
                    }
                    else if (targetLoc.y > transform.position.y)
                    {
                        transform.position = new Vector2(transform.position.x, Mathf.MoveTowards(transform.position.y, targetLoc.y, walkSpeed * Time.deltaTime));
                        anim.Play("WalkUp");
                        anim.SetBool("hasStopped", false);
                    }
                    else if (targetLoc.y < transform.position.y)
                    {
                        transform.position = new Vector2(transform.position.x, Mathf.MoveTowards(transform.position.y, targetLoc.y, walkSpeed * Time.deltaTime));
                        anim.Play("WalkDown");
                        anim.SetBool("hasStopped", false);
                    }
                }
                else
                {
                    arrived = true;
                    anim.SetBool("hasStopped", true);
                }
            }
        }

        Vector2 vel = ((Vector2)transform.position - prevPosition) / Time.deltaTime;
        textMesh.text = vel.normalized.ToString();
        prevPosition = transform.position;
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }

    private IEnumerator Run()
    {
        while (true)
        {
            walkSpeed = 10f;
            yield return new WaitForSeconds(Random.Range(2.5f, 7f));
            walkSpeed = 3f;
            yield return new WaitForSeconds(Random.Range(20, 60));
        }
    }

    private IEnumerator DoAction()
    {
        while (true)
        {
            doingAction = true;

            int r = Random.Range(0, 4);

            if (r == 0)
            {
                anim.Play("Pose");
            }
            else if (r == 1)
            {
                anim.Play("Laugh");
            }
            else if (r == 2)
            {
                anim.Play("Shake");
            }
            else if (r == 3)
            {
                anim.Play("Nod");
            }

            if (r <= 3)
            {
                yield return new WaitForSeconds(Random.Range(2.5f, 7f));
            }

            doingAction = false;
            yield return new WaitForSeconds(Random.Range(chanceToPerformAction.x, chanceToPerformAction.y));
        }
    }
}
