using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int id;
    bool hasAdded = false;

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
    
    public AudioClip[] hitClip;

    Animator anim;
    private enum Direction { Fail, North, East, South, West };
    private bool IsAttacking = false;
    private int delay = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
        if (IsAttacking)
        {
            delay++;
            if (delay == 16)
            {
                anim.SetBool("IsAttacking", false);
                IsAttacking = false;
                delay = 0;
            }
        }

        if (enemyHealth <= 0)
        {
            if (!hasAdded)
            {
                FindObjectOfType<QuestHolder>().UpdateKillQuests(id);
                targetStats.GainExp(xpReward);
                targetStats.GainGold(goldReward);
                hasAdded = true;
            }
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
                    //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    SetAnimDir(angle);

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
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            SetAnimDir(angle);

            if (Vector2.Distance(transform.position, target.position) > stoppingDist)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, chaseSpeed * Time.deltaTime);
            }
            else //Attack the player if within range
            {
                rigBod.constraints = RigidbodyConstraints2D.FreezePosition;
                rigBod.constraints = RigidbodyConstraints2D.FreezeRotation;
                IsAttacking = true;
                anim.SetBool("IsAttacking", true);

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
                        PlayerController.instance.PlayClip(hitClip[Random.Range(0, hitClip.Length - 1)], 0.3f);
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

    void SetAnimDir(float angleIn)
    {
        if ((angleIn >= 0 && angleIn <= 45) || (angleIn >= 315 && angleIn < 360))
        {
            anim.SetInteger("Direction", 2);
        }
        else if (angleIn > 45 && angleIn < 135)
        {
            anim.SetInteger("Direction", 1);
        }
        else if (angleIn >= 135 && angleIn <= 225)
        {
            anim.SetInteger("Direction", 4);
        }
        else if (angleIn > 225 && angleIn < 315)
        {
            anim.SetInteger("Direction", 3);
        }
        else
        {
            anim.SetInteger("Direction", 3);
            //Debug.Log("findMovmentDir function failed");
        }
    }

    public void EnemyTakeDamage(float amt)
    {
        float dmg = amt - enemyDef;
        if (dmg > 0)
            enemyHealth -= dmg;
    }
}
