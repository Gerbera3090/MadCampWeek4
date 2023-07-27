using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    private bool stageCompleted = false;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player") && !stageCompleted) {
            stageCompleted = true;
            Invoke("CompleteStage", 2f); // 2초 후에 함수 부름
        }
    }

    private void CompleteStage() {
        audioSource.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
