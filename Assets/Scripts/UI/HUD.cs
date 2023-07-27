using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { HP, EXP, Level, Kill, Point, PlayTime }
    public InfoType type;

    public GameObject playerObject;//여기해ㅑㅇ함
    
    Text myText;
    Slider mySlider;
    Damageable player;

    // Start is called before the first frame update
    void Awake() {
        {
            mySlider = GetComponent<Slider>();
            myText = GetComponent<Text>();
            if(type == InfoType.HP) player = playerObject.GetComponent<Damageable>();
        }
    }

    void LateUpdate()
    {
        switch (type) {
            case InfoType.HP:
                float curHealth = player.Health;
                float maxHealth = player.MaxHealth;
                mySlider.value=curHealth / maxHealth;
                break; 
            case InfoType.EXP:
                float curExp = GameManager.instance.PlayerExp;
                float maxExp = GameManager.instance.NeededExp;
                mySlider.value=curExp / maxExp;
                break;
            case InfoType.Level: 
                myText.text = string.Format("{0:F0}", GameManager.instance.PlayerLevel + 1);
                break; 
                //Kill, Point, PlayeTime
            case InfoType.Kill: 
                myText.text = string.Format("{0:F0} Kill", GameManager.instance.playerKills);
                break;
            case InfoType.Point: 
                myText.text = string.Format("{0:F0} P", GameManager.instance.playerPoints);
                break; 
            case InfoType.PlayTime: 
                float remainTime = GameManager.instance.playTime;
                int min = Mathf.FloorToInt(remainTime/60);
                int sec = Mathf.FloorToInt(remainTime%60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
        }
    }
}
