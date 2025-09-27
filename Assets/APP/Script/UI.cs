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
        string charaname = Charadata.NAME;
        int lv = Charadata.LV;

        NAME.text = charaname;
        string lvtext = ($"LV{lv}");
        LV.text = lvtext;
    }
}
