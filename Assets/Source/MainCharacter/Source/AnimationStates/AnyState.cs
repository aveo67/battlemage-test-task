using UnityEngine;

namespace Battlemage.MainCharacter
{
	internal class AnyState : StateMachineBehaviour
	{
		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			var character = animator.gameObject.GetComponent<LichAnimationHandler>();

			if (character != null)
				character.EndCurrentState();
		}
	}
}
