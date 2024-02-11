using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject creditsMenuUI;
    [SerializeField] private GameObject volumeMenuUI;

    //mainTheme
    
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void OpenCreditsMenu()
    {
        if (volumeMenuUI.activeSelf)
        {
            volumeMenuUI.SetActive(false);
        }
        creditsMenuUI.SetActive(true);
    }
    public void CloseCreditsMenu()
    {
        creditsMenuUI.SetActive(false);
    }
    public void OpenVolumeMenu()
    {
        if (creditsMenuUI.activeSelf)
        {
            creditsMenuUI.SetActive(false);
        }
        volumeMenuUI.SetActive(true);
    }
    public void CloseVolumeMenu()
    {
        volumeMenuUI.SetActive(false);
    }
}
