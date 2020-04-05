using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DialogueEditorWindow : EditorWindow
{
    
    private readonly List<Conversation> conversations = new List<Conversation>();
    private string characterName = "Input NPC name...";
    
    [MenuItem("Window/Dialogue Editor")]
    public static void ShowWindow()
    { 
        GetWindow<DialogueEditorWindow>("DialogueEditor");
    }

    private void OnGUI()
    {
        EditorStyles.textArea.wordWrap = true;
        EditorStyles.label.alignment = TextAnchor.UpperCenter;

        GUILayout.BeginVertical();
        
        GUILayout.Space(EditorGUIUtility.singleLineHeight);
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("NPC Name", EditorStyles.boldLabel);
        characterName = EditorGUILayout.TextField(characterName);
        GUILayout.EndHorizontal();

        GUILayout.Space(EditorGUIUtility.singleLineHeight);
        
        if (GUILayout.Button("New Dialogue"))
        {
            var conversation = new Conversation();
            conversations.Add(conversation);
        }
        
        EditorGUILayout.LabelField(String.Empty, GUI.skin.horizontalSlider);

        for(var i = 0; i < conversations.Count; i++)
        {
            conversations[i].displayed = EditorGUILayout.Foldout(conversations[i].displayed, "Conversation " + (i + 1));
            
            GUILayout.Space(EditorGUIUtility.singleLineHeight);
            
            if (conversations[i].displayed) {
                for (var j = 0; j < conversations[i].lines.Count; j++)
                {
                    GUILayout.Label("Line", EditorStyles.boldLabel);
                
                    conversations[i].lines[j].line = EditorGUILayout.TextArea(conversations[i].lines[j].line, EditorStyles.textArea, GUILayout.MinWidth(250), GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 3));

                    GUILayout.Space(EditorGUIUtility.singleLineHeight/2);
                    
                    conversations[i].lines[j].endConversation = GUILayout.Toggle(conversations[i].lines[j].endConversation, "Ends Conversation");
                    
                    GUILayout.Space(EditorGUIUtility.singleLineHeight);

                    if (!conversations[i].lines[j].endConversation)
                    {
                        conversations[i].lines[j].responsesDisplayed = EditorGUILayout.Foldout(conversations[i].lines[j].responsesDisplayed, "Player Choices");
                        
                        if (conversations[i].lines[j].responsesDisplayed)
                        {
                            for (var k = 0; k < conversations[i].lines[j].responses.Count; k++)
                            {
                                GUILayout.Label("Response " + conversations[i].lines.Count, EditorStyles.centeredGreyMiniLabel);
                                conversations[i].lines[j].responses[k].response = EditorGUILayout.TextArea(conversations[i].lines[j].responses[k].response, EditorStyles.textArea, GUILayout.MinWidth(250), GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 2));
                                if (GUILayout.Button("Delete Response"))
                                {
                                    conversations[i].lines[i].responses.Remove(conversations[i].lines[i].responses[j]);
                                }
                            }
                
                            GUILayout.Space(EditorGUIUtility.singleLineHeight);
                    
                            if (GUILayout.Button("Add Response")) {
                                conversations[i].lines[j].responses.Add(new Response());
                            }
                    
                            EditorGUILayout.LabelField(String.Empty, GUI.skin.horizontalSlider);   
                        }
                        
                        if (GUILayout.Button("Add Line")){
                            conversations[i].lines.Add(new Line());
                        }
                    }
                }

                if (GUILayout.Button("Delete Conversation"))
                {
                    conversations.Remove(conversations[i]);
                }
                
                GUILayout.Space(EditorGUIUtility.singleLineHeight/2);
            }
            
            EditorGUILayout.LabelField(String.Empty, GUI.skin.horizontalSlider);
        }

        GUILayout.EndVertical();
    }
}
