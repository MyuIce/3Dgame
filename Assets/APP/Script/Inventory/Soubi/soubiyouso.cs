using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Soubiyouso : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Soubiitemname;
    [SerializeField] GameObject SoubiIcon;
    [SerializeField] GameObject Soubisoutyaku;
    [SerializeField] GameObject Soubityu;
    [SerializeField] GameObject Hokasoubityu;



    public TextMeshProUGUI soubiitemname()
    {
        return Soubiitemname;

    }


    public GameObject soubiyousoicon()
    {

        return SoubiIcon;

    }


    public GameObject soubisoutyaku()
    {

        return Soubisoutyaku;

    }


    public GameObject soubityu()
    {

        return Soubityu;

    }
    public GameObject hokasoubityu()
    {

        return Hokasoubityu;

    }

}
