using UnityEngine;
using System.Collections;

public class llamadaMusica : MonoBehaviour
{
    private MusicManager musicManager;

    void Awake()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }

    IEnumerator Start()
    {
        if (musicManager != null)
        {
            // Espera un breve momento para asegurar que MusicManager haya terminado la inicialización
            yield return new WaitForSeconds(0.01f);
            musicManager.SetEventParameter("Escape", "escape", 0);
            musicManager.PlayEvent("Escape");
            
            yield return new WaitForSeconds(5f);
            musicManager.SetEventParameter("Escape", "escape", 1);

            yield return new WaitForSeconds(8f);
            musicManager.SetEventParameter("Escape", "escape", 1);
            musicManager.StopEvent("Escape");
            musicManager.PlayEvent("Settings");
        }
        else
        {
            Debug.LogError("MusicManager no encontrado en la escena.");
        }
    }
}
