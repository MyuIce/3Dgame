using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;

using static UnityEditor.PlayerSettings;

public class PlayerMovement : MonoBehaviour
{
    Animator animator; // Animatorのコンポーネント
    Rigidbody rb;
    RaycastHit ground;

    private Transform cam;//カメラの位置
    public float speed = 1f;
    public float rotationSpeed = 1f;

    float moveZ; // 前後移動量
    float moveY; // 上下移動量
    float moveX; // 左右移動量

    float normalSpeed = 4f; // 通常の移動速度
    public float jumpSpeed = 250f; // ジャンプ速度
    public float dashSpeed = 1500f;//ダッシュ速度
    float dashtime = 0.3f;//ダッシュ時間
    float dashCool = 1.5f;//ダッシュクールタイム
    bool dashPermission = true;//ダッシュ許可

    Vector3 moveDirection = Vector3.zero;//通常移動方向
    Vector3 dashDirection = Vector3.zero;//ダッシュ移動方向

    Vector3 startPosition;   // 開始位置
    Vector3 currentRotation;   // 現在の回転
    Vector3 originalRotation; // 回転記憶
    Vector3 raycalc; //光計算


    float mouseX;   // マウス移動量X
    float mouseY;   // マウス移動量Y

    //bool Ground_tuku = false; // 接地判定フラグ
    float Attack; //攻撃
    bool Combo = false; //コンボ許容
    bool Attack_Possible = true; //攻撃可能
    float ComboEndtime = 1f; //コンボ終了時のクールタイム

    void Start()
    {
        animator = GetComponent<Animator>(); // Animatorのコンポーネントを取得
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;

        // マウスカーソルを非表示にし、位置を固定
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // 開始位置を記録
        startPosition = transform.position;
    }

    void Update()
    {
        // 移動前に回転を一時的にリセット（X,Z軸）
        currentRotation = transform.eulerAngles;
        originalRotation = transform.eulerAngles;
        currentRotation.x = 0f;
        currentRotation.z = 0f;
        transform.eulerAngles = currentRotation;

        // 前後左右の入力を取得
        moveZ = Input.GetAxis("Vertical");
        moveX = Input.GetAxis("Horizontal");
        moveDirection = new Vector3(moveX, 0, moveZ).normalized * normalSpeed * Time.deltaTime;
        transform.Translate(moveDirection.x, moveDirection.y, moveDirection.z);

        // アニメーターに移動速度を渡す
        animator.SetFloat("MoveSpeed", moveDirection.magnitude);

        //攻撃のアニメーション(コンボ、通常)
        if(Attack_Possible == true)
        {
            animator = GetComponent<Animator>();
            Attack = animator.GetFloat("Attack");

            if(Combo == true)
            {
                if(Attack==4)
                {
                    animator.SetFloat("Attack", 0f);
                    Attack_Possible = false;
                    StartCoroutine(ComboEnd());
                }
                if(Input.GetMouseButtonDown(0)) 
                {
                    Combo = false;
                    Attack += 1f;
                    animator.SetFloat("Attack", Attack);
                }
            }
            else
            {
                if (Attack == 0)
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        animator.SetFloat("Attack", 1f);
                    }
                }
            }
        }

        //ダッシュの処理
        if(Input.GetButtonDown("Fire3") && dashPermission == true)
        {
            dashDirection = new Vector3(moveX, 0, moveZ).normalized;
            rb.AddForce(transform.TransformDirection(dashDirection) * dashSpeed, ForceMode.Impulse);
            dashPermission = false;

            StartCoroutine(Dashwari());
        }

        //カメラの処理
        Vector3 movePlayer = new Vector3(moveZ, 0, moveX).normalized;
        if (movePlayer != Vector3.zero)
        {
            Vector3 camForward = cam.forward;
            Vector3 camRight = cam.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = camForward * movePlayer.z + camRight * movePlayer.x;

            rb.MovePosition(rb.position + moveDirection.normalized * speed * Time.deltaTime);

            Quaternion targetRot = Quaternion.LookRotation(camForward);
            transform.rotation = targetRot;
        }

        // アニメーションの再生方向を判定
        float absMoveZ = Mathf.Abs(moveDirection.z);
        float absMoveX = Mathf.Abs(moveDirection.x);

        //アニメーションの遷移処理
        if (absMoveZ > 0f || absMoveX > 0f)
        {
            if (absMoveZ > absMoveX)
            {
                if (moveDirection.z >= 0f)
                {
                    // 前進アニメーション
                    animator.SetBool("Forward", true);
                    animator.SetBool("Back", false);
                    animator.SetBool("Right", false);
                    animator.SetBool("Left", false);
                }
                else
                {
                    // 後退アニメーション
                    animator.SetBool("Forward", false);
                    animator.SetBool("Back", true);
                    animator.SetBool("Right", false);
                    animator.SetBool("Left", false);
                }
            }
            else
            {
                if (moveDirection.x >= 0)
                {
                    // 右移動アニメーション
                    animator.SetBool("Forward", false);
                    animator.SetBool("Back", false);
                    animator.SetBool("Right", true);
                    animator.SetBool("Left", false);
                }
                else
                {
                    // 左移動アニメーション
                    animator.SetBool("Forward", false);
                    animator.SetBool("Back", false);
                    animator.SetBool("Right", false);
                    animator.SetBool("Left", true);
                }
            }
        }
        else
        {
            // 移動していないときはすべてのアニメーションを停止
            animator.SetBool("Forward", false);
            animator.SetBool("Back", false);
            animator.SetBool("Right", false);
            animator.SetBool("Left", false);
        }

        raycalc = transform.position;
        raycalc.y += 0.5f;

        
        //ジャンプアニメーション設定
        if (Physics.SphereCast(raycalc, 0.3f, Vector3.down, out ground, 0.6f))
        {
            //Ground_tuku = true;
            animator.SetBool("Jump", false);

            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            }

        }
        else
        {
            //Ground_tuku = false;
            animator.SetBool("Jump", true);
        }

        
    }

    //ダッシュ関数
    IEnumerator Dashwari()
    {
        //ダッシュを終わらせる
        yield return new WaitForSeconds(dashtime);
        rb.velocity = Vector3.zero;

        //ダッシュクールタイムだけ待機
        yield return new WaitForSeconds(dashCool);
        dashPermission = true;
    }
    void FootR()
    {
        //本当はここに足音入れる
    }

    void FootL()
    {
        //本当はここに足音入れる
    }

    void Hit()
    {
        //攻撃ヒット時に使う？
        Combo = true;
        Debug.Log("攻撃ヒット");
        
    }

    void AttackEnd()
    {
        //攻撃アニメーション終了時の処理
        Combo = false;
        animator = GetComponent<Animator>();
        animator.SetFloat("Attack", 0f);
    }
    IEnumerator ComboEnd()
    {
        //コンボ終了待機
        yield return new WaitForSeconds(ComboEndtime);
        Attack_Possible = true;
    }

}

