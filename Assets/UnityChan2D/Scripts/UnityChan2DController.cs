using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class UnityChan2DController : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float jumpPower = 1000f;
    public Vector2 backwardForce = new Vector2(-4.5f, 5.4f);

    public LayerMask whatIsGround;

    private Animator m_animator;
    private BoxCollider2D m_boxcollier2D;
    private Rigidbody2D m_rigidbody2D;
    private bool m_isGround;
    private const float m_centerY = 1.5f;

    private State m_state = State.Normal;

    void Reset()
    {
        Awake();

        // UnityChan2DController
        maxSpeed = 10f;
        jumpPower = 1000;
        backwardForce = new Vector2(-4.5f, 5.4f);
        whatIsGround = 1 << LayerMask.NameToLayer("Ground");

        // Transform
        transform.localScale = new Vector3(1, 1, 1);

        // Rigidbody2D
        m_rigidbody2D.gravityScale = 3.5f;
        m_rigidbody2D.fixedAngle = true;

        // BoxCollider2D
        m_boxcollier2D.size = new Vector2(1, 2.5f);
        m_boxcollier2D.offset = new Vector2(0, -0.25f);

        // Animator
        m_animator.applyRootMotion = false;
    }

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_boxcollier2D = GetComponent<BoxCollider2D>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (m_state != State.Damaged)
        {
            float x = Input.GetAxis("Horizontal");
            //bool jump = Input.GetButtonDown("Jump");
            //jump = Input.GetButtonDown("joystick 1 button 1");
            bool jump = false;
            //if player is not dead and is not idle state we will jump into gameplay . alive state
            if (Input.GetKeyDown(KeyCode.Mouse0) && currentState != playerStates.dead && currentState != playerStates.idle)
            {
                jump = true;
                currentState = playerStates.alive; //changing ball/player state to alive 
            }

            Move(x, jump);
        }
    }

    void Move(float move, bool jump)
    {
        //向きの時に反転しない
     if (Mathf.Abs(move) > 0)
     {
         Quaternion rot = transform.rotation;
         transform.rotation = Quaternion.Euler(rot.x, Mathf.Sign(move) == 1 ? 0 : 180, rot.z);
     }

        m_rigidbody2D.velocity = new Vector2(move * maxSpeed, m_rigidbody2D.velocity.y);

        m_animator.SetFloat("Horizontal", move);
        m_animator.SetFloat("Vertical", m_rigidbody2D.velocity.y);
        m_animator.SetBool("isGround", m_isGround);

        if (jump && m_isGround)
        {
            m_animator.SetTrigger("Jump");
            SendMessage("Jump", SendMessageOptions.DontRequireReceiver);
            m_rigidbody2D.AddForce(Vector2.up * jumpPower);
        }
    }

    void FixedUpdate()
    {
        Vector2 pos = transform.position;
        Vector2 groundCheck = new Vector2(pos.x, pos.y - (m_centerY * transform.localScale.y));
        Vector2 groundArea = new Vector2(m_boxcollier2D.size.x * 0.49f, 0.05f);

        m_isGround = Physics2D.OverlapArea(groundCheck + groundArea, groundCheck - groundArea, whatIsGround);
        m_animator.SetBool("isGround", m_isGround);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "DamageObject" && m_state == State.Normal)
        {
            m_state = State.Damaged;
            StartCoroutine(INTERNAL_OnDamage());
        }
    }

    IEnumerator INTERNAL_OnDamage()
    {
        m_animator.Play(m_isGround ? "Damage" : "AirDamage");
        m_animator.Play("Idle");

        SendMessage("OnDamage", SendMessageOptions.DontRequireReceiver);

        m_rigidbody2D.velocity = new Vector2(transform.right.x * backwardForce.x, transform.up.y * backwardForce.y);

        yield return new WaitForSeconds(.2f);

        while (m_isGround == false)
        {
            yield return new WaitForFixedUpdate();
        }
        m_animator.SetTrigger("Invincible Mode");
        m_state = State.Invincible;
    }

    void OnFinishedInvincibleMode()
    {
        m_state = State.Normal;
    }

    enum State
    {
        Normal,
        Damaged,
        Invincible,
    }
    // Use this for initialization
    public static EventHandler IncreaseScore, PlayerDead;
    public static playerStates currentState;
    public enum playerStates
    {

        idle,
        alive,
        dead
    }

    void OnTriggerEnter2D(Collider2D coll)
    {

        //if player collides with score trigger we will increment score
        if (coll.name.Contains("Score"))
        {
            if (IncreaseScore != null)
                IncreaseScore(null, null);//to fire the score increment event,uicontroller will uses this event
            coll.name = "USED"; //to avoid repeated trigger enter events
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (currentState == playerStates.dead)
            return;

        //if player collides with enemy , ie left or right white bar .
        if (coll.gameObject.tag == "Enemy")
        {
            currentState = playerStates.dead;//changing state to dead, so the camera will also track the ball falldown 
            iTween.PunchPosition(gameObject, iTween.Hash("amount", new Vector3(0.4f, 0.0f, 0), "time", 0.6f, "easetype", iTween.EaseType.easeInOutBounce));
            if (PlayerDead != null)
                PlayerDead(null, null);//to say every script that player is dead.those who every register to this event will fire their statements ,for ex uicontroller or Admanager
            SoundController.Static.PlayGameOver(); //to play sound
        }
    }

}
