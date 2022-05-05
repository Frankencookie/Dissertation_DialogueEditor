using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearAnimation : MonoBehaviour
{
    public AnimationCurve appearAnim;
    public AnimationCurve hideAnim;
    public RectTransform rectTransform;
    public GameObject backgroundObject;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        backgroundObject.SetActive(false);
    }

    void OnEnable()
    {
        InitEventCallbacks();
    }

    void OnDisable()
    {
        ClearEventCallbacks();
    }

    void InitEventCallbacks()
    {
        DialogueEventHandler.OnDialogueOpen += PlayAppearAnim;
        DialogueEventHandler.OnDialogueClose += PlayHideAnim;
    }

    void ClearEventCallbacks()
    {
        DialogueEventHandler.OnDialogueOpen -= PlayAppearAnim;
        DialogueEventHandler.OnDialogueClose -= PlayHideAnim;
    }

    void PlayAppearAnim()
    {
        StartCoroutine("AppearAnim");
    }

    void PlayHideAnim()
    {
        StartCoroutine("HideAnim");
    }

    IEnumerator AppearAnim()
    {
        backgroundObject.SetActive(true);
        float currentTime = 0;

        while(currentTime < appearAnim[appearAnim.length - 1].time)
        {
            rectTransform.anchoredPosition = new Vector3(0, appearAnim.Evaluate(currentTime), 0);
            currentTime += Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator HideAnim()
    {
        float currentTime = 0;

        while(currentTime < appearAnim[appearAnim.length - 1].time)
        {
            rectTransform.anchoredPosition = new Vector3(0, hideAnim.Evaluate(currentTime), 0);
            currentTime += Time.deltaTime;

            yield return null;
        }

        backgroundObject.SetActive(false);
    }
}
