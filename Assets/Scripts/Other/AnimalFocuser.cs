using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFocuser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject canvas;

    private void OnMouseDown()
    {
        // Disable all the other animal info UIs
        foreach (AnimalInfoUI ui in FindObjectsOfType<AnimalInfoUI>())
        {
            ui.transform.parent.gameObject.SetActive(false);
        }

        canvas.SetActive(true);
        Camera.main.GetComponent<GameCamera>().focus = transform.parent.gameObject;
    }
}
