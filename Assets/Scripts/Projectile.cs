using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;

    private float enemyDmg;
    private EnemyController enemyStats;
    private PlayerStats targetStats;
    private Transform player;
    private Vector2 target;

    //Help via Blackthornprod YouTube tutorials
    void Start()
    {
        enemyDmg = GameObject.FindGameObjectWithTag("enemy_ranged").GetComponent<EnemyController>().enemyAttack;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(player.position.x, player.position.y);
        enemyStats = GameObject.FindGameObjectWithTag("enemy_ranged").GetComponent<EnemyController>();
        targetStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        //help from inScope Studios YouTube tutorials
        Vector2 dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    //Help via Blackthornprod YouTube tutorials
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if(transform.position.x == target.x && transform.position.y == target.y)
        {
            DestroyProjectile();
        }
    }

    //Help via Blackthornprod YouTube tutorials
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            //targetStats.TakeDamage(enemyDmg);
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
