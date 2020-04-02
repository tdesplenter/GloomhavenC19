using System.IO;
using UnityEngine;

public class JsonSaver
{
  private static readonly string _fileName = "ScenarioData.sav";

  public static string GetPathToSaveFile()
  {
    return Application.persistentDataPath + "/" + _fileName;
  }

  public void Save(SaveData data)
  {
    var json = JsonUtility.ToJson(data);
    var saveFileName = GetPathToSaveFile();

    File.WriteAllText(saveFileName, json);
  }

  public bool Load(SaveData data)
  {
    var saveFileName = GetPathToSaveFile();

    Debug.Log($"Saving to {saveFileName}");

    if (File.Exists(saveFileName))
    {
      var json = File.ReadAllText(saveFileName);
      JsonUtility.FromJsonOverwrite(json, data);
      return true;
    }
    return false;
  }

  public void Delete()
  {
    File.Delete(GetPathToSaveFile());
  }
}
