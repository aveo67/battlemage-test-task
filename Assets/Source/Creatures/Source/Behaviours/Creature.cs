using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Battlemage.Creatures
{
	public class Creature : MonoBehaviour
	{
		public event Action Dead;

		[SerializeField]
		private CreatureDescriptor _descriptor;

		[SerializeField]
		private ResistanceModifierDescriptor _resistanceModifier;

		[SerializeField]
		private DamageDescriptor _damageModifier;

		[SerializeField]
		private bool _immortal;

		private float _health;

		private HashSet<Artifact> _artifacts = new HashSet<Artifact>();

		public float Speed => _descriptor.BaseMovementSpeed;

		public float Health => _health;



		private void Awake()
		{
			if (_descriptor == null)
			{
				var message = "Creature config not set to Creature";

				Debug.LogError(message, this);

				throw new NullReferenceException(message);
			}
		}

		private void OnEnable()
		{
			_health = _descriptor.BaseHealth;
		}

		private void OnDisable()
		{
			Dead = null;
		}

		private void OnDestroy()
		{
			Dead = null;
		}

		public void SetArtifact(Artifact artifact)
		{
			_artifacts.Add(artifact);
		}

		public void Hit(Damage damage)
		{
			if (_immortal || _health <= 0f)
				return;

			var totalResistance = Mathf.Clamp(GetResistance() + _artifacts.Sum(n => n.Resistance) + _descriptor.BaseResistance, -0.85f, 0.85f) * (1f - Mathf.Clamp(damage.ResistanceIgnoring, 0f, 1f));

			var totalDamage = Mathf.Clamp(damage.Value * (1f - totalResistance), 0f, float.MaxValue);

			_health -= totalDamage;

			if (_health <= 0f)
				Dead?.Invoke();
		}

		public Damage GetDamage()
		{
			var res = _damageModifier != null ? _damageModifier.Damage : default;

			foreach (var artifact in _artifacts)
			{
				res = res.Combine(artifact.Damage);
			}

			return res;
		}

		public float GetResistance()
		{
			return _resistanceModifier != null ? _resistanceModifier.Value : 0f;
		}

		public override string ToString()
		{
			return $"Creature: {name}, {_descriptor}, CurrenHeals: {_health} {GetDamage()}, Resistance: {GetResistance()}, Artifacts: {_artifacts.Count}";
		}
	}
}
