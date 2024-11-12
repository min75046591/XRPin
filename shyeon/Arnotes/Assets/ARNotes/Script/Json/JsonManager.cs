using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonManager : MonoBehaviour
{
    private string filePath;
    private string jsonExtension;

    private void Awake()
    {
        this.filePath = Application.persistentDataPath;
        jsonExtension = ".json";
    }

    public void Save(Pin pin)
    {
        string pinJson = JsonUtility.ToJson(pin);
        string path = Path.Combine(Application.persistentDataPath, pin.GetPinName() + ".json");

        File.WriteAllText(path, pinJson);

    }

    public List<Pin> LoadAll()
    {
        List<Pin> pins = new List<Pin>();
        string[] jsonFiles = Directory.GetFiles(filePath, "*.json");
        foreach (string jsonFile in jsonFiles)
        {
            string jsonContent = File.ReadAllText(jsonFile);
            Pin pin = JsonUtility.FromJson<Pin>(jsonContent);
            pins.Add(pin);
        }
        return pins;
    }

    public void DeleteJsonFile(string fileName)
    {
        fileName += jsonExtension;
        string fileFullPath = Path.Combine(filePath, fileName);
        if (File.Exists(fileFullPath)) File.Delete(fileFullPath);
        
    }
}
