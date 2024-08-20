using System.Collections.Generic;
using UnityEngine;

public struct StatusEffects
{
    public bool IsShield;
    public bool IsIllusion;
    public bool IsInvisibility;
    public bool IsStun;
    public bool IsInvulnerability;

    public bool IsShielded;
    public bool IsStunned;
}

public struct Card
{
    public string Name;
    public string SecondName;
    public string Description;

    public int MaxPoints;
    public int Points;

    public Texture ImageTexture;
    public Sprite Sprite;

    public AudioClip StartOrderSound;
    public Color ColorTheme;

    public int Boost;
    public int RangeBoost;
    public int ChangeBoost;

    public int Damage;
    public int RangeDamage;
    public int ChangeDamage;

    public int SelfBoost;
    public int SelfDamage;

    public bool AddictionWithSelfField;
    public bool AddictionWithEnemyField;

    public bool EndTurnAction;
    public int EndTurnActionQuantity;
    public int EndTurnDamage;
    public int EndTurnBoost;

    public bool Summon;
    public int SummonCardNumber;
    public int SummonCardCount;

    public int DrawCardCount;


    public StatusEffects StatusEffects;

    public Card(string name, string secondName, string description,
        string spritePath, string startOrderSoundPath, Color color,
        int maxPoints, int points,
        int boost = 0, int rangeBoost = 0, int changeBoost = 0,
        int damage = 0, int rangeDamage = 0, int changeDamage = 0,
        int selfBoost = 0,
        int selfDamage = 0,
        bool addictionWithSelfField = false, bool addictionWithEnemyField = false, bool endTurnAction = false, int endTurnQuantity = 0, int endTurnDamage = 0, int endTurnBoost = 0,
        bool summon = false, int summonCardNumber = 0, int summonCardCount = 0,
        bool shield = false, bool stun = false, bool invisibility = false, bool invulnerability = false, bool isShielded = false,
        int drawCardCount = 0)
    {
        Name = name;
        SecondName = secondName;
        Description = description;
        MaxPoints = maxPoints;
        Points = points;

        ImageTexture = Resources.Load<Texture>(spritePath);
        Sprite = Resources.Load<Sprite>(spritePath);
        StartOrderSound = Resources.Load<AudioClip>(startOrderSoundPath);
        ColorTheme = color;

        Boost = boost;
        RangeBoost = rangeBoost;
        ChangeBoost = changeBoost;

        Damage = damage;
        RangeDamage = rangeDamage;
        ChangeDamage = changeDamage;

        SelfBoost = selfBoost;
        SelfDamage = selfDamage;

        AddictionWithSelfField = addictionWithSelfField;
        AddictionWithEnemyField = addictionWithEnemyField;

        EndTurnAction = endTurnAction;
        EndTurnActionQuantity = endTurnQuantity;
        EndTurnDamage = endTurnDamage;
        EndTurnBoost = endTurnBoost;

        Summon = summon;
        SummonCardNumber = summonCardNumber;
        SummonCardCount = summonCardCount;

        StatusEffects = new StatusEffects();
        StatusEffects.IsShield = shield;
        StatusEffects.IsStun = stun;
        StatusEffects.IsInvisibility = invisibility;
        StatusEffects.IsInvulnerability = invulnerability;
        StatusEffects.IsShielded = isShielded;

        DrawCardCount = drawCardCount;
    }
}

public static class CardManagerList
{
    public static bool IsAddCardInGame;

    public static List<Card> AllCards = new List<Card>();
    public static List<Card> SummonCards = new List<Card>();

    public static List<Card> DebugCards = new List<Card>();
}

public class CardManager : MonoBehaviour
{


    private void Awake()
    {
        if (!CardManagerList.IsAddCardInGame)
        {

            CardManagerList.DebugCards.Add(new Card("Debug", "Debug", "Debug",
                "Sprites/Cards/Lina1", "Sounds/Cards/StartOrder/LinaDragonSlave", Color.white,
                10, 10,
                0, 0, 0,
                0, 0, 0));

            /* EXAMPLE
            CardManagerList.AllCards.Add(new Card("Lina", "Dragon Slave", "Damage enemy card by 3",  = NAME, SECOND NAME and DESCRIPTION
                "Sprites/Cards/Lina1", "Sounds/Cards/StartOrder/LinaDragonSlave", Color.red,  = IMAGE, SOUND and COLOR
                5, 5,  = POINTS and MAXPOINTS
                0, 0, 0, = BOOST and RANGEBOOST and CHANGEBOOST
                3 ,0, 0, = DAMAGE and RANGEDAMAGE and CHANGEDAMAGE
                4, 0, false, true, = SELFBOOST, SELFDAMAGE and ADDICTIONS with SELFFIELD, ENEMTFIELD
                false, false, false


            ));
             */

            CardManagerList.AllCards.Add(new Card("Lina", "Dragon Slave", "Damage 3 enemy unit by 3.",
                "Sprites/Cards/Lina1", "Sounds/Cards/StartOrder/LinaDragonSlave", Color.red,
                5, 5,
                0, 0, 0,
                3, 1, 0));

            CardManagerList.AllCards.Add(new Card("Lina", "Light Strike Array", "Damage enemy unit by 3 and stun its.",
                 "Sprites/Cards/Lina2", "Sounds/Cards/StartOrder/LinaLightStrikeArray", Color.red,
                 6, 6,
                 0, 0, 0,
                 3, 0, 0,
                 0, 0, false, false, false, 0, 0, 0, false, 0, 0, false, true));

            CardManagerList.AllCards.Add(new Card("Lina", "Fiery Soul", "At the end of the your turn, deal 1 damage to a random enemy unit 3 times.",
                 "Sprites/Cards/Lina3", "Sounds/Cards/StartOrder/LinaFierySoul", Color.red,
                 2, 2,
                 0, 0, 0,
                 0, 0, 0,
                 0, 0, false, false, true, 3, 1));

            CardManagerList.AllCards.Add(new Card("Lina", "Laguna Blade", "Damage enemy unit by 11.",
                 "Sprites/Cards/Lina4", "Sounds/Cards/StartOrder/LinaLagunaBlade", Color.red,
                 1, 1,
                 0, 0, 0,
                 11, 0, 0));

            CardManagerList.AllCards.Add(new Card("Luna", "Lucent Beam", "Damage enemy unit by 2 and stun it.",
                "Sprites/Cards/Luna1", "Sounds/Cards/StartOrder/LunaLucentBeam", Color.blue,
                7, 7,
                0, 0, 0,
                2, 0, 0,
                0, 0, false, false, false, 0, 0, 0, false, 0, 0, false, true));

            CardManagerList.AllCards.Add(new Card("Luna", "Moon Glaives", "At the end of the your turn, deal 1 damage to a random enemy unit 2 times.",
                "Sprites/Cards/Luna2", "Sounds/Cards/StartOrder/LunaMoonGlaives", Color.blue,
                 6, 6,
                 0, 0, 0,
                 0, 0, 0,
                 0, 0, false, false, true, 2, 1));

            CardManagerList.AllCards.Add(new Card("Luna", "Eclipse", "Damage 5 enemy units by 2.",
                "Sprites/Cards/Luna3", "Sounds/Cards/StartOrder/LunaEclipse", Color.blue,
                8, 8,
                0, 0, 0,
                2, 2, 0));

            CardManagerList.AllCards.Add(new Card("Templar Assasin", "Refraction", "Boost allied unit by 8. Shield.",
                 "Sprites/Cards/TemplarAssasin1", "Sounds/Cards/StartOrder/TemplarAssasinRefraction", Color.magenta,
                 3, 3,
                 8, 0, 0,
                 0, 0, 0,
                 0, 0, false, false, false, 0, 0, 0, false, 0, 0,
                 false, false, false, false, true));

            CardManagerList.AllCards.Add(new Card("Templar Assasin", "Meld", "Damage enemy unit by 5. Shield.",
                "Sprites/Cards/TemplarAssasin2", "Sounds/Cards/StartOrder/TemplarAssasinMeld", Color.magenta,
                7, 7,
                0, 0, 0,
                5, 0, 0,
                0, 0, false, false,
                false, 0, 0,
                0, false, 0, 0,
                false, false, false, false, true));

            CardManagerList.AllCards.Add(new Card("Axe", "Berserker's Call", "Boost enemy unit by 3.",
                 "Sprites/Cards/Axe1", "Sounds/Cards/StartOrder/AxeBerserker'sCall", Color.red,
                 14, 13,
                 0, 0, 0,
                 -3, 0, 0));

            CardManagerList.AllCards.Add(new Card("Centaur Warrunner", "Double Edge", "Damage self by 3.",
                 "Sprites/Cards/Centaur1", "Sounds/Cards/StartOrder/CentaurDoubleEdge", Color.gray,
                 20, 16,
                 0, 0, 0,
                 0, 0, 0,
                 0, 3));

            CardManagerList.AllCards.Add(new Card("Huskar", "Life Break", "Damage self and enemy unit by 7.",
                "Sprites/Cards/Huskar1", "Sounds/Cards/StartOrder/HuskarLifeBreak", Color.yellow,
                19, 17,
                0, 0, 0,
                7, 0, 0,
                0, 7, false, true));

            CardManagerList.AllCards.Add(new Card("Windranger", "Powershot", "Damage enemy unit by 5 and units near by 1.",
                "Sprites/Cards/Windranger1", "Sounds/Cards/StartOrder/WindrangerPowershot", Color.green,
                4, 4,
                0, 0, 0,
                5, 1, -4));

            CardManagerList.AllCards.Add(new Card("Windranger", "Windrun", "Draw 1 card",
                "Sprites/Cards/Windranger2", "Sounds/Cards/StartOrder/WindrangerWindrun", Color.green,
                3, 3,
                0, 0, 0,
                0, 0, 0,
                0, 0, false, false,
                false, 0, 0,
                0, false, 0, 0,
                false, false, false, false, false,
                1));

            CardManagerList.AllCards.Add(new Card("Kunkka", "Tidebringer", "Damage enemy unit by 2, and units near by 4.",
                "Sprites/Cards/Kunkka1", "Sounds/Cards/StartOrder/KunkkaTidebringer", Color.blue,
                5, 5,
                0, 0, 0,
                2, 1, 2));

            CardManagerList.AllCards.Add(new Card("Earthshaker", "Fissure", "Damage 3 enemy units by 3 then stun them.",
                "Sprites/Cards/Earthshaker1", "Sounds/Cards/StartOrder/EarthshakerFissure", Color.yellow,
                6, 6,
                0, 0, 0,
                3, 1, 0,
                0, 0, false, false, false, 0, 0, 0, false, 0, 0, false, true));

            CardManagerList.AllCards.Add(new Card("Earthshaker", "Echo Slam", "Damage all enemy units by 1.",
                "Sprites/Cards/Earthshaker2", "Sounds/Cards/StartOrder/EarthshakerEchoSlam", Color.yellow,
                4, 4,
                0, 0, 0,
                1, -1, 0));

            CardManagerList.AllCards.Add(new Card("Chen", "Hand Of God", "Boost all allied units by 1.",
                "Sprites/Cards/Chen1", "Sounds/Cards/StartOrder/ChenHandOfGod", Color.yellow,
                3, 3,
                1, -1, 0,
                0, 0, 0));

            CardManagerList.AllCards.Add(new Card("Chen", "Divine Fervor", "At the end of the your turn, deal 1 boost to a random 2 allied units.",
                "Sprites/Cards/Chen2", "Sounds/Cards/StartOrder/ChenDivineFervor", Color.yellow,
                5, 5,
                0, 0, 0,
                0, 0, 0,
                0, 0, false, false,
                true, 2, 0, 1));

            CardManagerList.AllCards.Add(new Card("Sniper", "Assasinate", "Damage enemy unit by 7.",
                "Sprites/Cards/Sniper1", "Sounds/Cards/StartOrder/SniperAssasinate", Color.yellow,
                3, 3,
                0, 0, 0,
                7, 0, 0));

            CardManagerList.AllCards.Add(new Card("Bane", "BrainSap", "Damage enemy unit and boost self by 4.",
                "Sprites/Cards/Bane1", "Sounds/Cards/StartOrder/BaneBrainSap", Color.black,
                4, 4,
                0, 0, 0,
                4, 0, 0,
                4, 0, false, true));

            CardManagerList.AllCards.Add(new Card("Zeus", "Arc Lightning", "Damage enemy unit by 2, and near units by 1.",
                "Sprites/Cards/Zeus1", "Sounds/Cards/StartOrder/ZeusArcLightning", Color.blue,
                3, 3,
                0, 0, 0,
                2, 1, -1));

            CardManagerList.AllCards.Add(new Card("Zeus", "Lightning Bolt", "Damage enemy unit by 5.",
                "Sprites/Cards/Zeus2", "Sounds/Cards/StartOrder/ZeusLightningBolt", Color.blue,
                3, 3,
                0, 0, 0,
                5, 0, 0));

            CardManagerList.AllCards.Add(new Card("Abaddon", "Mist Coil", "Boost allied unit by 4 and damage self by 4.",
                "Sprites/Cards/Abaddon1", "Sounds/Cards/StartOrder/AbaddonMistCoil", Color.gray,
                8, 8,
                4, 0, 0,
                0, 0, 0,
                0, 4, true, false));

            CardManagerList.AllCards.Add(new Card("Abaddon", "Aphotic Shield", "Boost allied unit by 2 and give it shield. Shield.",
            "Sprites/Cards/Abaddon2", "Sounds/Cards/StartOrder/AbaddonAphoticShield", Color.gray,
                6, 6,
                2, 0, 0,
                0, 0, 0,
                0, 0, false, false,
                false, 0, 0,
                0, false, 0, 0,
                true, false, false, false, true));

            CardManagerList.AllCards.Add(new Card("Chaos Knight", "Phantasm", "Create 3 your illusion units near.",
                "Sprites/Cards/ChaosKnight1", "Sounds/Cards/StartOrder/ChaosKnightPhantasm", Color.red,
                6, 6,
                0, 0, 0,
                0, 0, 0,
                0, 0, false, false,
                false, 0, 0,
                0, true, -1, 3));

            CardManagerList.AllCards.Add(new Card("Chaos Knight", "Chaos Bolt", "Damage enemy unit by 2, stun him and create 1 your illusion unit near.",
                "Sprites/Cards/ChaosKnight2", "Sounds/Cards/StartOrder/ChaosKnightChaosBolt", Color.red,
                6, 6,
                0, 0, 0,
                2, 0, 0,
                0, 0, false, true,
                false, 0, 0,
                0, true, -1, 1,
                false, true));

            CardManagerList.AllCards.Add(new Card("Lycan", "Summon Wolves", "Create 2 units Wolves near.",
                "Sprites/Cards/Lycan1", "Sounds/Cards/StartOrder/LycanSummonWolves", Color.gray,
                11, 11,
                0, 0, 0,
                0, 0, 0,
                0, 0, false, false,
                false, 0, 0,
                0, true, 0, 2));

            CardManagerList.AllCards.Add(new Card("Lycan", "Feral Impulce", "At the end of the your turn, deal 2 boost to a random allied unit.",
                "Sprites/Cards/Lycan2", "Sounds/Cards/StartOrder/LycanFeralImpulce", Color.gray,
                6, 6,
                0, 0, 0,
                0, 0, 0,
                0, 0, false, false,
                true, 1, 0,
                2));

            CardManagerList.AllCards.Add(new Card("Riki", "Tricks of the Trade", "Damage 3 enemy units by 4. Invisibility. Invulnerability.",
                "Sprites/Cards/Riki1", "Sounds/Cards/StartOrder/RikiTricksOfTheTrade", Color.magenta,
                1, 1,
                0, 0, 0,
                4, 1, 0,
                0, 0, false, false,
                false, 0, 0, 0,
                false, 0, 0,
                false, false, true, true));

            CardManagerList.AllCards.Add(new Card("Riki", "Cloak And Dagger", "Damage enemy unit by 12. Invisibility.",
                "Sprites/Cards/Riki2", "Sounds/Cards/StartOrder/RikiCloakAndDagger", Color.magenta,
                1, 1,
                0, 0, 0,
                12, 0, 0,
                0, 0, false, false,
                false, 0, 0, 0,
                false, 0, 0,
                false, false, true, false));

            CardManagerList.AllCards.Add(new Card("Juggernaut", "Omnislash", "Damage 5 enemy units by 2. Invulnerability.",
                "Sprites/Cards/Juggernaut1", "Sounds/Cards/StartOrder/JuggernautOmnislash", Color.green,
                5, 5,
                0, 0, 0,
                2, 2, 0,
                0, 0, false, false,
                false, 0, 0,
                0, false, 0, 0,
                false, false, false, true));

            CardManagerList.AllCards.Add(new Card("Juggernaut", "BladeDance", "At the end of the your turn, deal 4 damage to a random enemy unit.",
                "Sprites/Cards/Juggernaut2", "Sounds/Cards/StartOrder/JuggernautBladeDance", Color.green,
                1, 1,
                0, 0, 0,
                0, 0, 0,
                0, 0, false, false,
                true, 1, 4));

            CardManagerList.AllCards.Add(new Card("Naga Siren", "Mirror Image", "Create 2 your illusion units copy near.",
                "Sprites/Cards/NagaSiren1", "Sounds/Cards/StartOrder/NagaSirenMirrorImage", Color.cyan,
                7, 7,
                0, 0, 0,
                0, 0, 0,
                0, 0, false, false,
                false, 0, 0,
                0, true, -1, 2));

            CardManagerList.AllCards.Add(new Card("Naga Siren", "Song of the Siren", "Stun all enemy units. Invulnerability.",
                "Sprites/Cards/NagaSiren2", "Sounds/Cards/StartOrder/NagaSirenSongOfTheSiren", Color.cyan,
                8, 8,
                0, 0, 0,
                0, -1, 0,
                0, 0, false, false,
                false, 0, 0,
                0, false, 0, 0,
                false, true, false, true));



            //SUMMONS

            CardManagerList.SummonCards.Add(new Card("Wolf", "Summon", "",
                "Sprites/Cards/Wolf1", "Sounds/Cards/StartOrder/Wolf", Color.gray,
                2, 2));

            CardManagerList.IsAddCardInGame = true;
        }
    }
}
