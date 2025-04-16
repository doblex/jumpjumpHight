using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public delegate void OnCrystalObtained(List<Crystal> crystalsObtained);

    public OnCrystalObtained onCrystalObtained;

    List<Crystal> crystals = new List<Crystal>();

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
    }

    private void CountCrystals()
    {
        crystals.AddRange(FindObjectsByType<Crystal>(FindObjectsSortMode.None));
    }

    public void CrystalCollected() 
    {
        onCrystalObtained?.Invoke(crystals);

        //ConditionCheck
    }

}
