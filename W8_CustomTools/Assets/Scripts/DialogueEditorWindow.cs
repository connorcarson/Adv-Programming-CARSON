using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueEditorWindow : EditorWindow
{
    
    [MenuItem("Window/Dialogue Editor")]
    public static void ShowWindow()
    {
        GetWindow<DialogueEditorWindow>("DialogueEditor");
    }
    private void OnGUI()
    {
        
    }
}
