using System;
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
    
    private List<Conversation> conversations = new List<Conversation>();
    private bool showConversation = true;

    private void OnGUI()
    {
        EditorStyles.textArea.wordWrap = true;
        EditorStyles.label.alignment = TextAnchor.UpperCenter;

        GUILayout.BeginVertical();
        
        GUILayout.Space(EditorGUIUtility.singleLineHeight);
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Character Name", EditorStyles.boldLabel);
        EditorGUILayout.TextField("Name");
        GUILayout.EndHorizontal();

        GUILayout.Space(EditorGUIUtility.singleLineHeight);
        
        if (GUILayout.Button("New Dialogue"))
        {
            var conversation = new Conversation();
            conversations.Add(conversation);
            conversation.lines.Add(new Line());
        }
        
        EditorGUILayout.LabelField(String.Empty, GUI.skin.horizontalSlider);

        for(var i = 0; i < conversations.Count; i++)
        {
            showConversation = EditorGUILayout.Foldout(showConversation, "Conversation " + (i + 1));
            
            if (showConversation) {
                for (var j = 0; j < conversations[i].lines.Count; j++)
                {

                    GUILayout.Label("Line", EditorStyles.boldLabel);
                
                    EditorGUILayout.TextArea("Write dialogue text here...", EditorStyles.textArea, GUILayout.MinWidth(250), GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 3));

                    //GUILayout.Space(EditorGUIUtility.singleLineHeight);
                    
                    GUILayout.Space(EditorGUIUtility.singleLineHeight/2);
                    for (var k = 0; k < conversations[i].lines[j].responses.Count; k++)
                    {
                        GUILayout.Label("Response " + conversations[i].lines.Count, EditorStyles.centeredGreyMiniLabel);
                        EditorGUILayout.TextArea("Write dialogue text here...", EditorStyles.textArea, GUILayout.MinWidth(250), GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2));
                    }
                
                    GUILayout.Space(EditorGUIUtility.singleLineHeight/2);
                    
                    if (GUILayout.Button("Add Response"))
                    {
                        conversations[i].lines[j].responses.Add(new Response());
                    }
                }
                
                if (GUILayout.Button("Add Line"))
                {
                    conversations[i].lines.Add(new Line());
                }
                
                GUILayout.Space(EditorGUIUtility.singleLineHeight/2);
            }
            
            EditorGUILayout.LabelField(String.Empty, GUI.skin.horizontalSlider);
        }

        GUILayout.EndVertical();
    }
}
