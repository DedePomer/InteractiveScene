using Core.Model;
using Core.Scene;
using System.Collections.Generic;
using UnityEngine;

namespace Core.SaveSystemm
{

    [DisallowMultipleComponent, RequireComponent(typeof(SaveManagerInput))]
    public class SaveManager : MonoBehaviour
    {
        // жалко что интерфейсы нельзя серилизовать
        [SerializeField] private JsonSaveRepository saveRepository;
        [SerializeField] private SceneObjectRegistry sceneObjectRegistry;

        private SaveManagerInput _saveManagerInput;

        private void Awake()
        {
            _saveManagerInput = GetComponent<SaveManagerInput>();
        }

        private void Update()
        {

        }

        public void OpenFolderAndSave()
        {
            var dialog = new FolderBrowserDialog
            {
                Description = "Выберите папку для сохранения",
                SelectedPath = Application.persistentDataPath,
                ShowNewFolderButton = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _repository = new JsonSaveRepository(dialog.SelectedPath);
                Save();
            }
        }



        public void Save()
        {
            if (!_saveManagerInput.IsSave)
                return;
            var data = GetDataFromSceneObject(sceneObjectRegistry.GetAll());
            saveRepository.Save(data);
        }

        public void Load()
        {
            if (!_saveManagerInput.IsLoad)
                return;
            if (!saveRepository.HasExist())
            {
                Debug.LogError("file dont exist", this);
                return;
            }

            var data = saveRepository.Load();
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
