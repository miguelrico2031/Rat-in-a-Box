using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 24f;
    public TMP_Text creditsText;

    void Start()
    {
        MusicManager.Instance.PlayMusic("finalMusic",false);
    }

    void Update()
    {
        if (creditsText != null)
        {
            creditsText.transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
            if (transform.localPosition.y >= 1660f)
            {
                creditsText = null;
                StartCoroutine(FadeOutAndMenu());
            }
        }
    }

    private IEnumerator FadeOutAndMenu()
    {
        GameObject.Find("Fade").GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(3.25f);
        Destroy(MusicManager.Instance);
        SceneManager.LoadScene("Main Menu");
    }
}