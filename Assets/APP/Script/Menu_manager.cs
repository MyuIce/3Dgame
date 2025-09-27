using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_manager: MonoBehaviour
{
    //メニューオブジェクトをシリアル化
    [SerializeField] GameObject MenuObject;
    bool menuzyoutai;

    void Update()
    {

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
}