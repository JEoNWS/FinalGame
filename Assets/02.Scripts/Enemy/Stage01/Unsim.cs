using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Unsim : Enemy
{
    private bool attack02Movement;
    public Material material;
    private Animator anim;
    CurrentState state;
    public LayerMask playerFilter;
    Vector3 targetPos;

    [SerializeField]
    float midRangeAttackRange;
    [SerializeField]
    float stunTime;
    [SerializeField]
    float xMin;
    [SerializeField]
    float xMax;
    [SerializeField]
    float HideCooltime;
    [SerializeField]
    Stage01Manager stageManager;

    public GameObject Arrow;

    private float distance;
    void Start()
    {
        attack02Movement = false;
        anim = GetComponent<Animator>();
        state = CurrentState.Idle;
        Hp = 30;
        MaxHp = Hp;
        HealthBar = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        stuned = false;
    }

    void Update()
    {
        HideCooltime -= Time.deltaTime;
        if (stuned) return;
        Debug.DrawRay(transform.position + Vector3.up, transform.right * attackRange, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * 0.5f, transform.right * midRangeAttackRange, Color.red);
        if (attack02Movement)
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
                AttackStart();
                if (HideCooltime < 0.0f)
                {
                    anim.SetTrigger("Hide");
                    state = CurrentState.Attack;
                    HideCooltime = 10.0f;
                    break;
                }
                else
                {
                    if (distance > attackRange)
                    {
                        anim.SetTrigger("Attack02");
                        state = CurrentState.Attack;
                        break;
                    }
                    else if(distance > midRangeAttackRange)
                    {
                        anim.SetTrigger("Attack01");
                        state = CurrentState.Attack;
                        break;
                    }
                    else
                    {
                        anim.SetTrigger("Attack03");
                        state = CurrentState.Attack;
                        break;
                    }

                }

            default:
                break;
        }
    }
    public override void Hit(float rotY, float force)
    {
        StopCoroutine("Hit_Color_Change");
        Hp -= 1;
        HealthBar.fillAmount = Hp / MaxHp;
        if (Hp <= 0)
        {
            Die();
        }
        ResetColor();
        StartCoroutine("Hit_Color_Change");
    }
    protected override void Die()
    {
        stageManager.Clear();
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
    }
    public void AttackStart()
    {
        distance = transform.position.x - Player.GetInstance().transform.position.x;
        Facing(distance);
        distance = Mathf.Abs(distance);
    }
    public void Attack(float a)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up * 0.1f, transform.right, a, playerFilter);
        if (hit)
        {
            Player.GetInstance().Hit();
        }
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
    public void Attack02Object(float yPos)
    {
        if(yPos > 1)
            Instantiate(Arrow, new Vector3(transform.position.x + transform.right.x, yPos, 0.0f) , transform.rotation);
        else
            Instantiate(Arrow, new Vector3(transform.position.x + transform.right.x * 1.2f, yPos, 0.0f), transform.rotation);
    }
    void Teleport()
    {
        if(Random.Range(0, 2) == 0)
        {
            transform.position = new Vector3(Random.Range(xMin, xMax), transform.position.y, 0.0f);
        }
        else
        {
            transform.position = new Vector3(Random.Range(xMin, xMax) + 16.0f, transform.position.y, 0.0f);
        }
    }
}
