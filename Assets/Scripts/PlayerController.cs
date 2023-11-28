using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour{
    [SerializeField]private Rigidbody2D rb;
    public float speed = 5f;
    public float jumpForce = 6f;
    public Animator anim;
    private int jumpCount = 0;

    public Collider2D PlayerColl;
    public Collider2D CloseColl;

    public UIDocument ui;
    private Label CherryLabel;
    private Label GemLabel;
    private Label EnemyLabel;
    private Label SuccessLabel;
    private Label FailLabel;
    private VisualElement LevelUpDialog;

    private int CherryCount = 0;
    private int GemCount = 0;
    private int EnemyCount = 0;

    private bool isHurt = false;
    private int healthy = 10;

    private bool ReachEnd = false;
    private bool Failed = false;

    public AudioSource JumpAudio;
    public AudioSource HurtAudio;
    public AudioSource CherryAudio;
    public AudioSource GemAudio;

    private Vector2 InitPostion;

    private void Awake() {
        PlayerColl = GetComponent<CircleCollider2D>();

        var root = ui.rootVisualElement;

        CherryLabel = root.Q<Label>("CherryCount");
        GemLabel = root.Q<Label>("GemCount");
        EnemyLabel = root.Q<Label>("EnemyCount");

        SuccessLabel = root.Q<Label>("SuccessLabel");
        FailLabel = root.Q<Label>("FailLabel");

        LevelUpDialog = root.Q<VisualElement>("LevelUpDialog");
    }

    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InitPostion = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Failed) {
            if (!isHurt) {
                Move();
            }
        }
        if (ReachEnd) {
            Interact();
        }
    }

    void Recover() {
        anim.SetBool("isHurt", false);
        isHurt = false;
    }

    void Interact() {
        if (Input.GetButtonDown("Interact")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            ReachEnd = false;
            InitPostion = transform.position;
        }
    }

    void Crouch() {
        if (Input.GetButton("Crouch") && PlayerColl.IsTouchingLayers(LayerMask.GetMask("Platform"))) {
            anim.SetBool("isCrouching", true);
            CloseColl.enabled = false;
        } else {
            if (!Physics2D.OverlapCircle(transform.position, 0.3f, LayerMask.GetMask("Platform"))) {
                anim.SetBool("isCrouching", false);
                CloseColl.enabled = true;
            }
        }
    }
    

    void Move() {
        Crouch();
        float horizontalMove = Input.GetAxis("Horizontal");
        float faceDirection = Input.GetAxisRaw("Horizontal");
        if (horizontalMove != 0 && !anim.GetBool("isCrouching")) {
            anim.SetBool("isRunning", true);
        } else {
            anim.SetBool("isRunning", false);
        }
        rb.velocity = new Vector2(speed * horizontalMove, rb.velocity.y);
        if (faceDirection != 0) {
            transform.localScale = new Vector3(faceDirection, 1, 1);
        }
        if (Input.GetButtonDown("Jump") && jumpCount < 2) {
            rb.velocity = Vector2.up * jumpForce;
            JumpAudio.Play();
            anim.SetBool("isJumpping", true);
            jumpCount += 1;
        }
        if (!anim.GetBool("isJumpping") && PlayerColl.IsTouchingLayers(LayerMask.GetMask("Platform"))) {
            jumpCount = 0;
        }
        // 下落
        if (rb.velocity.y < 0) {
            anim.SetBool("isJumpping", false);
            anim.SetBool("isFalling", true);
            
        } else {
            anim.SetBool("isFalling", false);
        }
        
    }

    void Restart() {
        Debug.Log("Restart");
        if (transform.position.y < -20f) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Cherry") {
            CherryAudio.Play();
            CherryCount += 1;
            CherryLabel.text = "Cherry: " + CherryCount.ToString();
            Destroy(other.gameObject);
        }
        if (other.tag == "Gem") {
            GemAudio.Play();
            GemCount += 1;
            GemLabel.text = "Gem: " + GemCount.ToString();
            Destroy(other.gameObject);
        }
        if (other.tag == "End") {
            showResult(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Enemies") {
            if (anim.GetBool("isFalling")) {
                // Hit the enemy
                EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
                enemy.JumpOn();
                EnemyCount += 1;
                EnemyLabel.text = "Enemy: " + EnemyCount.ToString();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                anim.SetBool("isJumpping", true);
                jumpCount += 1;
            } else {
                HurtAudio.Play();
                anim.SetBool("isHurt", true);
                healthy -= 1;
                isHurt = true;
                if (healthy == 0) {
                    showResult(false);
                }
                if (transform.position.x < other.gameObject.transform.position.x) {
                    rb.velocity = new Vector2(-5, rb.velocity.y);
                } else {
                    rb.velocity = new Vector2(5, rb.velocity.y);
                }
            }
        }
    }
    void showResult(bool success) {
        if (success) {
            ReachEnd = true;
            LevelUpDialog.style.display = DisplayStyle.Flex;
            // SuccessLabel.style.display = DisplayStyle.Flex;
            // FailLabel.style.display = DisplayStyle.None;
        } else {
            Failed = true;
            SuccessLabel.style.display = DisplayStyle.None;
            FailLabel.style.display = DisplayStyle.Flex;
        }
    }
}
