using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    enum EnemyState {Patrol, ChaseAttack};
    EnemyState enemyAI;

    public float enemyHealth = 25f;
    public float enemyAttack = 10f;
    public float enemyDef = 3f;
    public float xpReward = 15f;
    public float goldReward = 10f;
    public float chaseSpeed;
    public float patrolSpeed;
    public float stoppingDist;
    public float chaseDist;
    public float timeBetweenAttacks;
    public float startPatrolDelay;         //same as waitTime but for manipulation purposes in the inspector

    public Transform[] patrolSpots;
    public GameObject projectile;

    private Rigidbody2D rigBod;
    private int randomSpot;
    private float patrolDelay;         //time to wait at each patrol spot
    private float attackCooldown;

    private Transform target;
    private PlayerStats targetStats;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        rigBod = GetComponent<Rigidbody2D>();
        enemyAI = EnemyState.Patrol;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        targetStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        attackCooldown = timeBetweenAttacks;
        randomSpot = Random.Range(0, patrolSpots.Length);
        patrolDelay = startPatrolDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyHealth <= 0)
        {
            targetStats.GainExp(xpReward);
            targetStats.GainGold(goldReward);
            Destroy(gameObject);

            playerController.target = GameObject.FindGameObjectWithTag("holder").GetComponent<EnemyController>();
        }

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }

        if (enemyAI == EnemyState.Patrol && this.tag != "holder")
        {
            //With help from Blackthornprod YouTube tutorials
            transform.position = Vector2.MoveTowards(transform.position, patrolSpots[randomSpot].position, patrolSpeed * Time.deltaTime);

            if(Vector2.Distance(transform.position, patrolSpots[randomSpot].position) < 0.2f)
            {
                if(patrolDelay <= 0)
                {
                    randomSpot = Random.Range(0, patrolSpots.Length);

                    Vector2 dir = patrolSpots[randomSpot].transform.position - transform.position;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    patrolDelay = startPatrolDelay;
                }
                else
                {
                    patrolDelay -= Time.deltaTime;
                }
            }
        }
        else if(enemyAI == EnemyState.ChaseAttack && this.tag != "holder")
        {
            //With help from Blackthornprod YouTube tutorials
            //Chase the player

            Vector2 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (Vector2.Distance(transform.position, target.position) > stoppingDist)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);
            }
            else //Attack the player if within range
            {
                rigBod.constraints = RigidbodyConstraints2D.FreezePosition;

                if (attackCooldown <= 0)
                {
                    if(this.tag == "enemy_ranged")
                    {
                        Quaternion enemyRotation = this.transform.rotation;
                        Instantiate(projectile, transform.position, Quaternion.identity);
                        attackCooldown = timeBetweenAttacks;
                    }
                    else
                    {
                        targetStats.TakeDamage(enemyAttack);
                        attackCooldown = timeBetweenAttacks;
                    }
                }
            }
        }

        if(Vector2.Distance(transform.position, target.position) <= chaseDist)
        {
            enemyAI = EnemyState.ChaseAttack;
        }
        else
        {
            enemyAI = EnemyState.Patrol;
        }
    }

    public void EnemyTakeDamage(float amt)
    {
        float dmg = amt - enemyDef;
        if (dmg > 0)
            enemyHealth -= dmg;
    }
}
