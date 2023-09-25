using UnityEngine;
using UnityEngine.UI;

public class CloseBully : Enemy
{
    Animator anim;
    Rigidbody2D rigd;

    RaycastHit2D rayHit;
    public LayerMask playerFilter;

    [SerializeField]
    float stunTime;
    [SerializeField]
    float speed;
    [SerializeField]
    GameObject lightningEffect;
    [SerializeField]
    Stage1Door[] door;
    [SerializeField]
    bool isSpawned;
    CurrentState state;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigd = GetComponent<Rigidbody2D>();
        HealthBar = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        stuned = false;
        Hp = 5;
        MaxHp = Hp;
        canMove = false;
        moveSpeed = speed;
        if (isSpawned)
        {
            target = Player.GetInstance().transform;
            anim.SetBool("Spawned", true);
        }
        else
        {
            target = null;
            anim.SetBool("Spawned", false);
        }
    }
    void FixedUpdate()
    {
        if (stuned) return;
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, transform.right * attackRange, new Color(0, 1, 0));
        rayHit = Physics2D.Raycast(transform.position + Vector3.up * 0.1f, transform.right, attackRange, playerFilter);
        if (target == null && rayHit)
        {
            target = rayHit.transform;
            anim.SetBool("Spawned", true);
        }
        if (target)
        {
            if (rayHit)
            {
                anim.SetBool("InRange", true);
                rigd.velocity = Vector3.zero;
            }
            else
            {
                anim.SetBool("InRange", false);
                if (canMove)
                    rigd.velocity = new Vector3(speed * transform.right.x, rigd.velocity.y, 0.0f);
                else
                    rigd.velocity = Vector3.zero;
            }
        }
        else
        {

        }
    }
    public void Attack()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up * 0.1f, transform.right, attackRange, playerFilter);
        if (hit)
        {
            Player.GetInstance().Hit();
        }
    }

    public override void Hit(float rotY, float force)
    {
        anim.SetTrigger("Hit");
        rigd.AddForce(Vector3.right * rotY * force / Mathf.Abs(rotY) * -1.0f);
        Facing(rotY);
        Hp--;
        HealthBar.fillAmount = Hp / MaxHp;
        if (target == null)
        {
            target = Player.GetInstance().transform;
            anim.SetBool("Spawned", true);
            DoorOpen();
        }
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
        anim.SetBool("StunEnd", false);
        stuned = false;
        lightningEffect.SetActive(false);
    }

    void CanMoveBool(int a)
    {
        if (a == 0)
        {
            canMove = false;
            Facing(target.transform.position.x - transform.position.x);
        }
        else
            canMove = true;
    }

    void AttackEndAndStart()
    {
        float distance = Player.GetInstance().transform.position.x - transform.position.x;
        Facing(distance);
    }
    void DoorOpen()
    {
        for (int i = 0; i < door.Length; i++)
            door[i].Spotted();
    }
}