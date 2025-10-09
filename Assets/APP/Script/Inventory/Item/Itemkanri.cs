using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.AI;

using UnityEngine.UI;
using static Itemdata1;
using static UnityEditor.Progress;


//========================================
//アイテムスロットの管理
//========================================
public class Itemkanri : MonoBehaviour
{
    [SerializeField] private Itemdatabase itemDataBase;

    //作った各アイテムスロットのiconを指定(スロットの番号順に)
    [SerializeField] GameObject icon1;
    [SerializeField] GameObject icon2;
    [SerializeField] GameObject icon3;
    [SerializeField] GameObject icon4;
    [SerializeField] GameObject icon5;
    [SerializeField] GameObject icon6;
    [SerializeField] GameObject icon7;
    [SerializeField] GameObject icon8;
    [SerializeField] GameObject icon9;
    [SerializeField] GameObject icon10;
    [SerializeField] GameObject icon11;
    [SerializeField] GameObject icon12;
    [SerializeField] GameObject icon13;
    [SerializeField] GameObject icon14;
    [SerializeField] GameObject icon15;
    [SerializeField] GameObject icon16;
    [SerializeField] GameObject icon17;
    [SerializeField] GameObject icon18;
    [SerializeField] GameObject icon19;
    [SerializeField] GameObject icon20;
    [SerializeField] GameObject icon21;
    [SerializeField] GameObject icon22;
    [SerializeField] GameObject icon23;
    [SerializeField] GameObject icon24;
    [SerializeField] GameObject icon25;
    
    //トグルグループであるinventoryを指定。
    [SerializeField] ToggleGroup togglegroup;

    //アイテム説明表示欄
    //説明欄のTextMeshProを指定。
    [SerializeField] TextMeshProUGUI itemname;
    [SerializeField] TextMeshProUGUI itemsetumei;


    //　アイテム数管理
    private Dictionary<Itemdata1, int> itemkazu = new Dictionary<Itemdata1, int>();

    //持ち物管理
    List<Itemdata1> MotimonoList = new List<Itemdata1>();

    //アイコン管理の配列
    Image[] Icons = new Image[25];


    // Start is called before the first frame update
    void Start()
    {
        //初期化アイテム処理
        for (int i = 0; i < itemDataBase.GetItemLists().Count; i++)
        {
            //　アイテム数を全て0に
            itemkazu.Add(itemDataBase.GetItemLists()[i], 0);
        }


        //持っている初期アイテム設定。//初期アイテムという概念がなければこの項目は必要ない

        //ポーションの数を2にする。
        itemkazu[itemDataBase.GetItemLists()[1]] = 4;
        

        //持ち物更新処理を呼び出す。
        Motimonokoushin();


    }


    //どこからでもアクセス可能。返り値なし。
    public void Motimonokoushin()
    {

        //持ち物更新処理
        //持ち物リストのクリア（UI上）
        MotimonoList.Clear();

        //持っている個数が1個以上のアイテムを持ち物リストに追加する。
        for (int i = 0; i < itemkazu.Count; i++)
        {
            var e = itemkazu[itemDataBase.GetItemLists()[i]];

            if (e > 0)
            {
                MotimonoList.Add(itemDataBase.GetItemLists()[i]);
            }
        }

        //アイテムスロットのアイコンimageをGetComponentしてアイコン配列に代入。
        Icons[0] = icon1.GetComponent<Image>();
        Icons[1] = icon2.GetComponent<Image>();
        Icons[2] = icon3.GetComponent<Image>();
        Icons[3] = icon4.GetComponent<Image>();
        Icons[4] = icon5.GetComponent<Image>();
        Icons[5] = icon6.GetComponent<Image>();
        Icons[6] = icon7.GetComponent<Image>();
        Icons[7] = icon8.GetComponent<Image>();
        Icons[8] = icon9.GetComponent<Image>();
        Icons[9] = icon10.GetComponent<Image>();
        Icons[10] = icon11.GetComponent<Image>();
        Icons[11] = icon12.GetComponent<Image>();
        Icons[12] = icon13.GetComponent<Image>();
        Icons[13] = icon14.GetComponent<Image>();
        Icons[14] = icon15.GetComponent<Image>();
        Icons[15] = icon16.GetComponent<Image>();
        Icons[16] = icon17.GetComponent<Image>();
        Icons[17] = icon18.GetComponent<Image>();
        Icons[18] = icon19.GetComponent<Image>();
        Icons[19] = icon20.GetComponent<Image>();
        Icons[20] = icon21.GetComponent<Image>();
        Icons[21] = icon22.GetComponent<Image>();
        Icons[22] = icon23.GetComponent<Image>();
        Icons[23] = icon24.GetComponent<Image>();
        Icons[24] = icon25.GetComponent<Image>();
        //各アイコンの画像をnullにする(いったん全て空スロットとする)。
        Icons[0].sprite = null;
        Icons[1].sprite = null;
        Icons[2].sprite = null;
        Icons[3].sprite = null;
        Icons[4].sprite = null;
        Icons[5].sprite = null;
        Icons[6].sprite = null;
        Icons[7].sprite = null;
        Icons[8].sprite = null;
        Icons[9].sprite = null;
        Icons[10].sprite = null;
        Icons[11].sprite = null;
        Icons[12].sprite = null;
        Icons[13].sprite = null;
        Icons[14].sprite = null;
        Icons[15].sprite = null;
        Icons[16].sprite = null;
        Icons[17].sprite = null;
        Icons[18].sprite = null;
        Icons[19].sprite = null;
        Icons[20].sprite = null;
        Icons[21].sprite = null;
        Icons[22].sprite = null;
        Icons[23].sprite = null;
        Icons[24].sprite = null;
        //各アイコンのカラー設定。以下の数値だと灰色のような色となる(空スロット表現用)
        Icons[0].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[1].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[2].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[3].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[4].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[5].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[6].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[7].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[8].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[9].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[10].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[11].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[12].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[13].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[14].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[15].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[16].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[17].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[18].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[19].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[20].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[21].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[22].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[23].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        Icons[24].color = new Color(0.2196f, 0.2196f, 0.2196f, 1f);
        
        //持ち物リストの要素数だけ繰り返す。
        for (int i = 0; i < MotimonoList.Count; i++)
        {
            //持ち物リストのi番目をfに代入。
            var f = MotimonoList[i];
            //fのアイコンを配列に代入。
            Icons[i].sprite = f.GetItemicon();
            //配列のアイコンのカラーを白色にする(アイコンを見やすくするため)。
            Icons[i].color = new Color(1, 1, 1, 1);
        }


    }

    public void slotkoushin()
    {
        //スロット更新処理


        //選択されているトグル(つまり選択されているアイテムスロット)を代入。
        Toggle tgl = togglegroup.ActiveToggles().FirstOrDefault();

        //選択されているトグルのゲームオブジェクト名を代入。
        string x = tgl.name;

        //Parseにより文字列の数字をint型やfloat型にできる。今回はint型にしてyに代入。
        int y = int.Parse(x);


        //持ち物リストの要素数がy以上かどうかを確認する。
        if (MotimonoList.Count >= y)
        {
            //選択されているトグルはアイコン表示されている。

            //持ち物リストのy-1番(リストが始まるのが0番からであるため)の名前と個数をZ、Kに代入。
            string z = MotimonoList[y - 1].GetItemname();

            int k = itemkazu[MotimonoList[y - 1]];


            //ZとKを合わせてアイテム名の文字列を作成しtextに出す。
            itemname.text = ($"{z}×{k}");


            //持ち物リストのアイテムの説明をjに代入するしtextに出す。
            string j = MotimonoList[y - 1].GetItemexplanation();
            itemsetumei.text = j;


        }
        else
        {

            //条件に当てはまらなかった場合選択されたアイテムスロットが空だったということ。よってtextにはnullを代入する。
            itemname.text = null;
            itemsetumei.text = null;
        }   
    }

    public bool itemtuika(int[] itemnumberkazu)
    {
        //アイテム追加処理
        //アイテムの要素番号を代入
        var tuikaitem = itemDataBase.GetItemLists()[itemnumberkazu[0]];

        //アイテムの所持限界を代入
        int tuikagenkai = tuikaitem.GetItemlimit();

        //要素番号のアイテムの現在持っている数を代入
        int syozisuu = itemkazu[itemDataBase.GetItemLists()[itemnumberkazu[0]]];



        //要素番号のアイテムの(所持している数+追加しようとしている数)が所持限界を超えないか調べる

        if (tuikagenkai < (syozisuu + itemnumberkazu[1]))
        {
            //所持限界を超えるのでアイテム追加失敗。falseを返り値にしてリターン
            return false;

        }


        //要素番号のアイテムの所持数が0でないならアイテム追加成功(インベントリのスロットが増えることがない)
        if (syozisuu != 0)
        {
            //要素番号のアイテムの数を追加数だけ増加。
            itemkazu[itemDataBase.GetItemLists()[itemnumberkazu[0]]] += itemnumberkazu[1];

            //持ち物更新処理を呼び出す。
            Motimonokoushin();


            //アイテム追加に成功したので返り値をtrueにしてリターン。
            return true;


        }
        //アイテムが0個から追加するのでインベントリのスロットが増加確定
        else
        {
            //インベントリの最大数は今回は25個であるため、25番目までのリストが埋まっているか確認。


            if (MotimonoList.Count == 24)
            {
                //インベントリの上限を超えるのでアイテム追加失敗。falseを返り値にしてリターン
                return false;

            }
            else
            {
                //インベントリが最低でも1つは余っているのでアイテム追加成功
                itemkazu[itemDataBase.GetItemLists()[itemnumberkazu[0]]] += itemnumberkazu[1];

                //持ち物更新処理を呼び出す。
                Motimonokoushin();

                //アイテム追加に成功したので返り値をtrueにしてリターン。
                return true;


            }

        }
    }

    // Itemkanri.cs の中（クラス内）
    private Dictionary<Itemdata1, int> itemCounts = new Dictionary<Itemdata1, int>();

    // 初期化などで itemCounts を作っておく必要があります。
    // 既に似たメンバがあるなら、このメソッド名と実装を合わせてください。

    public int GetItemkazu(Itemdata1 item)
    {
        if (item == null) return 0;
        if (itemCounts.TryGetValue(item, out int count))
            return count;
        return 0;
    }

}