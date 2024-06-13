using Battlemage.Creatures;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;

namespace Battlemage.Spells
{
	[RequireComponent(typeof(DecalProjector))]
	public class ZoneSpellHandler : SpellHandler
	{
		private Plane _plane = new Plane(Vector3.up, 0f);

		[SerializeField, Range(1f, 10f)]
		private float _size;

		[SerializeField, Range(1, 50f)]
		private float _maxAimDistance;

		private DecalProjector _projector;

		private float _sqrDistance;



		protected override void Awake()
		{
			base.Awake();

			transform.localScale = new Vector3(_size, _size, _size);
			_sqrDistance = _maxAimDistance * _maxAimDistance;

			_projector = GetComponent<DecalProjector>();
			_projector.enabled = false;
		}

		protected override async void TakeAim()
		{
			_projector.enabled = true;

			while (_projector.enabled)
			{
				var pos = GetAimPoint();

				if (pos == Vector3.positiveInfinity)
					pos = Vector3.zero - Vector3.up * 1000f;

				transform.position = pos;

				try
				{
					await Awaitable.NextFrameAsync(destroyCancellationToken);
				}

				catch (OperationCanceledException)
				{
					Debug.Log("Spell aiming has stoped because game object was destroyed");

					break;
				}
			}
		}

		protected override void Stop()
		{
			_projector.enabled = false;
		}

		private Vector3 GetAimPoint()
		{
			var ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);

			return GetHit(ray);
		}

		private Vector3 GetHit(Ray ray)
		{
			if (_plane.Raycast(ray, out var hit))
			{
				var pos = ray.GetPoint(hit);

				var dir = pos - _parent.position;

				if (dir.sqrMagnitude > _sqrDistance)
				{
					pos = _parent.position + dir.normalized * _maxAimDistance;
				}

				return pos;
			}

			return Vector3.positiveInfinity;
		}

		public override void Release(Ray ray, Damage damage, int bulletNumber)
		{
			var aimPoint = GetHit(ray);

			if (aimPoint == Vector3.positiveInfinity)
				return;

			for (int i = 0; i < bulletNumber; i++)
			{
				var bullet = GetBullet(damage);

				var x = _size * Mathf.Sin(UnityEngine.Random.Range(-1f, 1f)) * 0.5f * UnityEngine.Random.Range(-1f, 1f);
				var y = _size * Mathf.Cos(UnityEngine.Random.Range(-1f, 1f)) * 0.5f * UnityEngine.Random.Range(-1f, 1f);

				var v = new Vector3(x, 20f, y) ;

				var pos = v + aimPoint;
				bullet.transform.position = pos;

				bullet.gameObject.SetActive(true);

				bullet.Release();
			}
		}

		public override string ToString()
		{
			return base.ToString() + $", Size: {_size}, Max Aim Distance: {_maxAimDistance}";
		}
	}
}
