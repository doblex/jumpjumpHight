using UnityEngine;

public class Crystal : MonoBehaviour
{
    int crystalIndex;
    bool isCollected = false;

    public bool IsCollected { get => isCollected; }

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
