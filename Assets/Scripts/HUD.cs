using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { HP }
    public InfoType type;

    public GameObject playerObject;//여기해ㅑㅇ함

    public Slider leftSlider;
    public Damageable player;

    // Start is called before the first frame update
    void Awake() {
        {
            leftSlider = GetComponent<Slider>();
            player = playerObject.GetComponent<Damageable>();
        }
    }

    void LateUpdate()
    {
        switch (type) {
            case InfoType.HP:
                float curHealth = player.Health;
                float maxHealth = player.MaxHealth;
                leftSlider.value=curHealth / maxHealth;
                break;
        }
    }
}
