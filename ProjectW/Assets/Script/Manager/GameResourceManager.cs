
using UnityEngine;

namespace Script.Manager
{
    public class GameResourceManager : Singleton<GameResourceManager>
    {
        public Sprite GetImage(string imageName)
        {
            var image = Resources.Load<Sprite>($"Sprites/{imageName}");
            return image;
        }

        public GameObject GetLoadUIPrefab(string prefabName)
        {
            GameObject prefab = Resources.Load($"Prefabs/{prefabName}") as GameObject;
            if (prefab == null)
                return null;
            return Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
        }
        
        public GameObject GetLoadActorPrefab(string prefabName)
        {
            GameObject prefab = Resources.Load($"Prefabs/Actor/{prefabName}") as GameObject;

            if (prefab == null)
                return null;
            return Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
        }
        
        public T GetLoadScriptableObject<T>(string fileName) where T : ScriptableObject
        {
            var loadScriptableObject = Resources.Load<T>($"ScriptableOjbects/{fileName}");
            return loadScriptableObject;
        }
    }
}
