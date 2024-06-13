﻿using System;
using UnityEngine;

namespace Battlemage.MainCharacter
{
	internal class AccelerationState : MotionStateBase
	{
		public AccelerationState(LichHandler context) : base(context)
		{
		}

		public override void Brake()
		{
			if (_terminated)
				return;

			_terminated = true;

			_context.SetNextState(new DeccelerationState(_context));
		}

		public override async void Process()
		{
			while (!_context.AchivedMaxSpeed)
			{
				if (_terminated)
					return;

				_context.Move();

				try
				{
					await Awaitable.NextFrameAsync(_context.destroyCancellationToken);
				}

				catch (OperationCanceledException)
				{
					Debug.Log("Main Character Game object was destroyed and moution has been terminated");

					return;
				}
			}

			if (!_terminated)
				_context.SetNextState(new MaxSpeedState(_context));
		}

		public override void Push()
		{
			if (!_terminated)
				_context.Accelerate();
		}
	}
}
