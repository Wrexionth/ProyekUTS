using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public int enemyHealth;
    public int pointsOnDeath;
    public GameObject Health;
    private int delay = 0;
    private Spriter2UnityDX.EntityRenderer sprite;
    public Rigidbody2D rb;
    private Character player;
    public float moveSpeed;
    public bool moveRight;

    public Transform wallCheck;
    public float wallCheckRadius;
    public LayerMask whatIswall;
    public LayerMask whatIsEdge;
    private bool hittingWall;

    private bool notAtEdge;
    public Transform edgeCheck;


    private Animator animatorEnemy;
    
    // Use this for initialization
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        animatorEnemy = GetComponent<Animator>();
        sprite = GetComponent<Spriter2UnityDX.EntityRenderer>();
        player = GameObject.Find("Player").GetComponent<Character>();
    }


    // Update is called once per frame
    void Update()
    {
        hittingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIswall);
        notAtEdge = Physics2D.OverlapCircle(edgeCheck.position, 0.2f, whatIsEdge);
        delay--;
        if(delay==0) sprite.Color = new Color(255, 255, 255, 255);
        if (hittingWall || notAtEdge)
        {
            moveRight = !moveRight;
        }
        if (enemyHealth <= 0)
        {

            if (delay % 4 == 1) sprite.Color = new Color(255, 255, 255, 0);
            else sprite.Color = new Color(255, 255, 255, 255);
            moveSpeed = 0;
            gameObject.layer = 15;
            animatorEnemy.Play("die_0");

            if (delay == 0)
            {
                player.addScore(pointsOnDeath);
                if (Random.Range(0, 4) == 0) Instantiate(Health, new Vector3(transform.position.x, transform.position.y + 0.55f, transform.position.z), gameObject.transform.rotation);
            }
            rb.velocity = new Vector2(0, 0);
            Destroy(gameObject, 1f);
        }
        else if (delay > 0)
        {
            animatorEnemy.Play("hit_0");
            rb.velocity = new Vector2(0, 0);
            if (delay % 4 == 1) sprite.Color = new Color(255, 255, 255, 0);
            else sprite.Color = new Color(255, 255, 255, 255);
        }
        else if (moveRight && delay <= 0)
        {
            animatorEnemy.Play("walk");
            transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else if(!moveRight && delay <=0)
        {
            animatorEnemy.Play("walk");
            transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Melee")
        {
            moveSpeed = 0;
            giveDamage(5);
        }
        if (collision.gameObject.tag == "Weapon" && delay < 0)
        {
            giveDamage(1);
        }
        if (collision.gameObject.tag == "Fall")
        {
            Destroy(gameObject);
        }
    }
    public void giveDamage(int DamageToGive)
    {
        enemyHealth -= DamageToGive;
        delay = 30;
    }
}
