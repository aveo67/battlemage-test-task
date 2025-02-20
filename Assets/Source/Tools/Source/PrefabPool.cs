using System.Collections.Generic;
using UnityEngine;
using Zenject;

internal class PrefabPoolDiTag { }

public static class PrefabPoolDiExtensions
{
	public static DiContainer UsePrefabPools(this DiContainer container)
	{
		if (!container.HasBinding<PrefabPoolDiTag>())
		{
			container.Bind<PrefabPoolDiTag>().AsSingle().NonLazy();

			container.Bind(typeof(PrefabPool<>)).AsSingle();
		}

		return container;
	}
}

public class PrefabPool<T>
	where T : MonoBehaviour
{
	private readonly Dictionary<GameObject, Stack<T>> _pools = new Dictionary<GameObject, Stack<T>>();

	private readonly Dictionary<T, GameObject> _associations = new Dictionary<T, GameObject>();

	private readonly GameObject _root;

	private readonly DiContainer _container;

	public PrefabPool(DiContainer container)
	{
		_container = container;
		_root = new GameObject($"{typeof(T).Name} Pool Root");
	}

	public T Get(T prefab)
	{
		if (!_pools.TryGetValue(prefab.gameObject, out var pool))
		{
			pool = new Stack<T>();

			_pools.Add(prefab.gameObject, pool);
		}

		if (pool.Count > 0)
			return pool.Pop();

		var instance = _container.InstantiatePrefabForComponent<T>(prefab);
		instance.gameObject.SetActive(false);

		_associations.Add(instance, prefab.gameObject);

		return instance;
	}

	public void Push(T instance)
	{
		if (instance != null && _associations.TryGetValue(instance, out var prefab) && _pools.TryGetValue(prefab, out var pool))
		{
			instance.transform.SetParent(_root.transform);
			instance.transform.position = Vector3.zero;
			instance.gameObject.SetActive(false);

			pool.Push(instance);
		}

		else
		{
			GameObject.Destroy(instance.gameObject);
		}
	}

	private void OnDestroy()
	{
        foreach (var pool in _pools.Values)
        {
			pool.Clear();
		}
	}
}
