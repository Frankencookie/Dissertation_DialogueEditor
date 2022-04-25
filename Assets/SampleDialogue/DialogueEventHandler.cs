using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DialogueEventHandler : MonoBehaviour
{
    public delegate void OnDialogueEvent();
    public delegate void OnDialogueChangeEvent(int id);
    public delegate void OnDialogueCharacterChangeEvent(string name);

    public static OnDialogueEvent OnDialogueOpen;
    public static OnDialogueEvent OnDialogueClose;

    public static OnDialogueChangeEvent OnDialogueNodeStart;
    public static OnDialogueChangeEvent OnDialogueNodeFinish;
    public static OnDialogueChangeEvent OnDialogueChoiceChosen;

    public static OnDialogueCharacterChangeEvent OnDialogueCharacterChange;

    #region EventDispatchers

    //For when rhe conversation starts
    public static void DialogueOpen()
    {
        OnDialogueOpen?.Invoke();
    }

    //For when the conversation ends
    public static void DialogueClose()
    {
        OnDialogueClose?.Invoke();
    }

    //For when a new node is displayed
    public static void DialogueNodeStart(int eventId)
    {
        OnDialogueNodeStart?.Invoke(eventId);
    }

    //For when a node stops being displayed
    public static void DialogueNodeFinish(int eventId)
    {
        OnDialogueNodeFinish?.Invoke(eventId);
    }

    //For when the player has chosen a dialogue choice
    public static void DialogueChoiceChosen(int eventId)
    {
        OnDialogueChoiceChosen?.Invoke(eventId);
    }

    //For when the current speaking character changes
    public static void DialogueCharacterChange(string newCharacterName)
    {
        OnDialogueCharacterChange?.Invoke(newCharacterName);
    }

    #endregion
}