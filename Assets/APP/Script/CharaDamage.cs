using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// キャラクターがダメージを受けるときの「基底クラス」
/// キャラクターのダメージ処理とHP管理を行うクラス
/// IDamageableインターフェースを実装し、攻撃を受けられるようにする
/// </summary>
public class CharaDamage : MonoBehaviour, IDamageable
{
    [Header("キャラクターデータ")]
    [SerializeField] protected Charadata charadata;  
    [SerializeField] protected Slider Slider;        

    [Header("ダメージUI")]
    [SerializeField] protected DamageTextManager damageTextManager; 

    [Header("キャラのステータス")]
    protected int HP;
    protected int MAXHP;
    protected int ATK;
    protected int DEF;
    protected int INT;
    protected int RES;
    protected int AGI;
    protected int EXP;

    [Header("プレイヤー専用設定")]
    [SerializeField] private TotalRuntimeStatus runtimeStatus;


    //無敵フラグ
    protected bool invincible = false;
    protected int invincibleTime = 0;

    protected virtual void Awake()
    {
        IsInitializeStatus();
    }

    /// <summary>
    /// ステータスの初期化処理（キャラの基礎ステータスの参照）
    /// 派生クラスでの使用可
    /// </summary>
    protected virtual void IsInitializeStatus()
    {
        //ステータスの取得
        var Status = charadata.GetCharaStatus();
        var Raw = charadata.GetRawStatus();

        if(charadata != null)
        {
            //HPゲージのスライダーを最大値(1.0)に設定
            Slider.value = 1;

            //Charadataから最大HPとステータスを初期化
            HP = Raw.MAXHP;
            MAXHP = Raw.MAXHP;
            ATK = Status.ATK;
            DEF = Status.DEF;
            INT = Status.INT;
            RES = Status.RES;
            AGI = Status.AGI;
            
        }
    }

    /// <summary>
    /// ダメージ処理を受けたときの処理
    /// virtualで定義して派生クラスで実装
    /// </summary>
    /// <param name="value"></param>
    public virtual void Damage(int value)
    {

        // 無敵状態チェック
        if (invincible && Time.frameCount < invincibleTime)
        {
            int remainingFrames = invincibleTime - Time.frameCount;
            Debug.Log($"[CharaDamage] 無敵中！ダメージ無効 (残り{remainingFrames}フレーム)");
            return;
        }
        // 無敵時間が終了していたらフラグをリセット
        if (Time.frameCount >= invincibleTime)
        {
            invincible = false;
        }

        var Raw = charadata.GetRawStatus();
        int defenseValue;

        if(runtimeStatus != null)
        {
            var totalStatus = runtimeStatus.GetTotalStatus();
            defenseValue = totalStatus.DEF;
        }
        else
        {
            var Status = charadata.GetCharaStatus();
            defenseValue = Status.DEF;
        }
        int actualDamage = value - defenseValue;
        HP -= actualDamage;
            
        // ダメージテキストを表示(派生クラスで表示方法を変更可能)
        OnDamageReceived(actualDamage);
        
        // HPゲージのスライダーを更新(現在HP / 最大HP)
        Slider.value = (float)HP / (float)Raw.MAXHP;

        // HPが0以下になったら死亡処理を実行
        if (HP <= 0)
        {
            Death();
        }
    }

    ///<summary>
    /// ダメージを受けた際のフィードバック
    /// ダメージテキストを表示
    /// <summary>
    protected virtual void OnDamageReceived(int damage)
    {
        Debug.Log($"[CharaDamage] 受けたダメージ: {damage}");
        
        // ダメージテキストを表示
        if (damageTextManager != null)
        {
            // HPバーの位置を基準にダメージテキストを表示
            Vector3 damageTextPosition = transform.position + Vector3.up * 2f; // キャラの頭上2m
            damageTextManager.ShowDamage(damage, damageTextPosition);
        }
    }

    public void SetInvincible(float time)
    {
        invincible = true;
        invincibleTime = Time.frameCount + (int)time;
        Debug.Log($"[CharaDamage] 無敵開始: {(int)time}フレーム (現在: {Time.frameCount}, 終了: {invincibleTime})");
    }
    /// <summary>
    /// 死亡処理
    /// </summary>
    protected virtual void Death()
    {
        Destroy(gameObject);
    }


    
    //================================================
    // アイテム効果用のメソッド（現在はプレイヤーしかアイテムを使用しない）
    //================================================

    /// <summary>
    /// HP回復
    /// </summary>
    public void Heal(int amount)
    {   
        HP += amount;
        if (HP > MAXHP) HP = MAXHP;

        // HPゲージ更新
        if (Slider != null)
        {
            Slider.value = (float)HP / (float)MAXHP;
        }

        Debug.Log($"[CharaDamage] Healed {amount}. Current HP: {HP}/{MAXHP}");
    }

    /// <summary>
    /// 攻撃力バフ
    /// </summary>
    public void AddAttackBuff(int amount, float duration)
    {
        StartCoroutine(BuffCoroutine(amount, duration, (val) => ATK += val, "ATK"));
    }

    /// <summary>
    /// 防御力バフ
    /// </summary>
    public void AddDefenseBuff(int amount, float duration)
    {
        StartCoroutine(BuffCoroutine(amount, duration, (val) => DEF += val, "DEF"));
    }

    /// <summary>
    /// 素早さバフ
    /// </summary>
    public void AddSpeedBuff(int amount, float duration)
    {
        StartCoroutine(BuffCoroutine(amount, duration, (val) => AGI += val, "AGI"));
    }

    /// <summary>
    /// バフ共通コルーチン
    /// </summary>
    private IEnumerator BuffCoroutine(int amount, float duration, System.Action<int> applyBuff, string statName)
    {
        applyBuff(amount);
        Debug.Log($"[CharaDamage] {statName} Buff applied: +{amount}");
        
        yield return new WaitForSeconds(duration);
        
        applyBuff(-amount);
        Debug.Log($"[CharaDamage] {statName} Buff expired: -{amount}");
    }
}
