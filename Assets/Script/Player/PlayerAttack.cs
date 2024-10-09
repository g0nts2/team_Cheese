using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;

public class PlayerAttack : MonoBehaviour
{
    private PlayerControl playerControl;
    private EnemyManager enemyManager;

    // ���Ÿ� ���� ����
    public GameObject bullet;
    public Transform bulletPos;
    public float fireCooltime;
    private float fireCurtime;

    // �������� �� enemy�� �浹
    [SerializeField] List<GameObject> hp = new List<GameObject>();
    private Collider2D[] meleeAttackableEnemies;
    private Vector2 meleeAttackBoxSize;
    private Vector2 nearEnemyBoxSize;

    
    // ���� ���ݿ��� enemy ������ �޾ƿ��� ���ؼ� ����
    private Collider2D enemyCollider;

    void PlayerAttacks()
    {
        enemyCollider = meleeAttackableEnemy();

        // ���� ���� ó��
        if (Input.GetKeyDown(KeyCode.LeftControl) && enemyCollider != null)  // ���� ���� ���� ���� ������ �����Ǿ��ٸ�
        {
            if (enemyCollider.gameObject.layer == LayerMask.NameToLayer("enemy"))
            {
                meleeAttack();
                enemyManager.takeDamage(enemyCollider.tag);
            }
        }
        // ���Ÿ� ���� ó��
        else if (Input.GetKeyDown(KeyCode.LeftControl) && enemyCollider == null && fireCurtime <= 0) // ��Ÿ�� Ȯ��
        {
            rangedAttack();
        }

        attackStop();

        // bullet�� �ִ� �ڵ带 ����� , �ܹ� ���
        if (fireCurtime > 0)
        {
            fireCurtime -= Time.deltaTime;  // ��Ÿ�� ����
        }
    }

    void meleeAttack()
    {
        if (playerControl.Direction == 1)
        {
            playerControl.animator.Play("PlayerMeleeAttackBack");
        }
        if (playerControl.Direction == 2)
        {
            playerControl.animator.Play("PlayerMeleeAttackFront");
        }
        if (playerControl.Direction == 3)
        {
            playerControl.animator.Play("PlayerMeleeAttackLeft");
        }
        if (playerControl.Direction == 4)
        {
            playerControl.animator.Play("PlayerMeleeAttackRight");
        }
    }

    void rangedAttack()
    {
        // ���⿡ ���� �ִϸ��̼� ����
        if (playerControl.Direction == 1) // ��
        {
            playerControl.animator.Play("PlayerLongAttackBack");
        }
        else if (playerControl.Direction == 2) // ����
        {
            playerControl.animator.Play("PlayerLongAttackFront");
        }
        else if (playerControl.Direction == 3) // ����
        {
            playerControl.animator.Play("PlayerLongAttackLeft");
        }
        else if (playerControl.Direction == 4) // ������
        {
            playerControl.animator.Play("PlayerLongAttackRight");
        }


        // �߻� ��Ÿ���� ������ ���� �Ѿ� �߻�
        Instantiate(bullet, bulletPos.position, transform.rotation);  // �Ѿ� ����
        fireCurtime = fireCooltime; // ��Ÿ�� �ʱ�ȭ
    }

    void attackStop()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (playerControl.Direction == 1)
            {
                playerControl.animator.Play("PlayerUp_Stop");
            }
            else if (playerControl.Direction == 2)
            {
                playerControl.animator.Play("PlayerBack_Stop");
            }
            else if (playerControl.Direction == 3)
            {
                playerControl.animator.Play("PlayerLeft_Stop");
            }
            else if (playerControl.Direction == 4)
            {
                playerControl.animator.Play("PlayerRight_Stop");
            }
        }
    }


    // ���� ����   -------------------------------------------------------------------------------------------

    public bool showRangeGizmo = false;
    /* Player�� enemy Ž�� Gizmo */
    private void OnDrawGizmosSelected()
    {
        if (showRangeGizmo)
        {
            Gizmos.color = new Color(1.0f, 0f, 0f, 0.8f);
            Gizmos.DrawCube(this.transform.position + playerControl.CenterOffset, new Vector2(meleeAttackBoxSize.x, meleeAttackBoxSize.y));
        }
    }

    /*  ���� ���� ����
     linq(������ ���� ���)�� �̿��ؼ� ���� ����
     Gizmo�� ���� �ȿ� �����ϴ� ��� 2D �ݶ��̴��� ������
     => : ����
        Where : ������ �����ϴ� ��� ���͸�
        OrderBy: �������� ����
        oArray: �迭�� ��ȯ
     'enemy' �±׸� ���� PolygonCollider2D�� ���͸�

     */

    private Collider2D meleeAttackableEnemy()
    {
        Collider2D[] enemyArray = Physics2D.OverlapBoxAll((Vector2)(this.transform.position) + (Vector2)playerControl.CenterOffset, meleeAttackBoxSize, 0f);

        meleeAttackableEnemies = enemyArray
        .Where(collider => collider.gameObject.layer == 6 /*6�� Layer�� enemy, LayerMask.NameToLayer("enemy")*/ && collider is PolygonCollider2D)
        .OrderBy(collider => Vector2.Distance(this.transform.position, collider.transform.position))
        .ToArray();


        if (meleeAttackableEnemies.Length > 0)
        {
            Debug.Log("Melee Attackable Enemy: " + meleeAttackableEnemies[0].name);
            return meleeAttackableEnemies[0];
        }
        else
            return null;
    }


    // Player HP ---------------------------------------------------------------------
    void getPlayerHP()
    {
        int numHp = GameObject.Find("playerHP").transform.childCount;
        for (int i = 0; i < numHp; i++)
        {
            GameObject hpObj = GameObject.Find("playerHP").transform.GetChild(i).gameObject;
            hp.Add(hpObj);
        }
    }

    /* HP ���� Gizmo */
    public bool showHPGizmo = false;
    private void OnDrawGizmos()
    {
        if (showHPGizmo)
        {
            Gizmos.color = new Color(0f, 3f, 0f, 0.7f);
            Gizmos.DrawCube(this.transform.position + playerControl.CenterOffset, new Vector2(nearEnemyBoxSize.x, nearEnemyBoxSize.y));
        }
    }


    private Collider2D[] nearEnemies;
    public float elapsedTime = 0f;
    private float destroyTime = 1f;
    private bool isCollidingWithEnemy = false;

    public bool isChangingSprite = false; // playerControl.MoveControl���� ����ϱ� ���� public - isMove

    /* CollideWithEnemy �Լ� ����
   'enemy' �±׸� ���� polygonCollider2D�� ���͸�
    => : ����
     Where : ������ �����ϴ� ��� ���͸�
     OrderBy: �������� ����
     oArray: �迭�� ��ȯ
  */

    public bool CollideWithEnemy()
    {
        Collider2D[] enemyArray = Physics2D.OverlapBoxAll((Vector2)(this.transform.position) + (Vector2)playerControl.CenterOffset, nearEnemyBoxSize, 0f);

        nearEnemies = enemyArray
            .Where(collider => collider.gameObject.layer == 6 /*LayerMask.NameToLayer("enemy")*/ && collider is PolygonCollider2D)
            .OrderBy(collider => Vector2.Distance(this.transform.position, collider.transform.position))
            .ToArray();

        if (nearEnemies.Length > 0)
        {
            Debug.Log("Near Enemy: " + nearEnemies[0].name);
            return true;
        }
        else
            return false;
    }

     void Player_Collision()
    {
        if (hp != null)
        {
            if (CollideWithEnemy() == true)
            {
                isCollidingWithEnemy = true;
            }
            else
            {
                isCollidingWithEnemy = false;
                elapsedTime = 0f;
            }

            // 1���̻� ���� ��� �� hp--
            if (isCollidingWithEnemy == true  && isChangingSprite != true)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= destroyTime && hp.Count > 0)
                {
                    GameObject lastHp = hp[hp.Count - 1];
                    StartCoroutine(changeToDamaged());
                    hp.RemoveAt(hp.Count - 1);
                    Destroy(lastHp);
                    elapsedTime = 0f; // �ٽ� �ð� �ʱ�ȭ
                }
            }
        }
    }

    //void returnToNormalSprite()
    //{
    //}
    IEnumerator changeToDamaged()
    {
        isChangingSprite = true;
        playerControl.isMove = false;
        if (playerControl.Direction == 1)
        {
            playerControl.animator.Play("playerDamagedBack");
        }
        if (playerControl.Direction == 2)
        {
            playerControl.animator.Play("playerDamagedFront");
        }
        if (playerControl.Direction == 3)
        {
            playerControl.animator.Play("playerDamagedLeft");
        }
        if (playerControl.Direction == 4)
        {
            playerControl.animator.Play("playerDamagedRight");
        }
        yield return new WaitForSeconds(0.5f);
        isChangingSprite = false;
        playerControl.isMove = true;
        elapsedTime = 0f;
    }
    

    void Start()
    {
        playerControl = FindObjectOfType<PlayerControl>();
        enemyManager = FindObjectOfType<EnemyManager>();

        getPlayerHP();

        // ���� ���� offset ��
        meleeAttackBoxSize = new Vector2(2.8f, 2.3f);
        nearEnemyBoxSize = new Vector2(1.2f, 1.7f);
        fireCooltime = 0.2f;
    }

    void Update()
    {
        PlayerAttacks();
        Player_Collision();
    }
}
