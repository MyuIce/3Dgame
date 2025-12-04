using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>
/// プレイヤー専用ダメージ処理
/// CharaDamageを継承
/// [追加]なし
/// </summary>
public class PlayerDamage : CharaDamage,IDamageable
{
    protected override void Awake()
    {
        base.Awake();  // 基底クラスの初期化を呼び出す
        
    }

    /*
    public override void Damage(int value)
    {
        // 無敵状態チェック（基底クラスと同じ）
        if (invincible && Time.frameCount < invincibleTime)
        {
            int remainingFrames = invincibleTime - Time.frameCount;
            Debug.Log($"[PlayerDamage] 無敵中！ダメージ無効 (残り{remainingFrames}フレーム)");
            return;
        }
        if (Time.frameCount >= invincibleTime)
        {
            invincible = false;
        }

        var Raw = charadata.GetRawStatus();
        
        // 装備込みの最終ステータスを取得
        var totalStatus = runtimeStatus.GetTotalStatus();
        
        // ダメージ計算: 敵の攻撃力 - プレイヤーの装備込み防御力
        int actualDamage = value - totalStatus.DEF;
        
        Debug.Log($"[PlayerDamage] ダメージ計算: 敵ATK={value}, プレイヤーDEF={totalStatus.DEF}, 計算結果={actualDamage}");
        
        // 最低1ダメージは受ける（オプション）
        if (actualDamage < 1) actualDamage = 1;
        
        HP -= actualDamage;
        
        // ダメージテキストを表示
        OnDamageReceived(actualDamage);
        
        // HPゲージのスライダーを更新
        Slider.value = (float)HP / (float)Raw.MAXHP;
        
        // HPが0以下になったら死亡処理を実行
        if (HP <= 0)
        {
            Death();
        }
    }
    */

    protected override void Death()
    {
        Debug.Log("プレイヤー死亡");
        base.Death();
    }
}
