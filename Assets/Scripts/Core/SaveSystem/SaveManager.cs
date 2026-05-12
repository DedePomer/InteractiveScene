using Core.Model;
using Core.Scene;
using Newtonsoft.Json;
using SFB;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Core.SaveSystemm
{

    [DisallowMultipleComponent, RequireComponent(typeof(SaveManagerInput))]
    public class SaveManager : MonoBehaviour
    {
        private const string FileExtension = "json";
        private const string DefaultFileName = "save";
        private const string DefaultLoadTitle = "Load file";
        private const string DefaultSaveTitle = "Save file";


        [SerializeField] private SceneObjectRegistry sceneObjectRegistry;

        private SaveManagerInput _saveManagerInput;
        private string _filePath;

        private void Awake()
        {
            _saveManagerInput = GetComponent<SaveManagerInput>();
        }

        private void Update()
        {
            Save();
            Load();
        }


        public void Save()
        {
            if (!_saveManagerInput.IsSave)
                return;
            if (!IsSaveDialogClose())
                return;


            var data = GetDataFromSceneObject(sceneObjectRegistry.GetAll());
            string json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(_filePath, json);
            Debug.Log("Saved");
        }

        public void Load()
        {
            if (!_saveManagerInput.IsLoad)
                return;
            if (!IsLoadDialogClose())
                return;
            if (!FileHasExist())
            {
                Debug.LogError("file dont exist", this);
                return;
            }

            string json = File.ReadAllText(_filePath);
            var data = JsonConvert.DeserializeObject<List<SceneObjectData>>(json);
            SetDataFromSceneObject(data);
            Debug.Log("Loaded");
        }

        public bool IsLoadDialogClose()
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

        public bool IsSaveDialogClose()
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

        private bool FileHasExist() => File.Exists(_filePath);

    }
}
