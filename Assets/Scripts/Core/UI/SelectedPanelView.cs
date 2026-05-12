using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Ui
{
    public class SelectedPanelView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Core.Scene.SceneObjectRegistry sceneObjectRegistry;
        [SerializeField] private Transform content;
        [SerializeField] private SceneObjectListItemView objectListItemPrefab;

        [Header("Controls")]
        [SerializeField] private Toggle selectAllToggle;
        [SerializeField] private Toggle visibleToggle;
        [SerializeField] private Slider opacitySlider;

        private readonly List<SceneObjectListItemView> _items = new();

        private void Start()
        {
            foreach (var sceneObject in sceneObjectRegistry.GetAll())
            {
                var item = Instantiate(objectListItemPrefab, content);
                item.Init(sceneObject);
                _items.Add(item);
            }
            Init(_items);
        }

        public void Init(List<SceneObjectListItemView> items)
        {
            visibleToggle.onValueChanged.AddListener(v => OnVisibleChanged(v));
            opacitySlider.onValueChanged.AddListener(v => OnOpacityChanged(v));
            selectAllToggle.onValueChanged.AddListener(v => OnAllSelectChanged(v));
        }

        private List<SceneObjectListItemView> GetSelectedItems()
        {
            List<SceneObjectListItemView> selectedItems = _items.Where(x => x.IsSelected == true).ToList();
            return selectedItems;
        }
        private void OnAllSelectChanged(bool value)
        {
            foreach (var obj in _items)
                obj.IsSelected = value;
        }

        private void OnVisibleChanged(bool value)
        {
            foreach (var obj in GetSelectedItems())
                obj.GetObject().IsVisible = value;
        }

        private void OnOpacityChanged(float value)
        {
            foreach (var obj in GetSelectedItems())
                obj.GetObject().SetAlpha(value);
        }
    }
}


