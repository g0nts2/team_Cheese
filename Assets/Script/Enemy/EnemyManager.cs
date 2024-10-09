using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies = new();
    private List<Enemy> enemyList = new();
    private List<SpriteRenderer> enemySprites = new();
    private List<Animator> enemyEffects = new();  // Animator override �ؼ� �ϰ� ������ ����, ���� ���� ����

    private Bullet bullet;
    private bool isCoroutining = false; // ��ø�Ǽ� ����Ǵ� ���� ����

    public void takeDamage(string enemyName) // name : bullet���� ������ enemy�� tag
    {
        (GameObject objEnemy, Enemy enemy, SpriteRenderer enemySprite, Animator enemyAni)
            = getEnemyInformation(enemyName);

        enemy.hp -= 1;
        if (!isCoroutining && enemy.hp > 0)
        {
            Debug.Log("hp ����");
            enemyAni.Play(enemy.name + "Hit");
            StartCoroutine(changeColor(enemySprite));
            StartCoroutine(ResetToDefaultState(enemyAni));
        }

        if (!isCoroutining && enemy.hp == 0)
        {
            destroyEnemy(objEnemy, enemy, enemySprite, enemyAni);  // �� �ı�
        }
    }

    // �����ϸ� �ڵ������� enemy���� components�� list�� �߰��ǵ��� ����
    void addEnemyInformationInLists()
    {
        // enemies.AddRange(transform.GetChild());
        enemyList.AddRange(GetComponentsInChildren<Enemy>());
        enemySprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
        enemyEffects.AddRange(GetComponentsInChildren<Animator>());
    }

    // Ʃ�÷� ���� ������ ��ȯ�ϰ� ��
    // Damage�� �޴� �ش� ������Ʈ�� Components���� list�鿡�� ��ȯ
    (GameObject, Enemy, SpriteRenderer, Animator) getEnemyInformation(string enemyName)
    {
        int objIndex = enemies.FindIndex(x => x.name.Equals(enemyName));
        GameObject obj = enemies[objIndex];

        int enemyIndex = enemyList.FindIndex(x => x.name.Equals(enemyName));
        Enemy enemy = enemyList[enemyIndex];

        int spriteIndex = enemySprites.FindIndex(x => x.name.Equals(enemyName));
        SpriteRenderer enemySprite = enemySprites[spriteIndex];

        int aniIndex = enemySprites.FindIndex(x => x.name.Equals(enemyName));
        Animator enemyAni = enemyEffects[aniIndex];

        return (obj, enemy, enemySprite, enemyAni);
    }

    void destroyEnemy(GameObject objEnemy, Enemy enemy, SpriteRenderer enemySprite, Animator enemyAni)
    {
        string enemyTag = enemy.name;
        switch (enemyTag)
        {
            case "pinkDollEnemy":
                {
                    deleteEnemyInLists(objEnemy, enemy, enemySprite, enemyAni);
                    break;
                }
            case "rabbitDollEnemy":
                {
                    deleteEnemyInLists(objEnemy, enemy, enemySprite, enemyAni);
                    break;
                }
        }
    }

    // �ı��Ǵ� �� ���� �ڵ����� �ǰԲ�
    void deleteEnemyInLists(GameObject objEnemy, Enemy enemy, SpriteRenderer enemySprite, Animator enemyAni)
    {
        enemies.Remove(objEnemy);
        enemyList.Remove(enemy);
        enemySprites.Remove(enemySprite);
        enemyEffects.Remove(enemyAni);
    }

    IEnumerator changeColor(SpriteRenderer enemySprite)
    {
        isCoroutining = true;
        enemySprite.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        enemySprite.color = Color.white;
        isCoroutining = false;
    }

    IEnumerator ResetToDefaultState(Animator enemyAni)   // �ִϸ��̼��� �⺻ ���·� ��ȯ
    {
        yield return new WaitForSeconds(0.4f);
        enemyAni.Play("None");
    }

    // null reference ���� �߻��� �Ƚ�Ű����
    void removeNullEnemiesFromLists()
    {
        // �ڿ������� ������ �ε��� ������ �� ����Ƿ�, �������� ��ȸ
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] == null)
            {
                // �ش� �ε����� �ִ� ��ҵ��� ��� ����
                enemies.RemoveAt(i);
                enemyList.RemoveAt(i);
                enemySprites.RemoveAt(i);
                enemyEffects.RemoveAt(i);
            }
        }
    }


    void Start()
    {
        bullet = FindObjectOfType<Bullet>();
        addEnemyInformationInLists();
    }

    void Update()
    {
        removeNullEnemiesFromLists();
    }
}
