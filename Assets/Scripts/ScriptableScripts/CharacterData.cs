using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public int damage;
    public int maxHealth;

    public float speed;
    public float maxSpeed;
    public float jumpForce;

    public float attackDuration;
}
