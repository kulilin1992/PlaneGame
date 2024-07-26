using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public GameObject Prefab { get => prefab; }
    [SerializeField] GameObject prefab;

    [SerializeField] int size = 1;
    Queue<GameObject> queue;

    Transform parent;

    public int Size => size;
    public int RuntimeSize => queue.Count;

    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.parent = parent;
        for (int i = 0; i < size; i++)
        {
            queue.Enqueue(Copy());
        }
    }
    GameObject Copy()
    {
        var obj = GameObject.Instantiate(prefab, parent);
        obj.SetActive(false);
        return obj;
    }
    GameObject GetObject()
    {
        GameObject obj = null;
        if (queue.Count > 0 && !queue.Peek().activeSelf)
        {
            obj = queue.Dequeue();

        }
        else
        {
            obj = Copy();
        }
        queue.Enqueue(obj);
        return obj;
    }
    public GameObject PreparedObject()
    {
        GameObject obj = GetObject();
        obj.SetActive(true);
        return obj;
    }

    public GameObject PreparedObject(Vector3 position)
    {
        GameObject obj = GetObject();
        obj.SetActive(true);
        obj.transform.position = position;
        return obj;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion rotation)
    {
        GameObject obj = GetObject();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    public GameObject PreparedObject(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        GameObject obj = GetObject();
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.transform.localScale = scale;
        return obj;
    }
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        queue.Enqueue(obj);
    }
}
