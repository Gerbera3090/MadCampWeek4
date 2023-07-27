using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
  [SerializeField] private string StoryMode = "StoryModeStage1";
  [SerializeField] private string ChallengeMode = "ChallengeMode";
  [SerializeField] private string FreeMode = "FreeMode";

  public GameObject modeSelectionPanel;

  public void ShowModeSelection()
  {
    modeSelectionPanel.SetActive(true);
  }

  public void HideModeSelection()
  {
    modeSelectionPanel.SetActive(false);
  }

  public void ChooseStoryMode() {
    SceneManager.LoadScene(StoryMode);
    Debug.Log("chose story mode");
  }

  public void ChooseChallengeMode() {
    SceneManager.LoadScene(ChallengeMode);
  }

  public void ChooseFreeMode() {
    SceneManager.LoadScene(FreeMode);
  }
}
