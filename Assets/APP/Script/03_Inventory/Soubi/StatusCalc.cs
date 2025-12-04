using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装備と基礎ステータスの計算を返すクラス
/// （装備変更したときなどのTotalStatusを返す、アイテムを使用した時のTotalStatusを返す）
/// </summary>
public class StatusCalc : MonoBehaviour
{
    [SerializeField] private Soubikanri soubiManager; //装備管理オブジェクト
    [SerializeField] private Charadata charaData;      // キャラデータ(ScriptableObject)
    [SerializeField] public CharaStatuskanri statusUI; // ステータスUI
    [SerializeField] private TotalRuntimeStatus runtimeStatus; // TotalRuntimeStatus

    //private StatusData totalStatus;
    private StatusData equipSum;

    void Start()
    {
        CalculateTotalStatus();
    }

    public void CalculateTotalStatus()
    {
        // キャラデータ(ATK,DEF,INT,AGI,RES)の取得と定義
        var CharaStatus = charaData.GetCharaStatus();

        // 装備中の装備を取得
        var equippedItems = soubiManager.GetEquippedItems(); 

        //Dictionary<EquipmentData1.Equipmenttype, EquipmentData1> equippedItems = soubiManager.GetEquippedItems();
        // 装備中の装備のステータスを合計
        equipSum = new StatusData();

        foreach (var eq in equippedItems.Values)
        {
            if (eq == null)
            {
                Debug.Log("[DEBUG] 装備が null です");
                continue;
            }
            else
            {
                Debug.Log("[DEBUG] 装備が null ではありません");
            }
            var s = eq.GetItemStatus();
            equipSum.ATK += s.ATK;
            equipSum.DEF += s.DEF;
            equipSum.AGI += s.AGI;
            equipSum.INT += s.INT;
            equipSum.RES += s.RES;

            Debug.Log($"[DEBUG] 装備合計: ATK={equipSum.ATK}, DEF={equipSum.DEF}, AGI={equipSum.AGI}, INT={equipSum.INT}, RES={equipSum.RES}");
            Debug.Log($"[DEBUG] {eq.GetItemname()} の装備ステータス: ATK={s.ATK}, DEF={s.DEF}, AGI={s.AGI}, INT={s.INT}, RES={s.RES}");
        }

        
        //UI
        if (statusUI != null)
        {
            statusUI.UpdateStatusDisplay();
            Debug.Log("[DEBUG] UIが更新されました");
        }
        else
        {
            Debug.LogWarning("[WARNING] statusUI が設定されていません");
        }

        //TotalStatusのコピー（作成）
        StatusData TotalStatus = CharaStatus + equipSum;
        //トータルステータスの更新
        runtimeStatus.SetTotalStatus(TotalStatus);
        Debug.Log("TotalStatusが更新されました");

    }

    public StatusData GetEquipSum()
    {
        return equipSum;
    }

    
}
