using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBG_LogManager : Singleton<DBG_LogManager>
{

    [SerializeField] bool LogEvents = true;


    public void LogEvent(object _channel, object _event, params object[] _params)
    {
        if (LogEvents)
        {

            string paramsString = "";
            for (int i = 0; i < _params.Length; i++)
            {
                paramsString += $" {_params[i]}";
            }
            Debug.Log($"[<color=green>Broadcast</color>] <color=cyan>{_channel?.GetType().Name}</color> -> <color=cyan>{_event?.ToString()}</color> -> {paramsString}");
        }
    }
}
