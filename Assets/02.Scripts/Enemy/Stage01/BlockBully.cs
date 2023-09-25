using UnityEngine;

public class BlockBully : Enemy
{
    [SerializeField]
    LayerMask filter;
    [SerializeField]
    float stunTime;
    RaycastHit2D rayHit;

    [SerializeField]
    GameObject Wall;

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
            anim.SetTrigger("Block");
            Wall.SetActive(true);
        }
    }

    public override void Hit(float rotY, float force)
    {
        anim.SetTrigger("Hit");
        Die();
    }

    protected override void Die()
    {
        Destroy(Wall);
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
