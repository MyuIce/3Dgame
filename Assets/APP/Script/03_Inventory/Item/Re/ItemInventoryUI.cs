using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;


/// <summary>
/// アイテムインベントリのUI表示管理 
/// 
/// slotParent
/// slotIcons
/// toggleGroup
/// itemNameText
/// itemDescriptionText
/// slotkoushin()
/// UpdateSelectedSlot()
/// 
/// </summary>
/// 
public class ItemInventoryUI : MonoBehaviour
{
    [Header("スロット親オブジェクト (子にImageを25個持つ)")]
    [SerializeField] private Transform slotParent;

    [Header("トグルグループ & 説明UI")]
    [SerializeField] private ToggleGroup toggleGroup;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    private List<Image> slotIcons = new();
    private ItemInventory inventory;

    public void Initialize(ItemInventory inv)
    {
        inventory = inv;
        slotIcons = slotParent.GetComponentsInChildren<Image>(true)
                              .Where(img => img.name.StartsWith("icon"))
                              .ToList();

        // Toggle設定
        foreach (var toggle in slotParent.GetComponentsInChildren<Toggle>())
            toggle.onValueChanged.AddListener(delegate { UpdateSelectedSlot(); });

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (inventory == null) return;
        var ownedItems = inventory.GetOwnedItems();
        for (int i = 0; i < slotIcons.Count; i++)
        {
            if (i < ownedItems.Count)
            {
                slotIcons[i].sprite = ownedItems[i].GetItemicon();
                slotIcons[i].color = Color.white;              
            }
            else
            {
                slotIcons[i].sprite = null;
                slotIcons[i].color = new Color(0.22f, 0.22f, 0.22f, 1f);
            }
        }
    }

    public void UpdateSelectedSlot()
    {
        Toggle selectedToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        if (selectedToggle == null || inventory == null) return;

        if (!int.TryParse(selectedToggle.name, out int slotNumber)) return;
        int index = slotNumber - 1;

        var owned = inventory.GetOwnedItems();
        if (index < 0 || index >= owned.Count) return;

        Itemdata1 item = owned[index];
        itemNameText.text = $"{item.GetItemname()} × {inventory.GetitemCount(item)}";
        itemDescriptionText.text = item.GetItemexplanation();
    }
}
