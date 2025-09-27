using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class Treasurebox : MonoBehaviour
{

    //playerの位置情報をScriptableObjectによりassetfileに入れたものを指定。
    //playerの位置がわかるならこの方法以外でもOK。
    [SerializeField] PlayerData playerdata;

    //調べるボタンを押したら宝箱が開く距離を指定。
    [SerializeField] float kidoukyori;

    //宝箱に入れるアイテムをitemdataで指定。
    [SerializeField] Itemdata1 ireruitem;

    //宝箱入れるアイテムの数を指定。
    [SerializeField] int itemkazu;

    //宝箱開いた時のメッセージを表示するtext(itemnyusyu)を指定
    [SerializeField] TextMeshProUGUI itemnyusyubunnsyou;

    //宝箱の開いた時のitemnyusyuやimageをまとめたcanvasを指定。
    [SerializeField] GameObject canvas;


    [SerializeField] private Itemdatabase itemDataBase;

    //itemkanriゲームオブジェクトを指定。
    [SerializeField] GameObject itemObject;
    Itemkanri itemscript;

    //メニュー管理オブジェクトを指定。メニューを開いている時には宝箱を開けなくしたいので指定
    [SerializeField] GameObject menuObject;
    Menutyousei Menuscript;

    //宝箱を開いた時のメッセージを表示する時間を指定
    [SerializeField] float moziteruzikan;

    //バックがいっぱいだった時のメッセージを表示する時間を指定
    [SerializeField] float moziteruzikan2;


    Animator anim;

    //渡す値格納用の配列。 2つの変数を値として渡したいので配列を使用。
    int[] itemnumberkazu = new int[2];

    int open;

    bool tuikakanou;
    bool menuhirakikaeshi;


    void Start()
    {
        anim = GetComponent<Animator>(); //animatorのコンポーネントを取得
        
    }


    // Update is called once per frame
    void Update()
    {
        //宝箱を開き中、開いた後はリターンして処理しない。
        open = anim.GetInteger("open");

        if (open != 0)
        {
            return;
        }

        //メニューを開いているならリターンして処理しない。
        Menuscript = menuObject.GetComponent<Menutyousei>();
        menuhirakikaeshi = Menuscript.menuhiraki();
        if (menuhirakikaeshi)
        {
            return;

        }


        //プレイヤーと宝箱との距離を求める。
        Vector3 takarabakoposition = this.transform.position;
        Vector3 playerposition = playerdata.player.position;
        Vector3 kyori = playerposition - takarabakoposition;


        //宝箱とプレイヤー間の距離ベクトルが指定した起動距離以下かどうか
        if (kidoukyori > kyori.magnitude)
        {
            //この状態で調べるボタンが押された場合に宝箱を開ける処理を行う。
            if (Input.GetButtonDown("shiraberu"))
            {
                //openパラメータを1にすることで遷移が行われopemのanimationclipが再生
                anim.SetInteger("open", 1);

                //指定した宝箱のアイテムの名前を代入
                string itemname = ireruitem.GetItemname();
                canvas.SetActive(true);

                //コルーチンで待機処理を行う。
                StartCoroutine(mozideruzikan());

                //出すメッセージ
                itemnyusyubunnsyou.text = ($"{itemname}を{itemkazu}つ手に入れた。");

                //宝箱にいれたアイテムのitemdataのリストの要素番号を求める。
                var index = itemDataBase.GetItemLists().IndexOf(ireruitem);



                //要素番号を配列の0番に代入。
                itemnumberkazu[0] = index;

                //宝箱のアイテムの数を配列の1番に代入。
                itemnumberkazu[1] = itemkazu;


                //itemkanriのゲームオブジェクトのitemkanriスクリプトを取得
                itemscript = itemObject.GetComponent<Itemkanri>();
                //itemkanriスクリプトのアイテム追加メソッドを呼び出し、返り値(bool)を格納。
                tuikakanou = itemscript.itemtuika(itemnumberkazu);


            }


        }

        IEnumerator mozideruzikan()
        {
            // 文字が出る時間だけ待機  
            yield return new WaitForSeconds(moziteruzikan);

            if (tuikakanou)
            {
                //追加可能だった場合にはメッセージを消す。
                canvas.SetActive(false);
                //宝箱からアイテムを取った証としてopenパラメータを2にする。
                anim.SetInteger("open", 2);

            }
            else
            {
                //インベントリのアイテムスロットが埋まるかアイテム1種の持てる数の限界が超えた場合は以下のメッセージを出す。
                //バックスラッシュnの組み合わせで改行可能。ただし、日本語windowsだと以下のように\マークに表示される。
                itemnyusyubunnsyou.text = ("バックがいっぱいだ。\nアイテムを元の場所に戻した。");

                //更にコルーチンで待機処理を行う。
                StartCoroutine(mozideruzikan2());
            }

        }

        IEnumerator mozideruzikan2()
        {
            // 文字が出る時間だけ待機  
            yield return new WaitForSeconds(moziteruzikan2);

            //バックがいっぱい・・のメッセージを消す。
            canvas.SetActive(false);
            //openパラメータを0にすることで宝箱が閉じる。
            anim.SetInteger("open", 0);


        }

    }

}