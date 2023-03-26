using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [Header("Health")]
    public int health = 5;

    [Header("Targeting")]
    public GameObject target;
    public LayerMask layerMaskTarget;
    public float radius;
    public Collider[] hitColliders;

    [Header("Turning")]
    public float turnRadius;
    public float distanceToTarget;
    public float timer = 3;
    public GameObject zombiePrefab;
    public GameObject scientistPrefab;


    [Header("Wander")]
    public float wanderTimer;
    public float wanderTimerConst;
    public float wanderRadius;

    [Header("Parents")]
    public Transform zombieParent;
    public Transform scientistParent;


    //Caches
    NavMeshAgent agent;
    float time;

    private bool doOnce = false;

    void Start()
    {
        zombieParent = GameObject.Find("Zombies").transform;
        scientistParent = GameObject.Find("Scientists").transform;
        agent = GetComponent<NavMeshAgent>();
        wanderTimer = wanderTimerConst;
    }

    void Update()
    {
        FindTargets();
        FollowTarget();
        Turning();
        if (target == null)
        {
            Wander();
        }
    }

    private void Turning()
    {
        if (target != null)
        {
        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget <= turnRadius)
            {
                time -= Time.deltaTime;
                if (time <= 0)
                {
                    TurnTarget(target);
                    target = null;
                }
            }
            else
            {
                time = timer;
            }
        }
    }
    private void FollowTarget()
    {
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
        }
    }
    private void FindTargets()
    {
        float smallestDistance = 1000;
        hitColliders = Physics.OverlapSphere(transform.position, radius, layerMaskTarget);
        foreach (var hitCollider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                target = hitCollider.gameObject;
            }
        }
    }
    private void TurnTarget(GameObject target)
    {
        time = timer;
        Instantiate(zombiePrefab, target.transform.position, target.transform.rotation, zombieParent);
        DestroyImmediate(target);
    }
    private void Wander()
    {
        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderTimerConst)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            wanderTimer = 0;
        }
    }

    public void Damage()
    {
        health -= 1;
        if (health <= 0)
        {
            if (!doOnce)
            {
                Instantiate(scientistPrefab, transform.position, transform.rotation, scientistParent);
                doOnce = true;
            }
            Destroy(gameObject);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);


        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, turnRadius);
    }
}
