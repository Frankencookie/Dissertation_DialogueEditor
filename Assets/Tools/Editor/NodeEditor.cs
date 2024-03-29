using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Linq;

public class NodeEditor : Node
{
    public EditorNodeBase nodeToEdit;
    public ENodeType typeOfNode;
}

public class NodeInspector : EditorWindow
{
    public static EditorNodeBase nodeToEdit;
    public static ENodeType typeOfNode;
    public static DialogueGraphView _graphView;

    public static void Init(EditorNodeBase toEdit, DialogueGraphView graphView)
    {
        nodeToEdit = toEdit;
        typeOfNode = toEdit.nodeType;
        _graphView = graphView;
        NodeInspector nodeInspector = (NodeInspector)EditorWindow.GetWindow(typeof(NodeInspector));
        nodeInspector.Show();
    }

    public static void EmptyInspector()
    {
        nodeToEdit = null;
        typeOfNode = ENodeType.EMPTY;
    }

    void OnGUI()
    {
        GUILayout.Label("Dialogue Node Editor", EditorStyles.boldLabel);

        if(nodeToEdit == null)
        {
            GUILayout.Label("No Node Selected");
            return;
        }

        GUILayout.Label("Node is a " + nodeToEdit.nodeType.ToString() + " Node");

        EditorGUILayout.Separator();

        nodeToEdit.ChangeName(EditorGUILayout.TextField("Node Name", nodeToEdit.nodeName));

        GUILayout.Space(1);

        nodeToEdit.eventID_editor = EditorGUILayout.IntField("Event ID", nodeToEdit.eventID_editor);

        EditorGUILayout.Separator();

        switch(nodeToEdit.nodeType)
        {
            case ENodeType.RANDOM:
                if(GUILayout.Button("Add new Option"))
                {
                    _graphView.AddPort(nodeToEdit);
                }
                if(GUILayout.Button("Remove Option"))
                {
                    _graphView.RemoveLastPort(nodeToEdit);
                }
                break;
            case ENodeType.SPEAK:
                GUILayout.Label("Dialogue Text", EditorStyles.boldLabel);
                nodeToEdit.UpdateText(EditorGUILayout.TextArea(nodeToEdit.dialogueText.engText, GUILayout.Height(100)));
                break;

            case ENodeType.PLAYER:
                //Display all text fields
                Debug.Log(nodeToEdit.choices.Count);
                for(int i = 0; i < nodeToEdit.choices.Count; i++)
                {
                    GUILayout.Label("Choice " + i, EditorStyles.boldLabel);
                    nodeToEdit.UpdatePlayerChoice(EditorGUILayout.TextArea(nodeToEdit.choices[i].dialogueText.engText, GUILayout.Height(100)), i);
                }
                //Add and remove field buttons
                if(GUILayout.Button("Add New Choice"))
                {
                    nodeToEdit.AddNewPlayerChoice();
                }
                if(GUILayout.Button("Remove Choice"))
                {
                    //_graphView.RemoveLastPort(nodeToEdit);
                    nodeToEdit.RemovePlayerChoice();
                }
                break;
        }

    }
}