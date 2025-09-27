using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //シリアル化している。charadataのPlayer1を指定。
    [SerializeField] private Charadata Charadata;

    //剣がゲームオブジェクトに侵入した瞬間に呼び出し
    void OnTriggerEnter(Collider other)
    {
        //otherのゲームオブジェクトのインターフェースを呼び出す
        IDamageable damageable = other.GetComponent<IDamageable>();

        //damageableにnull値が入っていないかチェック
        if (damageable != null)
        {
            //damageableのダメージ処理メソッドを呼び出す。引数としてPlayer1のATKを指定
            damageable.Damage(Charadata.ATK);
        }
    }
}