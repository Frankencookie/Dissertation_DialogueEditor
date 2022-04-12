using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueContainerSO : ScriptableObject
{
    public List<DialogueNodeBase> nodes = new List<DialogueNodeBase>();
    public List<NodeLinkData> linkData = new List<NodeLinkData>();
}
