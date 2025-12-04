using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UI : MonoBehaviour
{
    [SerializeField] Charadata Charadata;
    [SerializeField] TextMeshProUGUI NAME;
    [SerializeField] TextMeshProUGUI LV;

    // Update is called once per frame
    void Update()
    {
        var Status = Charadata.GetRawStatus();

        //string charaname = Status.NAME;
        //int lv = Status.LV;

        NAME.text = Status.NAME;
        //string lvtext = ($"LV{lv}");
        //LV.text = lvtext;
    }
}
