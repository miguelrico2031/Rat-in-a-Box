using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class CreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 20f;
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
        }
    }
}