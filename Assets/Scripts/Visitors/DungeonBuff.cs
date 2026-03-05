using UnityEngine;

[CreateAssetMenu(fileName = "DungeonBuff", menuName = "Game/Dungeon Buff")]
public class DungeonBuff : ScriptableObject {
    public float damageBonus;
    public float speedBonus;
    public float luckBonus;
}