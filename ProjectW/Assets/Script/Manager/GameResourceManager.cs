using Script.Manager;
using UnityEngine;

public class ResourceImporter : MonoBehaviour
{
    public static Sprite GetImage(string imageName)
    {
        var image = Resources.Load<Sprite>($"Sprites/{imageName}");
        return image;
    }

    public static GameObject GetLoadUIPrefab(string prefabName)
    {
        GameObject prefab = Resources.Load($"Prefabs/{prefabName}") as GameObject;
        if (prefab == null)
            return null;
        return Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
    }

    public static GameObject GetLoadActorPrefab(string prefabName)
    {
        GameObject prefab = Resources.Load($"Prefabs/Actor/{prefabName}") as GameObject;

        if (prefab == null)
            return null;
        return Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
    }

    public static T GetLoadScriptableObject<T>(string fileName) where T : ScriptableObject
    {
        var loadScriptableObject = Resources.Load<T>($"ScriptableOjbects/{fileName}");
        return loadScriptableObject;
    }
}
