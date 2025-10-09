using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Soubikanri : MonoBehaviour
{
    // --- Inspectorで設定する項目 ---
    [Header("データ参照")]
    [SerializeField] private Itemdatabase itemDatabase;    // 全アイテム一覧（ScriptableObject等）
    [SerializeField] private GameObject playerManagerObject;         // Playeriti をアタッチしたオブジェクト
    [SerializeField] private GameObject itemManagerObject;           // Itemkanri をアタッチしたオブジェクト

    [Header("スロット表示 (配列で管理)")]
    [Tooltip("インスペクタでスロット数に合わせて要素を設定してください")]
    [SerializeField] private GameObject[] iconSlotObjects = new GameObject[5];   // アイコンを持つ GameObject 配列
    [SerializeField] private TextMeshProUGUI[] nameSlotTexts = new TextMeshProUGUI[5]; // アイテム名表示の配列

    [Header("装備一覧UI (動的生成部分)")]
    [SerializeField] private GameObject yousoPrefab;    // 装備要素プレハブ
    [SerializeField] private GameObject hazusuPrefab;   // 外す用プレハブ
    [SerializeField] private GameObject content;        // スクロールの Content

    [Header("装備情報表示")]
    [SerializeField] private TextMeshProUGUI ATKplus, DEFplus, AGIplus, RESplus, INTplus;
    [SerializeField] private TextMeshProUGUI /*soubitypeText*/ soubiSetumeiText;

    [Header("UI 制御 (表示/非表示)")]
    [SerializeField] private GameObject soubiSetumeiPanel;
    [SerializeField] private GameObject scrollPanel;
    [SerializeField] private GameObject menuMask;
    //[SerializeField] private GameObject soubiRanMask;

    // --- ランタイムで使う内部データ ---
    private int slotCount = 5;                      // スロット数（iconSlotObjects.Lengthから自動設定）
    private Itemdata1[] equippedItems;              // 現在の装備（スロットごと）
    private Image[] iconImages;                     // アイコンの Image コンポーネントキャッシュ
    private List<Itemdata1> canEquipList = new List<Itemdata1>(); // 現在のキャラが装備可能なアイテム一覧
    private Dictionary<Itemdata1, int> equipCountDict = new Dictionary<Itemdata1, int>(); // アイテムの所持数等（初期化）
    private int currentSlotIndex = 0;               // 選択中のスロットインデックス

    // 参照キャッシュ
    private Playeriti playerScript;
    private Itemkanri itemManagerScript;

    // --- 初期化 ---
    private void Awake()
    {
        // 配列サイズの決定（Inspector の配列長に合わせる）
        slotCount = Mathf.Max(1, iconSlotObjects != null ? iconSlotObjects.Length : 5);

        // 配列を初期化
        equippedItems = new Itemdata1[slotCount];
        iconImages = new Image[slotCount];

        // icon の Image コンポーネントをキャッシュしておく
        for (int i = 0; i < slotCount; i++)
        {
            if (iconSlotObjects != null && i < iconSlotObjects.Length && iconSlotObjects[i] != null)
            {
                iconImages[i] = iconSlotObjects[i].GetComponent<Image>();
            }
        }
    }

    private void Start()
    {
        // Player / Item 管理スクリプトを取得（Inspectorで未設定でも自動取得を試みる）
        if (playerManagerObject != null && playerScript == null)
            playerScript = playerManagerObject.GetComponent<Playeriti>();

        if (itemManagerObject != null && itemManagerScript == null)
            itemManagerScript = itemManagerObject.GetComponent<Itemkanri>();

        // 所持アイテム数辞書を初期化（全アイテムをキーにして 0 を入れておく）
        InitEquipCountDictionary();

        // プレイヤーの装備情報を UI に反映
        RefreshEquipmentUI();

        //装備欄隠しパネルとスクロールパネルをOFFにする
        if (menuMask != null) menuMask.SetActive(false);
        if (scrollPanel != null) scrollPanel.SetActive(false);
    }

    // 全アイテムをキーにして辞書を作る（所持数の初期化）
    private void InitEquipCountDictionary()
    {
        equipCountDict.Clear();
        if (itemDatabase == null) return;

        var all = itemDatabase.GetItemLists();
        foreach (var it in all)
        {
            if (!equipCountDict.ContainsKey(it))
                equipCountDict.Add(it, 0);
        }
    }

    // ------------------------
    // Helper: 装備の読み書き（CharaEquipment の既存APIに合わせる）
    // ------------------------
    private void SetEquippedItem(CharaEquipment data, int index, Itemdata1 item)
    {
        // 元コードの構造に合わせて switch でセット
        switch (index)
        {
            case 0: data.Soubi1 = item; break;
            case 1: data.Soubi2 = item; break;
            case 2: data.Soubi3 = item; break;
            case 3: data.Soubi4 = item; break;
            case 4: data.Soubi5 = item; break;
            default: Debug.LogWarning("SetEquippedItem: index out of range"); break;
        }
    }

    private Itemdata1 GetEquippedItem(CharaEquipment data, int index)
    {
        return index switch
        {
            0 => data.Soubi1,
            1 => data.Soubi2,
            2 => data.Soubi3,
            3 => data.Soubi4,
            4 => data.Soubi5,
            _ => null
        };
    }

    // スロットに対応する装備タイプを取得してプロパティに格納する（UI表示用）
    private void SetSoubitypeFromEquipment(CharaEquipment data, int index)
    {
        // これも元コードに合わせて switch
        Soubitype = index switch
        {
            0 => data.Soubitype1,
            1 => data.Soubitype2,
            2 => data.Soubitype3,
            3 => data.Soubitype4,
            4 => data.Soubitype5,
            _ => Soubitype
        };
    }

    // --- 元と同じ名前の公開 API（UI から呼ばれることを想定） ---
    // 装備欄（5スロット）全体を UI に反映するメソッド
    public void Soubirankoushin()
    {
        RefreshEquipmentUI();
    }

    // 実際の更新処理（安全性のため nullチェックを多めに）
    private void RefreshEquipmentUI()
    {
        if (playerScript == null)
        {
            if (playerManagerObject != null) playerScript = playerManagerObject.GetComponent<Playeriti>();
            if (playerScript == null)
            {
                Debug.LogError("Playeriti is not assigned or found.");
                return;
            }
        }

        CharaEquipment equip = playerScript.GetEquipment();
        if (equip == null) return;

        // equippedItems をクリアして、プレイヤー情報から取得した値で埋める
        Array.Clear(equippedItems, 0, equippedItems.Length);

        for (int i = 0; i < slotCount; i++)
        {
            equippedItems[i] = GetEquippedItem(equip, i);

            // UI の更新
            if (i < iconSlotObjects.Length && iconSlotObjects[i] != null)
            {
                if (equippedItems[i] != null)
                {
                    iconSlotObjects[i].SetActive(true);
                    if (i < nameSlotTexts.Length && nameSlotTexts[i] != null)
                        nameSlotTexts[i].text = equippedItems[i].GetItemname();

                    if (iconImages[i] != null)
                        iconImages[i].sprite = equippedItems[i].GetItemicon();
                }
                else
                {
                    iconSlotObjects[i].SetActive(false);
                    if (i < nameSlotTexts.Length && nameSlotTexts[i] != null)
                        nameSlotTexts[i].text = "";
                    if (iconImages[i] != null)
                        iconImages[i].sprite = null;
                }
            }
        }
    }

    // 装備選択用リスト（スクロール領域）を更新して表示する
    public void Soubiransentakukoushin()
    {
        // clear existing children
        if (content == null)
        {
            Debug.LogError("Content is not set.");
            return;
        }

        ClearContentChildren(content);

        canEquipList.Clear();

        if (itemDatabase == null || itemManagerScript == null)
        {
            Debug.LogWarning("ItemDatabase or ItemManager not assigned.");
            return;
        }

        List<Itemdata1> allItems = itemDatabase.GetItemLists();
        foreach (Itemdata1 item in allItems)
        {
            // 装備タイプが一致して、所持数が 0 より大きければ表示
            if (item.GetItemtype() == Soubitype && itemManagerScript.GetItemkazu(item) > 0)
            {
                GameObject element = Instantiate(yousoPrefab, content.transform);
                Soubiyouso script = element.GetComponent<Soubiyouso>();
                if (script != null)
                {
                    script.soubiitemname().text = item.GetItemname();
                    var imgObj = script.soubiyousoicon();
                    if (imgObj != null)
                    {
                        var img = imgObj.GetComponent<Image>();
                        if (img != null) img.sprite = item.GetItemicon();
                    }

                    script.soubisoutyaku().SetActive(true);
                    script.soubityu().SetActive(false);
                    script.hokasoubityu().SetActive(false);

                    // 装備決定ボタンやクリックで Soubiransentakukettei(item) を呼ぶようにプレハブで設定しておく
                }
                canEquipList.Add(item);
            }
        }

        // 「外す」ボタンを追加
        GameObject hazusuObj = Instantiate(hazusuPrefab, content.transform);
        var tmp = hazusuObj.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null) tmp.text = "外す";
    }

    // 装備選択決定（UI側から選択アイテムを渡して呼ぶ）
    public void Soubiransentakukettei(Itemdata1 selectedItem)
    {
        if (playerScript == null)
        {
            Debug.LogError("Playeriti is not assigned.");
            return;
        }

        CharaEquipment equip = playerScript.GetEquipment();
        if (equip == null) return;

        SetEquippedItem(equip, currentSlotIndex, selectedItem);

        // UI を更新して閉じる
        RefreshEquipmentUI();
        CloseScrollPanels();
    }

    // 指定インデックスの装備タイプを表示し、選択パネルを開く（ボタンに割り当てて使う）
    public void Soubihyoujikousin(int index)
    {
        if (playerScript == null)
        {
            if (playerManagerObject != null) playerScript = playerManagerObject.GetComponent<Playeriti>();
            if (playerScript == null) return;
        }

        CharaEquipment equip = playerScript.GetEquipment();
        if (equip == null) return;

        // 選択中のスロットインデックスを保持
        currentSlotIndex = Mathf.Clamp(index, 0, slotCount - 1);

        // 対応する装備タイプを UI 表示用プロパティに反映
        SetSoubitypeFromEquipment(equip, currentSlotIndex);

        // 装備選択リストを作って表示
        Soubiransentakukoushin();

        // UI 表示切替
        //（制作上不要）if (soubitypeText != null) soubitypeText.text = Soubitype.ToString();
        if (soubiSetumeiText != null) soubiSetumeiText.text = "";
        if (scrollPanel != null) scrollPanel.SetActive(true);
        if (menuMask != null) menuMask.SetActive(true);
        //（制作上不要）if (soubiRanMask != null) soubiRanMask.SetActive(true);
    }

    // 装備説明表示（プレハブから呼ばれる想定）
    public void Soubisetumeihyouji(string setumei)
    {
        if (soubiSetumeiText != null) soubiSetumeiText.text = setumei;
    }

    // スクロールパネルを閉じる
    public void ScrollClose()
    {
        CloseScrollPanels();
    }

    // ------------------------
    // ユーティリティ
    // ------------------------
    private void ClearContentChildren(GameObject parent)
    {
        for (int i = parent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(parent.transform.GetChild(i).gameObject);
        }
    }

    private void CloseScrollPanels()
    {
        if (scrollPanel != null) scrollPanel.SetActive(false);
        if (menuMask != null) menuMask.SetActive(false);
        //（制作上不要）if (soubiRanMask != null) soubiRanMask.SetActive(false);
    }

    // --- 元コードの Soubitype フィールド（そのまま維持） ---
    [SerializeField] private Itemdata1.itemtype Soubitype;
}
