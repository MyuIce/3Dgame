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
    [SerializeField] Transform player;
    //調べるボタンを押したら宝箱が開く距離を指定。
    [SerializeField] float kidoukyori;
    //宝箱に入れるアイテムをitemdataで指定。
    //[SerializeField] Itemdata1 ireruitem;

    //宝箱入れるアイテムの数を指定。
    [SerializeField] int itemkazu;
    [Header("UI関連(テキスト、アイコン、canvas)")]
    //宝箱開いた時のメッセージを表示するtext(itemnyusyu)を指定
    [SerializeField] TextMeshProUGUI[] getItemNames = new TextMeshProUGUI[3];
    //[SerializeField] Sprite itemnyusyuicon;
    [SerializeField] UnityEngine.UI.Image[] getItemImages = new UnityEngine.UI.Image[3];
    //宝箱の開いた時のitemnyusyuやimageをまとめたcanvasを指定。
    [SerializeField] GameObject canvas;

    [Header("データベース参照")]
    [SerializeField] private Itemdatabase itemDataBase;
    [SerializeField] private EquipmentDatabase equipmentDataBase;

    //itemkanriゲームオブジェクトを指定。
    [SerializeField] GameObject itemObject;
    [SerializeField] GameObject equipmentObject;
    [SerializeField] GameObject menuObject;
    private ItemManager itemManager;
    private Soubikanri equipmentManager;
    private Menutyousei menuScript;
    //メニュー管理オブジェクトを指定。メニューを開いている時には宝箱を開けなくしたいので指定



    [Header("時間関連")]
    //宝箱を開いた時のメッセージを表示する時間を指定
    [SerializeField] float messagetime;

    //バックがいっぱいだった時のメッセージを表示する時間を指定
    [SerializeField] float errorMessagetime;


    private Animator anim;

    //渡す値格納用の配列。 2つの変数を値として渡したいので配列を使用。
    //int[] itemnumberkazu = new int[2];

    int open;

    bool tuikakanou;
    bool menuhirakikaeshi;

    void Awake()
    {

        //prefab外のマネージャーオブジェクトとスクリプトの取得
        if (itemObject == null)
        {
            itemObject = GameObject.Find("ItemManagement");
        }
        if (equipmentObject == null)
        {
            equipmentObject = GameObject.Find("SoubiManagement");
        }
        if (menuObject == null)
        {
            menuObject = GameObject.Find("Menutyousei");
        }

        // コンポーネント取得
        anim = GetComponent<Animator>(); //animatorのコンポーネントを取得
        itemManager = itemObject?.GetComponent<ItemManager>();
        equipmentManager = equipmentObject?.GetComponent<Soubikanri>();
        menuScript = menuObject?.GetComponent<Menutyousei>();
    }

    void Update()
    {
        //宝箱を開き中、開いた後はリターンして処理しない。
        open = anim.GetInteger("open");

        if (open != 0)
        {
            return;
        }

        //メニューを開いているならリターンして処理しない。
        //Menuscript = menuObject.GetComponent<Menutyousei>();
        menuhirakikaeshi = menuScript.menuhiraki();
        if (menuhirakikaeshi)
        {
            return;

        }

        /*
        //プレイヤーと宝箱との距離を求める。
        Vector3 takarabakoposition = this.transform.position;//宝箱の位置
        //Vector3 playerposition = playerdata.player.position;  
        Vector3 playerposition = player.position;//プレイヤーの位置
        float kidoukyori = 3f; //= playerposition - takarabakoposition;
        float distance = Vector3.Distance(playerposition, takarabakoposition); //プレイヤーと宝箱の距離
        */
        
        if (Input.GetButtonDown("shiraberu"))
        {
            StartCoroutine(OpenTreasureBox());
            Debug.Log("宝箱を開きます");
        }
        else
        {
            Debug.Log("宝箱を開くボタンが押されていません");
        }
    }

    private IEnumerator OpenTreasureBox()
    {
        anim.SetInteger("open", 1);
        canvas.SetActive(true);

        var randomItem = GetRandomItem();
        var randomEquipment1 = GetRandomEquipment();
        var randomEquipment2 = GetRandomEquipment();

        var obtainedList = new List<IGameItem> { randomItem, randomEquipment1, randomEquipment2 };

        //UIに反映
        for (int i = 0; i < 3; i++)
        {
            getItemNames[i].text = obtainedList[i].GetItemname();
            getItemImages[i].sprite = obtainedList[i].GetItemicon();
            getItemImages[i].enabled = true;
        }

        bool addedItem = itemManager.AddItem(randomItem, 1);
        bool addedEq1 = equipmentManager.AddItem(randomEquipment1, 1);       
        bool addedEq2 = equipmentManager.AddItem(randomEquipment2, 1);

        tuikakanou = addedItem&&addedEq1&& addedEq2;

        // メッセージ時間待機
        yield return new WaitForSeconds(messagetime);

        if (tuikakanou)
        {
            canvas.SetActive(false);
            anim.SetInteger("open", 2); // 開封済み
        }
        else
        {
            foreach (var t in getItemNames) t.text = "バックがいっぱいです";
            yield return new WaitForSeconds(errorMessagetime);
            canvas.SetActive(false);
            anim.SetInteger("open", 0); // 閉じる
        }

    }

    private Itemdata1 GetRandomItem()
    {
        var items = itemDataBase.GetItemLists();
        int index = UnityEngine.Random.Range(0, items.Count);
        return items[index];
    }
    private EquipmentData1 GetRandomEquipment()
    {
        var equips = equipmentDataBase.GetItemLists();
        int index = UnityEngine.Random.Range(0, equips.Count);
        return equips[index];
    }

}
