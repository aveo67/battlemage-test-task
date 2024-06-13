using Battlemage.Creatures;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Battlemage.Spells
{
	[RequireComponent(typeof(DecalProjector))]
	public class ExplosionSellHandler : SpellHandler
	{
		private Plane _plane = new Plane(Vector3.up, 0f);

		[SerializeField, Range(1, 10f)]
		private float _aimRadius;

		private DecalProjector _projector;



		protected override void Awake()
		{
			base.Awake();

			var size = _aimRadius * 2f;
			transform.localScale = new Vector3(size, size, size);

			_projector = GetComponent<DecalProjector>();
			_projector.enabled = false;
		}

		protected override void TakeAim()
		{
			_projector.enabled = true;
		}

		protected override void Stop()
		{
			_projector.enabled = false;
		}

		public override void Release(Ray _, Damage damage, int bulletNumber)
		{
			var bullet = GetBullet(damage);

			bullet.transform.position = _parent.transform.position;

			bullet.gameObject.SetActive(true);
			bullet.Release();
		}

		public override string ToString()
		{
			return base.ToString() + $", Aim Radius: {_aimRadius}";
		}
	}
}
