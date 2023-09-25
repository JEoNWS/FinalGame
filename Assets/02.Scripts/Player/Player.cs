using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{
    private static Player player;

    public bool isGround;
    // ���� �� ���� ���� �Ƚ�� �����
    public bool jumping = false;
    public bool dead = false;

    // ������ �÷��̾� ����
    [SerializeField]
    bool playerULT;

    // ĳ���� ����
    public int hp;
    public int maxHp;
    public int dodgeCount;
    private float atk;
    private float def;
    private float attackSpeed;
    private float facing;
    public bool playerCanHit;
    public bool playeCanRoll;
    private bool playerRoll;
    private bool canMove;
    private bool canJump;
    float floorY;
    int jumpCount;
    bool canPowerAttack;
    bool powerAttackClicked;
    Animator anim;
    Rigidbody2D rigd;


    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpPower;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private LayerMask ground_Layer;
    [SerializeField]
    private LayerMask enemy_Layer;
    [SerializeField]
    private GameObject lightning;
    [SerializeField]
    private GameObject water;
    [SerializeField]
    private GameObject earth;
    [SerializeField]
    private GameObject fire;
    [SerializeField]
    private GameObject jumpDust;
    [SerializeField]
    private GameObject Bujuk;
    // ����Ƚ��
    int current_attack_count;
    int skill_attack_count;
    float attackRange;

    List<Enemy> enemies = new List<Enemy>();

    public static Player GetInstance()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();

            if (player == null)
            {
                GameObject container = new GameObject("Player");
                player = container.AddComponent<Player>();
            }
        }
        return player;
    }

    private void Start()
    {
        //�̱���
        if (player != null)
        {
            if (player != this)
                Destroy(gameObject);
        }
        dodgeCount = 5;
        maxHp = 5;
        jumpCount = 0;
        hp = maxHp;
        current_attack_count = 0;
        playerCanHit = true;
        playeCanRoll = true;
        playerRoll = false;
        canMove = true;
        canJump = false;

        anim = GetComponent<Animator>();
        rigd = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        Debug.DrawRay((transform.position + Vector3.up * 0.1f), transform.right * attackRange, new Color(1, 0, 0));
        if (!jumping)
        {
            Debug.DrawRay(transform.position, Vector3.down * 0.1f, new Color(0, 1, 0));
            Debug.DrawRay((transform.position + Vector3.right * 0.5f * facing), Vector3.down * 0.1f, new Color(0, 1, 0));
            Debug.DrawRay((transform.position + Vector3.up), Vector3.down * 5.0f, new Color(1, 0, 0));
            Debug.DrawRay((transform.position + Vector3.right * 0.5f * facing + Vector3.up), Vector3.down * 5.0f, new Color(1, 0, 0));
        }
        if (rigd.velocity.y <= 0.0f)
        {
            GroundedCheck();
        }
        if (!isGround)
        {
            rigd.velocity += Vector2.down * gravity * Time.deltaTime;
            anim.SetFloat("Jump", rigd.velocity.y);
        }
        else if (playerRoll)
        {
            rigd.velocity = new Vector2(facing * moveSpeed * 1.5f, rigd.velocity.y);
        }
    }

    private void GroundedCheck()
    {
        if (jumping) return;

        RaycastHit2D hitb = Physics2D.Raycast((transform.position + Vector3.up), Vector2.down, 10.0f, ground_Layer);
        RaycastHit2D hitf = Physics2D.Raycast((transform.position + Vector3.up + transform.right * 0.5f), Vector2.down, 10.0f, ground_Layer);
        if (hitf && hitb)
        {
            if (hitf.collider.CompareTag("Ground"))
                floorY = hitf.point.y;
            else if (hitb.collider.CompareTag("Ground"))
                floorY = hitb.point.y;
            if (hitf.point.y >= hitb.point.y)
            {
                if (transform.position.y - hitf.point.y < 0.1f)
                {
                    if (hitf.collider.CompareTag("Stair"))
                    {
                        if (hitf.transform.parent.transform.right == transform.right)
                            transform.position = new Vector3(transform.position.x, hitf.point.y, transform.position.z);
                        else
                        {
                            if (transform.position.y > hitf.transform.position.y)
                                transform.position = new Vector3(transform.position.x, hitf.point.y, transform.position.z);
                            else
                                transform.position = new Vector3(transform.position.x, floorY, transform.position.z);
                        }
                    }
                    else
                        transform.position = new Vector3(transform.position.x, hitf.point.y, transform.position.z);
                    IsGround();
                }
                else
                {
                    anim.SetBool("Fall", true);
                    anim.SetBool("Attack", false);
                    anim.ResetTrigger("PowerAttack");
                    isGround = false;
                    playeCanRoll = false;
                    playerRoll = false;
                }
            }
            else
            {
                if (transform.position.y - hitb.point.y < 0.1f)
                {
                    if (hitb.collider.CompareTag("Stair"))
                    {
                        if (hitb.transform.parent.transform.right == transform.right)
                            transform.position = new Vector3(transform.position.x, hitb.point.y, transform.position.z);
                        else
                        {
                            if (transform.position.y > hitb.transform.position.y)
                                transform.position = new Vector3(transform.position.x, hitb.point.y, transform.position.z);
                            else
                            transform.position = new Vector3(transform.position.x, floorY, transform.position.z);
                        }
                    }
                    else
                        transform.position = new Vector3(transform.position.x, hitb.point.y, transform.position.z);
                        IsGround();
                }
                else
                {
                    anim.SetBool("Fall", true);
                    anim.SetBool("Attack", false);
                    anim.ResetTrigger("PowerAttack");
                    isGround = false;
                    playeCanRoll = false;
                    playerRoll = false;
                }
            }
        }
        else if(hitf)
        {
            if (hitf.collider.CompareTag("Ground"))
                floorY = hitf.point.y;
            if (transform.position.y - hitf.point.y < 0.1f)
            {
                transform.position = new Vector3(transform.position.x, hitf.point.y, transform.position.z);
                IsGround();
            }
            else
            {
                anim.SetBool("Fall", true);
                anim.SetBool("Attack", false);
                anim.ResetTrigger("PowerAttack");
                isGround = false;
                playeCanRoll = false;
                playerRoll = false;
            }
        }
        else if(hitb)
        {
            if (hitb.collider.CompareTag("Ground"))
                floorY = hitb.point.y;
            if (transform.position.y - hitb.point.y < 0.1f)
            {
                transform.position = new Vector3(transform.position.x, hitb.point.y, transform.position.z);
                IsGround();
            }
            else
            {
                anim.SetBool("Fall", true);
                anim.SetBool("Attack", false);
                anim.ResetTrigger("PowerAttack");
                isGround = false;
                playeCanRoll = false;
                playerRoll = false;
            }
        }
        else
        {
            anim.SetBool("Fall", true);
            anim.SetBool("Attack", false);
            anim.ResetTrigger("PowerAttack");
            isGround = false;
            playeCanRoll = false;
            playerRoll = false;
        }
    }

    private void IsGround()
    {
        if (!isGround)
        {
            rigd.velocity = new Vector2(rigd.velocity.x * 0.2f, 0.0f);
        }
        anim.SetBool("Fall", false);
        isGround = true;
    }
    // �̱���

    public void AttackAnim()
    {
        switch (current_attack_count)
        {
            case 0:
                anim.SetInteger("AttackCount", current_attack_count);
                anim.SetBool("Attack", true);
                break;
            case 1:
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f)
                {
                    anim.SetInteger("AttackCount", current_attack_count);
                    anim.SetBool("Attack", true);
                }
                break;
            case 2:
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f)
                {
                    anim.SetInteger("AttackCount", current_attack_count);
                    anim.SetBool("Attack", true);
                }
                break;
            case 3:
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
                {
                    anim.SetInteger("AttackCount", current_attack_count);
                    anim.SetBool("Attack", true);
                }
                break;
            default:
                break;
        }

    }
    [SerializeField]
    void Basic_Attack()
    {
        canPowerAttack = true;
        switch (current_attack_count)
        {
            case 0:
                for(int i = 0; i < 4; i++)
                    transform.GetChild(i).gameObject.SetActive(false);
                transform.GetChild(current_attack_count).gameObject.SetActive(true);
                attackRange = 2.3f;
                PlayerFacing(facing);
                EnemyCheck();
                current_attack_count++;
                UIManager.GetInstance().ChangeBujuk(current_attack_count);
                Attack(50, 0.1f);
                Stop();
                break;
            case 1:
                attackRange = 2.3f;
                PlayerFacing(facing);
                EnemyCheck();
                transform.GetChild(current_attack_count).gameObject.SetActive(true);
                current_attack_count++;
                UIManager.GetInstance().ChangeBujuk(current_attack_count);
                Attack(70, 0.1f);
                Stop();
                break;
            case 2:
                attackRange = 3.2f;
                PlayerFacing(facing);
                EnemyCheck();
                transform.GetChild(current_attack_count).gameObject.SetActive(true);
                current_attack_count++;
                UIManager.GetInstance().ChangeBujuk(current_attack_count);
                Attack(100, 0.3f);
                Stop();
                break;
            case 3:
                attackRange = 4.4f;
                PlayerFacing(facing);
                EnemyCheck();
                transform.GetChild(current_attack_count).gameObject.SetActive(true);
                current_attack_count++;
                UIManager.GetInstance().ChangeBujuk(current_attack_count);
                Attack(200, 0.5f);
                Stop();
                break;
            default:
                break;
        }
    }
    public void Power_Attack()
    {
        if (current_attack_count == 0) return;
        if (!canPowerAttack) return;
        // ��� ������ ��Ŭ�� �ߴ��� ���
        skill_attack_count = current_attack_count;
        anim.SetBool("Move", false);
        anim.SetTrigger("PowerAttack");
        // �⺻���� Ƚ���� 0���� �ʱ�ȭ
        canJump = false;
        canMove = false;
        playeCanRoll = false;
        powerAttackClicked = true;
    }

    public void SkillAttack()
    {
        switch (skill_attack_count)
        {
            case 1:
                Instantiate(water, transform.position + transform.right * 0.5f, Quaternion.identity, transform);
                playerULT = true;
                Invoke("PlayerCanHit", 1.0f);
                UseBujuk(skill_attack_count);
                break;
            case 2:
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (enemies[i] != null)
                    {
                        Instantiate(lightning, enemies[i].transform.position, Quaternion.identity);
                        enemies[i].Stun();
                    }
                }
                Attack(0f, 1.0f);
                UseBujuk(skill_attack_count);
                break;
            case 3:
                Instantiate(earth, transform.position + transform.right * 2.25f, transform.rotation);
                UseBujuk(skill_attack_count);
                break;
            case 4:
                Instantiate(fire, transform.position + transform.right * 2.25f, transform.rotation);
                UseBujuk(skill_attack_count);
                break;
            default:
                break;
        }
    }
    void UseBujuk(int attackCount)
    {
        for (int i = 0; i < attackCount; i++)
            transform.GetChild(i).GetComponent<Animator>().SetTrigger("Use");
    }

    public void BojukEnd()
    {
        canJump = true;
        canMove = true;
        playeCanRoll = true;
        powerAttackClicked = false;
        canPowerAttack = false;
        anim.ResetTrigger("PowerAttack");
    }

    public void Move(float a)
    {
        if (dead) return;
        if (playerRoll) return;
        if (!canMove) return;
        // �̵� �ִϸ��̼� ��������
        anim.SetBool("Move", true);
        if (a < 0.0f)
            facing = -1.0f;
        else if (a > 0.0f)
            facing = 1.0f;

        // ���� �ִϸ��̼��� Run �϶��� �����̰� ����
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run") || !isGround)
        {
            // ���� �̵���Ű�� �κ�
            rigd.velocity = new Vector2(a * moveSpeed, rigd.velocity.y);
            // �������� �� ��� ��������Ʈ ȸ��
            if (a < 0.0f)
                PlayerFacing(a);
            else if (a > 0.0f)
                PlayerFacing(a);
        }
    }

    public void Jump()
    {
        if(dead) return;
        if (playerRoll) return;
        if (jumpCount >= 2) return;
        if(isGround)
            anim.SetTrigger("JumpT");
        anim.SetBool("Fall", true);
        rigd.velocity = new Vector3(rigd.velocity.x, 0.0f, 0.0f);
        rigd.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        Instantiate(jumpDust, transform.position, transform.rotation);
        ResetAttackCount();
        isGround = false;
        jumping = true;
        playeCanRoll = false;
        canJump = false;
        jumpCount++;
        Invoke("ResetRay", 0.1f);
    }
    public void JumpEnd()
    {
        canJump = true;
        canMove = true;
        playeCanRoll = true;
        playerCanHit = true;
        powerAttackClicked = false;
        jumpCount = 0;
    }

    public void Stop()
    {
        anim.SetBool("Move", false);
        rigd.velocity = new Vector2(0.0f, rigd.velocity.y);
    }

    // ���ݸ�ǿ��� �ٽ� idle �� ���ư� �� ���ݼ� 0���� �ʱ�ȭ �ϴ� �κ�
    public void ResetAttackCount()
    {
        anim.SetBool("Attack", false);
        current_attack_count = 0;
        UIManager.GetInstance().ChangeBujuk(0);
        if (!powerAttackClicked)
        {
            for (int i = 0; i < 4; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf)
                    transform.GetChild(i).GetComponent<Animator>().SetTrigger("Destroy");
            }
        }
    }

    // �÷��̾� ȸ����Ű�� �Լ�
    public void PlayerFacing(float a)
    {
        // �������� �� ��� ��������Ʈ ȸ��
        if (a < 0.0f)
            transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        else if (a > 0.0f)
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
    public void Hit()
    {
        if (playerULT) return;
        if (!playerCanHit) return;
        if (playerRoll) return;
        if (dead) return;
        hp -= 1;
        anim.SetTrigger("Hit");
        rigd.velocity = Vector2.zero;
        playerCanHit = false;
        playeCanRoll = false;
        powerAttackClicked = false;
        canJump = false;
        canMove = false;
        if (hp <= 0)
        {
            Dead();
            return;
        }
        UIManager.GetInstance().ChangePlayerHP(hp);
    }
    public void HitEnd()
    {
        playerCanHit = true;
        playeCanRoll = true;
        canMove = true;
        powerAttackClicked = false;
        canJump = true;
    }
    void ResetRay()
    {
        jumping = false;
    }
    public void Dodge()
    {
        if (!playeCanRoll) return;
        if (!isGround) return;
        if (dodgeCount <= 0) return;

        anim.SetTrigger("Dodge");
        ResetAttackCount();
        dodgeCount--;
        playerCanHit = false;
        playeCanRoll = false;
        playerRoll = true;
        canMove = false;
        canJump = false;
        canPowerAttack = false;
        UIManager.GetInstance().ChangeStamina();
        Invoke("DodgeCountUp", 5.0f);
    }
    public void PlayerRollEnd()
    {
        playeCanRoll = true;
        playerRoll = false;
        canMove = true;
        canJump = true;
        powerAttackClicked = false;
        PlayerUltimate();
    }
    public void PlayerUltimate()
    {
        playerCanHit = true;
    }

    private void Attack(float force, float time)
    {
        if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] == null) continue;
                enemies[i].Hit(transform.position.x - enemies[i].transform.position.x, force);
            }
            CameraManager.GetInstance().ShakeCamera(time);
        }
    }
    void EnemyCheck()
    {
        enemies.Clear();    
        RaycastHit2D[] hit = Physics2D.RaycastAll((transform.position + Vector3.up * 0.5f), transform.right, attackRange, enemy_Layer);
        for(int i = 0; i < hit.Length; i++)
        {
            enemies.Add(hit[i].transform.GetComponent<Enemy>());
        }
    }

    void Dead()
    {
        UIManager.GetInstance().ChangePlayerHP(0);
        anim.SetTrigger("Die");
        dead = true;
        rigd.velocity = Vector2.zero;
    }
    public void GroundFall()
    {
        if (isGround == false)
            return;
        RaycastHit2D hit = Physics2D.Raycast((transform.position), Vector2.down, 10.0f, ground_Layer);
        if (!hit.collider.CompareTag("HalfFloor"))
            return;
        jumping = true;
        Invoke("ResetRay", 0.5f);
        anim.SetBool("Fall", true);
        anim.SetBool("Attack", false);
        anim.ResetTrigger("PowerAttack");
        isGround = false;
        playeCanRoll = false;
        playerRoll = false;
    }
    private void PlayerCanHit()
    {
        playerULT = false;
    }
    void DodgeCountUp()
    {
        dodgeCount++;
        UIManager.GetInstance().ChangeStamina();
    }
    public void Heal()
    {
        hp = maxHp;
        UIManager.GetInstance().ChangePlayerHP(hp);
    }
}