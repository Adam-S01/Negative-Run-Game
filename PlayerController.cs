using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Collider2D col;
    public Animator anim;
    public float speed;
    public float jumpForce;
    public float moveInput;
    public float moveInput2;
    private bool isAlive = true;


    public bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    public bool canDash;
    public bool isDashing;
    public int direction;
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;

    [Range(0f,5f)]
    public float BumpMultiplier;
    public float SquishDuration;
    [Range(0.9f,0.5f)]
    public float SquishValue;

    public GameObject arrow;

    public float dashCoolDown;
    public float startDashCoolDown;
    public bool dashGroundReady;

    public ParticleSystem ground;
    public ParticleSystem dash;
    public ParticleSystem run;

    public GameObject blast;
    public GameObject blast2;
    public GameObject blast3;
    public GameObject blast4;
    public GameObject contact;

    public ParticleSystem.MinMaxCurve mmc;

    [SerializeField] private int playerID = 0;

    [SerializeField] private Player playerC;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        playerC = ReInput.players.GetPlayer(playerID);
        dashTime = startDashTime;


        var emission = run.emission;
        emission.rateOverTime = mmc;
    }

    void Update()
    {
        //Move
        moveInput = playerC.GetAxis("MoveH");
        moveInput2 = playerC.GetAxis("MoveV");

        anim.SetFloat("Speed", Mathf.Abs(moveInput));

        if(moveInput != 0 && isGrounded){
            mmc.constant = 50;
            var emission = run.emission;
            emission.rateOverTime = mmc;
            
        }else{
            mmc.constant = 0;
            var emission = run.emission;
            emission.rateOverTime = mmc;
        }

        if(moveInput < 0){
             rb.velocity = new Vector2(-speed, rb.velocity.y);
             transform.eulerAngles = new Vector3(0, 180, 0);
        } else if(moveInput > 0){
             rb.velocity = new Vector2(speed, rb.velocity.y);
             transform.eulerAngles = new Vector3(0, 0, 0);
        } else if(moveInput == 0){
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        //Jump
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if(!wasGrounded && isGrounded){
            ground.Play();
        }

        if(isGrounded){
        if(playerC.GetButtonDown("Jump")){
            anim.SetTrigger("jump");
            AudioManager.instance.Play("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);       
        }
            dashGroundReady = true;
            anim.SetBool("floor", true);
        }else{
            anim.SetBool("floor", false);
        }

        //dash

        if(direction == 0){
            if(playerC.GetButtonDown("Dash")){
                if(canDash){
            if(moveInput == 1 && moveInput2 < 0.5f && moveInput2 > -0.5f){//right
                direction = 1;
            }else if(moveInput > 0.5f && moveInput2 > 0.5f){//right up
                direction = 2;
            }else if(moveInput < 0.5f && moveInput > -0.5f && moveInput2 == 1){// up
                direction = 3;
            }else if(moveInput < -0.5f && moveInput2 > 0.5f){//left up
                direction = 4;
            }else if(moveInput == -1 && moveInput2 < 0.5f && moveInput2 > -0.5f){//left 
                direction = 5;
            }else if(moveInput < -0.5f && moveInput2 < -0.5f){//left down
                direction = 6;
            }else if(moveInput > -0.5f && moveInput < 0.5f && moveInput2 == -1){// down
                direction = 7;
            }else if(moveInput > 0.5f && moveInput2 < 0){// right down
                direction = 8;
            }else if (moveInput == 0 && moveInput2 == 2){
                dashCoolDown = startDashCoolDown;
                canDash = true;
            }
            canDash = false;
            dashGroundReady = false;
            isDashing = true;
            anim.SetTrigger("Dash");
            dash.Play();
            AudioManager.instance.Play("Dash");
            }
            }
        }else{
            if(dashTime <=0){
                direction =0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
                isDashing = false;
            }else{
                dashTime -= Time.deltaTime;

                if(direction == 1){
                    rb.velocity = Vector2.right * dashSpeed;
                }
                if(direction == 2){
                    rb.velocity = new Vector2(dashSpeed, dashSpeed);
                }
                if(direction == 3){
                    rb.velocity = Vector2.up * dashSpeed;
                }
                if(direction == 4){
                    rb.velocity = new Vector2(-dashSpeed, dashSpeed);
                }
                if(direction == 5){
                    rb.velocity = Vector2.right * -dashSpeed;
                }
                if(direction == 6){
                    rb.velocity = new Vector2(-dashSpeed, -dashSpeed);
                }
                if(direction == 7){
                    rb.velocity = Vector2.up * -dashSpeed;
                }
                if(direction == 8){
                    rb.velocity = new Vector2(dashSpeed, -dashSpeed);
                }
            }
        }

        if(!canDash){
            dashCoolDown -= Time.deltaTime;
        }
        if(dashCoolDown <= 0 && dashGroundReady){
            dashCoolDown = startDashCoolDown;
            canDash = true;
        }
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public void NoLongerDead()
    {
        isAlive = true;
    }


    void OnTriggerEnter2D(Collider2D truc){
        if (truc.gameObject.tag == "DeathRight"){  
            col.isTrigger = true;
            Instantiate(blast, transform.position, Quaternion.identity);
            transform.position = new Vector3(-10,-10,-10);
            isAlive = false;
            GameManager.instance.RemoveFromTheLiving(this.gameObject);
            GameManager.instance.CheckForWinner();
            if (gameObject == GameManager.instance.WhoIsCursed())
            {
                GameManager.instance.PickNewRandomPlayer();
                if(GameManager.instance.IsReadyToPunish())
                    GameManager.instance.ResetTimer();
            }
            AudioManager.instance.Play("Death");
        }

        if (truc.gameObject.tag == "DeathUp"){  
            col.isTrigger = true;
            Instantiate(blast2, transform.position, Quaternion.identity);
            transform.position = new Vector3(-10,-10,-10);
            isAlive = false;
            GameManager.instance.RemoveFromTheLiving(this.gameObject);
            GameManager.instance.CheckForWinner();
            if (gameObject == GameManager.instance.WhoIsCursed())
            {
                GameManager.instance.PickNewRandomPlayer();
                if(GameManager.instance.IsReadyToPunish())
                    GameManager.instance.ResetTimer();
            }
            AudioManager.instance.Play("Death");
        }

        if (truc.gameObject.tag == "DeathLeft"){  
            col.isTrigger = true;
            Instantiate(blast3, transform.position, Quaternion.identity);
            transform.position = new Vector3(-10,-10,-10);
            isAlive = false;
            GameManager.instance.RemoveFromTheLiving(this.gameObject);
            GameManager.instance.CheckForWinner();
            if (gameObject == GameManager.instance.WhoIsCursed())
            {
                GameManager.instance.PickNewRandomPlayer();
                if(GameManager.instance.IsReadyToPunish())
                    GameManager.instance.ResetTimer();
            }
            AudioManager.instance.Play("Death");
            
        }

        if (truc.gameObject.tag == "DeathDown"){  
            col.isTrigger = true;
            Instantiate(blast4, transform.position, Quaternion.identity);
            transform.position = new Vector3(-10,-10,-10);
            isAlive = false;
            GameManager.instance.RemoveFromTheLiving(this.gameObject);
            GameManager.instance.CheckForWinner();
            if (gameObject == GameManager.instance.WhoIsCursed())
            {
                GameManager.instance.PickNewRandomPlayer();
                if(GameManager.instance.IsReadyToPunish())
                    GameManager.instance.ResetTimer();
            }
            AudioManager.instance.Play("Death");
        }
    }
    void OnColliderEnter2D(Collider2D truc){
        if (truc.gameObject.tag == "Player"){  
            AudioManager.instance.Play("Slime_Choc");
            Rigidbody2D RBPlayer = gameObject.GetComponent<Rigidbody2D>();
            Rigidbody2D RBTarget = truc.gameObject.GetComponent<Rigidbody2D>();
            Vector2 VelocityPlayer = RBPlayer.velocity;
            Vector2 VelocityTarget = RBTarget.velocity;
            float SpeedPlayer = VelocityPlayer.x * VelocityPlayer.y;
            float SpeedTarget = VelocityTarget.x * VelocityPlayer.y;
            if(SpeedPlayer > SpeedTarget){
                RBTarget.AddForce((VelocityPlayer - VelocityTarget) * BumpMultiplier);
            }
            Instantiate(contact, transform.position, Quaternion.identity);
            RBPlayer.transform.localScale = new Vector3(Mathf.Lerp(1f, SquishValue, SquishDuration/2f), Mathf.Lerp(1f, SquishValue, SquishDuration/2f), Mathf.Lerp(1f, SquishValue, SquishDuration/2f));
            RBTarget.transform.localScale = new Vector3(Mathf.Lerp(1f, SquishValue, SquishDuration/2f), Mathf.Lerp(1f, SquishValue, SquishDuration/2f), Mathf.Lerp(1f, SquishValue, SquishDuration/2f));
            if(gameObject == GameManager.instance.WhoIsCursed() && GameManager.instance.IsReadyToPunish())
            {
                GameManager.instance.IGuessImCursed(truc.gameObject);
            }
        }
    }

}
