using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SRai : MonoBehaviour
{
    public float speed;
    public float checkJarak;
    public float attackJarak;

    public bool muter;
    public LayerMask whatIsPlayer;
    
    private Transform target;
    public Transform[] Path;
    public int currentPoint;
    public Transform currentGoal;
    public float roundingDistance;
    /* public NavMeshAgent agent; */

    //private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    private Vector2 ptroll;
    public Vector3 dir;
    public Vector3 dir1;
    public Vector3 temp;
    public bool isInChaseRange;
    public bool isInAttackRange;
    // Start is called before the first frame update
    private void Start()
    {
        /* agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false; */
        
        //rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }
 
    // Update is called once per frame
    private void Update()
    {
        anim.SetBool("ngejar", true);

        isInChaseRange = Physics2D.OverlapCircle(transform.position, checkJarak, whatIsPlayer);
        isInAttackRange = Physics2D.OverlapCircle(transform.position, attackJarak, whatIsPlayer);
        temp = Path[currentPoint].position;

        dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        dir.Normalize();
        movement = dir;


        dir1 = Path[currentPoint].position - transform.position;
        if (muter && isInChaseRange) {
            anim.SetFloat("X", dir.x);
            anim.SetFloat("Y", dir.y);
        }
        else if (muter && !isInChaseRange) {
            anim.SetFloat("X", movement.x);
            anim.SetFloat("Y", movement.y);
        }

    }

    private void FixedUpdate() {
        if (isInChaseRange && !isInAttackRange) {
            //MoveCharacter(movement);
            //agent.SetDestination(movement);
        }
        if (isInAttackRange) {
            //rb.velocity = Vector2.zero;
        }
        if (!isInChaseRange && !isInAttackRange) {
            /* temp = Path[currentPoint].position;
            if (Vector3.Distance(transform.position, temp) > roundingDistance) {
                float angle1 = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg;
                temp.Normalize();
                ptroll = temp;
            }
            else {
                ChangeGoal();
            }
            
            MoveCharacter(ptroll); */
            PlsWork();
        } 
    }

    private void MoveCharacter(Vector2 dir) {
        //  rb.MovePosition((Vector2)transform.position + (dir * speed * Time.fixedDeltaTime));
    }

    private void ChangeGoal() {
        if (currentPoint == Path.Length - 1) {
            currentPoint = 0;
            currentGoal = Path[0];
        }
        else {
            currentPoint++;
            currentGoal = Path[currentPoint];
        }
    }

    private void PlsWork() {
        if (Vector3.Distance(transform.position, Path[currentPoint].position) > roundingDistance) {
            Vector3 temp = Vector3.MoveTowards(transform.position, Path[currentPoint].position, speed * Time.fixedDeltaTime);
            //rb.MovePosition(temp);
            //agent.SetDestination(Path[currentPoint].position);
            /* temp = Path[currentPoint].position - transform.position;
            float angle1 = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg;
            temp.Normalize();
            ptroll = temp;
            MoveCharacter(ptroll); */
        }
        else {
            ChangeGoal();
        }
    }
}
