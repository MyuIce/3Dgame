using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ItemButton : MonoBehaviour,

    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler
{

    [SerializeField] GameObject ActiveObject;
    [SerializeField] GameObject itemkanri;
    Itemkanri itemscript;




    void Start()
    {
        itemscript = itemkanri.GetComponent<Itemkanri>();

    }

    // 押す  
    public void OnPointerClick(PointerEventData eventData)
    {


        ActiveObject.gameObject.SetActive(true);
        itemscript.slotkoushin();

    }
    // 押されたまま
    public void OnPointerDown(PointerEventData eventData)
    {

    }
    // 押した後放した 
    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
