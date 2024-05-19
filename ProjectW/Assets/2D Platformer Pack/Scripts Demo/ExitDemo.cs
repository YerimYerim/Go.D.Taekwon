using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDemo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Preprocessor for exiting when in Unity Editor Play mode
            #if UNITY_EDITOR
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
            #endif

            Application.Quit();
        }
    }
}
