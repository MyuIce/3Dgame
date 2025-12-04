using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalRuntimeStatus : MonoBehaviour
{
    //TotalStatus
    public StatusData TotalStatus { get; private set; }

    //TotalRuntimeStatusの更新
    public void SetTotalStatus(StatusData newStatus)
    {
        TotalStatus = newStatus;

        Debug.Log($"[RuntimeStatus] Final Updated: " +
                  $"ATK={TotalStatus.ATK}, DEF={TotalStatus.DEF}, " +
                  $"AGI={TotalStatus.AGI}, INT={TotalStatus.INT}, RES={TotalStatus.RES}");
    }

    //ステータスが初期化されているか確認
    public bool IsInitialized()
    {
        //ATKが0より大きければ初期化済みと判断(簡易チェック)
        return TotalStatus.ATK > 0 || TotalStatus.DEF > 0 || 
               TotalStatus.AGI > 0 || TotalStatus.INT > 0 || TotalStatus.RES > 0;
    }

    //安全にステータスを取得(初期化されていない場合は警告)
    public StatusData GetTotalStatus()
    {
        if (!IsInitialized())
        {
            Debug.LogWarning("[RuntimeStatus] TotalStatusがまだ初期化されていません!");
        }
        return TotalStatus;
    }
}
