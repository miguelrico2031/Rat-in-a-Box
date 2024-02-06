using UnityEngine;
using System.Collections;

public class soundTest : MonoBehaviour
{
    private MusicManager musicManager;

    void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();

        musicManager.PlayMusic("Escape", true);

        // Inicia la Coroutine para reproducir sonidos con retraso
        StartCoroutine(PlaySoundsWithDelay());
    }

    IEnumerator PlaySoundsWithDelay()
    {
        musicManager.PlaySound("Anda0");
        yield return new WaitForSeconds(0.3f); // Espera 1 segundo

        musicManager.PlaySound("Anda1");
        yield return new WaitForSeconds(0.3f); // Espera otro segundo

        musicManager.PlaySound("Anda2");
        // Aquí puedes continuar con más acciones si lo necesitas
    }
}
