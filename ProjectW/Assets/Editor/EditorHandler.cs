using System;
using Script;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class EditorHandler : MonoBehaviour
{
    private static FixedDisplay fixedDisplay;
    private static Vector2 previousSize;
    static EditorHandler()
    {
        EditorApplication.update += Update;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            fixedDisplay ??= FindObjectOfType<FixedDisplay>();
            fixedDisplay.SetResolution();
        }
    }

    private static void Update()
    {
        if(EditorWindow.focusedWindow != null && (EditorWindow.focusedWindow.titleContent.text.Equals("Game")|| (EditorWindow.focusedWindow.titleContent.text.Equals("Simulator"))))
        {
            // 현재 창 크기를 가져옵니다.
            Vector2 currentSize = new Vector2(EditorWindow.focusedWindow.position.width, EditorWindow.focusedWindow.position.height);

            // 이전 창 크기와 현재 크기를 비교합니다.
            if (currentSize != previousSize)
            {
                fixedDisplay ??= FindObjectOfType<FixedDisplay>();
                fixedDisplay.SetResolution();
                previousSize = currentSize;
            }
        }
    }
}
