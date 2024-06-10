using System;
using UnityEngine;

namespace Battlemage.Spawner
{
	public class SpawnHandle : MonoBehaviour
	{
		private ISpawner _spawner;

		private void Awake()
		{
			_spawner = GetComponent<ISpawner>();

			if (_spawner == null)
			{
				var message = "ISpawner component is not attached";

				Debug.LogError(message, this);

				throw new NullReferenceException(message);
			}
		}

		public void Spawn()
		{
			_spawner.Spawn();
		}
	}
}
