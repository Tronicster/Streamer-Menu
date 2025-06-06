using UnityEngine;
using System.Collections;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("CoroutineRunner");
                Object.DontDestroyOnLoad(obj);
                _instance = obj.AddComponent<CoroutineRunner>();
            }
            return _instance;
        }
    }

    public static Coroutine Run(IEnumerator routine)
    {
        return Instance.StartCoroutine(routine);
    }

    public static void Stop(Coroutine routine)
    {
        if (routine != null && _instance != null)
        {
            _instance.StopCoroutine(routine);
        }
    }
}
