using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
    public TextMeshPro spokenTextComponent;
    public GameObject dialogueOptionPrefab;

    float currentTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class DialogueSequence
{

}

public class DialogueNode
{
    
}

public class DialogueSelectItem
{

}

public enum NodeType
{
    NODE_SPEAK,
    NODE_CHOOSE
}