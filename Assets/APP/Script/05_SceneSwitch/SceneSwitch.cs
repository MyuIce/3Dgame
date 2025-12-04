using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class SceneSwitch : MonoBehaviour
{
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("StageChoice");
    }
    public void OnClickHowButton()
    {

    }
    public void OnClickExitButton()
    {

    }
    public void OnClickStage1()
    {
        SceneManager.LoadScene("Test2");
    }
    public void OnClickBackStageButton()
    {
        SceneManager.LoadScene("StageChoice");
    }
    public void OnClickBackTitleButton()
    {
        SceneManager.LoadScene("Title");
    }

}
