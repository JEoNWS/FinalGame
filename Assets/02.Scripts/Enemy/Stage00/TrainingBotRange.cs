using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TrainingBotRange : Enemy
{
    Animator anim;
    Rigidbody2D rigd;

    RaycastHit2D rayHit, dashRayHit;
    public LayerMask filter;
    public GameObject bullet;
    [SerializeField]
    private float dashRange;
    [SerializeField]
    private float stunTime;
    bool canDash;
    [SerializeField]
    GameObject lightningEffect;
    CurrentState state;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigd = GetComponent<Rigidbody2D>();
        HealthBar = transform.GetChild(0).GetChild(1).GetComponent<Image>();

        attackRange = 10.0f;
        Hp = 5;
        MaxHp = Hp;
        canDash = true;
        stuned = false;
    }
    void FixedUpdate()
    {
        if (stuned) return;
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, transform.right * attackRange, new Color(0, 1, 0));
        rayHit = Physics2D.Raycast(transform.position + Vector3.up * 0.1f, transform.right, attackRange, filter);
        dashRayHit = Physics2D.Raycast(transform.position + Vector3.up * 0.1f, transform.right, dashRange, filter);
        if (rayHit)
        {
            anim.SetBool("InRange", true);
            if(dashRayHit)
            {
                if (canDash)
                {
                    anim.SetBool("InDashRange", true);
                }
            }
            else
            {
                anim.SetBool("InDashRange", false);
            }
        }
        else
        {
            anim.SetBool("InRange", false);
        }
    }
    public void Attack()
    {
        Instantiate(bullet, transform.position + new Vector3(transform.right.x * 1.25f, 1.1f, 0), transform.rotation);
    }

    public override void Hit(float rotY, float force)
    {
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
        Door.deadEnemyCount++;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
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
        this.state = (CurrentState)state;
    }

    public void Dash()
    {
        transform.position += transform.right * 8.0f;
        canDash = false;
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


    public IEnumerator Turn()
    {
        transform.Rotate(0.0f, 180.0f, 0.0f);
        anim.SetBool("InDashRange", false);
        yield return new WaitForSeconds(5.0f);
        canDash = true;
    }
}

public abstract class Enemy : MonoBehaviour
{
    protected enum CurrentState
    {
        Idle, Move, Attack, Hit, Die, Follow
    }
    [SerializeField]
    protected float attackRange;
    protected float detectRange;
    [SerializeField]
    protected float Hp;
    protected float MaxHp;
    protected float moveSpeed;
    protected Transform target;
    protected bool canMove;
    protected bool stuned;

    protected Image HealthBar;

    public abstract void Hit(float rotY, float force);
    protected abstract void Die();

    protected abstract void Facing(float a);
    public abstract void StateChange(int state);
    public abstract void TurnAround();

    public abstract void Stun();
    public abstract void ReleaseStun();

}
