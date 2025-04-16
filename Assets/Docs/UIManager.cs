using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [Header("health")]
    [SerializeField] VisualTreeAsset healtPointTemplate;

    [Header("Crystal")]
    [SerializeField] VisualTreeAsset crystalTemplate;

    [Header("Cure Energy")]
    [SerializeField] Color cureEnergyColorBottom = Color.white;
    [SerializeField] Color cureEnergyColorTop = Color.green;



    UIDocument uiDocument;
    VisualElement rootElement;

    VisualElement healtContainer;
    VisualElement crystalContainer;

    VisualElement cureEnergyBar;


    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        rootElement = uiDocument.rootVisualElement;

        healtContainer = rootElement.Q<VisualElement>("HeartContainer");
        crystalContainer = rootElement.Q<VisualElement>("CrystalContainer");

        cureEnergyBar = rootElement.Q<VisualElement>("CureBarValue");

        FindAnyObjectByType<Player>().onHealthChange += UpdateHealth;
        FindAnyObjectByType<Fairy>().onCureEnergyChange += UpdateCureEnergy;
    }

    private void Start()
    {
        GameManager.Instance.onCrystalObtained += UpdateCrystals;
    }

    void UpdateCrystals(List<Crystal> crystalsObtained) 
    {
        crystalContainer.Clear();

        foreach (Crystal currentCrystal in crystalsObtained)
        {
            VisualElement crystalClone = crystalTemplate.CloneTree();

            VisualElement crystal = crystalClone.Q<VisualElement>("Crystal");

            if (currentCrystal.IsCollected)
            {
                crystal.AddToClassList("filled");
            }
            else
            {
                crystal.AddToClassList("emptyCrystal");
            }
            crystalContainer.Add(crystalClone);
        }
    }

    void UpdateHealth(int current, int maxHealth) 
    { 
        healtContainer.Clear();

        for (int i = 0; i < maxHealth; i++)
        {
            VisualElement healthPoint = healtPointTemplate.CloneTree();

            VisualElement heart = healthPoint.Q<VisualElement>("heart");

            if (i < current)
            {
                heart.AddToClassList("filled");
            }
            else
            {
                heart.AddToClassList("empty");
            }
            healtContainer.Add(healthPoint);
        }
    }

    void UpdateCureEnergy(float currentTimer, float maxTimer)
    { 
        //cureEnergyBar.style.width = Length.Percent(Mathf.Lerp(100, 0, currentTimer / maxTimer));
        //cureEnergyBar.style.backgroundColor = Color.Lerp(cureEnergyColorTop, cureEnergyColorBottom, currentTimer / maxTimer);
        
        cureEnergyBar.style.width = Length.Percent(Mathf.Lerp(0, 100, currentTimer / maxTimer));
        cureEnergyBar.style.backgroundColor = Color.Lerp(cureEnergyColorBottom, cureEnergyColorTop, currentTimer / maxTimer);
    }

}
