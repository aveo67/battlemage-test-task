namespace Battlemage.Creatures
{
	public readonly struct Damage
	{
		public readonly float Value;

		public readonly float ResistanceIgnoring;

		public Damage(float value, float resistanceIgnoring)
		{
			Value = value;
			ResistanceIgnoring = resistanceIgnoring;
		}

		public Damage Combine(Damage other)
		{
			return new Damage(Value + other.Value, ResistanceIgnoring + other.ResistanceIgnoring);
		}

		public Damage Combine(params Damage[] other) 
		{
			var damage = 0f;
			var ri = 0f;

			for (var i = 0; i < other.Length; i++)
			{
				damage += other[i].Value;
				ri += other[i].ResistanceIgnoring;
			}

			return new Damage(Value + damage, ResistanceIgnoring + ri);
		}

		public override string ToString()
		{
			return $"Damage: {Value}, Ignor: {ResistanceIgnoring}";
		}
	}
}
