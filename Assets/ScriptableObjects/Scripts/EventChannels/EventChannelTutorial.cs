using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "EventChannels/EventChannelTutorial", fileName = "EventChannelTutorial")]
public class EventChannelTutorial : ScriptableObject
{
    public UnityAction<string, Vector2, string> ShowTutorialMessageEvent;
    public UnityAction<string> ConfirmTutorialMessageEvent;


    public void RaiseShowTutorialMessageEvent(string factID, Vector2 screenPos, string textKey)
    {
        DBG_LogManager.Instance.LogEvent(this, "ShowTutorialMessageEvent");

        ShowTutorialMessageEvent?.Invoke(factID, screenPos, textKey);
    }
    public void RaiseConfirmTutorialMessageEvent(string factID)
    {
        DBG_LogManager.Instance.LogEvent(this, "ConfirmTutorialMessageEvent");

        ConfirmTutorialMessageEvent?.Invoke(factID);
    }

}