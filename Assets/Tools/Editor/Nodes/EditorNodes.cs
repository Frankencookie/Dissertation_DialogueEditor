using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Linq;

public class EditorNodeBase : Node
{
    public string GUID;
    public string nodeName;
    public Vector2 graphLocation;
    public ENodeType nodeType;
    public LocalisableText dialogueText = new LocalisableText();
    public EditorNodeBase[] connectedNodes;
    public bool entry = false;
    public DialogueGraphView graphView;
    public int eventID_editor = 0;

    public Label previewText;
    protected int previewLength = 25;

    public override void OnSelected()
    {
        base.OnSelected();
        if(!entry)
        {
            //graphView.LoadNodeEditor(this);
            NodeInspector.Init(this);
        }
    }

    public void ChangeName(string newName)
    {
        nodeName = newName;
        title = newName;
        graphView.RefreshNode(this);
        MarkDirtyRepaint();
    }

    public void UpdateText(string newText)
    {
        dialogueText.engText = newText;
        if (newText.Length > previewLength)
        {
            previewText.text = newText.Substring(0, previewLength);
        }
        else
        {
            previewText.text = newText;
        }
        previewText.MarkDirtyRepaint();
        MarkDirtyRepaint();
    }
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
