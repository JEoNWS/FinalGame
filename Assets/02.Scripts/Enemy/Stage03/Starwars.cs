using UnityEngine;
using UnityEngine.UI;

public class Starwars : Enemy
{
    RaycastHit2D attackRayHit;
    RaycastHit2D detectRayHit;

    public LayerMask playerFilter;
    public LayerMask groundFilter;
    [SerializeField]
    float stunTime;
    [SerializeField]
    GameObject lightningEffect;

    Animator anim;
    CurrentState state;

    private void Start()
    {
        anim = GetComponent<Animator>();
        HealthBar = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        detectRange = 10.0f;
        Hp = 5;
        MaxHp = Hp;
        moveSpeed = 1.0f;
        stuned = false;
        state = CurrentState.Move;
    }
    void Update()
    {
        if (stuned) return;
        switch (state)
        {
            case CurrentState.Die:
                return;
            case CurrentState.Move:
                {
                    Debug.DrawRay(transform.position + Vector3.up * 0.1f, transform.right * detectRange, new Color(0, 1, 0));
                    detectRayHit = Physics2D.Raycast(transform.position + Vector3.up * 0.1f, transform.right, detectRange, playerFilter);
                    if (detectRayHit)
                    {
                        anim.SetBool("InDetectRange", true);
                        state = CurrentState.Follow;
                        if (target == null)
                            target = detectRayHit.transform;
                        break;
                    }
                    Debug.DrawRay(transform.position + transform.right * 0.5f, Vector2.down, new Color(0, 0, 1));
                    if (Physics2D.Raycast(transform.position + transform.right * 0.5f, Vector2.down, 1.0f, groundFilter))
                        transform.position += transform.right * Time.deltaTime;
                    else
                    {
                        state = CurrentState.Idle;
                    }
                    break;
                }
            case CurrentState.Attack:
                {
                    Debug.DrawRay(transform.position + Vector3.up * 0.1f, transform.right * attackRange, new Color(0, 1, 0));
                    anim.speed = 1.0f;
                    // 공격하기 애니메이션에서 실행
                    break;
                }
            case CurrentState.Follow:
                {
                    if (Mathf.Abs(target.position.y - transform.position.y) > 1.0f)
                    {
                        anim.SetBool("InAttackRange", false);
                        state = CurrentState.Move;
                        anim.speed = 1.0f;
                        break;
                    }
                    Debug.DrawRay(transform.position + Vector3.up * 0.1f, transform.right * attackRange, new Color(0, 1, 0));
                    attackRayHit = Physics2D.Raycast(transform.position + Vector3.up * 0.1f, transform.right, attackRange, playerFilter);
                    if (attackRayHit)
                    {
                        anim.SetBool("InAttackRange", true);
                        state = CurrentState.Attack;
                        break;
                    }
                    else
                    {
                        anim.SetBool("InAttackRange", false);
                        anim.speed = 1.5f;
                        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * 2.0f * Time.deltaTime);
                        Facing(target.position.x - transform.position.x);
                    }
                    break;
                }
            case CurrentState.Hit:
                {
                    Debug.DrawRay(transform.position + transform.right * 0.5f, Vector2.down, new Color(0, 0, 1));
                    if (!Physics2D.Raycast(transform.position + transform.right * -0.5f, Vector2.down, 1.0f, groundFilter))
                    {
                        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    }
                    if (target == null)
                        target = Physics2D.Raycast(transform.position + Vector3.up * 0.1f, transform.right, detectRange, playerFilter).transform;
                    break;
                }
            case CurrentState.Idle:
                {
                    break;
                }
        }
    }

    public override void Hit(float rotY, float force)
    {
        state = CurrentState.Hit;
        GetComponent<Rigidbody2D>().AddForce(transform.right * force * -1.0f);
        anim.SetTrigger("Hit");
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
        state = CurrentState.Die;
        anim.SetTrigger("Die");
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Stage03Manager.deadEnemyCount++;
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
        if (state == (int)CurrentState.Move)
        {
            anim.SetBool("GroundEnd", false);
            transform.Rotate(0, 180.0f, 0);
        }
        else if (state == (int)CurrentState.Attack)
        {
            anim.SetBool("InAttackRange", false);
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
    public override void TurnAround()
    {
        state = CurrentState.Idle;
        anim.SetBool("GroundEnd", true);
    }
    public override void Stun()
    {
        CancelInvoke("ReleaseStun");
        anim.SetBool("StunEnd", true);
        StopAllCoroutines();
        stuned = true;
        Invoke("ReleaseStun", stunTime);
        lightningEffect.SetActive(true);
    }

    public override void ReleaseStun()
    {
        anim.SetBool("StunEnd", false);
        stuned = false;
        lightningEffect.SetActive(false);
    }
}
