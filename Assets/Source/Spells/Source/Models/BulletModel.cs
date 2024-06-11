namespace Battlemage.Spells
{
	public struct BulletModel
	{
		public int Number { get; set; }

		public BulletHandle Prefab { get; set; }

		public float LifeTime { get; set; }

		public bool StopWhenCollided { get; set; }
	}
}
