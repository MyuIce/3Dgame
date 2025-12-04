using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// シーン間でインベントリデータを保持するマネージャー
/// ビジネスロジックは各マネージャークラス（Soubikanri、ItemInventory(Itemのデータクラス)等）に委譲
/// </summary>
public class InventoryManager : MonoBehaviour
{
    //シングルトン化
    public static InventoryManager Instance { get; private set; }

    [Header("データベース参照")]
    [SerializeField] private EquipmentDatabase equipmentDatabase;

    // -------------------------------------------------------------------------
    // 公開データ（各マネージャーがアクセス）
    // -------------------------------------------------------------------------
    
    /// <summary>
    /// アイテムインベントリ（ItemInventoryクラスがロジックを持つ）
    /// </summary>
    public ItemInventory ItemInventory { get; private set; }
    
    /// <summary>
    /// 装備の所持数（Soubikanriが管理するデータの実体） Dictionary<key：装備、value：その装備を何個持ってるか>
    /// </summary>
    public Dictionary<EquipmentData1, int> EquipmentCounts { get; private set; }
    
    /// <summary>
    /// 現在装備中のアイテム（Soubikanriが管理するデータの実体） Dictionary<key：装備タイプ、value：その装備タイプを装備している装備>
    /// </summary>
    public Dictionary<EquipmentData1.Equipmenttype, EquipmentData1> EquippedItems { get; private set; }

    // -------------------------------------------------------------------------
    // 初期化
    // -------------------------------------------------------------------------
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeData();
            Debug.Log("[InventoryManager] インスタンスを作成しました（データ保持専用）");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeData()
    {
        // データ構造の初期化のみ
        ItemInventory = new ItemInventory();
        EquipmentCounts = new Dictionary<EquipmentData1, int>();
        EquippedItems = new Dictionary<EquipmentData1.Equipmenttype, EquipmentData1>();
        
        // 装備データベースから全装備を初期化
        if (equipmentDatabase != null)
        {
            //すべての装備を取得
            var allEquipments = equipmentDatabase.GetItemLists();
            foreach (var equipment in allEquipments)
            {
                //すべての装備の所持数を0にする
                EquipmentCounts[equipment] = 0;
            }
        }
    }

    // -------------------------------------------------------------------------
    // ユーティリティ（デバッグ用）
    // -------------------------------------------------------------------------
    
    /// <summary>
    /// 全データをリセット（Newゲームなど用）
    /// </summary>
    public void ResetAllData()
    {
        ItemInventory = new ItemInventory();
        
        foreach (var key in new List<EquipmentData1>(EquipmentCounts.Keys))
        {
            EquipmentCounts[key] = 0;
        }
        
        EquippedItems.Clear();
        
        Debug.Log("[InventoryManager] 全データをリセットしました");
    }

    /// <summary>
    /// デバッグ情報を出力
    /// </summary>
    public void DebugLogInventory()
    {
        Debug.Log("===== インベントリ状況 =====");
        
        Debug.Log("--- アイテム ---");
        var ownedItems = ItemInventory.GetOwnedItems();
        foreach (var item in ownedItems)
        {
            Debug.Log($"{item.GetItemname()}: {ItemInventory.GetitemCount(item)}個");
        }
        
        Debug.Log("--- 装備（所持） ---");
        foreach (var kvp in EquipmentCounts)
        {
            if (kvp.Value > 0)
            {
                Debug.Log($"{kvp.Key.GetItemname()}: {kvp.Value}個");
            }
        }
        
        Debug.Log("--- 装備（装着中） ---");
        foreach (var kvp in EquippedItems)
        {
            Debug.Log($"{kvp.Key}: {kvp.Value.GetItemname()}");
        }
        
        Debug.Log("========================");
    }

    
}
