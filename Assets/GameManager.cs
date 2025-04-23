using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public delegate void OnCrystalObtained(List<Crystal> crystalsObtained);
    public delegate void OnPause(bool isPausing, int status = 0);

    public OnCrystalObtained onCrystalObtained;
    public OnPause onPause;

    List<Crystal> crystals = new List<Crystal>();
    List<Enemy> enemies = new List<Enemy>();

    bool isPausing = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        CountCrystals();
        CountEnemies();
    }

    private void Update()
    {
        Pause();
    }

    private void CountCrystals()
    {
        crystals.AddRange(FindObjectsByType<Crystal>(FindObjectsSortMode.None));
    }

    private void CountEnemies() 
    {
        enemies.AddRange(FindObjectsByType<Enemy>(FindObjectsSortMode.None));
    }

    public int GetCrystals() 
    {
        int count = 0;

        foreach (Crystal crystal in crystals)
        {
            if (crystal.IsCollected)
                count++;
        }

        return count;
    }

    public int GetMaxCrystals() 
    { 
        return crystals.Count;
    }

    public void CrystalCollected() 
    {
        onCrystalObtained?.Invoke(crystals);

        if (GetCrystals() == GetMaxCrystals())
            WinCondition(true);
    }

    public int GetMaxEnemies() 
    { 
        return enemies.Count;
    }

    public int GetDeadEnemies() 
    {
        int count = 0;

        foreach (Enemy enemy in enemies) 
        {
            if(enemy.isDead()) count++;
        }

        return count;
    }

    public void WinCondition(bool win) 
    {
        onPause?.Invoke(true, win ? 1 : 2);
    }

    private void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPausing = !isPausing;
            onPause?.Invoke(isPausing);
        }
    }
}
