using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float returnSpeed;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;
    [Header("Bounce Info")]
    [SerializeField] private float bounceSpeed = 0f;
    private bool isBouncing;
    private int amountOfBounce;
    [Header("Pierce Info")]
    private bool isPiercing;
    private int pierceAmount;
    [Header("Spin Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;
    private float hitTimer;
    private float hitCoolDown;
    [Header("Freeze Info")]
    [SerializeField] float freezeTime = 1f;

    public List<Transform> enemyTarget;
    private int targetIndex;

    private void OnEnable()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }
    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        player = _player;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        if (pierceAmount <= 0)
            anim.SetBool("Rotation", true);
    }
    public void SetUpBounce(bool _isBouncing, int _amountOfBounce)
    {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounce;
        enemyTarget = new List<Transform>();
    }
    public void SetUpPierce(bool _isPiercing, int _pierceAmount)
    {
        isPiercing = _isPiercing;
        pierceAmount = _pierceAmount;

    }
    public void SetUpSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCoolDown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCoolDown = _hitCoolDown;

    }
    public void ReturnSword()
    {

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        /*rb.isKinematic = false;*/
        transform.parent = null;
        isReturning = true;
    }



    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed);
            if (Vector2.Distance(transform.position, player.transform.position) < 0.5)
                player.CatchSword();

        }
        BounceLogic();

        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhileSpinning();
            }
            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }
            }
            hitTimer -= Time.deltaTime;
            if (hitTimer < 0)
            {
                hitTimer = hitCoolDown;
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        hit.GetComponent<Enemy>().Damage();
                }
            }

        }
    }

    private void StopWhileSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1)
            {
                enemyTarget[targetIndex].GetComponent<Enemy>().Damage();
                targetIndex++;
                amountOfBounce--;
                if (amountOfBounce <= 0)

                {
                    isBouncing = false;
                    isReturning = true;
                }
                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;
        
        if (collision.GetComponent<Enemy>()!=null )
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.Damage();
            StartCoroutine(enemy.FreezeTimeFor(freezeTime));
        }
        SetUpBounceInfo(collision);
        StuckInto(collision);
    }

    private void SetUpBounceInfo(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
        else if(isBouncing)
        {
            isReturning = true;
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }
        if (isSpinning)
        {
            StopWhileSpinning();
            return;
        }
        canRotate = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count >= 0)
            return;
        cd.enabled = false;
        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;


    }
}

  
