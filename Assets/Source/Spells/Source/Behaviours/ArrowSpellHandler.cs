using Battlemage.Creatures;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Battlemage.Spells
{
	[RequireComponent(typeof(DecalProjector))]
	public class ArrowSpellHandler : SpellHandler
	{
		private Plane _plane = new Plane(Vector3.up, 0f);

		[SerializeField, Range(1, 10f)]
		private float _aimDistance;

		private DecalProjector _projector;



		protected override void Awake()
		{
			base.Awake();

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

				await Awaitable.NextFrameAsync(destroyCancellationToken);
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

				pos = _parent.position + (pos - _parent.position).normalized * _aimDistance;

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

				bullet.transform.position = _parent.transform.position + Vector3.up * 2f;
				bullet.transform.rotation = Quaternion.LookRotation(aimPoint, Vector3.up);

				bullet.gameObject.SetActive(true);
				bullet.Release();
			}
		}

		public override string ToString()
		{
			return base.ToString() + $", Aim Distance: {_aimDistance}";
		}
	}
}
