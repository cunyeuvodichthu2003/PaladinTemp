using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("KnockBack Info")]
    [SerializeField] protected Vector2 knockBackDir;
    [SerializeField] protected float knockBackDuration;
    protected bool isKnocked;

    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 0f;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = 0f;
    [SerializeField] protected LayerMask whatIsGround;
   

    #region Component
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    #endregion

    public int facingDir { get; private set; } = 1;
    protected bool facinRight = true;
     protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        fx = GetComponent<EntityFX>();
       
    }
    protected virtual void Update()
    {
       
    }
    public virtual void Damage()
    {
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockBack");
    }
    protected virtual IEnumerator HitKnockBack()
    {
        if (!anim.GetBool("Stunned"))

        {
            isKnocked = true;
            rb.velocity = new Vector2(knockBackDir.x * -facingDir, knockBackDir.y);
            yield return new WaitForSeconds(knockBackDuration);
            isKnocked = false;
        }
    }
    #region Collision
    public virtual bool IsGroundDetected()

    => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected()
    => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    #endregion
    #region FLip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facinRight = !facinRight;
        transform.Rotate(0, 180, 0);
    }
    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facinRight)
            Flip();
        else if (_x < 0 && facinRight)
            Flip();
    }
    #endregion
    #region Velocity
    public virtual void SetVelocity(float _xVelocity, float _yVelocity)
    {   if (isKnocked)
            return;
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    public virtual void SetZeroVelocity()
    {   if (isKnocked)
            return;
        rb.velocity = Vector2.zero; }
    #endregion 
}
