using Battlemage.MainCharacter;
using EasyInputHandling;
using UnityEngine;
using Zenject;

namespace Battlemage.Domain
{
	public class BattleSceneRoot : MonoBehaviour, IInitializable
	{
		private IInput _input;

		[Inject]
		private void Construct(LichHandler lichHandler, IInputFactory<LichHandler> inputFactory)
		{
			_input = inputFactory.Create(lichHandler);
		}

		public void Initialize()
		{
			_input.Enable();
		}

		private void OnDestroy()
		{
			_input.Dispose();
		}
	}
}
