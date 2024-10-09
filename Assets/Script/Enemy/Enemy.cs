using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hp = 3;

    private GameObject enemy;
    private Animator enemyEffects;  // Animator ������Ʈ

    public void destroyEnemy()
    {
        Destroy(gameObject);
    }

    public IEnumerator PlayDeathAnimationAndDestroy()
    {
        enemyEffects.Play(enemy.name + "Die");
        yield return new WaitForSeconds(enemyEffects.GetCurrentAnimatorStateInfo(0).length);
        // �ִϸ��̼� ��� �� ������Ʈ ����
        destroyEnemy();
    }

    void Start()
    {
        enemy = this.gameObject;
        enemyEffects = GetComponent<Animator>();
    }

    void Update()
    {
        if (hp == 0)
        {
            StartCoroutine(PlayDeathAnimationAndDestroy());
        }
    }
}
