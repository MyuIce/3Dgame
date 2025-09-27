using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create LvData")]
public class Lvdata : ScriptableObject
{
    //リストを宣言し、内部の経験値クラス(PlayerExpTable)を入れる
    public List<PlayerExpTable> playerExpTable = new List<PlayerExpTable>();

    //クラスをインスペクターに表示
    [System.Serializable]
    //各レベルに達するまでの必要経験値が入っている内部クラス
    public class PlayerExpTable
    {
        public int level;
        public int exp;
    }
}
