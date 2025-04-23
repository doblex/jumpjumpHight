using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    [Header("Healt")]
    public int maxHealth;
    public float immunityFrames;

    [Header("Attack")]
    public int damage;
    public float attackDuration;

    [Header("Movement")]
    public float speed;
    public float maxSpeed;
    public float jumpForce;

}
