using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpiderBot : Enemy
{
    Animator anim;
    Rigidbody2D rigd;

    RaycastHit2D rayHit;
    public LayerMask playerFilter;
    public GameObject bullet;
    [SerializeField]
    private float stunTime;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float xMin;
    [SerializeField]
    private float xMax;
    [SerializeField]
    private float bulletXOffset;
    [SerializeField]
    private float bulletYOffset;
    [SerializeField]
    GameObject lightningEffect;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigd = GetComponent<Rigidbody2D>();
        HealthBar = transform.GetChild(0).GetChild(1).GetComponent<Image>();

        attackRange = 10.0f;
        Hp = 5;
        MaxHp = Hp;
        stuned = false;
        canMove = false;
        moveSpeed = speed;
    }
    void FixedUpdate()
    {
        if (stuned) return;
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, transform.right * attackRange, new Color(0, 1, 0));
        rayHit = Physics2D.Raycast(transform.position + Vector3.up * 0.1f, transform.right, attackRange, playerFilter);
        if (rayHit)
        {
            anim.SetBool("InRange", true);
            target = rayHit.transform;
        }
        else
        {
            anim.SetBool("InRange", false);
        }
        if(canMove)
        {
            transform.position += transform.right * moveSpeed * Time.deltaTime;
            if (target)
            {
                Facing(target.position.x - transform.position.x);
            }
            else
            {
                if (transform.position.x > xMax)
                    Facing(-1);
                else if (transform.position.x < xMin)
                    Facing(1);
            }
        }
    }
    public void Attack()
    {
        Instantiate(bullet, transform.position + new Vector3(transform.right.x * bulletXOffset, transform.up.y * bulletYOffset, 0), transform.rotation);
    }

    public override void Hit(float rotY, float force)
    {
        canMove = false;
        anim.SetTrigger("Hit");
        rigd.AddForce(Vector3.right * rotY * force / Mathf.Abs(rotY) * -1.0f);
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
        anim.SetBool("Die", true);
        GetComponent<Collider2D>().enabled = false;
        rigd.bodyType = RigidbodyType2D.Static;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
        Lift.deadEnemyCount++;
        Destroy(this);
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
    }

    public override void TurnAround()
    {
    }

    public override void Stun()
    {
        lightningEffect.SetActive(true);
        CancelInvoke("ReleaseStun");
        anim.SetBool("StunEnd", true);
        StopAllCoroutines();
        stuned = true;
        Invoke("ReleaseStun", stunTime);
    }

    public override void ReleaseStun()
    {
        lightningEffect.SetActive(false);
        anim.SetBool("StunEnd", false);
        stuned = false;
    }
    void CanMoveBool()
    {
        if (canMove)
            canMove = false;
        else
            canMove = true;
    }
}