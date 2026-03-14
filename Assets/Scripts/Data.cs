using UnityEngine;

public class Data
{
    public static Data Instance { get; private set; }

    // Data fields
    public double RunStart;
    public double max_actions;
    public double actions_left;
    public double playermaxhp;
    public double playerhp;
    public double playershield;
    public double enemyshield;
    public double enemymaxhp;
    public double enemyhp;
    public double player_poison;
    public double enemy_poison;
    public double TurnToggle;
    public double enemySlashAct;
    public double enemySlash;
    public double CardsInHand;
    public double Activate;

    //currencys
    public double silver;
    public double gold;
    public double gem;

    //cards AND enemy attacks
    public double Pslash;
    public double Pdef;
    public double Ppsn;

    public double Eslash;

    //Perma Upgrades

    //cards Upgrade
    public double PslashUPgem;
    public double PdefUPgem;
    public double PpsnUPgem;

    //non Perma Upgrades

    //cards Upgrade
    public double PslashUPrun;
    public double PdefUPrun;
    public double PpsnUPrun;

    //poison triggers
    public double PpsnTrig;
    public double EpsnTrig;

    //upgrades
    public double playermaxhpUP;
    public double max_actionsUP;

    public Data()
    {
        // Initialize fields
        RunStart = 0;
        max_actions = 3;
        actions_left = 3;
        playermaxhp = 100;
        playerhp = 100;
        playershield = 0;
        enemyshield = 0;
        enemymaxhp = 100;
        enemyhp = 100;
        player_poison = 0;
        enemy_poison = 0;
        TurnToggle = 0;
        enemySlashAct = 0;
        enemySlash = 5;
        CardsInHand = 0;
        Activate = 0;

        //currencys
        silver = 0;
        gold = 0;
        gem = 0;

        //cards AND enemy attacks
        Pslash = 5;
        Pdef = 5;
        Ppsn = 3;

        Eslash = 5;

        //Perm UP

        //cards Upgrade
        PslashUPgem = 0;
        PdefUPgem = 0;
        PpsnUPgem = 0;

        //non Perm UP

        //cards Upgrade
        PslashUPrun = 0;
        PdefUPrun = 0;
        PpsnUPrun = 0;

        //poison triggers
        PpsnTrig = 0;
        EpsnTrig = 0;

        //upgrades
        playermaxhpUP = 0;
        max_actionsUP = 0;

        // Set static instance
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
