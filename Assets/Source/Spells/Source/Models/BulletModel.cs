namespace Battlemage.Spells
{
	public struct BulletModel
	{
		public int Number { get; set; }

		public BulletHandle Prefab { get; set; }

		public float LifeTime { get; set; }

		public bool StopWhenCollided { get; set; }

		public override string ToString()
		{
			return $"Bullet Model. Number: {Number}, Prefab: {Prefab.name}, Life Time: {LifeTime}, Stop When Collided: {StopWhenCollided}";
		}
	}
}
