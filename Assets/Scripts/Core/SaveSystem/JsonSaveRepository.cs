using Core.Model;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Core.SaveSystemm
{
    public class JsonSaveRepository: MonoBehaviour
    {
        private const string DefaultFileName = "objects.json";

        private readonly string _filePath;
        
        public JsonSaveRepository(string folderPath, string fileName = DefaultFileName)
        {
            _filePath = Path.Combine(folderPath, fileName);
        }

        public void Save(List<SceneObjectData> data)
        {
            string json = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(_filePath, json);
        }

        public List<SceneObjectData> Load()
        {
            if (!HasExist())
            {
                Debug.LogError("load return null", this);
                return null;
            }
            string json = File.ReadAllText(_filePath);
            return JsonUtility.FromJson<List<SceneObjectData>>(json);
        }

        public bool HasExist() => File.Exists(_filePath);
    }
}

