using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

//IEnemyのインターフェースを宣言しているので、public int EnemyAIkoudou()のメソッドを作る必要あり。
public class Enemykoudou : MonoBehaviour,IEnemy
{
    //シリアル化している。charadataの敵を指定。
    [SerializeField] Charadata Charadata;
    [SerializeField] GameObject Player;
    Vector3 distanceToPlayer; //Playerとの距離
    int State;



    public int EnemyAIaction()
    {


        //キャラ距離計算の処理
        //敵の位置からPlayerの位置を引いた後にMathf.Absで絶対値を出すことで距離がわかる。

        /*
         distance = transform.position - Player.transform.position;

        float distanceX = Mathf.Abs(distance.x);
        float distanceZ = Mathf.Abs(distance.z);
        */
        float distanceToPlayer = Vector3.Distance(transform.position, Player.transform.position);


        //X座標とZ座標の距離のどちらが大きいか調べ、大きいほうの距離が敵のShortAttackRange以下であればStateを1として返す。攻撃を行う。

        if(distanceToPlayer <= Charadata.ShortAttackRange)
        {
            State = 1;
        }
        else
        {
            State = 0;
        }
        return State;

    }
}