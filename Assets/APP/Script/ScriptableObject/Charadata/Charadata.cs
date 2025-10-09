using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//============================================
//キャラデータのScriptableObject
//============================================

[CreateAssetMenu(menuName= "Data/Create StatusData")]
public class Charadata : ScriptableObject 
{
    public string NAME; //キャラ名
    public int MAXHP; //最大HP
    public int MAXMP; //最大MP
    public int ATK; //攻撃力
    public int DEF; //防御力
    public int INT; //魔力
    public int RES; // 魔法抵抗力
    public int AGI; //移動速度
    public int LV; //レベル
    public int GETEXP; //取得経験値
    public int GETGOLD; //取得ゴールド
    public float ShortAttackRange; //認識距離
    public float Enemytime; //反応時間
    public int EXP;//総獲得経験値
}
