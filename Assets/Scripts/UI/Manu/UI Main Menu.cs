using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject creditsMenuUI;
    [SerializeField] private GameObject volumeMenuUI;
    [SerializeField] private GameObject tutorialMenuUI;

    private void Start()
    {
        MusicManager.Instance.PlayMusic("mainTheme", true);
    }
    
    
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
        Destroy(MusicManager.Instance.gameObject);
    }
    public void OpenTutorialMenu()
    {
        if (creditsMenuUI.activeSelf || volumeMenuUI.activeSelf)
        {
            creditsMenuUI.SetActive(false);
            volumeMenuUI.SetActive(false);
        }
        tutorialMenuUI.SetActive(true);
    }
    public void CloseTutorialMenu()
    {
        tutorialMenuUI.SetActive(false);
    }

    public void OpenCreditsMenu()
    {
        if (volumeMenuUI.activeSelf || tutorialMenuUI.activeSelf)
        {
            volumeMenuUI.SetActive(false);
            tutorialMenuUI.SetActive(false);
        }
        creditsMenuUI.SetActive(true);
    }
    public void CloseCreditsMenu()
    {
        creditsMenuUI.SetActive(false);
    }
    public void OpenVolumeMenu()
    {
        if (creditsMenuUI.activeSelf || tutorialMenuUI.activeSelf)
        {
            creditsMenuUI.SetActive(false);
            tutorialMenuUI.SetActive(false);
        }
        volumeMenuUI.SetActive(true);
    }
    public void CloseVolumeMenu()
    {
        volumeMenuUI.SetActive(false);
    }
}
