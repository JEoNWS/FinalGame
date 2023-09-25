using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BomBot : Enemy
{
    Animator anim;
    Rigidbody2D rigd;

    RaycastHit2D rayHit, dashRayHit;
    public LayerMask filter;
    public GameObject bullet;

    [SerializeField]
    private float BoomRange;
    [SerializeField]
    private float bulletXOffset;
    [SerializeField]
    private float bulletYOffset;
    [SerializeField]
    float stunTime;
    [SerializeField]
    float xMin;
    [SerializeField]
    float xMax;
    [SerializeField]
    float speed;
    [SerializeField]
    GameObject lightningEffect;

    CurrentState state;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigd = GetComponent<Rigidbody2D>();
        HealthBar = transform.GetChild(0).GetChild(1).GetComponent<Image>();

        stuned = false;
        attackRange = 10.0f;
        Hp = 5;
        MaxHp = Hp;
        target = null;
        canMove = true;
        moveSpeed = speed;
    }
    void FixedUpdate()
    {
        if (stuned) return;
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, transform.right * attackRange, new Color(0, 1, 0));
        rayHit = Physics2D.Raycast(transform.position + Vector3.up * 0.1f, transform.right, attackRange, filter);
        if (target == null && rayHit)
            target = rayHit.transform;
        if (target)
        {
            if (rayHit)
            {
                anim.SetBool("InRange", true);
                if(canMove)
                    transform.position += transform.right * moveSpeed * Time.deltaTime;
            }
            else
            {
                anim.SetBool("InRange", false);
                if (canMove)
                {
                    transform.position += transform.right * moveSpeed * Time.deltaTime * 1.5f;
                    if (transform.position.x > xMax)
                        Facing(-1);
                    else if (transform.position.x < xMin)
                        Facing(1);
                }
            }
        }
        else
        {
            if (canMove)
            {
                transform.position += transform.right * moveSpeed * Time.deltaTime;
                if (transform.position.x > xMax)
                    transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                else if (transform.position.x < xMin)
                    transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            }
        }
    }
    public void Attack()
    {
        Instantiate(bullet, transform.position + new Vector3(transform.right.x * bulletXOffset, bulletYOffset, 0), transform.rotation);
    }

    public override void Hit(float rotY, float force)
    {
        anim.SetTrigger("Hit");
        rigd.AddForce(Vector3.right * rotY * force / Mathf.Abs(rotY));
        Facing(rotY);
        Hp--;
        HealthBar.fillAmount = Hp / MaxHp;
        if (Hp <= 0)
        {
            Die();
        }
    }

    protected override void Die()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
        anim.SetBool("StunEnd", false);
        rigd.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        stuned = true;
        anim.SetBool("Die", true);
    }
    protected override void Facing(float a)
    {
        if (a < 0.0f)
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        else if (a > 0.0f)
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
    public override void StateChange(int state)
    {
        this.state = (CurrentState)state;
    }
    void Boom()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, BoomRange, filter);
        if (player != null)
            Player.GetInstance().Hit();
    }
    public override void TurnAround()
    {
    }
    public override void Stun()
    {
        lightningEffect.SetActive(true);
        rigd.bodyType = RigidbodyType2D.Kinematic;
        GetComponent<Collider2D>().enabled = true;
        CancelInvoke("ReleaseStun");
        anim.SetBool("StunEnd", true);
        StopAllCoroutines();
        stuned = true;
        Invoke("ReleaseStun", stunTime);
    }

    public override void ReleaseStun()
    {
        anim.SetBool("StunEnd", false);
        stuned = false;
        lightningEffect.SetActive(false);
    }

    void WalkEnd()
    {
        Facing(target.position.x - transform.position.x);
        SetRange();
    }
    void DestroyThisObj()
    {
        Lift.deadEnemyCount++;
        Destroy(gameObject);
    }
    void CanMoveBool()
    {
        if (canMove)
            canMove = false;
        else
            canMove = true;
    }
    void SetRange()
    {
        xMin = transform.position.x - 5.0f;
        xMax = transform.position.x + 5.0f;
    }
    void CanMoveTrue()
    {
        canMove = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Player.GetInstance().playerCanHit)
            {
                anim.SetTrigger("Explode");
                rigd.bodyType = RigidbodyType2D.Static;
                GetComponent<Collider2D>().enabled = false;
                stuned = true;
            }
        }
    }
}