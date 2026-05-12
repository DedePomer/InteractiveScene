using Core.Scene;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Core.Ui
{
    public class SceneObjectListItemView : MonoBehaviour
    {
        [Header("Controls")]
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private Button colorButtonR;
        [SerializeField] private Button colorButtonG;
        [SerializeField] private Button colorButtonB;
        [SerializeField] private Toggle selectToggle;
        [SerializeField] private Button chooseButton;

        public static event Func<SceneObjectController, Transform> OnObjectSelectedEvent;
        public static void SelectObject(SceneObjectController obj)
        {
            OnObjectSelectedEvent?.Invoke(obj);
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                selectToggle.SetIsOnWithoutNotify(value);
            }
        }
        private bool _isSelected;

        private readonly Color _colorR = Color.red;
        private readonly Color _colorG = Color.green;
        private readonly Color _colorB = Color.blue;

        private SceneObjectController _sceneObjectController;

        public void Init(SceneObjectController obj)
        {
            _sceneObjectController = obj;
            nameText.text = obj.objectName;


            chooseButton.onClick.AddListener(() => OnChoosed());
            colorButtonR.onClick.AddListener(() => OnColorChanged(_colorR));
            colorButtonG.onClick.AddListener(() => OnColorChanged(_colorG));
            colorButtonB.onClick.AddListener(() => OnColorChanged(_colorB));
            selectToggle.onValueChanged.AddListener(v => IsSelected = v);

            IsSelected = false;

            colorButtonR.image.color = _colorR;
            colorButtonG.image.color = _colorG;
            colorButtonB.image.color = _colorB;
        }

        public SceneObjectController GetObject()
        {
            return _sceneObjectController;
        }


        private void OnChoosed()
        {
            SelectObject(_sceneObjectController);
        }

        private void OnColorChanged(Color color)
        {
            _sceneObjectController.SetColor(color);
        }

    }
}
