using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SlotButton : MonoBehaviour,




    IPointerClickHandler,
    IPointerDownHandler,
    IPointerUpHandler
{

    [SerializeField]GameObject itemObject;

    Itemkanri itemscript;

    void Start()
    {
        itemscript = itemObject.GetComponent<Itemkanri>();

    }


    // ‰Ÿ‚·  
    public void OnPointerClick(PointerEventData eventData)
    {

        itemscript.slotkoushin();

    }
    // ‰Ÿ‚³‚ê‚½‚Ü‚Ü
    public void OnPointerDown(PointerEventData eventData)
    {

    }
    // ‰Ÿ‚µ‚½Œã•ú‚µ‚½ 
    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
