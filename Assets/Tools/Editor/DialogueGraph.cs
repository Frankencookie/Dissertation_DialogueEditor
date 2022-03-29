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
        GenerateMiniMap();
        //StartupPrompt();
    }

    private void GenerateMiniMap()
    {
        MiniMap miniMap = new MiniMap();
        miniMap.anchored = true;

        Vector2 pos = graphView.contentViewContainer.WorldToLocal(new Vector2(this.maxSize.x - 10, 300));
        miniMap.SetPosition(new Rect(pos.x, pos.y, 200, 140));
        graphView.Add(miniMap);
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

        rootVisualElement.Add(toolBar);
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
                nodeName = "Random Node";
                break;
            default:
                break;
        }
        graphView.CreateNewNode(nodeName, type, Vector2.zero);
    }
}
