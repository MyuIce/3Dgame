using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static Itemdata1;
using static UnityEditor.Progress;

public class Iteminitial : MonoBehaviour
{
    [SerializeField] private Itemdatabase itemDataBase;
    //　アイテム数管理
    private Dictionary<Itemdata1, int> itemkazu = new Dictionary<Itemdata1, int>();


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < itemDataBase.GetItemLists().Count; i++)
        {
            //　アイテム数を全て0に
            itemkazu.Add(itemDataBase.GetItemLists()[i], 0);


        }

        //ポーションのみ数を2にする。
        //itemkazu[itemDataBase.GetItemLists()[1]] = 2;

        var a = itemkazu[itemDataBase.GetItemLists()[0]];
        var b = itemkazu[itemDataBase.GetItemLists()[1]];
        var c = itemkazu[itemDataBase.GetItemLists()[2]];

        //Debug.Log(a);
        //Debug.Log(b);
        //Debug.Log(c);
        


        var d = itemDataBase.GetItemLists()[1].GetItemtype();
        Debug.Log(d);


    }

}
