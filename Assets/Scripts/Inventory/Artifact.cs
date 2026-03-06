using UnityEngine;

[CreateAssetMenu(fileName = "Artifact", menuName = "Game/Artifact")]
public class Artifact : Item {
    [Header("Visitor Stats"), Tooltip("Stats that are affecting the visitors and their behaviour.")]
    public int appeal;
    public int moneyModifier;
    public int someStat2;
}