using Battlemage.MainCharacter;
using EasyInputHandling;
using System;
using UnityEngine;

namespace Battlemage.Domain
{
	public class MainCharacterInputProfile : InputProfile<LichHandler>
	{
		public MainCharacterInputProfile(Action<IInputFactoryBuilder<LichHandler>>[] builderExpressions)
			: base(builderExpressions)
		{
		}

		public override void Configure()
		{
			_builder
				.UseHandling<CommonInputActions, LichHandler>(b => b
					.HandleInputContinuousAction(i => i.Keyboard.Movement, cc => cc.ReadValue<Vector2>(), (c, p) => c.Push(p), c => c.Brake())
					.HandleInputActionPerformed(i => i.Keyboard.Number1, c => c.SetSpell(1))
					.HandleInputActionPerformed(i => i.Keyboard.Number2, c => c.SetSpell(2))
					.HandleInputActionPerformed(i => i.Keyboard.Number3, c => c.SetSpell(3))
					)
				.UseHandling<MouseActions, LichHandler>(b => b
					.HandleInputActionPerformed(i => i.MouseActionsMap.LeftButtomClick, c => c.OpenFire()));

		}
	}
}
