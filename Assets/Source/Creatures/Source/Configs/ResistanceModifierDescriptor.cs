using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "ResistanceModifier", menuName = "Modifiers/ResistanceModifier", order = 1)]
public class ResistanceModifierDescriptor : ScriptableObject
{
	[SerializeField, Range(-0.85f, 0.85f)]
	private float _value;

	public float Value => _value;
}
