using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNodeBase
{
    public string GUID;
    public ENodeType nodeType;
    public bool entry = false;
    public LocalisableText dialogueText;
    public List<DialogueNodeBase> ConnectedNodes = new List<DialogueNodeBase>();
}

public class EntryNode : DialogueNodeBase
{
    //public DialogueNodeBase ConnectedNode;
}

public class RandomNode : DialogueNodeBase
{
    //public DialogueNodeBase[] ConnectedNodes;
}

public class SpeakNode : DialogueNodeBase
{
    //public DialogueNodeBase ConnectedNode;
    //public LocalisableText dialogueText;
}

public class PlayerNode : DialogueNodeBase
{
    public List<DialogueNodeChoice> choices;
}

public class DialogueNodeChoice
{
    public LocalisableText dialogueText;
    public DialogueNodeBase connectedNode;
}

public enum ENodeType
{
    EMPTY,
    ENTRY,
    SPEAK,
    PLAYER,
    RANDOM
}