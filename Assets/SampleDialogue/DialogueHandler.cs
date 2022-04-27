using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
    public DialogueHandler self;
    public TextMeshProUGUI spokenTextComponent;
    public GameObject dialogueOptionPrefab;
    public DialogueContainerSO dialogueObject;
    float currentTime = 0;
    float changeTime = 3;

    string currentText;
    DialogueNodeBase currentNode;
    bool dialogueActive = false;

    // Start is called before the first frame update
    void Start()
    {
        self = this;
        BeginDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueActive)
        {
            spokenTextComponent.text = currentText;
            currentTime += Time.deltaTime;

            if(currentTime > changeTime && currentNode.nodeType == ENodeType.SPEAK)
            {
                GetNextLine();
            }
        }
    }

    public void GetNextLine()
    {
        if(currentNode.ConnectedNodes.Count > 0)
        {
            currentTime = 0;
            currentNode = FindNodeByGUID(currentNode.ConnectedNodes[0]);
            ProcessNextNode();
        }
        else
        {
            EndDialogue();
        }
    }

    public void GetRandomNode()
    {
        Debug.Log(currentNode.ConnectedNodes.Count);
        int RandomNumber = Random.Range(0, currentNode.ConnectedNodes.Count);
        Debug.Log(RandomNumber);
        currentNode = FindNodeByGUID(currentNode.ConnectedNodes[RandomNumber]);
        ProcessNextNode();
    }

    public void ProcessNextNode()
    {
        switch(currentNode.nodeType)
        {
            case ENodeType.SPEAK:
                currentText = currentNode.dialogueText.engText;
                break;
            case ENodeType.RANDOM:
                GetRandomNode();
                return;
            }
    }

    public void BeginDialogue()
    {
        currentNode = FindNodeByGUID(dialogueObject.nodes[0].ConnectedNodes[0]);
        dialogueActive = true;
        ProcessNextNode();
        currentTime = 0;
    }

    public void EndDialogue()
    {
        dialogueActive = false;
        currentTime = 0;
    }

    public DialogueNodeBase FindNodeByGUID(string guid)
    {
        foreach (var item in dialogueObject.nodes)
        {
            if(item.GUID == guid)
            {
                return item;
            }
        }

        return null;
    }
}

public class DialogueSelectItem
{

}

public enum NodeType
{
    NODE_SPEAK,
    NODE_CHOOSE
}