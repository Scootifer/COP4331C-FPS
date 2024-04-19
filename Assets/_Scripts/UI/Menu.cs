using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public int Diff = 2;
    public TMP_Text diffNum;
    public bool Win = false;
    public TMP_Text WinLose;
    private InterSceneData _dataObject;

    void Start()
    {
        _dataObject = GameObject.Find("DataObject").GetComponent<InterSceneData>();
    }

    public void Play()
    {
        Debug.Log($"Difficulty{Diff}");
        _dataObject.difficulty = Diff;
        //play game
        SceneManager.LoadScene(1);
    }
    public void EZ()
    {
        //if easy is pushed
        Diff = 1;
        diffNum.SetText("Easy");

    }
    public void Norm()
    {
        Diff = 2;
        diffNum.SetText("Normal");
    }
    public void End_less()
    {
        Diff = 3;
        diffNum.SetText("Endless");
    }
    public void ExitConfirm()
    {
        Application.Quit();
    }
    public void checkWin(bool Win)
    {
        if (Win)
        {
            WinLose.SetText("You Win!");
        }
        else if (!Win)
        {
            WinLose.SetText("You Lost");
        }
    }
}
