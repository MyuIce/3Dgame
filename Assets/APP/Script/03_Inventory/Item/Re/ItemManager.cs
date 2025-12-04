using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ItemInventory.csとItemInventoryUI.csのデータとUIの紐づけ管理
/// 
/// Start()
/// AddInitialItems()
/// InitializeInventory()
/// 
/// </summary>

public class ItemManager : MonoBehaviour
{
    [Header("参照設定")]
    [SerializeField] private Itemdatabase itemDatabase;
    [SerializeField] private ItemInventoryUI inventoryUI;

    private ItemInventory inventory;

    private void Start()
    {
        // データ層生成
        inventory = new ItemInventory();
        inventory.Initialize(itemDatabase.GetItemLists());
        


        // 初期アイテム登録（任意）
        if (itemDatabase.GetItemLists().Count >= 2)
        {
            inventory.AddItem(itemDatabase.GetItemLists()[0], 1);
            inventory.AddItem(itemDatabase.GetItemLists()[1], 2);
            
            inventory.AddItem(itemDatabase.GetItemLists()[3], 1);
        }
        // UI層初期化
        inventoryUI.Initialize(inventory);

    }

    /// <summary>
    /// 外部から呼び出せるアイテム追加関数
    /// </summary>
    public bool AddItem(Itemdata1 item, int amount)
    {
        if (inventory.AddItem(item, amount))
            inventoryUI.UpdateUI();
        Debug.Log("追加したよ");
        return true;

    }
}
