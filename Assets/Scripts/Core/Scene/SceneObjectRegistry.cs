using System.Collections.Generic;
using UnityEngine;

namespace Core.Scene
{ 
    [System.Serializable]
    public class SceneObjectRegistry : MonoBehaviour
    {
        [SerializeField] private List<SceneObjectController> objects;
        public IReadOnlyList<SceneObjectController> GetAll() => objects;

    }
}
