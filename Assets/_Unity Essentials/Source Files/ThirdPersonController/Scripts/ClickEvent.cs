using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickEvent : MonoBehaviour
{
    public LayerMask clickableLayers;//Inspectorでクリック可能なレイヤーを指定
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if(Physics.Raycast(ray,out hit,clickableLayers))
            {
                Debug.Log("クリックされたオブジェクト：" + hit.collider.name);


            }
        }
    }
}
