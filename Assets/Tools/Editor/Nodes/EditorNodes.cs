using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class EditorNodeBase : Node
{
    public string GUID;
    public string nodeName;
    public Vector2 graphLocation;
    public ENodeType nodeType;
    public LocalisableText dialogueText;
    public EditorNodeBase[] connectedNodes;
    public bool entry = false;
}

public class EdEntryNode : EditorNodeBase
{
    
}

public class EdRandomNode : EditorNodeBase
{
    
}

public class EdSpeakNode : EditorNodeBase
{
    
}

public class EdPlayerNode : EditorNodeBase
{
    public EdDialogueChoices[] Choices;
}

public class EdDialogueChoices
{
    public LocalisableText dialogueText;
    public EditorNodeBase connectedNode;
}

public enum ENodeType
{
    EMPTY,
    ENTRY,
    SPEAK,
    PLAYER,
    RANDOM
}