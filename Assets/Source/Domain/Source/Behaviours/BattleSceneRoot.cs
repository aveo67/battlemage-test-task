using Battlemage.MainCharacter;
using EasyInputHandling;
using System;
using UnityEngine;
using Zenject;

namespace Battlemage.Domain
{
	public class BattleSceneRoot : MonoBehaviour, IInitializable
	{
		private LichHandler _mainCharacter;
		private IInput _input;

		[Inject]
		private void Construct(LichHandler lichHandler, IInputFactory<LichHandler> inputFactory)
		{
			_mainCharacter = lichHandler;
			_input = inputFactory.Create(lichHandler);
			lichHandler.Dead += OnMainCharacterDead;
		}

		private void OnMainCharacterDead()
		{
			Debug.Log("You Dead!");
		}

		public void Initialize()
		{
			_input.Enable();
		}

		private void OnDestroy()
		{
			_input.Dispose();
			_mainCharacter.Dead -= OnMainCharacterDead;
		}
	}
}
