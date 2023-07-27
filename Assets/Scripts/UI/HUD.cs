using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { HP, EXP, Level }
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
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.PlayerLevel + 1);
                break;
        }
    }
}
