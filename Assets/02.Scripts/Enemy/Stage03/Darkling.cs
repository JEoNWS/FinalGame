using UnityEngine;
using UnityEngine.UI;

public class Darkling : Enemy
{
    Animator anim;
    Rigidbody2D rigd;

    RaycastHit2D attackRayHit;
    RaycastHit2D detectRayHit;

    bool skillAttack;
    Vector3 skillTargeetPos;

    public LayerMask playerFilter;
    public LayerMask groundFilter;
    [SerializeField]
    float stunTime;

    [SerializeField]
    GameObject lightningEffect;

    CurrentState state;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigd = GetComponent<Rigidbody2D>();
        HealthBar = transform.GetChild(0).GetChild(1).GetComponent<Image>();

        detectRange = 12.0f;
        Hp = 10;
        MaxHp = Hp;
        moveSpeed = 1.0f;
        state = CurrentState.Move;
        stuned = false;
        skillAttack = false;
    }
    void Update()
    {
        if (stuned) return;
        if (skillAttack)
        {
            Follow();
            if(Mathf.Abs(target.position.y - transform.position.y) < 0.1f)
                anim.SetBool("Skill", false);
        }
        switch (state)
        {
            case CurrentState.Die:
                return;
            case CurrentState.Move:
                {
                    if (target == null)
                    {
                        Debug.DrawRay(transform.position + Vector3.up * 0.1f, transform.right * detectRange, new Color(0, 1, 0));
                        detectRayHit = Physics2D.Raycast(transform.position + Vector3.up * 0.1f, transform.right, detectRange, playerFilter);
                        if (detectRayHit)
                        {
                            anim.SetBool("InDetectRange", true);
                            state = CurrentState.Idle;
                            target = detectRayHit.transform;
                            break;
                        }
                    }
                    else
                    {
                        if (Player.GetInstance().isGround)
                        {
                            if (Mathf.Abs(target.position.y - transform.position.y) > 1.0f || Vector3.Magnitude(target.position - transform.position) > 10.0f)
                            {
                                state = CurrentState.Attack;
                                anim.SetBool("Skill", true);
                            }
                            else
                            {
                                state = CurrentState.Follow;
                            }
                            break;
                        }
                    }
                    transform.position += transform.right * Time.deltaTime;

                    break;
                }
            case CurrentState.Attack:
                {
                    // 공격하기 애니메이션에서 실행

                    break;
                }
            case CurrentState.Follow:
                {
                    if (Mathf.Abs(target.position.y - transform.position.y) > 1.0f)
                    {
                        anim.SetBool("InAttackRange", false);
                        anim.speed = 1.0f;
                        state = CurrentState.Move;
                        break;
                    }
                    Debug.DrawRay(transform.position + Vector3.up * 0.1f, transform.right * attackRange, new Color(0, 1, 0));
                    attackRayHit = Physics2D.Raycast(transform.position + Vector3.up * 0.1f, transform.right, attackRange, playerFilter);
                    if (attackRayHit)
                    {
                        anim.SetBool("InAttackRange", true);
                        anim.speed = 1.0f;
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
            case CurrentState.Idle:
                {
                    break;
                }
        }
    }
    void Skill()
    {
        Facing(target.position.x - transform.position.x);
        GetComponent<BoxCollider2D>().isTrigger = true;
        rigd.gravityScale = 0;
        skillTargeetPos = target.position - transform.right;
        Debug.Log(1);

        skillAttack = true;
    }
    void Follow()
    {
        if (skillAttack)
        {
            transform.position = Vector3.MoveTowards(transform.position, skillTargeetPos, 0.15f);
        }
    }
    void FollowEnd()
    {
        Attack();
        state = CurrentState.Follow;
        GetComponent<BoxCollider2D>().isTrigger = false;
        rigd.gravityScale = 1;
        anim.SetBool("Skill", false);
        skillAttack = false;
    }

    public override void Hit(float rotY, float force)
    {
        state = CurrentState.Attack;
        if (target == null)
            target = Player.GetInstance().transform;
        anim.SetTrigger("Hit");
        Facing(rotY);
        rigd.AddForce(target.right * force);
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
        rigd.bodyType = RigidbodyType2D.Static;
        Stage03Manager.deadEnemyCount++;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(false);
        Destroy(this);
    }

    protected override void Facing(float a)
    {
        if (a < 0.0f)
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        else
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
    public override void StateChange(int state)
    {
        this.state = (CurrentState)state;
        if(state == (int)CurrentState.Move)
        {
            anim.SetBool("GroundEnd", false);
            transform.Rotate(0, 180.0f, 0);
        }
        else if(state == (int)CurrentState.Attack)
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
