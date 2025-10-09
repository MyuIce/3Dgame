using UnityEngine;


[CreateAssetMenu(menuName = "Data/Create ItemData")]
public class Itemdata1 : ScriptableObject,GameItem
{
    public enum itemtype
    {
        Sword,recovery, collection, important

    }
    
    //============================================
    //アイテム情報定義
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
    private int Itemlimit; //最大個数


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
}