using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour {

    private Animator animatorEnemy;
    private Spriter2UnityDX.EntityRenderer sprite;
    private Rigidbody2D rb;
    public Slider enemyHealthBar;
    //public string animateText;
    private Character player;
    public GameObject rockLayout;
    public GameObject[] enemySpawner;
    public int enemyHealth;
    public int interval;
    public float moveSpeed;
    private int delay = 0;
    private int r;
    public string state;
    public string dir;
    public static bool bossFight = false;
    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        animatorEnemy = GetComponent<Animator>();
        sprite = GetComponent<Spriter2UnityDX.EntityRenderer>();
        player = GameObject.Find("Player").GetComponent<Character>();
        interval = 250;
        enemyHealth = 100;
        animatorEnemy.Play("1_IDLE");
    }
	
	// Update is called once per frame
	void Update ()
    {
        enemyHealthBar.value = enemyHealth;
        if (bossFight)
        {
            if (interval == 0)
            {
                if (r == 0)
                {
                    state = "Moving";
                    interval = Random.Range(150,250);
                    rockLayout.SetActive(false);
                    rockLayout.transform.position = new Vector3(rockLayout.transform.position.x, 3);
                    if (enemyHealth >= 76) r = Random.Range(1, 2);
                    else if (enemyHealth >= 50) r = Random.Range(1, 3);
                    else r = Random.Range(1, 4);
                }
                else if (r == 1)
                {
                    state = "LightAttack";
                    interval = 100;
                    r = 0;
                }
                else if (r == 2)
                {
                    state = "HeavyAttack";
                    interval = 100;
                    r = 0;
                }
                else if (r == 3)
                {

                    state = "SummonAttack";
                    interval = 100;
                    r = 0;
                }
            }
            interval--;
            if (state == "Moving")
            {
                animatorEnemy.Play("1_WALK");
                Move();
            }
            else if (state == "LightAttack")
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                animatorEnemy.Play("1_ATTACK");
            }
            else if (state == "HeavyAttack")
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                animatorEnemy.Play("1_ATTACK_2");
                rockLayout.SetActive(true);
            }
            else if (state == "SummonAttack")
            {
                if (interval == 0)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (!enemySpawner[i].activeInHierarchy) enemySpawner[i].SetActive(true);
                    }
                }
                rb.velocity = new Vector2(0, rb.velocity.y);
                animatorEnemy.Play("1_ATTACK_3");
            }
            else if (state == "idle")
            {
                if (interval == 100) { dir = "left"; rb.transform.localScale = new Vector3(-0.8f, 0.8f, 1); }
                animatorEnemy.Play("1_IDLE");
                enemyHealth = 100;
            }
        }
    }
    public void Move()
    {
        if (rb.transform.position.x <= 26.25f) { dir = "right"; rb.transform.localScale = new Vector3(0.8f, 0.8f, 1); }
        else if (rb.transform.position.x >= 45.4f) { dir = "left"; rb.transform.localScale = new Vector3(-0.8f, 0.8f, 1);}
        if (dir == "right") gameObject.transform.position = new Vector3(gameObject.transform.position.x + 0.1f, gameObject.transform.position.y);
        else if (dir == "left") gameObject.transform.position = new Vector3(gameObject.transform.position.x - 0.1f, gameObject.transform.position.y);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Melee")
        {
            GiveDamage(5);
        }
        if (collision.gameObject.tag == "Weapon")
        {
            GiveDamage(1);
        }
    }
    public void GiveDamage(int DamageToGive)
    {
        if (bossFight) enemyHealth -= DamageToGive;
    }
}
