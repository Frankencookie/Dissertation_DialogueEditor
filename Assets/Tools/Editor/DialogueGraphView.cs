using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Linq;

public class DialogueGraphView : GraphView
{
    public readonly Vector2 DefaultNodeSize = new Vector2(200, 150);
    public EdEntryNode EntryPointNode;
    public NodeEditor nodeEditor;

    public DialogueGraphView()
    {
        //Load Stylesheet to make everything look cool
        styleSheets.Add(Resources.Load<StyleSheet>("DialogueGraph"));

        //Add controls
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        //Add grid background
        GridBackground grid = new GridBackground();
        Insert(0, grid);

        CreateNewNode("Entry", ENodeType.ENTRY, new Vector2(100, 200));
    }

    public EditorNodeBase CreateNewNode(string newNodeName, ENodeType type, Vector2 position)
    {
        EditorNodeBase newNode = CreateNode(newNodeName, type, position);
        AddElement(newNode);
        return newNode;
    }

    public EditorNodeBase CreateNode(string newNodeName, ENodeType type, Vector2 position)
    {
        EditorNodeBase newNode;
        bool entry = false;

        //Create correct node type
        switch (type)
        {
            case ENodeType.ENTRY:
                EntryPointNode = new EdEntryNode();
                newNode = EntryPointNode;
                entry = true;
                newNode.entry = true;
                newNode.tooltip = "This is where the dialogue tree starts";
                newNode.styleSheets.Add(Resources.Load<StyleSheet>("EntryNode"));
                
                break;

            case ENodeType.SPEAK:
                newNode = new EdSpeakNode();
                newNode.tooltip = "This controls what the NPC is saying";
                newNode.styleSheets.Add(Resources.Load<StyleSheet>("NPCnode"));
                break;

            case ENodeType.PLAYER:
                newNode = new EdPlayerNode();
                newNode.tooltip = "This is a dialogue choice node for the player. Use this to create branches";
                newNode.styleSheets.Add(Resources.Load<StyleSheet>("PlayerNode"));
                break;

            case ENodeType.RANDOM:
                newNode = new EdRandomNode();
                newNode.tooltip = "This node picks a connected node at random";
                newNode.styleSheets.Add(Resources.Load<StyleSheet>("RandomNode"));
                break;
            default:
                return null;
        }

        //Set up global node stuff
        newNode.GUID = Guid.NewGuid().ToString();
        newNode.nodeName = newNodeName;
        //newNode.dialogueText.texts[0] = newNodeName;
        newNode.nodeType = type;

        //Style sheet for looks
        //newNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));
        
        if(!entry) //If not an entry node, add an input port
        {
            Port inputPort = CreatePort(newNode, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "In";
            newNode.inputContainer.Add(inputPort);
            newNode.graphView = this;
            Debug.Log(newNode.dialogueText.engText);
            newNode.dialogueText.engText = "Sample Text";
        }
        else
        {

        }
        
        //Create Default Output Port
        if(newNode.nodeType != ENodeType.PLAYER)
        {
            Port outputPort = CreatePort(newNode, Direction.Output);
            outputPort.portName = "Out";
            newNode.outputContainer.Add(outputPort);
        }
        else
        {

        }

        //If a speak node, add text preview
        if(newNode.nodeType == ENodeType.SPEAK)
        {
            Label textPreview = new Label(newNode.dialogueText.engText);
            newNode.previewText = textPreview;
            newNode.contentContainer.Add(textPreview);
        }


        newNode.titleContainer.Add(new Label(newNode.nodeType.ToString()));

        RefreshNode(newNode);

        //Add Title Text
        newNode.title = newNodeName;

        //Set Node Position
        newNode.SetPosition(new Rect(position, DefaultNodeSize));

        return newNode;

    }

    private Port CreatePort(EditorNodeBase node, Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();
        var startPortView = startPort;

        ports.ForEach((port) =>
        {
            var portView = port;
            if (startPortView != portView && startPortView.node != portView.node)
                compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }

    public void AddPort(EditorNodeBase node)
    {
        Port newport = CreatePort(node, Direction.Output);
        newport.portName = "ExtraPort";

        node.outputContainer.Add(newport);

        RefreshNode(node);
    }

    public Port AddChoicePort(EditorNodeBase node)
    {
        Port newport = CreatePort(node, Direction.Output);

        node.outputContainer.Add(newport);
        RefreshNode(node);

        return newport;
    }

    public void RemovePort(EditorNodeBase node, Port portToRemove)
    {
        if(portToRemove == null)
        {
            return;
        }
        if(portToRemove.connected)
        {
            portToRemove.DisconnectAll();
        }

        node.outputContainer.Remove(portToRemove);
        RefreshNode(node);
    }

    public void RemoveLastPort(EditorNodeBase node)
    {
        //If number of output ports is 1, dont remove any
        Debug.Log(node.outputContainer.childCount);
        if(node.outputContainer.childCount < 1)
        {
            return;
        }

        //if a port has no connections, remove it
        Port portRef = null;
        foreach(var item in node.outputContainer.Children())
        {
            //If default port, continue
            if((item as Port).portName == "Out")
            {
                continue;
            }
            portRef = (item as Port);
            if(!portRef.connected)
            {
                RemovePort(node, portRef);
                return;
            }
        }

        //if no ports have been removed, remove the last one saved
        RemovePort(node, portRef);
    }

    public void RefreshNode(Node node)
    {
        node.RefreshExpandedState();
        node.RefreshPorts();
        node.MarkDirtyRepaint();
    }
}
