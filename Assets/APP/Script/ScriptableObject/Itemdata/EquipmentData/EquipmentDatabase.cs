using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(menuName = "Data/Create EquipmentDataBase")]
public class EquipmentDatabase : ScriptableObject
{
    //EquipmentData1というデータベースを作成
    [SerializeField]
    private List<EquipmentData1> itemLists = new List<EquipmentData1>();

    //　アイテムリストを返す
    public List<EquipmentData1> GetItemLists()
    {
        return itemLists;
    }

}