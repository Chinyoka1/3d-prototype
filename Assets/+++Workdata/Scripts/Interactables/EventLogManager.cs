using System.Collections;
using TMPro;
using UnityEngine;

public class EventLogManager : MonoBehaviour
{
    public TextMeshProUGUI eventLog_Text;
    public Animator anim;
    public float visibleTime;

    private Coroutine _coroutine;

    public void SetEventLogText(string textValue)
    {
        if (_coroutine != null) return;
        
        if (!anim.enabled)
        {
            anim.enabled = true;
        }
        
        eventLog_Text.SetText(textValue);

        _coroutine = StartCoroutine(InitiateEventLog());
    }

    IEnumerator InitiateEventLog()
    {
        anim.Play("FadeIn");
        yield return new WaitForSeconds(visibleTime);
        anim.Play("FadeOut");
        yield return new WaitForSeconds(1);
        _coroutine = null;
    }
}