using Core.Model;
using Core.Scene;
using System.Collections.Generic;
using UnityEngine;
using SFB;

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
            if (!IsFolderDialogClose())
                return;

            Debug.Log("Save");
            var data = GetDataFromSceneObject(sceneObjectRegistry.GetAll());
            _saveRepository.Save(data);
        }

        public void Load()
        {
            if (!_saveManagerInput.IsLoad)
                return;
            if(!IsFolderDialogClose())
                return;
            if (!_saveRepository.HasExist())
            {
                Debug.LogError("file dont exist", this);
                return;
            }

            Debug.Log("Load");
            var data = _saveRepository.Load();
        }

        public bool IsFolderDialogClose()
        {
            string[] paths = StandaloneFileBrowser.OpenFolderPanel(
                title: "Choose directory",
                directory: Application.persistentDataPath,
                multiselect: false
            );

            if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
            {
                _saveRepository.FilePath = paths[0];
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

    }
}
