using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{

    public float speed;
    private float health;
    public string dir = "right";
    public static int score = 0;
    public static bool savepoint;
    private bool bossFight = false;
    private Spriter2UnityDX.EntityRenderer sprite;
    public static bool gameover = false;
    public static bool gamefinished = false;
    private Rigidbody2D Rigidbody2D;
    private Animator animator;
    public Camera mainCamera;
    public Transform groundCheck;
    public Slider healthBar;
    public Text scoreTxt;
    public GameObject UI;
    public GameObject wall;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    [Tooltip("text untuk main story(: -> enter, ; -> clear)")]
    public string main_story = "";
    [Tooltip("text untuk story1(: -> enter, ; -> clear)")]
    public string story1 = "";
    [Tooltip("text untuk story2(: -> enter, ; -> clear)")]
    public string story2 = "";
    [Tooltip("text untuk story3(: -> enter, ; -> clear)")]
    public string story3 = "";
    [Tooltip("text untuk story4(: -> enter, ; -> clear)")]
    public string story4 = "";
    [Tooltip("text untuk story5(: -> enter, ; -> clear)")]
    public string story5 = "";
    private bool grounded;
    private int invisibilityFrame = 0;
    private int delayAttack = 0;
    private List<Vector3> checkpoint = new List<Vector3>();
    private bool move = true;
    private bool attacking = false;
    public float jumpHeight;
    private bool btnMoveL = false;
    private bool btnMoveR = false;
    private bool btnJump = false;
    private bool btnRange = false;
    public GameObject LeftBullet, RightBullet;
    Transform firePos;

    public GameObject Melee;
    bool[] story_flag = new bool[] { true, true, true, true, true };

    // Use this for initialization


    void Start()
    {
        if(savepoint && SceneManager.GetSceneByName("Scenario2").isLoaded)
        {
            gameObject.transform.position = new Vector3(106.78f, -1.51f, 0f);
            savepoint = false;
        }
        Rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<Spriter2UnityDX.EntityRenderer>();
        checkpoint.Add(new Vector3(40.76451f, -2.166512f, 0));
        checkpoint.Add(new Vector3(64.09798f, -2.13701f, 0));
        checkpoint.Add(new Vector3(23.74f, -2.56f, 0));
        checkpoint.Add(new Vector3(51.6f, -2.56f, 0));
        checkpoint.Add(new Vector3(111.8f, -2.56f, 0));
        health = 100;
        gamefinished = false;
        firePos = transform.Find("BulletPos");
        //StartCoroutine(StartMainStory());
    }
    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
    // Update is called once per frame
    void Update()
    {
        if (!gameover)
        {
            scoreTxt.text = "Score :" + score;
            healthBar.value = health;
            if (invisibilityFrame > 0)
            {
                if (invisibilityFrame <= 70)
                {
                    move = true;
                }
                if (!move) animator.Play("hit_0");
                invisibilityFrame--;
                if (invisibilityFrame == 0) gameObject.layer = 9;
                if (invisibilityFrame % 4 == 1) sprite.Color = new Color(255, 255, 255, 0);
                else sprite.Color = new Color(255, 255, 255, 255);
            }
            if (Input.anyKey && move)
            {
                if (Input.GetKeyDown(KeyCode.Z) && !attacking)
                {
                    Melee.SetActive(true);
                    attacking = true;
                    delayAttack = 30;
                }
                if (Input.GetKeyDown(KeyCode.X) || btnRange)
                {
                    animator.Play("throw_axe");
                    if (!btnRange) Fire();
                    btnRange = false;
                }
                if ((Input.GetKey(KeyCode.UpArrow) || btnJump) && grounded) Jump();
                if (Input.GetKey(KeyCode.LeftArrow) || btnMoveL) Move(-1);
                if (Input.GetKey(KeyCode.RightArrow) || btnMoveR) Move(1);
            }
            else if (attacking && invisibilityFrame <= 0) animator.Play("sword_swing_0");
            else if (grounded && invisibilityFrame <= 0) animator.Play("idle");
            else if (!grounded && invisibilityFrame <= 0) animator.Play("jump_loop");
            if (delayAttack > 0)
            {
                delayAttack--;
                if (delayAttack == 0)
                {
                    attacking = false;
                    Melee.SetActive(false);
                }
            }
            if (health <= 0)
            {
                gameover = true;
                if (SceneManager.GetSceneByName("Scenario1").isLoaded) SceneManager.LoadScene("GameOver");
                if (SceneManager.GetSceneByName("Scenario2").isLoaded) SceneManager.LoadScene("GameOverScene2");
            }
        }
        if(SceneManager.GetSceneByName("GameOver").isLoaded || SceneManager.GetSceneByName("GameOverScene2").isLoaded)
        {
            animator.Play("die_0");
        }
    }
    public void MoveBtnR(){ btnMoveR = true; }
    public void StopBtnR() { btnMoveR = false; Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y); }
    public void MoveBtnL() { btnMoveL = true; }
    public void StopBtnL() { btnMoveL = false; Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y); }
    public void JumpBtn() { btnJump = true; }
    public void StopJumpBtn() { btnJump = false; }
    public void MeleeBtn()
    {
        if (!attacking)
        {
            Melee.SetActive(true);
            attacking = true;
            delayAttack = 30;
        }
    }
    public void RangeBtn()
    {
        btnRange = true;
        Fire();
    }
    public void Move(float h)
    {
        if (h >= 0) { dir = "right"; Rigidbody2D.transform.localScale = new Vector3(0.6f, 0.6f, 1); }
        else if (h <= 0) { dir = "left"; Rigidbody2D.transform.localScale = new Vector3(-0.6f, 0.6f, 1); }
        if (attacking) animator.Play("sword_swing_0");
        else if (grounded) animator.Play("walk");
        else animator.Play("jump_loop");
        Rigidbody2D.velocity = new Vector2(h * speed, Rigidbody2D.velocity.y);
    }
    public void Jump()
    {
        animator.Play("jump_loop");
        Rigidbody2D.velocity = new Vector2(Rigidbody2D.velocity.x, jumpHeight);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            Destroy(collision.gameObject);
            savepoint = true;
        }
        if (collision.gameObject.tag == "Trigger")
        {
            Destroy(collision.gameObject);
            UI.SetActive(true);
            wall.SetActive(true);
            CameraControl.CameraLock();
            BossController.bossFight = true;
        }
        if (collision.gameObject.tag == "Mud")
        {
            speed = 3;
            jumpHeight = 9;
        }
        else
        {
            speed = 6;
            jumpHeight = 10;
        }
        if (collision.gameObject.tag == "Health")
        {
            addHealth(5);
            Destroy(collision.gameObject);
        }
        if ((collision.gameObject.tag == "Rock" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss") && invisibilityFrame <= 0)
        {
            addScore(-50);
            health -= 10;
            Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y);
            move = false;
            gameObject.layer = 17;
            invisibilityFrame = 100;
        }
        if (collision.gameObject.tag == "EnemyWeapon" && invisibilityFrame <= 0)
        {
            addScore(-50);
            health -= 10;
            Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y);
            move = false;
            invisibilityFrame = 100;
        }
        else if (collision.gameObject.tag == "EnemyWeapon")
        {
            Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y);
        }
        if (collision.gameObject.tag == "Fall")
        {
            addScore(-100);
            health -= 20;
            invisibilityFrame = 100;
            if (collision.gameObject.name == "Fallpoint1") Rigidbody2D.position = checkpoint[0];
            if (collision.gameObject.name == "Fallpoint2") Rigidbody2D.position = checkpoint[1];
            if (collision.gameObject.name == "Fallpoint3") Rigidbody2D.position = checkpoint[2];
            if (collision.gameObject.name == "Fallpoint4") Rigidbody2D.position = checkpoint[3];
            if (collision.gameObject.name == "Fallpoint5") Rigidbody2D.position = checkpoint[4];
        }
        if (collision.gameObject.tag == "Portal")
        {
            if (collision.gameObject.name == "Regular")
            {
                story_flag[4] = false;
                StartCoroutine(ShowStory(story5, "story5_content"));
                Invoke("nextScene", 13.5f);
            }
            if (collision.gameObject.name == "Regular2")
            {
                gamefinished = true;
                story_flag[2] = false;
                //StartCoroutine(ShowStory(story3, "story3_content"));
                Invoke("mainmenu", 0f);
            }
        }
        if (collision.gameObject.tag == "story1")
        {
            if (collision.gameObject.name == "Story1" && story_flag[0])
            {
                story_flag[0] = false;
                StartCoroutine(ShowStory(story1, "story1_content"));
            }
            else if (collision.gameObject.name == "Story2" && story_flag[1])
            {
                story_flag[1] = false;
                StartCoroutine(ShowStory(story2, "story2_content"));
            }
            else if (collision.gameObject.name == "Story3" && story_flag[2])
            {
                story_flag[2] = false;
                StartCoroutine(ShowStory(story3, "story3_content"));
            }
            else if (collision.gameObject.name == "Story4" && story_flag[3])
            {
                story_flag[3] = false;
                StartCoroutine(ShowStory(story4, "story4_content"));
            }
        }
    }
    private void nextScene()
    {
        SceneManager.LoadScene("Story2");
    }
    private void mainmenu()
    {
        SceneManager.LoadScene("Epilog");
    }
    IEnumerator StartMainStory()
    {
        move = false;
        string story = main_story;
        foreach (char letter in story.ToCharArray())
        {
            if (letter == ':')
            {
                GameObject.Find("main_story").GetComponent<TextMesh>().text += "\n";
            }
            else if (letter == ';')
            {
                GameObject.Find("main_story").GetComponent<TextMesh>().text = "";
            }
            else GameObject.Find("main_story").GetComponent<TextMesh>().text += letter;
            yield return new WaitForSeconds(0.1f);
        }
        GameObject.Find("main_story").GetComponent<TextMesh>().text = "";
        move = true;
    }
    IEnumerator ShowStory(string story, string textmesh_target)
    {
        move = false;
        foreach (char letter in story.ToCharArray())
        {
            if (letter == ':')
            {
                GameObject.Find(textmesh_target).GetComponent<TextMesh>().text += "\n";
            }
            else if (letter == ';')
            {
                GameObject.Find(textmesh_target).GetComponent<TextMesh>().text = "";
            }
            else GameObject.Find(textmesh_target).GetComponent<TextMesh>().text += letter;
            Debug.Log(letter+"\n");
            yield return new WaitForSeconds(0.1f);
        }
        GameObject.Find(textmesh_target).GetComponent<TextMesh>().text = "";

        move = true;
    }
    void addHealth(int value)
    {
        health += value;
    }
    public void addScore(int value)
    {
        if (score + value <= 0) score = 0;
        else score += value;
    }
    public static void reset()
    {
        score = 0;
        gameover = false;
    }
    void Fire()
    {
        if (dir=="right")
            Instantiate(RightBullet, firePos.position, Quaternion.identity);
        if (dir=="left")
            Instantiate(LeftBullet, firePos.position, Quaternion.identity);
    }
}
