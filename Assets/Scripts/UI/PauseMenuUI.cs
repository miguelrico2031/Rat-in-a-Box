using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenuUI : MonoBehaviour
{
    public GameObject pauseMenuUI;

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        AudioListener.pause = true;
        Time.timeScale = 0f;
        
    }
    public void ResumeGame()
    {
        AudioListener.pause = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);

    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
        Destroy(GameManager.Instance.gameObject);
    }

}
