using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNodeBase
{
    public string GUID;
}

public class EntryNode : DialogueNodeBase
{
    public DialogueNodeBase ConnectedNode;
}

public class RandomNode : DialogueNodeBase
{
    public DialogueNodeBase[] ConnectedNodes;
}

public class SpeakNode : DialogueNodeBase
{
    public DialogueNodeBase ConnectedNode;
    public LocalisableText dialogueText;
}

public class PlayerNode : DialogueNodeBase
{

}