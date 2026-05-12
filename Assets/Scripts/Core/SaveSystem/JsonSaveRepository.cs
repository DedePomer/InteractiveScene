using Core.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace Core.SaveSystemm
{
    public class JsonSaveRepository : MonoBehaviour
    {
        public string FilePath { get; set; }

        public void Save(List<SceneObjectData> data)
        {
            Debug.Log(FilePath);
            string json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FilePath, json);
            Debug.Log("Saved");
        }

        public List<SceneObjectData> Load()
        {
            string json = File.ReadAllText(FilePath);           
            return JsonConvert.DeserializeObject<List<SceneObjectData>>(json);
        }

        public bool HasExist() => File.Exists(FilePath);
    }
}

