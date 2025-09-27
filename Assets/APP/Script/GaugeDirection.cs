using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeDirection : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    // Update is called once per frame
    void Update()
    {
        //EnemyGauge‚ðMainCamera‚ÖŒü‚©‚í‚¹‚é
        canvas.transform.rotation = Camera.main.transform.rotation;
    }
}
