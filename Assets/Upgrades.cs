using UnityEngine;

public class Upgrades : MonoBehaviour
{
    public Controller controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Up1()
    {
        if (controller.data.silver >= 100 || controller.data.gold >= 10)
        {
            if (controller.data.silver >= 100)
            {
                controller.data.silver -= 100;
                controller.data.PslashUPrun += 1;
            }
            else if (controller.data.gold >= 10)
            {
                controller.data.gold -= 10;
                controller.data.PslashUPrun += 1;
            }
            controller.Currency();
            controller.UpdateCardTexts();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }
    public void Up2()
    {
        if (controller.data.silver >= 100 || controller.data.gold >= 10)
        {
            if (controller.data.silver >= 100)
            {
                controller.data.silver -= 100;
                controller.data.PdefUPrun += 1;
            }
            else if (controller.data.gold >= 10)
            {
                controller.data.gold -= 10;
                controller.data.PdefUPrun += 1;
            }
            controller.Currency();
            controller.UpdateCardTexts();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }
    public void Up3()
    {
        if (controller.data.silver >= 100 || controller.data.gold >= 10)
        {
            if (controller.data.silver >= 100)
            {
                controller.data.silver -= 100;
                controller.data.PpsnUPrun += 1;
            }
            else if (controller.data.gold >= 10)
            {
                controller.data.gold -= 10;
                controller.data.PpsnUPrun += 1;
            }
            controller.Currency();
            controller.UpdateCardTexts();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }
    public void Up4()
    {
        if (controller.data.silver >= 100)
        {
            controller.data.silver -= 100;
            controller.data.playermaxhpUP += 10;
            controller.Currency();
            controller.UpdateStatsTexts();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }
    public void Up5()
    {
        if (controller.data.gold >= 100)
        {
            controller.data.gold -= 100;
            controller.data.max_actionsUP += 1;
            controller.Currency();
            controller.UpdateAction();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }

    public void GemUp1()
    {
        if (controller.data.gem >= 1)
        {
            controller.data.gem -= 1;
            controller.data.PslashUPgem += 1;
            controller.Currency();
            controller.UpdateCardTexts();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }
    public void GemUp2()
    {
        if (controller.data.gem >= 1)
        {
            controller.data.gem -= 1;
            controller.data.PdefUPgem += 1;
            controller.Currency();
            controller.UpdateCardTexts();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }
    public void GemUp3()
    {
        if (controller.data.gem >= 1)
        {
            controller.data.gem -= 1;
            controller.data.PpsnUPgem += 1;
            controller.Currency();
            controller.UpdateCardTexts();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }
    public void GemUp4()
    {
        if (controller.data.gem >= 5)
        {
            controller.data.gem -= 5;
            controller.data.playermaxhpUP += 10;
            controller.Currency();
            controller.UpdateStatsTexts();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }
    public void GemUp5()
    {
        if (controller.data.gem >= 10)
        {
            controller.data.gem -= 10;
            controller.data.max_actionsUP += 1;
            controller.Currency();
            controller.UpdateAction();
        }
        else
        {
            //editing text for currency
            Debug.Log("not enough currency");
        }
    }
}
