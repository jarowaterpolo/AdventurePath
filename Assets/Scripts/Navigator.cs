using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Navigator : MonoBehaviour 
{
    public Controller controller;
    public Data data;

    public Canvas StartScreen;
    public Canvas RunScreen;
    public Canvas OutRunUpgradesScreen;
    public Canvas InRunUpgradesScreen;



    public Canvas playerPoison;
    public Canvas enemyPoison;

    public Canvas playerShield;
    public Canvas enemyShield;

    public GameObject endturn;

    public void Start()
    {
        StartScreen.gameObject.SetActive(true);
        RunScreen.gameObject.SetActive(false);
        playerPoison.gameObject.SetActive(false);
        enemyPoison.gameObject.SetActive(false);
        playerShield.gameObject.SetActive(false); 
        enemyShield.gameObject.SetActive(false);
        endturn.SetActive(false);
        OutRunUpgradesScreen.gameObject.SetActive(false);
        InRunUpgradesScreen.gameObject.SetActive(false);
    }

    public void StartRun()
    {
        StartScreen.gameObject.SetActive(false);
        RunScreen.gameObject.SetActive(true);
        controller.NoActions.gameObject.SetActive(false);
        controller.StartStats();
        endturn.SetActive(true);
        OutRunUpgradesScreen.gameObject.SetActive(false);
        InRunUpgradesScreen.gameObject.SetActive(false);
    }

    public void ResumeRun()
    {
        StartScreen.gameObject.SetActive(false);
        RunScreen.gameObject.SetActive(true);
        controller.NoActions.gameObject.SetActive(false);
        controller.FightStatReset();
        endturn.SetActive(true);
        OutRunUpgradesScreen.gameObject.SetActive(false);
        InRunUpgradesScreen.gameObject.SetActive(false);
    }

    public void ToOutRunUpgrades()
    {
        StartScreen.gameObject.SetActive(false);
        RunScreen.gameObject.SetActive(false);
        controller.NoActions.gameObject.SetActive(false);
        endturn.SetActive(false);
        OutRunUpgradesScreen.gameObject.SetActive(true);
        InRunUpgradesScreen.gameObject.SetActive(false);
    }

    public void ToMainScreen()
    {
        StartScreen.gameObject.SetActive(true);
        RunScreen.gameObject.SetActive(false);
        playerPoison.gameObject.SetActive(false);
        enemyPoison.gameObject.SetActive(false);
        playerShield.gameObject.SetActive(false);
        enemyShield.gameObject.SetActive(false);
        endturn.SetActive(false);
        OutRunUpgradesScreen.gameObject.SetActive(false);
        InRunUpgradesScreen.gameObject.SetActive(false);
    }

    public void ToInRunUpgrades()
    {
        StartScreen.gameObject.SetActive(false);
        RunScreen.gameObject.SetActive(false);
        playerPoison.gameObject.SetActive(false);
        enemyPoison.gameObject.SetActive(false);
        playerShield.gameObject.SetActive(false);
        enemyShield.gameObject.SetActive(false);
        endturn.SetActive(false);
        OutRunUpgradesScreen.gameObject.SetActive(false);
        InRunUpgradesScreen.gameObject.SetActive(true);
    }

}
