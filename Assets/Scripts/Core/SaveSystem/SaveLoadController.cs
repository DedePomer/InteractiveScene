using Core.Model;
using Core.SaveSystemm;
using Core.Scene;
using Newtonsoft.Json;
using SFB;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Core.SaveSystem
{

    [DisallowMultipleComponent, RequireComponent(typeof(SaveLoadInput))]
    public class SaveLoadController : MonoBehaviour
    {
        private const string FileExtension = "json";
        private const string DefaultFileName = "save";
        private const string DefaultLoadTitle = "Load file";
        private const string DefaultSaveTitle = "Save file";

        [SerializeField] private SceneObjectRegistry sceneObjectRegistry;
        [SerializeField] private SaveLoadInput saveLoadInput;

        private string _filePath;

        private void Awake()
        {
            saveLoadInput = GetComponent<SaveLoadInput>();
        }
        private void OnEnable()
        {
            saveLoadInput.OnLoad += Load;
            saveLoadInput.OnSave += Save;
        }

        private void OnDisable()
        {
            saveLoadInput.OnLoad -= Load;
            saveLoadInput.OnSave -= Save;
        }

        public void Load()
        {
            if (!IsLoadDialogClose())
                return;
            if (!FileExist())
            {
                Debug.LogError("file did not load", this);
                return;
            }

            string json = File.ReadAllText(_filePath);
            var data = JsonConvert.DeserializeObject<List<SceneObjectData>>(json);
            SetDataFromSceneObject(data);
            Debug.Log("Loaded");
        }

        public void Save()
        {
            if (!IsSaveDialogClose())
                return;


            var data = GetDataFromSceneObject(sceneObjectRegistry.GetAll());
            string json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(_filePath, json);
            Debug.Log("Saved");
        }

        private bool IsSaveDialogClose()
        {
            string path = StandaloneFileBrowser.SaveFilePanel(
                title: DefaultSaveTitle,
                directory: Application.persistentDataPath,
                defaultName: DefaultFileName,
                extension: FileExtension
            );

            if (!string.IsNullOrEmpty(path))
            {
                _filePath = path;
                return true;
            }
            return false;
        }

        private bool IsLoadDialogClose()
        {
            string[] paths = StandaloneFileBrowser.OpenFilePanel(
                title: DefaultLoadTitle,
                directory: Application.persistentDataPath,
                extension: FileExtension,
                multiselect: false
            );

            if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
            {
                _filePath = paths[0];
                return true;
            }
            return false;
        }

        private List<SceneObjectData> GetDataFromSceneObject(IReadOnlyList<SceneObjectController> scenObjects)
        {
            List<SceneObjectData> sceneObjectDatas = new List<SceneObjectData>();

            foreach (var obj in scenObjects)
            {
                sceneObjectDatas.Add(obj.GetObjectData());
            }

            return sceneObjectDatas;
        }

        private void SetDataFromSceneObject(List<SceneObjectData> scenObjectsData)
        {
            var sceneObjects = sceneObjectRegistry.GetAll();

            for (int i = 0; i < sceneObjects.Count; i++)
            {
                sceneObjects[i].LoadObjectData(scenObjectsData[i]);
            }
        }

        private bool FileExist() => File.Exists(_filePath);

    }
}
