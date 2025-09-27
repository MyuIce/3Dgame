using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using UnityEngine.UI;


public class CharaDamage : MonoBehaviour,IDamageable
{
    //シリアル化している。CharadataのMini_Swordmanを指定。
    [SerializeField] private Charadata Charadata;
    [SerializeField] Slider Slider;

    int HP;
    int MAXHP;
    int ATK;
    int DEF;
    int INT;
    int RES;
    int EXP;

    void Start()
    {
        //Charadataがnullではないことを確認
        if(Charadata != null)
        {
            //valueのHPゲージのスライダーを最大の1に
            Slider.value = 1;

            //Charadataの最大HPを代入
            HP = Charadata.MAXHP;
            MAXHP = Charadata.MAXHP;
            ATK = Charadata.ATK;
            DEF = Charadata.DEF;
            INT = Charadata.INT;
            RES = Charadata.RES;
            EXP = Charadata.GETEXP;
        }
    }

    //ダメージ処理メソッド(valueにはPlayer1のATKの値が入っている)
    public void Damage(int value)
    {
        //Charadataがnullではないことを確認
        if(Charadata != null)
        {
            HP -= value - Charadata.DEF;
            //HPゲージに反映
            Slider.value = (float)HP / (float)Charadata.MAXHP;
        }
        if(HP <= 0)
        {
            Death();
        }
    }

    //死亡処理メソッド
    public void Death()
    {
        Destroy(gameObject);
        /*
         //獲得経験値があるなら経験値処理
        if (Kakutokuexp > 0)
        {
            Charadata.GETEXP = Charadata.GETEXP;

            var a = Lvdata.playerExpTable[Charadata.LV];

            if (Charadata.GETEXP >= a.exp)
            {
                Charadata.LV += 1;
            }


        }
        */
    }
}
