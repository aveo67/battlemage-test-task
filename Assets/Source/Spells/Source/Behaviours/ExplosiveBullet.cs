using UnityEngine;

namespace Battlemage.Spells
{
	public class ExplosiveBullet : MonoBehaviour
	{
		[SerializeField, Range(0f, 10f)]
		private float _startSize;

		[SerializeField, Range(1f, 11f)]
		private float _maxSize;

		[SerializeField, Range(1f, 5f)]
		private float _speed;

		private async void OnEnable()
		{
			float currentSize = _startSize * 2f;

			while (currentSize <= _maxSize * 2f)
			{
				currentSize += _speed * Time.deltaTime;

				SetScale(currentSize);

				await Awaitable.NextFrameAsync();
			}
		}

		private void SetScale(float size)
		{
			transform.localScale = new Vector3(size, size, size);
		}

		public override string ToString()
		{
			return $"Explosive Bullet. Start Size: {_startSize}, Max Size: {_maxSize}, Current Size: {transform.localScale}, Speed: {_speed}";
		}
	}
}
