using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView graphView;

    [MenuItem("Window/Dialogue Editor")]
    public static void OpenGraphWindow()
    {
        EditorWindow window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Editor");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
        //StartupPrompt();
    }

    private void ConstructGraphView()
    {
        graphView = new DialogueGraphView();
        graphView.name = "Dialogue Editor";

        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
    }

    private void GenerateToolbar()
    {
        Toolbar toolBar = new Toolbar();

        rootVisualElement.Add(toolBar);
    }

    private void StartupPrompt()
    {
        bool result = EditorUtility.DisplayDialog("Welcome", "Create New Dialogue Tree Or Open Existing?", "Create New", "Open Existing");
    }
}
