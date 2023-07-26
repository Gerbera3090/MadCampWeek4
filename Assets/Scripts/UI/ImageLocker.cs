using UnityEngine;
using UnityEngine.UI;

public class ImageLocker : MonoBehaviour
{
    public GameObject player;
    PlayerController playerScript;
    public string skillName;
    private void Awake()
    {
        playerScript = player.GetComponent<PlayerController>();
    }
    // Assuming the player script has a public bool variable .isCool to determine if the player is cool or not.

    void Update()
    {
        // Get the player script from the player GameObject

        // Get the Image component attached to the player GameObject
        Image playerImage = GetComponent<Image>();

        // Check the .isCool value of the player and change the image's color accordingly
        if (!playerScript.CheckCoolTime(skillName))
        {
            // Set the color to gray when the player is cool
            playerImage.color = Color.gray;
        }
        else
        {
            // Set the color to white (original color) when the player is not cool
            playerImage.color = Color.white;
        }
    }
}