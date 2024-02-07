using UnityEngine;
using System.Collections;

public class SoundTest : MonoBehaviour
{
    private MusicManager musicManager;

    void Start()
    {


        MusicManager.Instance.PlayMusic("Escape", true);

        // Inicia la Coroutine para reproducir sonidos con retraso
        StartCoroutine(PlaySoundsWithDelay());
    }

    IEnumerator PlaySoundsWithDelay()
    {
        MusicManager.Instance.PlaySound("Anda0");
        yield return new WaitForSeconds(0.3f); // Espera 1 segundo

        MusicManager.Instance.PlaySound("Anda1");
        yield return new WaitForSeconds(0.3f); // Espera otro segundo

        MusicManager.Instance.PlaySound("Anda2");
        // Aquí puedes continuar con más acciones si lo necesitas

        yield return new WaitForSeconds(4f);
        MusicManager.Instance.StopMusic(2f);
    }
}
