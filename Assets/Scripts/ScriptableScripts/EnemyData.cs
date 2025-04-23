using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]

public class EnemyData : ScriptableObject
{
    [Header("Movement")]
    public float updatePosition;
    public float idleDistance;
}
