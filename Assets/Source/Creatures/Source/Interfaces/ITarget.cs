using UnityEngine;

namespace Battlemage.Creatures
{
	public interface ITarget
	{
		Transform Transform { get; }

		bool IsDead { get; }

		void Hit(Demage demage);
	}
}
