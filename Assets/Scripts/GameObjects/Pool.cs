using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    public T Prefab { get; }
    public Transform Container { get; }
    private List<T> _pool;

    public Pool(T prefab, int size)
    {
        Prefab = prefab;
        CreatePool(size);
    }

    public Pool(T prefab, int size, Transform container)
    {
        Prefab = prefab;
        Container = container;
        CreatePool(size);
    }

    private void CreatePool(int size)
    {
        _pool = new List<T>();
        for (int i = 0; i < size; i++) CreateActor();
    }

    private T CreateActor(bool isActive = false)
    {
        var actor = Object.Instantiate(Prefab, Container);
        actor.gameObject.SetActive(isActive);
        _pool.Add(actor);
        return actor;
    }

    public bool HasFreeActor(out T actor, bool isActive = true)
    {
        foreach (var element in _pool)
        {
            if (!element.gameObject.activeInHierarchy)
            {
                actor = element;
                element.gameObject.SetActive(isActive);
                return true;
            }
        }

        actor = null;
        return false;
    }

    public T GetFreeActor(bool isForced = false, bool isActive = false)
    {
        if (HasFreeActor(out var actor, isActive)) return actor;
        if (isForced) return CreateActor(isActive);
        return null;
    }

    public void ReturnAllActorsToPool()
    {
        foreach (var actor in _pool)
        {
            actor.gameObject.SetActive(false);
        }
    }
}
