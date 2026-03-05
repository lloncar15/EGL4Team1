using UnityEngine;

[CreateAssetMenu(fileName = "BloodType", menuName = "Game/Blood Type")]
public class BloodType : ScriptableObject {
    public string bloodName;
    public bool isGroupie;
    public DungeonBuff buff;
    public Color color;
}