using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharController : MonoBehaviour {


    public NavMeshAgent agent;
    Animator anim;

    //public Animator hairAnim;

    Vector3 prevPosition;
    Vector3 targetLoc;
    TextMesh textMesh;

    public float walkSpeed = 3f;

    public GameObject target;

    bool arrived = false;

    Vector2 chanceToPerformAction = new Vector2(10, 20);

    bool doingAction = false;

    [SerializeField]

    bool hasFollowTarget = false;

    [SerializeField]
    Transform followTarget;

    public static float ScrollWheel { get { return Input.mouseScrollDelta.y / 10; } }

    public bool manualControl;

    void Start ()
    {
        anim = GetComponent<Animator>();

        //targetLoc = transform.right * 40f;

        //target = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), targetLoc, Quaternion.identity) as GameObject;

        FindNewTarget();

        textMesh = transform.GetChild(0).GetComponent<TextMesh>();

        StartCoroutine("DoAction");
        StartCoroutine("Run");

        if (manualControl)
        {
            gameObject.AddComponent<ManualCharacterController>().SetNewTarget(transform.localPosition);
            Destroy(this);
        }


    }

    IEnumerator Run()
    {
        while (true)
        {
            walkSpeed = 10f;
            yield return new WaitForSeconds(Random.Range(2.5f, 7f));
            walkSpeed = 3f;

            yield return new WaitForSeconds(Random.Range(20, 60));
        }
    }

    IEnumerator DoAction()
    {
        while (true)
        {
            doingAction = true;

            int r = Random.Range(0, 4);
            
            if(r == 0)
            {
                anim.Play("Pose");
            }
            else if(r == 1)
            {
                anim.Play("Laugh");
            }
            else if(r == 2)
            {
                anim.Play("Shake");
            }
            else if(r == 3)
            {
                anim.Play("Nod");
            }
            /*
            else if (r == 4)
            {
                Follow();
                walkSpeed = 15f;
                print("Following");
            }
            */

            if (r <= 3)
            {
                yield return new WaitForSeconds(Random.Range(2.5f, 7f));
            }

            /*
            else
            {
                textMesh.gameObject.GetComponent<Renderer>().enabled = true;

                yield return new WaitForSeconds(20f);
                hasFollowTarget = false;

                textMesh.gameObject.GetComponent<Renderer>().enabled = false;

            }
            */

            doingAction = false;


            yield return new WaitForSeconds(Random.Range(chanceToPerformAction.x, chanceToPerformAction.y));
        }
    }

    void Follow()
    {
        if (hasFollowTarget == false)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f, 6);

            if (colliders.Length > 0)
            {
                followTarget = colliders[0].transform;
                hasFollowTarget = true;
            }
        }
    }

    void FindNewTarget()
    {
        float randX = Random.Range(60, -60);
        float randY = Random.Range(30, -30);

        targetLoc = new Vector3(randX, 1, randY);
        //target.transform.position = targetLoc;

        arrived = false;
    }
	
	
	void Update ()
    {

        if (ScrollWheel > 0)
        {
            walkSpeed += ScrollWheel/2;
        }

        //anim.SetFloat("walkSpeed", walkSpeed * 1.5f);

        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetLoc = target.transform.position;
        }

        if(hasFollowTarget)
        {
            targetLoc = new Vector3(followTarget.position.x + 1, 1 , followTarget.position.y);
            textMesh.text = followTarget.name;
        }

        if (doingAction == false)
        {
            float dt = Time.deltaTime;

            float dist = Vector3.Distance(transform.position, targetLoc);

            if (dist > 0.3f)
            {

                if (targetLoc.x > transform.position.x - 0.3f && targetLoc.x > transform.position.x + 0.3f)
                {
                    transform.Translate(walkSpeed * dt, 0, 0);
                    anim.Play("WalkRight");


                    anim.SetBool("hasStopped", false);
                }
                else if (targetLoc.x < transform.position.x - 0.3f && targetLoc.x < transform.position.x + 0.3f)
                {
                    transform.Translate(-walkSpeed * dt, 0, 0);
                    anim.Play("WalkLeft");

                    anim.SetBool("hasStopped", false);
                }
                else if (targetLoc.z > transform.position.z)
                {
                    transform.Translate(0, 0, walkSpeed * dt);
                    anim.Play("WalkUp");

                    anim.SetBool("hasStopped", false);
                }
                else if (targetLoc.z < transform.position.z)
                {
                    transform.Translate(0, 0, -walkSpeed * dt);
                    anim.Play("WalkDown");

                    anim.SetBool("hasStopped", false);
                }
            }

            else
            {
                anim.SetBool("hasStopped", true);


                if (arrived == false)
                {
                    arrived = true;
                    Invoke("FindNewTarget", Random.Range(3, 15));
                }
            }
        }


        //transform.position = Vector2.MoveTowards(transform.position, targetLoc, .1f);

        Vector3 vel = ((Vector3)transform.position - prevPosition) / Time.deltaTime;

        //print(vel.normalized + " - " + Vector2.up);

        //textMesh.text = vel.normalized.ToString();

        /*
        if ((int)vel.normalized.x == (int)Vector2.up.x && (int)vel.normalized.x != (int)Vector2.down.x)
        {
            //print("Uppppppppppp");
            anim.Play("WalkUp");
            anim.SetBool("hasStopped", false);
        }
        else if ((int)vel.normalized.x == (int)Vector2.down.x)
        {
            anim.Play("WalkDown");
            anim.SetBool("hasStopped", false);
        }
        else if ((int)vel.normalized.y == (int)Vector2.right.x)
        {
            anim.Play("WalkRight");
            anim.SetBool("hasStopped", false);
        }
        else if ((int)vel.normalized.y == (int)-Vector2.right.x)
        {
            anim.Play("WalkLeft");
            anim.SetBool("hasStopped", false);
        }
        else if (vel.normalized == Vector2.zero)
        {
            anim.SetBool("hasStopped", true);
        }
        */

        /*
        if (Input.GetKey(KeyCode.W))
        {
            anim.Play("Walk_Up");
            anim.SetBool("hasStopped", false);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            anim.Play("Walk_Down");
            anim.SetBool("hasStopped", false);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            anim.Play("Walk_Left");
            anim.SetBool("hasStopped", false);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            anim.Play("Walk_Right");
            anim.SetBool("hasStopped", false);
        }
        else
        {
            anim.SetBool("hasStopped", true);
        }
        */

        prevPosition = transform.position;
        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
}
