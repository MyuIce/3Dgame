using UnityEngine;

// ファイル名: Playeriti.cs
// Player のランタイム状態（ステータスのコピー・装備）を管理する MonoBehaviour


/*
public class Playeriti : MonoBehaviour
{
    [Header("Editorで割り当てる Charadata アセット（テンプレ）")]
    public Charadata baseStatusAsset; // Charadata (ScriptableObject) のアセットを Inspector でセット

    [Header("ランタイム用（実行時に複製される）")]
    [HideInInspector] public Charadata runtimeStatus; // baseStatusAsset のコピー（実行時のみ編集）

    [Header("装備データ（Inspector で直接編集したい場合は public のまま）")]
    [SerializeField] public CharaEquipment equipment = new CharaEquipment();

    void Awake()
    {
        // ScriptableObject を new で作らないこと！ Instantiate または CreateInstance を使う
        if (baseStatusAsset != null)
        {
            runtimeStatus = Instantiate(baseStatusAsset);
        }
        else
        {
            runtimeStatus = ScriptableObject.CreateInstance<Charadata>();
        }
    }

    /// <summary>
    /// 現在のステータス（ランタイム用）を返す
    /// Soubikanri などの UI はこれを呼びます
    /// </summary>
    public Charadata Sousacharakoushin()
    {
        return runtimeStatus;
    }

    /// <summary>
    /// 装備データを返す
    /// </summary>
    public CharaEquipment GetEquipment()
    {
        if (equipment == null) equipment = new CharaEquipment();
        return equipment;
    }

    /// <summary>
    /// 指定スロットに装備する（例）。呼んだらステータス再計算を行います。
    /// </summary>
    public void EquipItem(int slotIndex, Itemdata1 item)
    {
        switch (slotIndex)
        {
            case 0: equipment.Soubi1 = item; break;
            case 1: equipment.Soubi2 = item; break;
            case 2: equipment.Soubi3 = item; break;
            case 3: equipment.Soubi4 = item; break;
            case 4: equipment.Soubi5 = item; break;
            default: break;
        }

        RecalculateStatusFromEquipment();
    }

    /// <summary>
    /// 装備を元に runtimeStatus を再計算する処理（Itemdata1 の設計に合わせて実装してください）
    /// </summary>
    public void RecalculateStatusFromEquipment()
    {
        if (baseStatusAsset == null || runtimeStatus == null) return;

        // 基礎値をコピーし（baseStatusAsset は変更しない）
        runtimeStatus.MAXHP = baseStatusAsset.MAXHP;
        //runtimeStatus.MAXMP = baseStatusAsset.MAXMP;
        runtimeStatus.ATK = baseStatusAsset.ATK;
        runtimeStatus.DEF = baseStatusAsset.DEF;
        runtimeStatus.INT = baseStatusAsset.INT;
        runtimeStatus.RES = baseStatusAsset.RES;
        runtimeStatus.AGI = baseStatusAsset.AGI;
        runtimeStatus.LV = baseStatusAsset.LV;
        // ... 必要なフィールドをすべてコピー

        // ここで equipment のボーナスを加算していく（Itemdata1 のフィールド名に合わせて書き換えてください）
        if (equipment != null)
        {
            Itemdata1[] items = { equipment.Soubi1, equipment.Soubi2, equipment.Soubi3, equipment.Soubi4, equipment.Soubi5 };
            foreach (var it in items)
            {
                if (it == null) continue;
                // 例: Itemdata1 に ATKBonus 等があると仮定
                // runtimeStatus.ATK += it.ATKBonus;
                // runtimeStatus.DEF += it.DEFBonus;
                // ... 実際のフィールド名に合わせて実装してください
            }
        }
    }
}




*/