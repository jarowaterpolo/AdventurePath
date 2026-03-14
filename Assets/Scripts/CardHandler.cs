using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler: MonoBehaviour
{
    private Controller controller;
    private GameObject[] Cards;
    private Canvas Hand;
    private Data data;
    private void Start()
    {
        Cards = controller.Cards;
        Hand = controller.Hand;
        data = controller.data;
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

        Button cardButton = newCard.GetComponent<Button>();
        if (cardButton != null)
        {
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(() => HandleCardUse(newCard)); // Use the specific card instance
        }

        data.CardsInHand += 1; // Increment the card count

        // Update the text of the new card based on the current upgrades
        controller.UpdateCardTexts(); // Update the text after adding a new card
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
        GameObject nextCard = Cards.FirstOrDefault(card => card.name.Contains(nextCardName));

        if (nextCard != null)
        {
            GameObject newCard = Instantiate(nextCard, Hand.transform); // Add the card to the hand
            newCard.name = nextCard.name;

            Button cardButton = newCard.GetComponent<Button>();
            if (cardButton != null)
            {
                cardButton.onClick.RemoveAllListeners();
                cardButton.onClick.AddListener(() => HandleCardUse(newCard)); // Use the specific card instance
            }

            data.CardsInHand += 1; // Increment the card count

            // Update the text of the new card based on the current upgrades
            controller.UpdateCardTexts(); // Update the text after adding a new card

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
            controller.UpdateStatsTexts();
            controller.UpdateAction();
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
            controller.UpdateStatsTexts();
            controller.UpdateAction();
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
            controller.UpdateStatsTexts();
            controller.UpdateAction();
        }
        else
        {
            Debug.Log("No actions left!");
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
        if (card.name.Contains("Slash")) Slash(card);
        else if (card.name.Contains("Protect")) Protect(card);
        else if (card.name.Contains("Apply_Poison")) ApplyPoison(card);

        // Replace the used card after using it
        ReplaceUsedCard(card);  // Add this line to replace the used card
    }
}
