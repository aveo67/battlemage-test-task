using System;
using System.Linq;
using UnityEngine;

namespace Battlemage.Spawner
{
	public interface ISpawner
	{
		void Spawn();
	}

	public class SpawnSystem : MonoBehaviour
	{
		[SerializeField]
		private Transform[] _points;

		[SerializeField, Range(1, 100)]
		private int _radius;

		private int _sqrRadius;

		private void Awake()
		{
			_sqrRadius = _radius * _radius;

			Spawn(Vector3.zero);
		}

		public void Spawn(Vector3 target)
		{
			var list = _points
				.Where(n => (target - n.position).sqrMagnitude >= _sqrRadius)
				.ToArray();

			if (list.Length == 0)
				return;

			var randomIndex = UnityEngine.Random.Range(0, list.Length);

			var spawner = list[randomIndex].transform.GetComponent<ISpawner>();

			spawner.Spawn();
		}


		private void OnDrawGizmos()
		{
			var color = Color.yellow;
			color.a = 0.3f;

			Gizmos.color = color;
			Gizmos.DrawSphere(transform.position, _radius);

			Gizmos.color = Color.white;
		}
	}
}
