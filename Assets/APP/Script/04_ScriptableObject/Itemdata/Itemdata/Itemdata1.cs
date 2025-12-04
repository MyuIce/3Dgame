using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create ItemData")]
public class Itemdata1 : ScriptableObject, IGameItem
{
    public enum itemtype
    {
        Sword, recovery, collection, important
    }

    public enum EffectType
    {
        None,
        Heal,        // HP回復
        BuffAttack,  // 攻撃力UP
        BuffDefense, // 防御UP
        BuffSpeed    // 速度UP
    }

    //============================================
    //変数定義
    //============================================

    [SerializeField]
    private string Itemname; //名前
    [SerializeField]
    private itemtype Itemtype; //タイプ
    [SerializeField]
    private Sprite Itemicon; //アイコン
    [SerializeField]
    private string Itemexplanation; //説明
    [SerializeField]
    private int Itemlimit; //個数上限

    [SerializeField]
    public EffectType effectType; // 効果の種類
    [SerializeField]
    public int value;             // 効果の値
    [SerializeField]
    public float duration;        // バフの持続時間

    public string GetItemname()
    {
        return Itemname;
    }
    public itemtype GetItemtype()
    {
        return Itemtype;
    }
    public Sprite GetItemicon()
    {
        return Itemicon;
    }
    public string GetItemexplanation()
    {
        return Itemexplanation;
    }
    public int GetItemlimit()
    {
        return Itemlimit;
    }

    public int GetValue()
    {
        return value;
    }
    public float GetDuration()
    {
        return duration;
    }

    public void Use(GameObject user)
    {
        var status = user.GetComponent<CharaDamage>();
        if (status == null) return;

        switch (effectType)
        {
            case EffectType.Heal:
                status.Heal(value);
                break;

            case EffectType.BuffAttack:
                status.AddAttackBuff(value, duration);
                break;

            case EffectType.BuffDefense:
                status.AddDefenseBuff(value, duration);
                break;

            case EffectType.BuffSpeed:
                status.AddSpeedBuff(value, duration);
                break;
        }
    }
}