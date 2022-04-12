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
    //private List<EditorNodeBase> Nodes => _graphView.nodes.ToList().Cast<EditorNodeBase>().ToList();
    private List<Node> Nodes => _graphView.nodes.ToList();

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
        if(!SaveNodes(fileName, dialogueContainer)) return;

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");

        UnityEngine.Object loadedAsset = AssetDatabase.LoadAssetAtPath($"Assets/Resources/{fileName}.asset", typeof(DialogueContainerSO));

        if (loadedAsset == null || !AssetDatabase.Contains(loadedAsset)) 
		{
            AssetDatabase.CreateAsset(dialogueContainer, $"Assets/Resources/{fileName}.asset");
        }
        else
        {
            DialogueContainerSO container = loadedAsset as DialogueContainerSO;
            container.linkData = dialogueContainer.linkData;
            container.nodes = dialogueContainer.nodes;

            EditorUtility.SetDirty(container);
        }

        AssetDatabase.SaveAssets();
    }

    private bool SaveNodes(string fileName, DialogueContainerSO container)
    {
        if(!Edges.Any()) return false;

        foreach(Node node in Nodes)
        {
            EditorNodeBase nodeBase = (node as EditorNodeBase);
            if(nodeBase != null)
            {
                CreateNodeAndAdd(nodeBase.nodeType, container, nodeBase);
            }
        }
        Edge[] connectedSockets = Edges.Where(x => x.input.node != null).ToArray();
        for(int i = 0; i < connectedSockets.Count(); i++)
        {
            EditorNodeBase outNode = (connectedSockets[i].output.node as EditorNodeBase);
            EditorNodeBase inNode = (connectedSockets[i].output.node as EditorNodeBase);
            container.linkData.Add(new NodeLinkData
            {
                BaseNodeGUID = outNode.GUID,
                PortName = connectedSockets[i].output.portName,
                TargetNodeGUID = inNode.GUID
            });
        }

        ConnectUpNodes(connectedSockets, container);

        return true;
    }

    void CreateNodeAndAdd(ENodeType type, DialogueContainerSO container, EditorNodeBase inNode)
    {
        Debug.Log(inNode.nodeType);
        DialogueNodeBase newNode;
        switch (inNode.nodeType)
        {
            case ENodeType.EMPTY:
                newNode = new DialogueNodeBase();
                break;
            case ENodeType.ENTRY:
                newNode = new EntryNode();
                newNode.entry = true;
                break;
            case ENodeType.SPEAK:
                newNode = new SpeakNode();
                (newNode as SpeakNode).dialogueText = inNode.dialogueText;
                break;
            case ENodeType.PLAYER:
                newNode = new PlayerNode();

                foreach(EdDialogueChoices choice in (inNode as EdPlayerNode).Choices)
                {
                    (newNode as PlayerNode).choices.Add(new DialogueNodeChoice
                    {
                        dialogueText = choice.dialogueText
                    });
                }

                break;
            case ENodeType.RANDOM:
                newNode = new RandomNode();
                break;
            default:
                newNode = new DialogueNodeBase();
                break;
        }

        newNode.GUID = inNode.GUID;
        newNode.nodeType = type;
        newNode.dialogueText = inNode.dialogueText;

        container.nodes.Add(newNode);
    }

    void ConnectUpNodes(Edge[] connectedEdges, DialogueContainerSO container)
    {
        for(int i = 0; i < connectedEdges.Count(); i++)
        {
            EditorNodeBase outNode = (connectedEdges[i].output.node as EditorNodeBase);
            EditorNodeBase inNode = (connectedEdges[i].input.node as EditorNodeBase);

            DialogueNodeBase outDiagNode = new DialogueNodeBase();
            DialogueNodeBase inDiagNode = new DialogueNodeBase();

            for(int j = 0; j < container.nodes.Count(); j++)
            {
                if(container.nodes[j].GUID == outNode.GUID)
                {
                    outDiagNode = container.nodes[j];
                }
            }

            for(int j = 0; j < container.nodes.Count(); j++)
            {
                if(container.nodes[j].GUID == inNode.GUID)
                {
                    inDiagNode = container.nodes[j];
                }
            }

            switch(outDiagNode.nodeType)
            {
                case ENodeType.EMPTY:
                    break;
                case ENodeType.ENTRY:
                    (outDiagNode as EntryNode).ConnectedNodes.Add(inDiagNode);
                    break;
                case ENodeType.SPEAK:
                    (outDiagNode as SpeakNode).ConnectedNodes.Add(inDiagNode);
                    break;
                case ENodeType.PLAYER:
                    break;
                case ENodeType.RANDOM:
                    break;
                default:
                    break;
            }
        }
    }

}