using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Scientist : MonoBehaviour
{
    [Header("Targeting")]
    public GameObject target;
    public LayerMask layerMaskTarget;
    public float radius;
    public Collider[] hitColliders;

    [Header("Shooting")]
    public float shootingRange;
    public float distanceToTarget;
    public GameObject scientistPrefab;
    public GameObject projectile;
    public float projectileSpeed;
    public Vector3 offset;
    public float shootTimer;
    public float timerCount;


    [Header("Wander")]
    public float wanderTimer;
    public float wanderTimerConst;
    public float wanderRadius;

    [Header("Parents")]
    public Transform zombieParent;
    public Transform scientistParent;

    //Caches
    NavMeshAgent agent;

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
        Shooting();
        if (target == null)
        {
            Wander();
        }
    }

    private void Shooting()
    {
        if (target != null)
        {
            distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
            if (distanceToTarget <= shootingRange)
            {
                transform.LookAt(target.transform);
                timerCount -= Time.deltaTime;
                if (timerCount <= 0)
                {
                    var obj = Instantiate(projectile, transform.position + offset, transform.rotation);
                    obj.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed);
                    Destroy(obj, 3);
                    timerCount = shootTimer;
                }
                
                if (target.GetComponent<Zombie>().health <= 0)
                {
                    TurnTarget(target);
                    target = null;
                }
                
            }
        }
        else
        {
            target = null;
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
        Instantiate(scientistPrefab, target.transform.position, target.transform.rotation, scientistParent);
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
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
