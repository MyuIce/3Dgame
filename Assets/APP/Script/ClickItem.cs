using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickItem : MonoBehaviour
{
    // Start is called before the first frame update
     // ScriptableObject参照
    [Header("参照設定")]
    [SerializeField] public Itemdata1 itemdata;
    
    public void OnClicked()
    {
        Debug.Log(itemdata.GetItemname() + " をクリックしました！");
    }
}



