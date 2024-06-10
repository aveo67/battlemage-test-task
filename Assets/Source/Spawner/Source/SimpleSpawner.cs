using UnityEngine;
using Zenject;

namespace Battlemage.Spawner
{
	public class SimpleSpawner : MonoBehaviour, ISpawner
	{
		[SerializeField]
		private GameObject _prefab;
		private DiContainer _container;

		[Inject]
		private void Construct(DiContainer container)
		{
			_container = container;
		}

		public void Spawn()
		{
			var instance = _container.InstantiatePrefab(_prefab);

			instance.transform.position = transform.position;
		}
	}
}
