using NUnit.Framework.Interfaces;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public Data data;
    public Navigator navigator;
    public CardHandler cardHandler;

    public TMP_Text PlayerHPText;
    public TMP_Text EnemyHPText;

    public TMP_Text ActionsText;
    public Canvas NoActions;

    public Canvas playerPoison;
    public Canvas enemyPoison;

    public TMP_Text playerPoisonText;
    public TMP_Text enemyPoisonText;

    public Canvas playerShield;
    public Canvas enemyShield;

    public TMP_Text playerShieldText;
    public TMP_Text enemyShieldText;

    public Canvas EnemyActionCanvas;
    public TMP_Text EnemyActionText;

    public GameObject[] Cards;
    public MonoBehaviour[] EnemyActions;

    public Canvas Hand;

    //currency text
    public TMP_Text[] CurrencyTexts;


    public void Start()
    {
        if (Data.Instance == null)
        {
            new Data();
        }
        data = Data.Instance;

        StartStats();
    }

    public void Update()
    {
        if (data.RunStart == 1)
        {
            UpdateAction();

            if (data.enemy_poison >= 1)
            {
                enemyPoison.gameObject.SetActive(true);
                UpdateStatsTexts();
            }

            if (data.player_poison >= 1)
            {
                playerPoison.gameObject.SetActive(true);
                UpdateStatsTexts();
            }

            if (data.enemyshield >= 1)
            {
                enemyShield.gameObject.SetActive(true);
                UpdateStatsTexts();
            }

            if (data.playershield >= 1) 
            {
                playerShield.gameObject.SetActive(true);
                UpdateStatsTexts();
            }

            if (data.TurnToggle == 0) 
            {
                //trigger psn
                if (data.enemy_poison > 0)
                {
                    if (data.EpsnTrig == 1)
                    {
                        EnemyPoisonEffect();
                        data.EpsnTrig = 0;
                    }
                }
                data.PpsnTrig = 1;

                EnemyActionCanvas.gameObject.SetActive(false);
                if (data.CardsInHand < 3)
                {
                    cardHandler.AddCardToHandOrder();
                }
                Cards[0].SetActive(true);
                Cards[1].SetActive(true);
                Cards[2].SetActive(true);
                data.Activate = 0;
            }
            else if(data.TurnToggle == 1)
            {
                //trigger psn
                if (data.player_poison > 0)
                {
                    if (data.PpsnTrig == 1)
                    {
                        PlayerPoisonEffect();
                    }
                }
                data.EpsnTrig = 1;

                EnemyActionCanvas.gameObject.SetActive(true);
                Cards[0].SetActive(false);
                Cards[1].SetActive(false);
                Cards[2].SetActive(false);
                // Call EnemyActionUse only once
                EnemyActionUse();
                TurnBack();
            }

            //player hp checker
            if (data.playerhp <= 0)
            {

                navigator.Start();
                data.silver = 0;
                data.gold = 0;



                data.RunStart = 0;
                StartStats();
                // Update game state after executing the action
                UpdateStatsTexts();
                UpdateAction();
                Currency();
            }

            //enemy hp checker
            if (data.enemyhp <= 0)
            {
                //random ranges are random numbers between x and x-1 so always put it 1 higher than you want the second value
                data.silver += Mathf.Round(Random.Range(7 + data.Stage, 13 + data.Stage) * 6 + 10 + data.Stage);
                data.gold += Mathf.Round(Random.Range(2, 7) * Random.Range(2, 7) + data.Stage * Random.Range(2, 7));
                data.gem += Mathf.Round(Random.Range(1, 3) * Random.Range(0, 3) * Random.Range(0, 3) + data.Stage * Random.Range(0,2) + data.Stage * Random.Range(0, 2) + data.Stage * Random.Range(0, 2));

                navigator.ToInRunUpgrades();

                //enemy scaling
                data.enemymaxhp *= 2;
                data.Eslash *= 2;

                data.TurnToggle = 0;
                FightStatReset();
                // Update game state after executing the action
                UpdateStatsTexts();
                UpdateAction();
                Currency();

            }

        }
    }

    public void StartStats()
    {
        data.RunStart = 1;

        //player cards
        data.Pslash = 5 + data.PslashUPgem;
        data.Pdef = 5 + data.PdefUPgem;
        data.Ppsn = 3 + data.PpsnUPgem;

        data.PslashUPrun = 0;
        data.PdefUPrun = 0;
        data.PpsnUPrun = 0;

        //enemy actions
        data.Eslash = 5;

        data.max_actions = 3 + data.max_actionsUP;

        data.actions_left = data.max_actions;

        data.playermaxhp = 100 + data.playermaxhpUP;
        data.enemymaxhp = 100;

        data.playerhp = data.playermaxhp;
        data.enemyhp = data.enemymaxhp;

        data.enemy_poison = 0;
        data.player_poison = 0;

        data.EpsnTrig = 0;

        data.enemyshield = 0;
        data.playershield = 0;

        data.TurnToggle = 0;
        UpdateStatsTexts();
        UpdateAction();
        UpdateCardTexts();
    }

    public void FightStatReset()
    {
        data.RunStart = 1;

        //stage 
        data.Stage++;

        //player cards
        data.Pslash = 5 + data.PslashUPgem + data.PslashUPrun;
        data.Pdef = 5 + data.PdefUPgem + data.PdefUPrun;
        data.Ppsn = 3 + data.PpsnUPgem + data.PpsnUPrun;

        data.max_actions = 3 + data.max_actionsUP;

        data.actions_left = data.max_actions;

        data.playermaxhp = 100 + data.playermaxhpUP;

        data.playerhp = data.playermaxhp;
        data.enemyhp = data.enemymaxhp;

        data.enemy_poison = 0;
        data.player_poison = 0;

        data.EpsnTrig = 0;

        data.enemyshield = 0;
        data.playershield = 0;

        data.TurnToggle = 0;
        UpdateStatsTexts();
        UpdateAction();
        UpdateCardTexts();
    }

    public void ENDTurn()
    {
        data.TurnToggle = 1;
        data.actions_left = data.max_actions;

    }

    public void UpdateAction()
    {
        if (data.actions_left <= 0)
        {
            NoActions.gameObject.SetActive(true);
        }
        else
        {
            NoActions.gameObject.SetActive(false);
        }
        ActionsText.text = data.actions_left + "/" + data.max_actions + " Actions left";
    }

    public void UpdateStatsTexts()
    {
        PlayerHPText.text = "Player HP = " + data.playerhp;
        EnemyHPText.text = "Enemy HP = " + data.enemyhp;
        playerPoisonText.text = data.player_poison + " PSN";
        enemyPoisonText.text = data.enemy_poison + " PSN";
        playerShieldText.text = data.playershield + " DEF";
        enemyShieldText.text = data.enemyshield + " DEF";
    }
    public void EnemyActionUse()
    {
        // Ensure the action is executed only once per turn
        if (data.Activate == 1)
        {
            return; // Skip execution if already activated this turn
        }

        // Select a random action
        int randomIndex = Random.Range(0, EnemyActions.Length);

        // Activate only the selected script
        EnemyActionText.text = "The Enemy Used " + EnemyActions[randomIndex].name + "!";

        for (int i = 0; i < EnemyActions.Length; i++)
        {
            EnemyActions[i].enabled = (i == randomIndex); // Enable only the selected action
        }

        // Execute the selected action if it has the Execute method
        if (EnemyActions[randomIndex] is EnemySlash enemySlash)
        {
            data.Activate = 1; // Mark the action as activated for this turn
            enemySlash.Execute(); // Execute the logic when this action is used

        }

        // Execute the selected action if it has the Execute method
        if (EnemyActions[randomIndex] is EnemyProtect enemyProtect)
        {
            data.Activate = 1; // Mark the action as activated for this turn
            enemyProtect.Execute(); // Execute the logic when this action is used

        }

        // Execute the selected action if it has the Execute method
        if (EnemyActions[randomIndex] is EnemyApplyPoison enemyApplyPoison)
        {
            data.Activate = 1; // Mark the action as activated for this turn
            enemyApplyPoison.Execute(); // Execute the logic when this action is used

        }


        // Update game state after executing the action
        UpdateStatsTexts();
        UpdateAction();
        // Optionally call TurnBack here if needed
        TurnBack();
    }
    public void TurnBack()
    {
        data.TurnToggle = 0;
        //data.Activate = 0;
        data.actions_left = data.max_actions;
        UpdateAction();
    }
    public void Currency()
    {
        foreach (var text in CurrencyTexts)
        {
            if (text.name.Contains("Silver"))
            {
                text.text = "Silver = " + data.silver;
            }

            if (text.name.Contains("Gold"))
            {
                text.text = "Gold = " + data.gold;
            }

            if (text.name.Contains("Gem"))
            {
                if (data.gem > 1)
                {
                    text.text = "Gems = " + data.gem;
                }
                else if(data.gem == 0)
                {
                    text.text = "Gems = " + data.gem;
                }
                else
                {
                    text.text = "Gem = " + data.gem;
                }
            }
        }
    }
    public void PlayerPoisonEffect()
    {
        data.playerhp -= data.player_poison;
        data.player_poison -= 1;
        UpdateStatsTexts();
    }
    public void EnemyPoisonEffect()
    {
        data.enemyhp -= data.enemy_poison;
        data.enemy_poison -= 1;
        UpdateStatsTexts();
    }
    public void Gemtest()
    {
        data.gem += Mathf.Round(Random.Range(1, 3) * Random.Range(0, 3) * Random.Range(0, 3) + data.Stage * Random.Range(0, 2) + data.Stage * Random.Range(0, 2) + data.Stage * Random.Range(0, 2));
        Currency();
    }
    public void UpdateCardTexts()
    {
        foreach (Transform card in Hand.transform)
        {
            TMP_Text textComponent = null;

            if (card.name.Contains("Slash"))
            {
                textComponent = card.Find("DMGText")?.GetComponent<TMP_Text>();
                if (textComponent != null)
                    textComponent.text = (5 + data.PslashUPgem + data.PslashUPrun) + " DMG";
            }
            else if (card.name.Contains("Protect"))
            {
                textComponent = card.Find("SHIELDText")?.GetComponent<TMP_Text>();
                if (textComponent != null)
                    textComponent.text = (5 + data.PdefUPgem + data.PdefUPrun) + " DEF";
            }
            else if (card.name.Contains("Apply_Poison"))
            {
                textComponent = card.Find("POISONText")?.GetComponent<TMP_Text>();
                if (textComponent != null)
                    textComponent.text = (3 + data.PpsnUPgem + data.PpsnUPrun) + " PSN";
            }
        }
    }

}
