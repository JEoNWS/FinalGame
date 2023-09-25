using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class RobotBoss : Enemy
{
    public Material material;
    private Animator anim;
    CurrentState state;
    public LayerMask playerFilter;
    bool dashAttack;
    bool bodyAttack;
    bool jump;
    bool fall;
    bool airAttack;
    bool goUp;
    bool rayAttack;
    bool canAirAttack;
    bool canRayAttack;
    Vector3 targetPos;
    [SerializeField]
    float stunTime;
    [SerializeField]
    float dashRange;
    float gravity;
    [SerializeField]
    float floorY;
    [SerializeField]
    float airAttackCoolDown;
    [SerializeField]
    float RayAttackCoolDown;
    [SerializeField]
    float xMax;
    [SerializeField]
    float xMin;
    [SerializeField]
    Stage02Manager stageManager;

    public GameObject MissleSet;
    public GameObject Lasor;
    public GameObject Missle;
    Vector3 a;

    private float distance;
    void Start()
    {
        Appear();
        anim = GetComponent<Animator>();
        state = CurrentState.Attack;
        //Hp = 10;
        gravity = 5.0f;
        MaxHp = Hp;
        HealthBar = transform.GetChild(0).GetChild(1).GetComponent<Image>();
        dashAttack = false;
        bodyAttack = false;
        stuned = false;
        jump = false;
        fall = true;
        goUp = false;
        canAirAttack = true;
        canRayAttack = true;
    }

    void Update()
    {
        if (stuned) return;
        Debug.DrawRay(transform.position + Vector3.up, transform.right * -attackRange, Color.green);
        Debug.DrawRay(transform.position + Vector3.up * 1.5f, transform.right * -dashRange, Color.red);
        switch (state)
        {
            case CurrentState.Die:
                return;
            case CurrentState.Attack:
                break;
            case CurrentState.Idle:
                AttackStart();
                if (Hp <= MaxHp / 2)
                {
                    if (canRayAttack)
                    {
                        anim.SetTrigger("Jump");
                        state = CurrentState.Attack;
                        targetPos = new Vector3(xMax, floorY + 7.0f, 0);
                        targetPos.z = 0;
                        rayAttack = true;
                        canRayAttack = false;
                        break;
                    }
                    else if (canAirAttack)
                    {
                        airAttack = true;
                        state = CurrentState.Attack;
                        anim.SetBool("Transform", true);
                        StartCoroutine("AirAttackCor");
                        canAirAttack = false;
                        break;
                    }

                }
                if (distance < dashRange)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        anim.SetTrigger("Dash");
                        state = CurrentState.Attack;
                        break;
                    }
                    else
                    {
                        anim.SetTrigger("Jump");
                        state = CurrentState.Attack;
                        targetPos = Player.GetInstance().transform.position + Vector3.up * 7.0f;
                        targetPos.z = 0;
                        break;
                    }
                }
                else
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        anim.SetTrigger("Attack");
                        state = CurrentState.Attack;
                        break;
                    }
                    else
                    {
                        anim.SetTrigger("Jump");
                        state = CurrentState.Attack;
                        targetPos = Player.GetInstance().transform.position + Vector3.up * 7.0f;
                        targetPos.z = 0;
                        break;
                    }
                }
            default:
                break;
        }
        if (dashAttack)
        {
            transform.position -= transform.right * 15.0f * Time.deltaTime;
        }
        else if(jump)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref a, 0.5f);
            //transform.position = Vector3.Slerp(transform.position, targetPos, 0.01f);
            if (Mathf.Abs(transform.position.x - targetPos.x) < 1.0f)
            {
                anim.SetTrigger("Fall");
                jump = false;
                fall = true;
                gravity = 5.0f;
            }
        }
        else if(fall)
        {
            transform.position -= Vector3.up * gravity * Time.deltaTime;
            gravity += 0.15f;
            if(transform.position.y < floorY)
            {
                anim.SetBool("IsGround", true);
                fall = false;
                transform.position = new Vector3(transform.position.x, floorY, 0.0f);
                if(rayAttack)
                {
                    anim.SetBool("Lasor", true);
                    transform.rotation = Quaternion.identity;
                }
            }
        }
        else if(airAttack)
        {
            transform.position -= transform.right * 15.0f * Time.deltaTime;
            if(transform.position.x < xMin)
            {
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            }
            else if(transform.position.x > xMax)
            {
                transform.rotation = Quaternion.identity;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (bodyAttack)
        {
            if (other.CompareTag("Player"))
            {
                Player.GetInstance().Hit();
            }
        }
    }
    public override void Hit(float rotY, float force)
    {
        StopCoroutine("Hit_Color_Change");
        Hp -= 1;
        HealthBar.fillAmount = (float)Hp / MaxHp;
        if (Hp <= 0)
        {
            Die();
            return;
        }
        ResetColor();
        StartCoroutine("Hit_Color_Change");
    }
    protected override void Die()
    {
        anim.SetTrigger("Die");
        GetComponent<SpriteRenderer>().sortingOrder = -1;
        stageManager.Clear();
        Destroy(this);
    }
    protected override void Facing(float a)
    {
        if (a > 0)
            transform.rotation = Quaternion.identity;
        else
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
    }
    public override void StateChange(int state)
    {
        this.state = (CurrentState)state;
    }
    public void AttackEnd()
    {
        if (rayAttack)
            return;
        dashAttack = false;
        bodyAttack = false;
        jump = false;
        anim.SetBool("IsGround", false);
        AttackStart();
        StateChange((int)CurrentState.Idle);
    }
    void AttackStart()
    {
        distance = transform.position.x - Player.GetInstance().transform.position.x;
        Facing(distance);
        distance = Mathf.Abs(distance);
    }
    public void Attack()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up * 0.1f, transform.right, attackRange, playerFilter);
        if (hit)
        {
            Player.GetInstance().Hit();
        }
    }
    void DashAttackBool()
    {
        BodyAttackBool();
        if (dashAttack)
            dashAttack = false;
        else
            dashAttack = true;
    }
    void JumpBool()
    {
        if (jump)
            jump = false;
        else
            jump = true;
    }
    void BodyAttackBool()
    {
        if (bodyAttack)
        {
            bodyAttack = false;
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            bodyAttack = true;
            GetComponent<BoxCollider2D>().enabled = true;
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
    void RunchMissle()
    {
        Instantiate(MissleSet, transform.position + Vector3.up * 5.0f, Quaternion.identity);
    }
    IEnumerator AirAttackCor()
    {
        while(transform.position.y < floorY + 7.0f)
        {
            transform.position += Vector3.up * 10.0f * Time.deltaTime;
            yield return null;
        }
        anim.SetTrigger("OnAir");
        yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
        anim.SetTrigger("AirAttack");
        yield return new WaitForSeconds(Random.Range(3.0f, 5.0f));
        anim.SetTrigger("AirAttack");
        yield return new WaitForSeconds(Random.Range(3.0f, 5.0f));
        anim.SetTrigger("AirAttack");
        yield return new WaitForSeconds(Random.Range(3.0f, 5.0f));
        AirAttackEnd();
    }
    void AirAttackReset()
    {
        canAirAttack = true;
    }
    void AirAttackEnd()
    {
        anim.SetBool("Transform", false);
        airAttack = false;
        Invoke("AirAttackReset", airAttackCoolDown);
    }
    void FallBool()
    {
        fall = true;
        gravity = 5.0f;
    }
    void LasorAttack()
    {
        Instantiate(Lasor, transform.position - transform.right * 1.0f + Vector3.up * 3.0f, Quaternion.identity, transform);
    }
    public void LasorAttackEnd()
    {
        Invoke("RayAttackReset", RayAttackCoolDown);
        anim.SetBool("Lasor", false);
        rayAttack = false;
        AttackEnd();
    }
    void RayAttackReset()
    {
        canRayAttack = true;
    }

    void MissleAttack()
    {
        Instantiate(Missle, transform.position - transform.right * 3.0f + Vector3.up * 3.0f, Quaternion.identity);
    }
    void Appear()
    {
        CameraManager.GetInstance().ChangeCameraSize(10.0f);
    }
    void CamShake()
    {
        CameraManager.GetInstance().ShakeCamera(1.0f);
    }
}
