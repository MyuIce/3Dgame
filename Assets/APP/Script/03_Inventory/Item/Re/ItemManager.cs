using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ItemInventory.cs��ItemInventoryUI.cs�̃f�[�^��UI�̕R�Â��Ǘ�
/// 
/// Start()
/// AddInitialItems()
/// InitializeInventory()
/// 
/// </summary>

public class ItemManager : MonoBehaviour
{
    [Header("�Q�Ɛݒ�")]
    [SerializeField] private Itemdatabase itemDatabase;
    [SerializeField] private ItemInventoryUI inventoryUI;

    private ItemInventory inventory;

    private void Start()
    {
        // �f�[�^�w����
        inventory = new ItemInventory();
        inventory.Initialize(itemDatabase.GetItemLists());
        


        // �����A�C�e���o�^�i�C�Ӂj
        if (itemDatabase.GetItemLists().Count >= 2)
        {
            inventory.AddItem(itemDatabase.GetItemLists()[0], 1);
            inventory.AddItem(itemDatabase.GetItemLists()[2], 2);
            
            inventory.AddItem(itemDatabase.GetItemLists()[4], 1);
        }
        // UI�w������
        inventoryUI.Initialize(inventory);

    }

    /// <summary>
    /// �O������Ăяo����A�C�e���ǉ��֐�
    /// </summary>
    public bool AddItem(Itemdata1 item, int amount)
    {
        if (inventory.AddItem(item, amount))
            inventoryUI.UpdateUI();
        Debug.Log("�ǉ�������");
        return true;

    }
}
