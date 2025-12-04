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
    protected override void Death()
    {
        Debug.Log("敵死亡");
        SpawnTreasure();
        base.Death();
    }

    /// <summary>
    /// 宝箱生成
    /// </summary>
    private void SpawnTreasure()
    {
        Vector3 spawnPos = dropPoint != null ? dropPoint.position : transform.position;
        Instantiate(treasureBoxPrefab, spawnPos, Quaternion.identity);
        Debug.Log("宝箱生成");
    }
}
