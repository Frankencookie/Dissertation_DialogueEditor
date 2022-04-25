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

    public static void Init(EditorNodeBase toEdit)
    {
        nodeToEdit = toEdit;
        typeOfNode = toEdit.nodeType;
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

        //nodeToEdit.UpdateText(EditorGUILayout.TextField("Dialogue Text", nodeToEdit.dialogueText.engText));
        GUILayout.Label("Dialogue Text", EditorStyles.boldLabel);
        nodeToEdit.UpdateText(EditorGUILayout.TextArea(nodeToEdit.dialogueText.engText, GUILayout.Height(100)));

    }
}