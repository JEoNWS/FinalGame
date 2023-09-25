using UnityEngine;

public class RangeBully : Enemy
{
    [SerializeField]
    LayerMask filter;
    [SerializeField]
    float stunTime;
    RaycastHit2D rayHit;

    public GameObject bullet;

    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        stuned = false;
    }

    void FixedUpdate()
    {
        if (stuned) return;
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, transform.right * attackRange, new Color(0, 1, 0));
        rayHit = Physics2D.Raycast(transform.position + Vector3.up * 0.1f, transform.right, attackRange, filter);
        if (rayHit)
        {
            anim.SetBool("InRange", true);
        }
        else
        {
            anim.SetBool("InRange", false);
        }
    }
    void Shoot()
    {
        Instantiate(bullet, transform.position + new Vector3(transform.right.x * 2.5f, 1.75f, 0), transform.rotation);
    }

    public override void Hit(float rotY, float force)
    {
        Die();
    }

    protected override void Die()
    {
        GetComponent<Animator>().SetTrigger("Die");
        GetComponent<Collider2D>().enabled = false;
        Destroy(this);
    }
    protected override void Facing(float a)
    {
    }
    public override void StateChange(int state)
    {
    }
    public override void TurnAround()
    {
    }
    public override void Stun()
    {
        StopAllCoroutines();
        stuned = true;
        Invoke("ReleaseStun", stunTime);
    }

    public override void ReleaseStun()
    {
        stuned = false;
    }
}
