using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemies = new();
    [SerializeField] List<Enemy> enemyList = new();
    [SerializeField] List<SpriteRenderer> enemySprites = new();
    [SerializeField] List<Animator> enemyEffects = new();  // Animator override 해서 하고 싶지만 실패, 추후 수정 예정

    private Bullet bullet;
    private bool isCoroutining = false; // 중첩되서 실행되는 것을 방지

    public void takeDamage(string enemyTag) // name : bullet에서 가져온 enemy의 tag
    {
        (GameObject objEnemy, Enemy enemy, SpriteRenderer enemySprite, Animator enemyAni) 
            = getEnemyInformation(enemyTag);

        enemy.hp -= 1;
        if (enemy.isDead != true && !isCoroutining && enemy.hp > 0)
        {
            Debug.Log("hp 까임");
            enemyAni.Play(enemy.tag + "Hit");
            StartCoroutine(changeColor(enemySprite));
            StartCoroutine(ResetToDefaultState(enemyAni));
        }

        if (!isCoroutining && enemy.hp ==0)
        {
            enemy.isDead = true;
            destroyEnemy(objEnemy, enemy, enemySprite, enemyAni);  // 적 파괴
        }
    }

    // 실행하면 자동적으로 enemy관련 components들 list에 추가되도록 설정
     void addEnemyInformationInLists()
    {
        for(int i=0; i< enemies.Count -1; i++)
        {
            enemyList.AddRange(GetComponentsInChildren<Enemy>());
            enemySprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
            enemyEffects.AddRange(GetComponentsInChildren<Animator>());
        }
    }

    // 튜플로 여러 형식을 반환하게 함
    // Damage를 받는 해당 오브젝트의 Components들을 list들에서 반환
    (GameObject , Enemy, SpriteRenderer , Animator) getEnemyInformation(string enemyTag)
    {
        int objIndex = enemies.FindIndex(x => x.tag.Equals(enemyTag));
        GameObject obj = enemies[objIndex];

        int enemyIndex = enemyList.FindIndex(x => x.tag.Equals(enemyTag));
        Enemy enemy = enemyList[enemyIndex];

        int spriteIndex = enemySprites.FindIndex(x => x.tag.Equals(enemyTag));
        SpriteRenderer enemySprite = enemySprites[spriteIndex];

        int aniIndex = enemySprites.FindIndex(x => x.tag.Equals(enemyTag));
        Animator enemyAni = enemyEffects[aniIndex];

        return (obj, enemy, enemySprite, enemyAni);
    }


    void destroyEnemy(GameObject objEnemy, Enemy enemy, SpriteRenderer enemySprite, Animator enemyAni)
    {
        string enemyTag = enemy.tag;
        switch (enemyTag)
        {
            case "pinkDollEnemy":
                {
                    deleteEnemyInLists(objEnemy, enemy, enemySprite, enemyAni);
                    enemyAni.Play(enemyTag + "Hit");
                    break;
                }
            case "rabbitDollEnemy":
                {
                    deleteEnemyInLists(objEnemy, enemy, enemySprite, enemyAni);
                    enemyAni.Play(enemyTag + "Hit");
                    break;
                }
        }
    }

    // 파괴되는 것 또한 자동으로 되게끔
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
        yield return new WaitForSeconds(0.3f);
        enemySprite.color = Color.white;
        isCoroutining = false;
    }

    IEnumerator ResetToDefaultState(Animator enemyAni)   // 애니메이션을 기본 상태로 전환
    {
        yield return new WaitForSeconds(0.4f);
        enemyAni.Play("None");
    }


    void Start()
    {
        bullet = FindObjectOfType<Bullet>();
        addEnemyInformationInLists();
    }

     void Update()
    {
    }
}
