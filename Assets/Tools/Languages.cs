using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Languages
{
    ENGLISH,
    FRENCH,
    GERMAN,

    //Not a language, for use with the editor's internals. Don't touch.
    LANGUAGESLENGTH
}

[System.Serializable]
public struct LocalisableText
{
    //public string[] texts = new string[(int)Languages.LANGUAGESLENGTH];
    //public List<string> texts = new List<string>();
    //public string[] texts;
    public string engText;
}