using Core.Model;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Core.SaveSystemm
{
    public class JsonSaveRepository: MonoBehaviour
    {
        private const string DefaultFileName = "objects.json";
        public  string FilePath { get; set; }

        private void Awake()
        {
            FilePath = Path.Combine(Application.persistentDataPath, DefaultFileName);
        }

        public void Save(List<SceneObjectData> data)
        {
            string json = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(FilePath, json);
        }

        public List<SceneObjectData> Load()
        {
            if (!HasExist())
            {
                Debug.LogError("load return null", this);
                return null;
            }
            string json = File.ReadAllText(FilePath);
            return JsonUtility.FromJson<List<SceneObjectData>>(json);
        }

        public bool HasExist() => File.Exists(FilePath);
    }
}

