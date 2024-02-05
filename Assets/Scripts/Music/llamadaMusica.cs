using UnityEngine;

public class llamadaMusica : MonoBehaviour
{
    private MusicManager musicManager;

    void Awake()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }

    void Start()
    {
        if (musicManager != null)
        {
            musicManager.SetEventParameter("Escape", "escape", 1);
            musicManager.PlayEvent("Escape");
        }
        else
        {
            Debug.LogError("MusicManager no encontrado en la escena.");
        }
    }
}
