using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Hyunmoo : Enemy
{
    private bool attack02Movement;
    public Material material;
    private Animator anim;
    public float dodgeAttackCooldown;
    public float arrowRainAttackCooldown;
    private bool canArrowRain;
    private bool canDodgeAttack;
    bool cantHit;
    CurrentState state;
    public LayerMask playerFilter;
    bool dashAttack;
    Vector3 targetPos;
    public GameObject Arrow;
    public GameObject ArrowRain;

    [SerializeField]
    float stunTime;


    private float distance;
    void Start()
    {
        attack02Movement = false;
        anim = GetComponent<Animator>();
        state = CurrentState.Idle;
        canDodgeAttack = true;
        canArrowRain = true;
        Hp = 1000.0f;
        MaxHp = Hp;
        HealthBar = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        dashAttack = false;
        stuned = false;
    }

    void Update()
    {
        if (stuned) return;
        Debug.DrawRay(transform.position + Vector3.up, transform.right * attackRange, Color.green);
        if(attack02Movement)
        {
            transform.position -= transform.right * 5.5f * Time.deltaTime;
        }
        switch (state)
        {
            case CurrentState.Die:
                return;
            case CurrentState.Attack:
                break;
            case CurrentState.Idle:
                AttackEnd();
                if (Mathf.Abs(distance) > attackRange)
                {
                    if (canArrowRain)
                    {
                        anim.SetTrigger("Attack03");
                        state = CurrentState.Attack;
                        canArrowRain = false;
                        Invoke("ResetArrowRain", arrowRainAttackCooldown);
                    }
                    else
                    {
                        anim.SetTrigger("Attack04");
                        state = CurrentState.Attack;
                        targetPos = transform.position + transform.right * 10;
                        targetPos.z = 0;
                    }
                    break;
                }
                else
                {
                    if (Random.Range(0, 2) == 1 || canDodgeAttack)
                    {
                        anim.SetTrigger("Attack02");
                        state = CurrentState.Attack;
                        canDodgeAttack = false;
                        Invoke("ResetDodgeAttack", dodgeAttackCooldown);
                        break;
                    }
                    else
                    {
                        anim.SetTrigger("Attack01");
                        state = CurrentState.Attack;
                        break;
                    }

                }

            default:
                break;
        }
        if(dashAttack)
        {
            transform.position += transform.right * 20.0f * Time.deltaTime;
        }
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, 118.0f, 148.0f), transform.position.y, 0);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dashAttack)
        {
            if (other.CompareTag("Player"))
            {
                Player.GetInstance().Hit();
            }
        }
    }
    public override void Hit(float rotY, float force)
    {
        if (attack02Movement)
            return;
        StopCoroutine("Hit_Color_Change");
        Hp -= 10;
        HealthBar.fillAmount = Hp / MaxHp;
        ResetColor();
        StartCoroutine("Hit_Color_Change");
    }
    protected override void Die()
    {
        anim.SetTrigger("Die");
    }
    protected override void Facing(float a)
    {
        if (a > 0)
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        else
            transform.rotation = Quaternion.identity;
    }
    public override void StateChange(int state)
    {
        this.state = (CurrentState)state;
    }
    public void AttackEnd()
    {
        distance = transform.position.x - Player.GetInstance().transform.position.x;
        Facing(distance);
        StateChange((int)CurrentState.Idle);
        dashAttack = false;
    }
    // 회피공격 쿨타임 초기화
    void ResetDodgeAttack()
    {
        canDodgeAttack = true;
    }
    void ResetArrowRain()
    {
        canArrowRain = true;
    }
    public void Attack()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up * 0.1f, transform.right, attackRange, playerFilter);
        if (hit)
        {
            Player.GetInstance().Hit();
        }
    }
    // 애니메이션 이동 관련
    void Attack01MoveForward()
    {
        transform.position += transform.right * 5.0f;
    }
    void Attack02Movement_BoolChange()
    {
        if (attack02Movement)
            attack02Movement = false;
        
        else
            attack02Movement = true;
    }
    void Attack02MoveForward()
    {
        transform.position += transform.right * 6.5f;
    }
    void Attack01AttackCheck(float offset)
    {
        if(transform.position.x > Player.GetInstance().transform.position.x)
        {

            if (transform.position.x - (transform.right * offset).x < Player.GetInstance().transform.position.x)
            {
                if(Player.GetInstance().isGround)
                    Player.GetInstance().Hit();
            }
        }
        else
        {
            if (transform.position.x - (transform.right * offset).x > Player.GetInstance().transform.position.x)
                if (Player.GetInstance().isGround)
                    Player.GetInstance().Hit();
        }
    }
    void Attack04DashAttackBool()
    {
        dashAttack = true;
    }
    void Attack03Attack()
    {
        StartCoroutine("Attack3AttackCor");
    }
    void Attack03ArrowRainObject()
    {
        ArrowRain.SetActive(true);
    }
    //맞았을때 색변하는거 관련
    IEnumerator Hit_Color_Change()
    {
        material.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        material.color = Color.black;
    }
    void ResetColor()
    {
        material.color = Color.black;
    }
    IEnumerator Attack3AttackCor()
    {
        for (int i = 0; i < 7; i++)
        {
            Instantiate(Arrow, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 5.0f, 0.0f), Quaternion.Euler(0.0f, 0.0f, 90.0f));
            yield return new WaitForSeconds(0.1f);
        }
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
