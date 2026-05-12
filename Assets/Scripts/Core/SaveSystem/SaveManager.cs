using Core.Model;
using Core.Scene;
using SFB;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Core.SaveSystemm
{

    [DisallowMultipleComponent, RequireComponent(typeof(SaveManagerInput), typeof(JsonSaveRepository))]
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private SceneObjectRegistry sceneObjectRegistry;

        private JsonSaveRepository _saveRepository;
        private SaveManagerInput _saveManagerInput;

        private void Awake()
        {
            _saveManagerInput = GetComponent<SaveManagerInput>();
            _saveRepository = GetComponent<JsonSaveRepository>();
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
            _saveRepository.Save(data);
        }

        public void Load()
        {
            if (!_saveManagerInput.IsLoad)
                return;
            if(!IsLoadDialogClose())
                return;
            if (!_saveRepository.HasExist())
            {
                Debug.LogError("file dont exist", this);
                return;
            }

            var data = _saveRepository.Load();
            SetDataFromSceneObject(data);
            Debug.Log("Loaded");
        }

        public bool IsLoadDialogClose()
        {
            string[] paths = StandaloneFileBrowser.OpenFilePanel(
                title: "Load file",
                directory: Application.persistentDataPath,
                extension: "json",
                multiselect: false
            );

            if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
            {
                _saveRepository.FilePath = paths[0];
                return true;
            }
            return false;
        }

        public bool IsSaveDialogClose()
        {
            string path = StandaloneFileBrowser.SaveFilePanel(
                title: "Save file",
                directory: Application.persistentDataPath,
                defaultName: "save",
                extension: "json"
            );

            if (!string.IsNullOrEmpty(path))
            {
                _saveRepository.FilePath = path;
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

    }
}
