
using System;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var rat = other.GetComponentInParent<RatController>();
        if (!rat) return;

        // ANTON sonido trampa
        MusicManager.Instance.PlaySound("trampa");
        
        GetComponentInChildren<UnityEngine.Animator>().SetBool("Trap", true);
        rat.CurrentState = new OnTrap(true);
    }
}
