using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemystate1 : MonoBehaviour
{
    Animator animator;
    [SerializeField] Charadata Charadata;
    [SerializeField] MonoBehaviour Enemykinsetu1;

    IEnemy Enemy_action;
    //bool isWaiting = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        Enemy_action = GetComponent<IEnemy>();

        // 最初の実行
        StartCoroutine(StateCheckLoop());
    }

    IEnumerator StateCheckLoop()
    {
        while (true)
        {
            float attackState = animator.GetFloat("Attack");
            if (attackState == 0 && Enemy_action != null)
            {
                int state = Enemy_action.EnemyAIaction();

                switch (state)
                {
                    case 0:
                        animator.SetFloat("Attack", 0f);
                        break;
                    case 1:
                        animator.SetFloat("Attack", 1f);
                        break;
                }
            }

            yield return new WaitForSeconds(Charadata.Enemytime); // 毎Enemytime秒に1回チェック
        }
    }
}
