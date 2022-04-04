using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphSaver
{
    private List<Edge> Edges => _graphView.edges.ToList();
    private List<EditorNodeBase> Nodes => _graphView.nodes.ToList().Cast<EditorNodeBase>().ToList();

    private DialogueContainerSO _dialogueContainer;
    private DialogueGraphView _graphView;

    public static GraphSaver GetInstance(DialogueGraphView graphView)
    {
        return new GraphSaver
        {
            _graphView = graphView
        };
    }

    public void SaveGraph(string fileName)
    {
        DialogueContainerSO dialogueContainer = ScriptableObject.CreateInstance<DialogueContainerSO>();
    }
/*
    private bool SaveNodes(string fileName, DialogueContainerSO container)
    {
        if(!Edges.Any()) return false;
        Edge[] connectedSockets = Edges.Where(x => x.input.node != null).ToArray();
    }
*/
}
