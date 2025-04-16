using UnityEngine;

public class CureParticle : MonoBehaviour
{
    [SerializeField] float lifeTime = 1f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
