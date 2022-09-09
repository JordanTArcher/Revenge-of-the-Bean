using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Patrol1 : MonoBehaviour
{
    public float enemyHealth = 5f;
    public int chaseSpeed = 5;
    public float visionRange = 10;

    public Transform[] points;
    private int destPoint = 0;
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator anim;
    public GameObject playerObject;

    public bool canAttack = true;
    public float attackCooldown = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        playerObject = GameObject.FindWithTag("Player");

        agent.autoBraking = false;
        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        // Returns if no points have been set up
        if (points.Length == 0)
            return;

        agent.destination = points[destPoint].position;

        // should be modified to randomize the next point when multiple enemies patrol the same area
        destPoint = (destPoint + 1) % points.Length;
    }

    // Update is called once per frame
    private void Update()
    {
        if (agent.pathPending)
        {
            return;
        }
        else if (CanSeePlayer())
        {

            ChasePlayer();
        }
        else if (agent.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        anim.SetFloat("Health", enemyHealth);
        if(enemyHealth <= 0)
        {
            EnemyDeath();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (canAttack)
            {
                anim.SetTrigger("Attack");
                StartCoroutine(StartCooldown());
            }
        }
    }

    private bool CanSeePlayer()
    {
        float fieldOfViewAngle = 110f;
        RaycastHit hit;
        Vector3 enemyToPlayer = playerObject.transform.position - transform.position;

        float angleToPlayer = Vector3.Angle(enemyToPlayer, transform.forward);
        bool isAngleUnderHalfAngleOfView = angleToPlayer < fieldOfViewAngle * 0.5f;

        if (!isAngleUnderHalfAngleOfView) return false;

        if (Physics.Raycast(transform.position + transform.up, enemyToPlayer.normalized, out hit, visionRange))
        {
            return hit.collider.gameObject.CompareTag("Player");
        }
        return false;
    }

    private void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.destination = playerObject.transform.position;
    }

    private void EnemyDeath()
    {
        agent.speed = 0;
        Destroy(gameObject, 3f);
    }

    public IEnumerator StartCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
