using UnityEngine;

public class Crystal : MonoBehaviour
{
    int crystalIndex;
    bool isCollected = false;

    Vector3 startPos;

    public bool IsCollected { get => isCollected; }

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float y = Mathf.Sin(Time.time * 2f) * 0.2f;
        transform.position = new Vector3(startPos.x, startPos.y + y, 0);
    }

    public void SetCrystalIndex(int index) 
    {
        crystalIndex = index;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollected = true;
            GameManager.Instance.CrystalCollected();
            gameObject.SetActive(false);
        }
    }
}
