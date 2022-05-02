using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView graphView;
    private string fileName = "NewFile";

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
        Button newNodeButton = new Button(() => CreateNewNode(ENodeType.SPEAK));
        newNodeButton.text = "New Speak Node";
        toolBar.Add(newNodeButton);

        Button newPlayerNode = new Button(() => CreateNewNode(ENodeType.PLAYER));
        newPlayerNode.text = "New Player Node";
        toolBar.Add(newPlayerNode);

        Button newRandomNode = new Button(() => CreateNewNode(ENodeType.RANDOM));
        newRandomNode.text = "New Random Node";
        toolBar.Add(newRandomNode);

        toolBar.Add(new Button(() => SaveGraph()) {text = "Save"});

        toolBar.Add(new Button(() => LoadGraph()){text = "Load"});

        TextField fileNameField = new TextField("Name: ");
        fileNameField.SetValueWithoutNotify(fileName);
        fileNameField.MarkDirtyRepaint();
        fileNameField.RegisterValueChangedCallback(evt => fileName = evt.newValue);
        toolBar.Add(fileNameField);

        rootVisualElement.Add(toolBar);
    }

    private void SaveGraph()
    {
        GraphSaver saver = GraphSaver.GetInstance(graphView);
        saver.SaveGraph(fileName);
    }

    private void LoadGraph()
    {
        GraphSaver saver = GraphSaver.GetInstance(graphView);
        saver.LoadGraph(fileName);
    }

    private void StartupPrompt()
    {
        bool result = EditorUtility.DisplayDialog("Welcome", "Create New Dialogue Tree Or Open Existing?", "Create New", "Open Existing");
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(graphView);
    }

    private void CreateNewNode(ENodeType type)
    {
        string nodeName = "NewNode";
        switch (type)
        {
            case ENodeType.SPEAK:
                nodeName = "Speak Node";
                break;
            case ENodeType.PLAYER:
                nodeName = "Player Node";
                break;
            case ENodeType.RANDOM:
                nodeName = "Random";
                break;
            default:
                break;
        }
        graphView.CreateNewNode(nodeName, type, Vector2.zero);
    }
}
