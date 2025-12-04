using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//====================================================
// アイテム管理（インベントリUI更新 + 所持数管理）
//====================================================
public class Itemkanri : MonoBehaviour//IInventoryManager<Itemdata1>
{
    [Header("データベース参照")]
    [SerializeField] private Itemdatabase itemDataBase;

    [Header("スロット親オブジェクト (子にImageを25個持つ)")]
    [SerializeField] private Transform slotParent;

    [Header("トグルグループ & 説明UI")]
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    private const int MaxSlot = 25;

    // アイテム数管理
    private readonly Dictionary<Itemdata1, int> itemCounts = new();
    private readonly List<Itemdata1> ownedItems = new();
    private List<Image> slotIcons = new();

    //============================================
    // 初期化
    //============================================
    private void Start()
    {
        InitializeInventory();//only　アイテムデータの初期化
        AddInitialItems();//only 初期アイテムの設定
        slotkoushin();　//スロットUIの更新

        // 自動でToggleイベントを設定
        foreach (var toggle in slotParent.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener(delegate { UpdateSelectedSlot(); });
        }
    }

    // アイテムデータ初期化
    public void InitializeInventory()
    {
        itemCounts.Clear();
        foreach (var item in itemDataBase.GetItemLists())
        {
            itemCounts.Add(item, 0);
        }

        // スロット画像を自動取得（slotParentの子Imageを全て登録）
        slotIcons = slotParent.GetComponentsInChildren<Image>(true)
                              .Where(img => img.name.StartsWith("icon"))
                              .ToList();
        if (slotIcons.Count < MaxSlot)
            Debug.LogWarning($"スロット数が {MaxSlot} 未満です。実際は {slotIcons.Count} 個です。");
    }

    // 初期所持アイテムの設定（要らない可能性）
    private void AddInitialItems()//only
    {
        if (itemDataBase.GetItemLists().Count >= 2)
        {
            itemCounts[itemDataBase.GetItemLists()[0]] = 1;
            itemCounts[itemDataBase.GetItemLists()[1]] = 2;
        }
    }

    //============================================
    // UI更新処理
    //============================================
    public void slotkoushin()
    {
        // 所持アイテムリストの再構築
        ownedItems.Clear();
        foreach (var kvp in itemCounts)
        {
            if (kvp.Value > 0)
                ownedItems.Add(kvp.Key);
        }

        // 全スロットを初期化
        foreach (var icon in slotIcons)
        {
            icon.sprite = null;
            icon.color = new Color(0.22f, 0.22f, 0.22f, 1f); // 灰色
        }

        // 所持アイテムをスロットに反映
        for (int i = 0; i < ownedItems.Count && i < slotIcons.Count; i++)
        {
            slotIcons[i].sprite = ownedItems[i].GetItemicon();
            slotIcons[i].color = Color.white;
        }
    }

    //============================================
    // スロット選択処理
    //============================================
    public void UpdateSelectedSlot()//only
    {
        Toggle selectedToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        if (selectedToggle == null) return;

        // トグル名 → 数値変換
        if (!int.TryParse(selectedToggle.name, out int slotNumber)) return;

        int index = slotNumber - 1;
        if (index < 0 || index >= ownedItems.Count)
        {
            itemNameText.text = "";
            itemDescriptionText.text = "";
            return;
        }

        Itemdata1 item = ownedItems[index];
        int count = itemCounts[item];
        itemNameText.text = $"{item.GetItemname()} × {count}";
        itemDescriptionText.text = item.GetItemexplanation();
    }

    //============================================
    // アイテム追加処理
    //============================================
    public bool AddItem(int itemIndex, int quantity)//only
    {
        if (itemIndex < 0 || itemIndex >= itemDataBase.GetItemLists().Count)
            return false;

        var targetItem = itemDataBase.GetItemLists()[itemIndex];
        int limit = targetItem.GetItemlimit();
        int current = itemCounts[targetItem];

        if (current + quantity > limit)
            return false;

        if (current == 0 && ownedItems.Count >= MaxSlot)
            return false; // スロット上限

        itemCounts[targetItem] = current + quantity;
        slotkoushin();
        return true;
    }

    //============================================
    // 所持数取得
    //============================================
    public int GetItemCount(Itemdata1 item)//only
    {
        if (item == null) return 0;
        return itemCounts.TryGetValue(item, out int count) ? count : 0;
    }
}
