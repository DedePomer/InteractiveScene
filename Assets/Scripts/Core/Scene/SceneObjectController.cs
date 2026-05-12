using UnityEngine;
using Core.Model;

namespace Core.Scene
{
    public class SceneObjectController : MonoBehaviour
    {
        [Header("Object Info")]
        [SerializeField] private string objectName = "SceneObject";

        
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                ApplyMaterial();
            }
        }

        private bool _isVisible = true;
        private Renderer? _renderer;
        private MaterialPropertyBlock _mpb;
        private Color _currentColor = Color.white;
        private float _currentAlpha = 1f;


        void Awake()
        {
            _renderer = GetComponent<Renderer>();
            if (_renderer == null)
            {
                Debug.LogError("renderer is null", this);
                return;
            }
            _mpb = new MaterialPropertyBlock();
            _currentColor = _renderer.sharedMaterial.color;

        }

        public void SetColor(Color color)
        {
            _currentColor = color;
            ApplyMaterial();
        }

        public void SetAlpha(float alpha)
        {
            _currentAlpha = Mathf.Clamp01(alpha);
            ApplyMaterial();
        }

        public SceneObjectData GetObjectData()
        {
            return new SceneObjectData
            {
                name = objectName,
                colorHex = ColorUtility.ToHtmlStringRGBA(_currentColor),
                alpha = _currentAlpha,
                isVisible = IsVisible,
                position = transform.position,
                rotation = transform.eulerAngles,
                scale = transform.localScale
            };
        }

        public void LoadObjectData(SceneObjectData data)
        {
            objectName = data.name;
            transform.position = data.position;
            transform.eulerAngles = data.rotation;
            transform.localScale = data.scale;

            Color c;
            if (ColorUtility.TryParseHtmlString("#" + data.colorHex, out c))
                SetColor(c);

            SetAlpha(data.alpha);
            IsVisible = data.isVisible;
        }

        private void ApplyMaterial()
        {
            gameObject.SetActive(IsVisible);

            _renderer.GetPropertyBlock(_mpb);
            _mpb.SetColor("_Color", new Color(_currentColor.r, _currentColor.g, _currentColor.b, _currentAlpha));
            _renderer.SetPropertyBlock(_mpb);
        }
    }
}
