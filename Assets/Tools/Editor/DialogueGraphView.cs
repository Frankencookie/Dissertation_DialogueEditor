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
                
                break;

            case ENodeType.SPEAK:
                newNode = new EdSpeakNode();
                newNode.tooltip = "This controls what the NPC is saying";
                break;

            case ENodeType.PLAYER:
                newNode = new EdPlayerNode();
                newNode.tooltip = "This is a dialogue choice node for the player. Use this to create branches";
                break;

            case ENodeType.RANDOM:
                newNode = new EdRandomNode();
                newNode.tooltip = "This node picks a connected node at random";
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
        newNode.styleSheets.Add(Resources.Load<StyleSheet>("Node"));
        
        if(!entry) //If not an entry node, add an input port
        {
            Port inputPort = GetPortInstance(newNode, Direction.Input, Port.Capacity.Multi);
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
        Port outputPort = GetPortInstance(newNode, Direction.Output);
        outputPort.portName = "Out";
        newNode.outputContainer.Add(outputPort);

        Label textPreview = new Label(newNode.dialogueText.engText);
        newNode.previewText = textPreview;
        newNode.contentContainer.Add(textPreview);
        newNode.titleContainer.Add(new Label(newNode.nodeType.ToString()));

        RefreshNode(newNode);

        //Add Title Text
        newNode.title = newNodeName;

        //Set Node Position
        newNode.SetPosition(new Rect(position, DefaultNodeSize));

        return newNode;

    }

    private Port GetPortInstance(EditorNodeBase node, Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
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

    public void RefreshNode(Node node)
    {
        node.RefreshExpandedState();
        node.RefreshPorts();
        node.MarkDirtyRepaint();
    }

    public void LoadNodeEditor(EditorNodeBase node)
    {
        if(nodeEditor != null)
        {
            RemoveElement(nodeEditor);
            nodeEditor = null;
        }
        nodeEditor = new NodeEditor();
        nodeEditor.nodeToEdit = node;
        nodeEditor.typeOfNode = node.nodeType;
        nodeEditor.titleContainer.Add(new Label(node.nodeType.ToString()));

        nodeEditor.SetPosition(new Rect(10, 30, 300, 400));
        nodeEditor.title = "Node Editor";

        TextField nameInput = new TextField();
        nameInput.name = "Node Name";
        nameInput.value = node.nodeName;
        nameInput.RegisterValueChangedCallback(evt => node.ChangeName(evt.newValue));
        nodeEditor.contentContainer.Add(nameInput);

        TextField textInput = new TextField();
        textInput.name = "Text";
        textInput.value = node.dialogueText.engText;

        textInput.RegisterValueChangedCallback(evt => node.UpdateText(evt.newValue));
        nodeEditor.contentContainer.Add(textInput);

        RefreshNode(node);

        AddElement(nodeEditor);
    }

    public void SaveNodeChanges()
    {

    }
}
