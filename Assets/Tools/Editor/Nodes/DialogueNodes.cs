using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class EntryNode : Node
{
    public Node ConnectedNode;
    public string GUID;
}

public class RandomNode : Node
{
    public Node[] ConnectedNodes;
    public string GUID;
}