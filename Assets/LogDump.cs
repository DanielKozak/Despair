using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class LogDump
{

    static List<string> log;
    static LogDump()
    {

        log = new List<string>();
    }
    public static void Log(string data)
    {
        log.Add(data);
    }

    public static void Dump()
    {
        Debug.Log($"DUMP {Application.persistentDataPath + "dump.txt"}");

        File.WriteAllLines(Application.persistentDataPath + "dump.txt", log);
    }
}
