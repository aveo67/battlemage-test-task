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
	}
}
