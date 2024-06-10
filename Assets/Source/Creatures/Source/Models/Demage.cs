namespace Battlemage.Creatures
{
	public readonly struct Demage
	{
		public readonly float Value;

		public readonly float ResistanceIgnoring;

		public Demage(float value, float resistanceIgnoring)
		{
			Value = value;
			ResistanceIgnoring = resistanceIgnoring;
		}
	}
}
