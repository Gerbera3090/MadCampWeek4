using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DashboardSceneManager : MonoBehaviour
{
    public GameObject dashboard;
    private List<Dictionary<DashBoardElements, string>> loadList;

    private void Awake()
    {
        foreach (GameType element in System.Enum.GetValues(typeof(GameType)))
        {
            // Perform some actions for each DashBoardElement
            // Load data and assign it to the loadList
            loadList = JSONSaver.LoadList(element); // Replace this with your loading method
            // Assuming JSONController.LoadData() returns the List<Dictionary<DashBoardElements, string>>
    
            // Sort the loadList using the custom comparer
            DashboardDataComparer comparer = new DashboardDataComparer();
            loadList.Sort(comparer);
            Transform gb = null;
            switch (element)
            {
                case GameType.Story:
                    // Your actions for NickName
                    gb = dashboard.transform.Find("Story");
                    break;
                case GameType.Challenge:
                    // Your actions for Score
                    gb = dashboard.transform.Find("Challenge");
                    break;
                case GameType.Free:
                    // Your actions for LifeTime
                    gb = dashboard.transform.Find("Free");
                    break;
                default:
                    // Default case (if there are additional elements in the enum)
                    break;
            }

            for (int i = 0; i < 5; i++)
            {
                if (i >= loadList.Count) break;
                Transform rank = gb.Find("Rank (" + i + ")");
                foreach (DashBoardElements dbelement in System.Enum.GetValues(typeof(DashBoardElements)))
                {
                    switch (dbelement)
                    {
                        case DashBoardElements.NickName:
                            rank.Find("NickName").gameObject.GetComponent<Text>().text = loadList[i][dbelement];
                            break;
                        case DashBoardElements.Score:
                            rank.Find("Score").gameObject.GetComponent<Text>().text = loadList[i][dbelement];
                            break;
                        case DashBoardElements.LifeTime:
                            rank.Find("Time").gameObject.GetComponent<Text>().text = loadList[i][dbelement];
                            break;
                    }
                        
                }
            }
        }

        // Now the loadList is sorted in descending order based on the "Score" value.
    }

    public void OnRetryButtonClicked()
    {
        SceneManager.LoadScene("StartMenu");
    }
}

public class DashboardDataComparer : IComparer<Dictionary<DashBoardElements, string>>
{
    public int Compare(Dictionary<DashBoardElements, string> data1, Dictionary<DashBoardElements, string> data2)
    {
        if (data1.TryGetValue(DashBoardElements.Score, out string scoreStr1) &&
            data2.TryGetValue(DashBoardElements.Score, out string scoreStr2))
        {
            if (int.TryParse(scoreStr1, out int score1) && int.TryParse(scoreStr2, out int score2))
            {
                return score2.CompareTo(score1); // Sorting in descending order (higher scores first)
            }
        }
        return 0; // Default case, no sorting
    }
}