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

    [Header("Documents")]
    [SerializeField] UIDocument pause;
    VisualElement rootPause;
    Button returnButtonPause;


    [SerializeField] UIDocument endgame;
    VisualElement rootEndgame;
    Button returnButtonEndgame;
    Label winConditionLabel;
    Label crystalsScore;
    Label enemiesScore;


    UIDocument uiDocument;
    VisualElement rootElement;

    VisualElement healtContainer;
    VisualElement crystalContainer;

    VisualElement cureEnergyBar;


    private void Awake()
    {
        #region UI
        uiDocument = GetComponent<UIDocument>();
        rootElement = uiDocument.rootVisualElement;

        healtContainer = rootElement.Q<VisualElement>("HeartContainer");
        crystalContainer = rootElement.Q<VisualElement>("CrystalContainer");

        cureEnergyBar = rootElement.Q<VisualElement>("CureBarValue");

        FindAnyObjectByType<Player>().onHealthChange += UpdateHealth;
        FindAnyObjectByType<Fairy>().onCureEnergyChange += UpdateCureEnergy;
        #endregion

        #region pauseDoc
        rootPause = pause.rootVisualElement;
        rootPause.style.display = DisplayStyle.None;

        returnButtonPause = rootPause.Q<Button>("Return");
        returnButtonPause.clicked += OnReturnToMenu_Clicked;
        #endregion

        #region endgameDoc
        rootEndgame = endgame.rootVisualElement;
        rootEndgame.style.display = DisplayStyle.None;

        returnButtonEndgame = rootEndgame.Q<Button>("Return");
        returnButtonEndgame.clicked += OnReturnToMenu_Clicked;

        winConditionLabel = rootEndgame.Q<Label>("WinCondition");
        crystalsScore = rootEndgame.Q<Label>("CrystalsObtained");
        enemiesScore = rootEndgame.Q<Label>("EnemiesKilled");
        #endregion
    }

    private void Start()
    {
        GameManager.Instance.onCrystalObtained += UpdateCrystals;
        GameManager.Instance.onPause += OnPause;
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
        cureEnergyBar.style.width = Length.Percent(Mathf.Lerp(0, 100, currentTimer / maxTimer));
        cureEnergyBar.style.backgroundColor = Color.Lerp(cureEnergyColorBottom, cureEnergyColorTop, currentTimer / maxTimer);
    }


    void OnPause(bool isPausing, int status) 
    {
        Time.timeScale = isPausing ? 0 : 1;

        switch (status)
        {
            case 0:
                rootPause.style.display = isPausing ? DisplayStyle.Flex : DisplayStyle.None;
                break;
            case 1:
                winConditionLabel.text = "You win";
                crystalsScore.text = GameManager.Instance.GetCrystals().ToString() + "/" + GameManager.Instance.GetMaxCrystals().ToString();
                enemiesScore.text = GameManager.Instance.GetDeadEnemies().ToString() + "/" + GameManager.Instance.GetMaxEnemies().ToString();
                rootEndgame.style.display = DisplayStyle.Flex;
                break;
            case 2:
                winConditionLabel.text = "You lose";
                crystalsScore.text = GameManager.Instance.GetCrystals().ToString() + "/" + GameManager.Instance.GetMaxCrystals().ToString();
                enemiesScore.text = GameManager.Instance.GetDeadEnemies().ToString() + "/" + GameManager.Instance.GetMaxEnemies().ToString();
                rootEndgame.style.display = DisplayStyle.Flex;
                break;
        }
    }

    void OnReturnToMenu_Clicked() 
    { }
}
