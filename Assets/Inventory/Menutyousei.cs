using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//====================================
//EscでのMenuの開閉、マウスカーソル表示
//====================================

public class Menutyousei : MonoBehaviour
{
    [SerializeField] GameObject MenuObject;

    bool menuzyoutai;

    void Update()
    {
        //ESCでのメニューの開閉
        if (menuzyoutai == false)
        {
            if (Input.GetButtonDown("Cancel"))
            {
                MenuObject.gameObject.SetActive(true);
                menuzyoutai = true;

                // マウスカーソルを表示にし、位置固定解除
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            if (Input.GetButtonDown("Cancel"))
            {
                MenuObject.gameObject.SetActive(false);
                menuzyoutai = false;

                // マウスカーソルを非表示にし、位置を固定
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    //アイテム・装備・キャラtoggleのメニュー開閉
    public void toggle_menu()
    {

    }
    //メニュー状態を返す
    public bool menuhiraki()
    {
        return menuzyoutai;
    }

}