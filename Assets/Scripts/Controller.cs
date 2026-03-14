using NUnit.Framework.Interfaces;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public Data data;
    public Navigator navigator;

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
                    AddCardToHandOrder();
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
                data.silver += Random.Range(7, 13) * 6 + 10;
                data.gold += Random.Range(2, 7) * Random.Range(2, 7);
                data.gem += (Random.Range(1, 3) * Random.Range(0, 3) * Random.Range(0, 3)) + 1 * Random.Range(0,2) + 1 * Random.Range(0, 2) + 1 * Random.Range(0, 2);

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

    public void AddCardToHandRandom()
    {
        // Check if there are already 3 cards in hand. If so, we do nothing
        if (data.CardsInHand >= 3)
        {
            return; // Exit early if the hand is full
        }

        // Find the first empty spot in the hand (this assumes your hand is structured to be empty or filled sequentially)
        GameObject randomCard = Cards[Random.Range(0, Cards.Length)];
        GameObject newCard = Instantiate(randomCard, Hand.transform); // Add the card to the hand
        newCard.name = randomCard.name;

        // Assign button functionality to the new card
        Transform useCardTransform = newCard.transform.Find("UseCard");
        if (useCardTransform != null)
        {
            Button cardButton = useCardTransform.GetComponent<Button>();
            if (cardButton != null)
            {
                cardButton.onClick.RemoveAllListeners();
                cardButton.onClick.AddListener(() => HandleCardUse(newCard)); // Use the specific card instance
            }
        }

        data.CardsInHand += 1; // Increment the card count

        // Update the text of the new card based on the current upgrades
        UpdateCardTexts(); // Update the text after adding a new card
    }

    private int cardIndex = 0; // Index to cycle through cards

    public void AddCardToHandOrder()
    {
        // Check if there are already 3 cards in hand. If so, we do nothing
        if (data.CardsInHand >= 3)
        {
            return; // Exit early if the hand is full
        }

        // Define the order in which cards should be added
        string[] cardOrder = { "Slash", "Protect", "Apply_Poison" };

        // Get the next card in the cycle based on cardIndex
        string nextCardName = cardOrder[cardIndex]; // Cycle through the cardOrder array

        // Find the card with the corresponding name
        GameObject nextCard = Cards.FirstOrDefault(card => card.name == nextCardName);

        if (nextCard != null)
        {
            GameObject newCard = Instantiate(nextCard, Hand.transform); // Add the card to the hand
            newCard.name = nextCard.name;

            // Assign button functionality to the new card
            Transform useCardTransform = newCard.transform.Find("UseCard");
            if (useCardTransform != null)
            {
                Button cardButton = useCardTransform.GetComponent<Button>();
                if (cardButton != null)
                {
                    cardButton.onClick.RemoveAllListeners();
                    cardButton.onClick.AddListener(() => HandleCardUse(newCard)); // Use the specific card instance
                }
            }

            data.CardsInHand += 1; // Increment the card count

            // Update the text of the new card based on the current upgrades
            UpdateCardTexts(); // Update the text after adding a new card

            // Increment cardIndex to cycle through cards
            cardIndex = (cardIndex + 1) % cardOrder.Length; // Reset to 0 when reaching the end of the cycle
        }
    }



    public void Slash(GameObject card)
    {
        if (data.actions_left >= 1)
        {
            if (data.enemyshield > 0)
            {
                data.enemyshield -= data.Pslash;
            }
            else
            {
                data.enemyhp -= data.Pslash;
            }

            if (data.enemyshield < 0)
            {
                data.enemyhp += data.enemyshield;
                data.enemyshield = 0;
            }

            data.actions_left -= 1;
            UpdateStatsTexts();
            UpdateAction();
        }
        else
        {
            Debug.Log("No actions left!");
        }
    }

    public void Protect(GameObject card)
    {
        if (data.actions_left >= 1)
        {
            data.playershield += data.Pdef;
            data.actions_left -= 1;
            UpdateStatsTexts();
            UpdateAction();
        }
        else
        {
            Debug.Log("No actions left!");
        }
    }

    public void ApplyPoison(GameObject card)
    {
        if (data.actions_left >= 1)
        {
            data.enemy_poison += data.Ppsn;
            data.actions_left -= 1;
            UpdateStatsTexts();
            UpdateAction();
        }
        else
        {
            Debug.Log("No actions left!");
        }
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
        data.gem += (Random.Range(0, 2) * Random.Range(0, 2) * Random.Range(0, 2)) + 1 * Random.Range(0, 1) + 1 * Random.Range(0, 1) + 1 * Random.Range(0, 1);
        Currency();
    }

    public void UpdateCardTexts()
    {
        foreach (Transform card in Hand.transform)
        {
            TMP_Text textComponent = null;

            if (card.name == "Slash")
            {
                textComponent = card.Find("DMGText")?.GetComponent<TMP_Text>();
                if (textComponent != null)
                    textComponent.text = (5 + data.PslashUPgem + data.PslashUPrun) + " DMG";
            }
            else if (card.name == "Protect")
            {
                textComponent = card.Find("SHIELDText")?.GetComponent<TMP_Text>();
                if (textComponent != null)
                    textComponent.text = (5 + data.PdefUPgem + data.PdefUPrun) + " DEF";
            }
            else if (card.name == "Apply_Poison")
            {
                textComponent = card.Find("POISONText")?.GetComponent<TMP_Text>();
                if (textComponent != null)
                    textComponent.text = (3 + data.PpsnUPgem + data.PpsnUPrun) + " PSN";
            }
        }
    }


    public void GemUp1()
    {
        if (data.gem >= 1)
        {
            data.gem -= 1;
            data.PslashUPgem += 1;
            Currency();
            UpdateCardTexts();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }

    public void GemUp2()
    {
        if (data.gem >= 1)
        {
            data.gem -= 1;
            data.PdefUPgem += 1;
            Currency();
            UpdateCardTexts();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }

    public void GemUp3()
    {
        if (data.gem >= 1)
        {
            data.gem -= 1;
            data.PpsnUPgem += 1;
            Currency();
            UpdateCardTexts();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }

    public void GemUp4()
    {
        if (data.gem >= 5)
        {
            data.gem -= 5;
            data.playermaxhpUP += 10;
            Currency();
            UpdateStatsTexts();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }

    public void GemUp5()
    {
        if (data.gem >= 10)
        {
            data.gem -= 10;
            data.max_actionsUP += 1;
            Currency();
            UpdateAction();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }

    public void ReplaceUsedCard(GameObject card)
    {
        if (data.actions_left > 0)
        {
            // Check if the card is not null before destroying it
            if (card != null)
            {
                Destroy(card); // Destroy the exact card clicked
                data.CardsInHand -= 1; // Decrease the card count
            }

            // Ensure there are no more than 3 cards in hand
            if (data.CardsInHand < 3)
            {
                AddCardToHandOrder(); // Add a new card if the hand is not full
            }
        }
        else
        {
            Debug.Log("no actions so no card replace");
        }
        
    }


    public void HandleCardUse(GameObject card)
    {
        if (card == null) return;

        // Handle the card logic based on its name
        if (card.name == "Slash") Slash(card);
        else if (card.name == "Protect") Protect(card);
        else if (card.name == "Apply_Poison") ApplyPoison(card);

        // Replace the used card after using it
        ReplaceUsedCard(card);  // Add this line to replace the used card
    }

    public void Up1()
    {
        if (data.silver >= 100 || data.gold >= 10)
        {
            if (data.silver >= 100)
            {
                data.silver -= 100;
                data.PslashUPrun += 1;
            }
            else if (data.gold >= 10)
            {
                data.gold -= 10;
                data.PslashUPrun += 1;
            }
            Currency();
            UpdateCardTexts();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }

    public void Up2()
    {
        if (data.silver >= 100 || data.gold >= 10)
        {
            if (data.silver >= 100)
            {
                data.silver -= 100;
                data.PdefUPrun += 1;
            }
            else if (data.gold >= 10)
            {
                data.gold -= 10;
                data.PdefUPrun += 1;
            }
            Currency();
            UpdateCardTexts();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }

    public void Up3()
    {
        if (data.silver >= 100 || data.gold >= 10)
        {
            if (data.silver >= 100)
            {
                data.silver -= 100;
                data.PpsnUPrun += 1;
            }
            else if (data.gold >= 10)
            {
                data.gold -= 10;
                data.PpsnUPrun += 1;
            }
            Currency();
            UpdateCardTexts();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }

    public void Up4()
    {
        if (data.silver >= 100)
        {
            data.silver -= 100;
            data.playermaxhpUP += 10;
            Currency();
            UpdateStatsTexts();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }

    public void Up5()
    {
        if (data.gold >= 100)
        {
            data.gold -= 100;
            data.max_actionsUP += 1;
            Currency();
            UpdateAction();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }

}
