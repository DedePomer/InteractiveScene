using UnityEngine;


namespace Core.Model
{
    [System.Serializable]
    public class SceneObjectData
    {
        public string name;
        public string colorHex;
        public float alpha;
        public bool isVisible;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }
}