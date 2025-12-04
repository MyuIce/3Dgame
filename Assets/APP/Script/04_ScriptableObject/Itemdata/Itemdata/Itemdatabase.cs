using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "Data/Create ItemDataBase")]
public class Itemdatabase : ScriptableObject
{

    [SerializeField]
    private List<Itemdata1> itemLists = new List<Itemdata1>();

    //　アイテムリストを返す
    public List<Itemdata1> GetItemLists()
    {
        return itemLists;
    }

}