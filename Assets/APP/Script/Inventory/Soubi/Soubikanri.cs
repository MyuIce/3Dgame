/*
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Soubikanri : MonoBehaviour
{
    [SerializeField] private Itemdatabase ItemDataBase;
    [SerializeField] private GameObject icon1, icon2, icon3, icon4, icon5;
    [SerializeField] private TextMeshProUGUI itemname1, itemname2, itemname3, itemname4, itemname5;
    [SerializeField] private ToggleGroup togglegroup1, togglegroup2;
    [SerializeField] private TextMeshProUGUI ATKKASAN, DEFKASAN, INTKASAN, RESKASAN, AGIKASAN, KYUU;
    [SerializeField] private TextMeshProUGUI soubitypehyouzi, soubisetumei;
    [SerializeField] private GameObject PlayrkanriObject, itemObject;
    [SerializeField] private GameObject yousoprefab, hazusuprefab;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject Soubisetumei, Scroll, menukakushi, soubirankakushi;

    private Itemdata1[] Soubi = new Itemdata1[5];
    private Image[] Icons = new Image[5];
    private List<Itemdata1> SoubikanouList = new List<Itemdata1>();
    private Dictionary<Itemdata1, int> SoubisoutyakuDictionary = new Dictionary<Itemdata1, int>();
    private soubiyouso soubiyousoscript;
    private Toggle kakotoggle;
    private int soubidankai;

    private Playeriti Playerkanricript;
    private Itemkanri itemscript;
    private Itemdata1.itemtype Soubitype;


    //Start,SetSoubi,GetSoubi,SetSoubitype,Soubirankoushin,Soubiransentakukoushin,Soubiransentakukettei,Soubihyoujikoushin,Soubisetumeihyouji,ScrollClose

    void Start()
    {
        foreach (var item in ItemDataBase.GetItemLists())
            SoubisoutyakuDictionary.Add(item, 0);

        Playerkanricript = PlayrkanriObject.GetComponent<Player>();
        itemscript = itemObject.GetComponent<Itemkanri>();

        charadata Sousachara = Playerkanricript.Sousacharakoushin();
        for (int i = 0; i < Soubi.Length; i++)
            SetSoubi(Sousachara, i, null);
    }

    void SetSoubi(charadata data, int index, Itemdata1 item)
    {
        switch (index)
        {
            case 0: data.Soubi1 = item; break;
            case 1: data.Soubi2 = item; break;
            case 2: data.Soubi3 = item; break;
            case 3: data.Soubi4 = item; break;
            case 4: data.Soubi5 = item; break;
        }
    }

    Itemdata1 GetSoubi(charadata data, int index)
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

    void SetSoubitype(charadata data, int index)
    {
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

    public void Soubirankoushin()
    {
        soubidankai = 0;
        charadata Sousachara = Playerkanricript.Sousacharakoushin();

        Array.Clear(Soubi, 0, Soubi.Length);

        GameObject[] icons = { icon1, icon2, icon3, icon4, icon5 };
        TextMeshProUGUI[] names = { itemname1, itemname2, itemname3, itemname4, itemname5 };

        for (int i = 0; i < Soubi.Length; i++)
        {
            Soubi[i] = GetSoubi(Sousachara, i);

            if (Soubi[i] != null)
            {
                icons[i].SetActive(true);
                names[i].text = Soubi[i].GetItemname();
                Icons[i] = icons[i].GetComponent<Image>();
                Icons[i].sprite = Soubi[i].GetItemicon();
            }
            else
            {
                icons[i].SetActive(false);
                names[i].text = "";
                Icons[i] = icons[i].GetComponent<Image>();
                Icons[i].sprite = null;
            }
        }
    }

    public void Soubiransentakukoushin()
    {
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        SoubikanouList.Clear();

        List<Itemdata1> allItems = ItemDataBase.GetItemLists();
        foreach (Itemdata1 item in allItems)
        {
            if (item.GetItemtype() == Soubitype && itemscript.GetItemkazu(item) > 0)
            {
                GameObject soubiElement = Instantiate(yousoprefab, content.transform);
                soubiyouso script = soubiElement.GetComponent<soubiyouso>();

                script.soubiitemname().text = item.GetItemname();
                script.soubiyousoicon().GetComponent<Image>().sprite = item.GetItemicon();
                script.soubisoutyaku().SetActive(true);
                script.soubityu().SetActive(false);
                script.hokasoubityu().SetActive(false);

                SoubikanouList.Add(item);
            }
        }

        GameObject hazusuObj = Instantiate(hazusuprefab, content.transform);
        hazusuObj.GetComponentInChildren<TextMeshProUGUI>().text = "ŠO‚·";
    }

    public void Soubiransentakukettei(Itemdata1 sentakuitem)
    {
        charadata Sousachara = Playerkanricript.Sousacharakoushin();
        SetSoubi(Sousachara, soubidankai, sentakuitem);
        Soubirankoushin();
        Scroll.SetActive(false);
        menukakushi.SetActive(false);
        soubirankakushi.SetActive(false);
    }

    public void Soubihyoujikousin(int index)
    {
        charadata Sousachara = Playerkanricript.Sousacharakoushin();
        SetSoubitype(Sousachara, index);
        soubidankai = index;
        Soubiransentakukoushin();
        soubitypehyouzi.text = Soubitype.ToString();
        soubisetumei.text = "";
        Scroll.SetActive(true);
        menukakushi.SetActive(true);
        soubirankakushi.SetActive(true);
    }

    public void Soubisetumeihyouji(string setumei)
    {
        soubisetumei.text = setumei;
    }

    public void ScrollClose()
    {
        Scroll.SetActive(false);
        menukakushi.SetActive(false);
        soubirankakushi.SetActive(false);
    }
}
*/