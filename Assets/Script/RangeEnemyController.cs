using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RangeEnemyController : MonoBehaviour {

    public int enemyHealth;
    public int pointsOnDeath;
    public GameObject Health;
    private int delay = 0;
    private Spriter2UnityDX.EntityRenderer sprite;
    private Character player;
    private int timerThrow = 100;
    private string dir = "left";
    private Animator animator;
    public GameObject EnemyRBullet, EnemyLBullet;
    Transform EfirePos;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        EfirePos = transform.Find("EBulletPos");
        sprite = GetComponent<Spriter2UnityDX.EntityRenderer>();
        player = GameObject.Find("Player").GetComponent<Character>();
    }
	
	// Update is called once per frame
	void Update () {
        if (delay <= 0) timerThrow--;
        if (delay == 0) sprite.Color = new Color(255, 255, 255, 255);
        delay--;
        if (enemyHealth <= 0)
        {
            gameObject.layer = 15;
            if(!SceneManager.GetSceneByName("Scenario3").isLoaded) animator.Play("die_0");
            if (delay % 4 == 1) sprite.Color = new Color(255, 255, 255, 0);
            else sprite.Color = new Color(255, 255, 255, 255);
            if (delay == 0)
            {
                player.addScore(pointsOnDeath);
                if (Random.Range(0, 2) == 0 && !SceneManager.GetSceneByName("Scenario3").isLoaded) Instantiate(Health, new Vector3(transform.position.x, transform.position.y + 0.55f, transform.position.z), gameObject.transform.rotation);
                else Invoke("DelayActivate", 0f);
            }
            if (!SceneManager.GetSceneByName("Scenario3").isLoaded) Destroy(gameObject, 1f);
        }
        else if (delay > 0)
        {
            animator.Play("hit_0");
            if (delay % 4 == 1) sprite.Color = new Color(255, 255, 255, 0);
            else sprite.Color = new Color(255, 255, 255, 255);
        }
        else if (timerThrow == 0)
        {
            timerThrow = Random.Range(60, 100);
            animator.Play("throw_axe");
            Fire();
        }
        else if (timerThrow < 60 && enemyHealth > 0) animator.Play("idle");
    }
    void Fire()
    {
        if (dir == "right")
            Instantiate(EnemyRBullet, EfirePos.position, Quaternion.identity);
        if (dir == "left")
            Instantiate(EnemyLBullet, EfirePos.position, Quaternion.identity);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Melee")
        {
            giveDamage(5);
        }
        if (collision.gameObject.tag == "Weapon" && delay < 0)
        {
            giveDamage(1);
        }
    }
    public void giveDamage(int DamageToGive)
    {
        enemyHealth -= DamageToGive;
        delay = 30;
    }
    void DelayActivate()
    {
        enemyHealth = 5;
        Instantiate(Health, new Vector3(transform.position.x, transform.position.y + 0.55f, transform.position.z), gameObject.transform.rotation);
        delay = 0;
        timerThrow = 100;
        gameObject.layer = 10;
        gameObject.SetActive(false);
    }
}
