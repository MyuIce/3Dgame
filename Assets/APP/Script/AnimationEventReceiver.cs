using UnityEngine;

/// <summary>
/// アニメーションイベントを受け取り、適切なコンポーネントに転送する汎用レシーバー
/// プレイヤーと敵で同じアニメーションクリップを使用できるようにする
/// </summary>

/*
public class AnimationEventReceiver : MonoBehaviour
{
    // プレイヤー用のコンポーネント(オプション)
    private PlayerMovement playerMovement;
    
    // 敵用のコンポーネント(オプション)
    private Enemykinsetu1 enemyKinsetu;
    
    // 汎用的な攻撃管理インターフェース
    private ICharaAttack charaAttack;

    void Start()
    {
        // 自動的に適切なコンポーネントを取得
        playerMovement = GetComponentInParent<PlayerMovement>();
        enemyKinsetu = GetComponentInParent<Enemykinsetu1>();
        charaAttack = GetComponentInParent<ICharaAttack>();
    }

    /// <summary>
    /// 攻撃開始イベント(アニメーションから呼ばれる)
    /// </summary>
    public void AttackStart()
    {
        // 敵の場合
        if (enemyKinsetu != null)
        {
            enemyKinsetu.AttackStart();
        }
        
        // プレイヤーの場合は特に処理なし(必要なら追加)
    }

    /// <summary>
    /// ヒット判定イベント(アニメーションから呼ばれる)
    /// </summary>
    public void Hit()
    {
        // 敵の場合
        if (enemyKinsetu != null)
        {
            enemyKinsetu.Hit();
        }
        
        // プレイヤーの場合は特に処理なし(必要なら追加)
    }

    /// <summary>
    /// 攻撃終了イベント(アニメーションから呼ばれる)
    /// </summary>
    public void AttackEnd()
    {
        // プレイヤーの場合
        if (playerMovement != null)
        {
            playerMovement.OnAttackEndFromAnimation();
        }
        
        // 敵の場合
        if (enemyKinsetu != null)
        {
            enemyKinsetu.AttackEnd();
        }
    }
}
*/
