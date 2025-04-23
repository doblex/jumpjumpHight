using UnityEngine;

public class Fairy : MonoBehaviour
{
    public delegate void OnCureEnergyChange(float current, float max);
    public event OnCureEnergyChange onCureEnergyChange;

    [Header("cure")]
    [SerializeField] GameObject fairyParticle;
    [SerializeField] float cureTimer = 10f;
    [SerializeField] float cureAmount = 1f;

    [Header("followUp")]
    [SerializeField] Transform pointToFollow;
    [SerializeField] float speed = 2f;

    float timer = 0f;
    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        timer = cureTimer;
    }

    private void Update()
    {
        if (player != null)
        {
            Vector3 direction = pointToFollow.position - transform.position;
            float distance = direction.magnitude;
            if (distance > 0.5f)
            {
                transform.position += speed * Time.deltaTime * direction.normalized;
            }

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, direction.x > 0 ? 0 :  -180, transform.rotation.eulerAngles.z);
            
            
        }

        if (timer <= 0)
        {
            timer = cureTimer;
            if (player != null)
            {
                Character character = player.GetComponent<Character>();
                if (character != null)
                {
                    character.Heal((int)cureAmount);
                    Instantiate(fairyParticle, transform.position, transform.rotation);
                }
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }

        onCureEnergyChange?.Invoke(timer, cureTimer);
    }
}
