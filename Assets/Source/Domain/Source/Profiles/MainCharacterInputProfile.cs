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
				.UseHandling<CommonInputActions, LichHandler>(b => b.
					HandleInputContinuousAction(i => i.Keyboard.Movement, cc => cc.ReadValue<Vector2>(), (c, p) => c.Push(p), c => c.Brake()));

		}
	}
}
