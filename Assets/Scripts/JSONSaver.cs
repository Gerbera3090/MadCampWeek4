using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum DashBoardElements
{
    NickName,
    Score,
    LifeTime
}
public static class JSONSaver
{
    public static string getPath(GameType gameType)
    {
        Dictionary<GameType, string> d = new Dictionary<GameType, string>
        {
            { GameType.Story, "story" },
            { GameType.Challenge, "challenge" },
            { GameType.Free, "free" }
        };
        return Application.dataPath + "/data_"+d[gameType]+".json";
    }
    
    public static List<Dictionary<DashBoardElements, string>> LoadList(GameType gameType)
    {
        string filePath = getPath(gameType);
        List<Dictionary<DashBoardElements, string>> result;
        if (!System.IO.File.Exists(filePath))
            return new List<Dictionary<DashBoardElements, string>>();

        string jsonString = System.IO.File.ReadAllText(filePath);
        if(JsonUtility.FromJson<SerializableList<Dictionary<DashBoardElements, string>>>(jsonString).list==null)return new List<Dictionary<DashBoardElements, string>>();
        return JsonUtility.FromJson<SerializableList<Dictionary<DashBoardElements, string>>>(jsonString).list;
        
    }
    
    public static void SaveList(GameType gameType, List<Dictionary<DashBoardElements, string>> dataList)
    {
        string filePath = getPath(gameType);
        string jsonString = JsonUtility.ToJson(new SerializableList<Dictionary<DashBoardElements, string>>(dataList));
        System.IO.File.WriteAllText(filePath, jsonString);
    }

    public static void AddList(GameType gameType, Dictionary<DashBoardElements, string> data)
    {
        string filePath = getPath(gameType);

        // Load the existing list from the file
        List<Dictionary<DashBoardElements, string>> dataList = LoadList(gameType);

        // Add the new data to the list
        dataList.Add(data);

        // Save the updated list to the file
        SaveList(gameType, dataList);
    }
    
    [System.Serializable]
    private class SerializableList<T>
    {
        public List<T> list;

        public SerializableList(List<T> data)
        {
            list = data;
        }

        public List<T> ToList()
        {
            return list;
        }
    }
}

