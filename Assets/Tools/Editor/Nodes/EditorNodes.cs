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
    public string[] connectedNodes;
    public bool entry = false;
    public DialogueGraphView graphView;
    public int eventID_editor = 0;
    public List<EdDialogueChoice> choices = new List<EdDialogueChoice>();

    public Label previewText;
    protected int previewLength = 25;
    protected int choicePreviewLength = 15;

    public override void OnSelected()
    {
        base.OnSelected();
        if(!entry)
        {
            //graphView.LoadNodeEditor(this);
            NodeInspector.Init(this, graphView);
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
        if(nodeType != ENodeType.SPEAK)
        {
            return;
        }
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

    public void AddNewPlayerChoice(string choiceText = "New Choice")
    {
        EdDialogueChoice newChoice = new EdDialogueChoice();
        choices.Add(newChoice);
        newChoice.dialogueText.engText = choiceText;

        newChoice.previewLabel = new Label(choiceText);
        newChoice.port = graphView.AddChoicePort(this);
        newChoice.port.portName = choiceText;
    }

    public void RemovePlayerChoice()
    {
        if(choices.Count < 1)
        {
            return;
        }
        graphView.RemoveLastPort(this);
        choices.RemoveAt(choices.Count - 1);
    }

    public void UpdatePlayerChoice(string newText, int index)
    {
        choices[index].dialogueText.engText = newText;
        string portNameText;
        if (newText.Length > choicePreviewLength)
        {
            portNameText = newText.Substring(0, choicePreviewLength);
        }
        else
        {
            portNameText = newText;
        }
        choices[index].port.portName = portNameText;

        choices[index].portName = portNameText;

        choices[index].port.MarkDirtyRepaint();
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
    //public EdDialogueChoices[] Choices;
}

public class EdDialogueChoice
{
    public LocalisableText dialogueText = new LocalisableText();
    public string connectedNode;
    public Label previewLabel;
    public Port port;
    public string portName;
}
