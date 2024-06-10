using UnityEngine;

[CreateAssetMenu(fileName = "Creature", menuName = "Creature/Creature", order = 1)]
public class CreatureDescriptor : ScriptableObject
{
	[SerializeField, Range(10f, 1000f)]
	private float _baseHealth;

	[SerializeField, Range(0f, 0.85f)]
	private float _baseResistance;

	[SerializeField, Range(0.1f, 10f)]
	private float _baseMovementSpeed;

	public float BaseHealth => _baseHealth;

	public float BaseResistance => _baseResistance;

	public float BaseMovementSpeed => _baseMovementSpeed;
}
