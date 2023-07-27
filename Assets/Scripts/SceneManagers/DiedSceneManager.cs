using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DiedSceneManager : MonoBehaviour
{
    public InputField nickNameInput;
    public Text survivalTimeText;
    public Text scoreText;
    public void Awake()
    {
        Debug.Log(GameManager.instance);
        //GameManager.instance.IsAlive = false;
        float remainTime = GameManager.instance.playTime;
        int min = Mathf.FloorToInt(remainTime/60);
        int sec = Mathf.FloorToInt(remainTime%60);
        survivalTimeText.text = string.Format("{0:D2}:{1:D2}", min, sec);
        scoreText.text = string.Format("Score : {0:F0}", (int)GameManager.instance.playerScore);
    }

    public void OnSaveButtonClicked()
    {
        // Get the NickName from the InputField
        string nickName = nickNameInput.text;

        // Get the Score and Time from the GameManager
        int score = (int) GameManager.instance.playerScore; // Replace this with the actual method to get the score from the GameManager
        int time = (int)GameManager.instance.playTime;  // Replace this with the actual method to get the time from the GameManager

        // Create a Dictionary to store the data
        Dictionary<DashBoardElements, string> data = new Dictionary<DashBoardElements, string>
        {
            { DashBoardElements.NickName, nickName },
            { DashBoardElements.Score, score.ToString() },
            { DashBoardElements.LifeTime, time.ToString() }
        };

        // Add the data to the existing list and save it
        JSONSaver.AddList(GameManager.instance.gameType, data); // Replace YourGameType with your actual game type
    }

    public void OnRetryButtonClicked()
    {
        SceneManager.LoadScene("StartMenu");
    }
    public void OnDashBoardButtonClicked()
    {
        SceneManager.LoadScene("DashboardScene");
    }
}
