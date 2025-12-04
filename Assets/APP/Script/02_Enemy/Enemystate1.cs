using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Idle = 0,      // 待機
    Chase = 1,     // 追跡
    Approach = 2,  // 接近(攻撃準備)
    Attack = 3,    // 攻撃中
    Cooldown = 4   // クールダウン
}

/// <summary>
/// 敵AIの状態管理と行動制御を行うクラス
/// プレイヤーを追跡し、攻撃範囲内で攻撃を実行する
/// </summary>
public class Enemystate1 : MonoBehaviour, IEnemy
{
    [Header("参照")]
    [SerializeField] Charadata charadata;
    [SerializeField] GameObject player;
    
    Animator animator;
    CharacterController characterController;

    [Header("距離パラメータ")]
    [SerializeField] float detectionRange = 15f;  // プレイヤー検知範囲
    [SerializeField] float chaseRange = 20f;      // 追跡範囲
    [SerializeField] float attackRange = 2.5f;    // 攻撃範囲
    [SerializeField] float optimalDistance = 2.0f; // 最適距離(未使用)

    [Header("行動パラメータ")]
    [SerializeField] float moveSpeed = 3f;        // 移動速度
    [SerializeField] float rotationSpeed = 5f;    // 回転速度
    [SerializeField] float attackCooldown = 1f;   // 攻撃クールダウン時間
    [SerializeField] float approachTimeout = 2f;  // Approachタイムアウト時間

    [Header("デバッグ")]
    [SerializeField] bool showDebugGizmos = true; // デバッグ用Gizmosの表示

    private EnemyState currentState = EnemyState.Idle;
    private float attackCooldownTimer = 0f;
    private bool isAttacking = false;
    private float approachTimer = 0f;  // Approach状態の経過時間

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        
        // プレイヤーが未設定の場合、自動で検索
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError($"[{gameObject.name}] Playerが見つかりません。Playerタグを設定してください。");
            }
        }
    }

    void Update()
    {
        // クールダウンタイマーの更新
        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;
        
        // AI行動の実行
        EnemyAIaction();
    }

    /// <summary>
    /// 敵AIの行動判断と実行
    /// IEnemyインターフェースの実装
    /// </summary>
    public int EnemyAIaction()
    {
        if (player == null) return (int)currentState;

        float distance = Vector3.Distance(transform.position, player.transform.position);

        // 状態遷移の判断
        switch (currentState)
        {
            case EnemyState.Idle:
                // 待機状態: プレイヤーが検知範囲内に入ったら追跡開始
                if (distance <= detectionRange)
                {
                    currentState = EnemyState.Chase;
                    // Chaseへ遷移時のアニメーション
                    animator.SetFloat("Attack", 0f);
                }
                else
                {
                    // Idle時のアニメーション
                    //animator.SetFloat("Attack", 0f);
                    animator.SetFloat("MoveX", 0f);
                    animator.SetFloat("MoveZ", 0f);
                }
                break;

            case EnemyState.Chase:
                // 追跡状態: プレイヤーを追いかける
                if (distance > chaseRange)
                {
                    // 追跡範囲外なら待機に戻る
                    currentState = EnemyState.Idle;
                    // Idleへ遷移時のアニメーション
                    //animator.SetFloat("Attack", 0f);
                    animator.SetFloat("MoveX", 0f);
                    animator.SetFloat("MoveZ", 0f);
                }
                else if (distance <= attackRange && attackCooldownTimer <= 0)
                {
                    // 攻撃範囲内かつクールダウン完了なら攻撃準備
                    currentState = EnemyState.Approach;
                    approachTimer = 0f;  // タイマーをリセット
                    // Approachへ遷移時のアニメーション
                    //animator.SetFloat("Attack", 0f);
                    animator.SetFloat("MoveX", 0f);
                    animator.SetFloat("MoveZ", 0f);
                }
                else
                {
                    // プレイヤーを追跡（移動処理とアニメーション更新）
                    ChasePlayer();
                    //animator.SetFloat("Attack", 0f);
                }
                break;

            case EnemyState.Approach:
                // 接近状態: プレイヤーの方を向く
                approachTimer += Time.deltaTime;
                
                // プレイヤーが攻撃範囲外に出たらChaseに戻る
                if (distance > attackRange)
                {
                    currentState = EnemyState.Chase;
                    approachTimer = 0f;
                    // Chaseへ遷移時のアニメーション
                    //animator.SetFloat("Attack", 0f);
                }
                // タイムアウト: 一定時間内に向けなかったらChaseに戻る
                else if (approachTimer > approachTimeout)
                {
                    currentState = EnemyState.Chase;
                    approachTimer = 0f;
                    // Chaseへ遷移時のアニメーション
                    //animator.SetFloat("Attack", 0f);
                }
                // 30度以内に向いたら攻撃
                else if (IsPlayerInFront(30f))
                {
                    currentState = EnemyState.Attack;
                    approachTimer = 0f;
                    // Attackへ遷移時のアニメーション
                    animator.SetFloat("Attack", 1f);
                    animator.SetFloat("MoveX", 0f);
                    animator.SetFloat("MoveZ", 0f);
                }
                else
                {
                    FacePlayer();
                    // Approach継続時のアニメーション
                    animator.SetFloat("Attack", 0f);
                    animator.SetFloat("MoveX", 0f);
                    animator.SetFloat("MoveZ", 0f);
                }
                break;

            case EnemyState.Attack:
                // 攻撃状態: 攻撃実行
                Debug.Log($"[{gameObject.name}] Entering Attack state! Setting cooldown to {attackCooldown}s");
                isAttacking = true;
                attackCooldownTimer = attackCooldown;
                
                // Attack時のアニメーション
                animator.SetFloat("Attack", 1f);
                animator.SetFloat("MoveX", 0f);
                animator.SetFloat("MoveZ", 0f);

                StartCoroutine(AttackWait());
                currentState = EnemyState.Cooldown;
                break;

            case EnemyState.Cooldown:
                // クールダウン状態: 攻撃後の待機
                if (attackCooldownTimer <= 0)
                {
                    isAttacking = false;
                    // クールダウン終了後は必ずChase状態に戻る（連続攻撃を防ぐ）
                    currentState = EnemyState.Chase;
                    // Chaseへ遷移時のアニメーション
                    //animator.SetFloat("Attack", 0f);
                }
                else
                {
                    // Cooldown継続時のアニメーション
                    animator.SetFloat("MoveX", 0f);
                    animator.SetFloat("MoveZ", 0f);
                }
                break;
        }

        // デバッグログ（1秒ごとに表示）
        if (showDebugGizmos && Time.frameCount % 60 == 0)
        {
            float moveX = animator != null ? animator.GetFloat("MoveX") : 0f;
            float moveZ = animator != null ? animator.GetFloat("MoveZ") : 0f;
            Debug.Log($"[{gameObject.name}] State: {currentState}, Distance: {distance:F2}m, " +
                      $"CooldownTimer: {attackCooldownTimer:F2}s, ApproachTimer: {approachTimer:F2}s, " +
                      $"MoveX: {moveX:F2}, MoveZ: {moveZ:F2}");
        }

        return (int)currentState;
    }

    /// <summary>
    /// プレイヤーを追跡する
    /// </summary>
    void ChasePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0; // Y軸方向の移動を無効化(水平移動のみ)

        // CharacterControllerがあれば使用、なければtransform.positionを直接変更
        if (characterController != null)
        {
            characterController.Move(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        // Blend Tree用のパラメータを更新
        UpdateMovementAnimation(direction);

        FacePlayer();
    }

    /// <summary>
    /// プレイヤーの方を向く(スムーズな回転)
    /// </summary>
    void FacePlayer()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0; // Y軸は固定(水平回転のみ)

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// プレイヤーが前方にいるかチェック
    /// </summary>
    /// <param name="angleThreshold">判定角度(度)</param>
    /// <returns>前方にいればtrue</returns>
    bool IsPlayerInFront(float angleThreshold)
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        directionToPlayer.y = 0; // 高さの違いを無視（水平方向のみで判定）
        
        if (directionToPlayer.sqrMagnitude < 0.001f) return true; // ほぼ同じ位置なら正面とみなす

        float angle = Vector3.Angle(transform.forward, directionToPlayer.normalized);
        return angle < angleThreshold;
    }

    /// <summary>
    /// 移動アニメーションのパラメータを更新
    /// ワールド座標の移動方向をローカル座標系に変換してBlend Treeに渡す
    /// </summary>
    /// <param name="worldDirection">ワールド座標での移動方向</param>
    void UpdateMovementAnimation(Vector3 worldDirection)
    {
        if (animator == null) return;

        // ワールド座標の移動方向をローカル座標系に変換
        Vector3 localDirection = transform.InverseTransformDirection(worldDirection);

        // Blend Treeのパラメータを更新
        // MoveX: 左右の移動 (-1: 左, 0: なし, 1: 右)
        // MoveZ: 前後の移動 (-1: 後ろ, 0: なし, 1: 前)
        animator.SetFloat("MoveX", localDirection.x);
        animator.SetFloat("MoveZ", localDirection.z);
    }

    /// <summary>
    /// デバッグ用Gizmosの描画
    /// </summary>
    void OnDrawGizmosSelected()
    {
        if (!showDebugGizmos) return;

        // 検知範囲(黄色)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // 追跡範囲(オレンジ)
        Gizmos.color = new Color(1f, 0.5f, 0f);
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        // 攻撃範囲(赤)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // プレイヤーへの線
        if (player != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
    }

    IEnumerator AttackWait()
    {
        yield return new WaitForSeconds(0.5f); // 攻撃している時間

        animator.SetFloat("Attack", 0f);
        currentState = EnemyState.Cooldown;
    }
}