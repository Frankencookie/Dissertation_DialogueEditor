using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEditor.Events;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
    public DialogueHandler self;
    public TextMeshProUGUI spokenTextComponent;
    public GameObject dialogueOptionPrefab;
    public GameObject dialogueOptionParent;
    public string dialogueOptionTextObjectName;
    public List<GameObject> dialogueOptions = new List<GameObject>();
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
        int RandomNumber = Random.Range(0, currentNode.ConnectedNodes.Count);
        currentNode = FindNodeByGUID(currentNode.ConnectedNodes[RandomNumber]);
        ProcessNextNode();
    }

    public void ProcessNextNode()
    {
        //Make the spoken text appear if hidden
        spokenTextComponent.enabled = true;

        //If dialogue options are on the screen, get rid of them
        if(dialogueOptions.Count > 0)
        {
            for(int i = 0; i < dialogueOptions.Count; i++)
            {
                Destroy(dialogueOptions[i]);
            }
            dialogueOptions.Clear();
        }

        switch(currentNode.nodeType)
        {
            case ENodeType.SPEAK:
                currentText = currentNode.dialogueText.engText;
                break;
            case ENodeType.RANDOM:
                GetRandomNode();
                return;
            case ENodeType.PLAYER:
                //Hide the spoken text object
                spokenTextComponent.enabled = false;
                int number = 0;
                //Get All choices and add them
                foreach (var item in currentNode.choices)
                {
                    GameObject newItem = Instantiate(dialogueOptionPrefab, Vector3.zero, Quaternion.identity, dialogueOptionParent.transform);
                    newItem.transform.Find(dialogueOptionTextObjectName).GetComponent<TextMeshProUGUI>().text = item.dialogueText.engText;
                    //newItem.GetComponent<Button>().onClick
                    UnityEventTools.AddIntPersistentListener(newItem.GetComponent<Button>().onClick, new UnityAction<int>(OptionChosen), number);
                    dialogueOptions.Add(newItem);
                    number++;
                }
                break;
            }
    }

    public void OptionChosen(int id)
    {
        Debug.Log(id);
        currentNode = FindNodeByGUID(currentNode.choices[id].connectedNode);
        currentTime = 0;
        ProcessNextNode();
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