using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵専用ダメージ処理
/// CharaDamageを継承
/// [追加]宝箱ドロップ
/// </summary>
public class EnemyDamage : CharaDamage
{
    [SerializeField] private GameObject treasureBoxPrefab;
    [SerializeField] private Transform dropPoint;

    /// <summary>
    /// 死亡処理
    /// </summary>
    private Animator anim;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    public override void Death()
    {
        Debug.Log("敵死亡");
        SpawnTreasure();
        
        // アニメーション再生
        if (anim != null)
        {
            anim.SetInteger("Death", 1);
        }
        else
        {
            Debug.Log("アニメーション再生失敗");
        }
        
    }

    /// <summary>
    /// 宝箱生成
    /// </summary>
    private void SpawnTreasure()
    {
        Vector3 spawnPos = dropPoint != null ? dropPoint.position : transform.position;

        Quaternion spawnRotation =  Quaternion.Euler(0, 180, 0);
        Instantiate(treasureBoxPrefab, spawnPos, spawnRotation);
        Debug.Log("宝箱生成");
    }

    
}
