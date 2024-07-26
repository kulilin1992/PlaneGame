using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] playerBulletPools;

    static Dictionary<GameObject, Pool> dictionary;

    // Start is called before the first frame update
    void Start()
    {
        dictionary = new Dictionary<GameObject, Pool>();
        Initialize(playerBulletPools);
    }


    #if UNITY_EDITOR
    void OnDisable()
    {
        Debug.Log("aaa");
        CheckPoolSize(playerBulletPools);
    }
    #endif

    void CheckPoolSize(Pool[] pools)
    {
        foreach (Pool pool in pools)
        {
            if (pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning(
                    string.Format("PoolManager: Pool for {0} has a runtime size of {1} which is greater than the size of {2}",
                    pool.Prefab.name,
                    pool.RuntimeSize,
                    pool.Size));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Initialize(Pool[] pools)
    {
        foreach (Pool pool in pools)
        {
            #if UNITY_EDITOR
            if (dictionary.ContainsKey(pool.Prefab))
            {
                Debug.LogError("PoolManager: Pool for " + pool.Prefab.name + " already exists");
                continue;
            }
            #endif

            dictionary.Add(pool.Prefab, pool);
            
            Transform poolParent = new GameObject("Pool:" + pool.Prefab.name).transform;
            poolParent.parent = transform;
            pool.Initialize(poolParent);
        }
    }

    public static GameObject Release(GameObject prefab)
    {
        #if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("PoolManager: Pool for " + prefab.name + " does not exist");
            return null;
        }
        #endif
        return dictionary[prefab].PreparedObject();
    }

    public static GameObject Release(GameObject prefab, Vector3 position)
    {
        #if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("PoolManager: Pool for " + prefab.name + " does not exist");
            return null;
        }
        #endif
        return dictionary[prefab].PreparedObject(position);
    }

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        #if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("PoolManager: Pool for " + prefab.name + " does not exist");
            return null;
        }
        #endif
        return dictionary[prefab].PreparedObject(position, rotation);
    }

    public static GameObject Release(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        #if UNITY_EDITOR
        if (!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("PoolManager: Pool for " + prefab.name + " does not exist");
            return null;
        }
        #endif
        return dictionary[prefab].PreparedObject(position, rotation, localScale);
    }
}
