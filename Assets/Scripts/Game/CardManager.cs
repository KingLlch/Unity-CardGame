using System.Collections.Generic;
using UnityEngine;

public struct BaseCard
{
    public string Name;
    public string AbilityName;
    public string Description;

    public int MaxPoints;
    public int Points;

    public Texture ImageTexture;
    public Sprite Sprite;

    public AudioClip CardPlaySound;
    public Color ColorTheme;

    public bool isDestroyed;
}

public struct BoostOrDamage
{
    public int Boost;
    public int NearBoost;
    public int ChangeNearBoost;
    public int SelfBoost;

    public int Damage;
    public int NearDamage;
    public int ChangeNearDamage;
    public int SelfDamage;

    public bool AddictionWithAlliedField;
    public bool AddictionWithEnemyField;
}

public struct EndTurnActions
{
    public int EndTurnActionCount;

    public int EndTurnRandomBoost;
    public int EndTurnRandomDamage;

    public int EndTurnSelfBoost;
    public int EndTurnSelfDamage;

    public int EndTurnNearBoost;
    public int EndTurnNearDamage;
}

public struct Summons
{
    public int SummonCardNumber;
    public int SummonCardCount;
}

public struct DrawCard
{
    public int DrawCardCount;
}

public struct StatusEffects
{
    public bool IsShieldOther;
    public bool IsIllusion;
    public bool IsInvisibility;
    public bool IsStunOther;
    public bool IsInvulnerability;

    public bool IsSelfShielded;
    public bool IsSelfStunned;

    public int EnduranceOrBleedingOther;
    public int SelfEnduranceOrBleeding;
    public bool IsEnemyTargetEnduranceOrBleeding;
}

public struct UniqueMechanics
{
    public int DestroyCardPoints;

    public bool SwapPoints;

    public int TransformationNumber;

    public int ReturnDamageValue;

    public int HealDamageValue;
}


public struct Card
{
    public BaseCard BaseCard;
    public BoostOrDamage BoostOrDamage;
    public EndTurnActions EndTurnActions;
    public Summons Summons;
    public DrawCard DrawCard;
    public UniqueMechanics UniqueMechanics;
    public StatusEffects StatusEffects;

    public Card(string name, string secondName, string description, string spritePath, string startOrderSoundPath, Color color, int maxPoints, int points,

        int boost = 0, int rangeBoost = 0, int changeBoost = 0, int damage = 0, int rangeDamage = 0, int changeDamage = 0, 
        int selfBoost = 0, int selfDamage = 0, bool addictionWithSelfField = false, bool addictionWithEnemyField = false,

        int endTurnQuantity = 0, int endTurnRandomBoost = 0, int endTurnRandomDamage = 0, 
        int endTurnSelfBoost = 0, int endTurnSelfDamage = 0, int endTurnNearBoost = 0, int endTurnNearDamage = 0,

        int summonCardCount = 0, int summonCardNumber = 0, 

        int drawCardCount = 0,

        bool shieldOther = false, bool stunOther = false, bool invisibility = false, bool invulnerability = false, bool isSelfShielded = false,
        int enduranceOrBleedingSelf = 0, int enduranceOrBleedingOther = 0, bool isEnemyTargetEnduranceOrBleeding = false,

        int destroyCardPoints = 0, bool swapPoints = false, int transformationNumber = -1, int returnDamageValue = 0, int healDamageValue = 0)

    {

        BaseCard = new BaseCard();
        BaseCard.Name = name;
        BaseCard.AbilityName = secondName;
        BaseCard.Description = description;
        BaseCard.MaxPoints = maxPoints;
        BaseCard.Points = points;
        BaseCard.ImageTexture = Resources.Load<Texture>(spritePath);
        BaseCard.Sprite = Resources.Load<Sprite>(spritePath);
        BaseCard.CardPlaySound = Resources.Load<AudioClip>(startOrderSoundPath);
        BaseCard.ColorTheme = color;


        BoostOrDamage = new BoostOrDamage();
        BoostOrDamage.Boost = boost;
        BoostOrDamage.NearBoost = rangeBoost;
        BoostOrDamage.ChangeNearBoost = changeBoost;
        BoostOrDamage.SelfBoost = selfBoost;

        BoostOrDamage.Damage = damage;
        BoostOrDamage.NearDamage = rangeDamage;
        BoostOrDamage.ChangeNearDamage = changeDamage;
        BoostOrDamage.SelfDamage = selfDamage;

        BoostOrDamage.AddictionWithAlliedField = addictionWithSelfField;
        BoostOrDamage.AddictionWithEnemyField = addictionWithEnemyField;


        EndTurnActions = new EndTurnActions();
        EndTurnActions.EndTurnActionCount = endTurnQuantity;
        EndTurnActions.EndTurnRandomDamage = endTurnRandomDamage;
        EndTurnActions.EndTurnRandomBoost = endTurnRandomBoost;

        EndTurnActions.EndTurnSelfBoost = endTurnSelfBoost;
        EndTurnActions.EndTurnSelfDamage = endTurnSelfDamage;
        EndTurnActions.EndTurnNearBoost = endTurnNearBoost;
        EndTurnActions.EndTurnNearDamage = endTurnNearDamage;


        Summons = new Summons();
        Summons.SummonCardNumber = summonCardNumber;
        Summons.SummonCardCount = summonCardCount;


        DrawCard = new DrawCard();
        DrawCard.DrawCardCount = drawCardCount;


        StatusEffects = new StatusEffects();
        StatusEffects.IsShieldOther = shieldOther;
        StatusEffects.IsStunOther = stunOther;
        StatusEffects.IsInvisibility = invisibility;
        StatusEffects.IsInvulnerability = invulnerability;
        StatusEffects.IsSelfShielded = isSelfShielded;

        StatusEffects.SelfEnduranceOrBleeding = enduranceOrBleedingSelf;
        StatusEffects.EnduranceOrBleedingOther = enduranceOrBleedingOther;
        StatusEffects.IsEnemyTargetEnduranceOrBleeding = isEnemyTargetEnduranceOrBleeding;


        UniqueMechanics = new UniqueMechanics();
        UniqueMechanics.DestroyCardPoints = destroyCardPoints;
        UniqueMechanics.SwapPoints = swapPoints;
        UniqueMechanics.TransformationNumber = transformationNumber;
        UniqueMechanics.ReturnDamageValue = returnDamageValue;
        UniqueMechanics.HealDamageValue = healDamageValue;
    }
}

public static class CardManagerList
{
    public static bool IsAddCardInGame;

    public static List<Card> AllCards = new List<Card>();
    public static List<Card> TransformationCards = new List<Card>();
    public static List<Card> SummonCards = new List<Card>();

    public static List<Card> DebugCards = new List<Card>();
}

public class CardManager : MonoBehaviour
{


    private void Awake()
    {
        if (!CardManagerList.IsAddCardInGame)
        {
            //Debug

            CardManagerList.DebugCards.Add(new Card("Debug", "Debug", "Debug",
                "Sprites/Cards/Lina1", "Sounds/Cards/StartOrder/LinaDragonSlave", Color.white,
                10, 10));

            //Game

            CardManagerList.AllCards.Add(new Card("Lina", "Dragon Slave", "Damage 3 enemy unit by 3.",
                "Sprites/Cards/Lina1", "Sounds/Cards/StartOrder/LinaDragonSlave", Color.red,
                5, 5, damage:3, rangeDamage: 1));

            CardManagerList.AllCards.Add(new Card("Lina", "Light Strike Array", "Damage enemy unit by 3 and stun its.",
                 "Sprites/Cards/Lina2", "Sounds/Cards/StartOrder/LinaLightStrikeArray", Color.red,
                 6, 6, damage:3, stunOther:true));

            CardManagerList.AllCards.Add(new Card("Lina", "Fiery Soul", "At the end of the your turn, deal 1 damage to a random enemy unit 3 times.",
                 "Sprites/Cards/Lina3", "Sounds/Cards/StartOrder/LinaFierySoul", Color.red,
                 2, 2, endTurnQuantity:3, endTurnRandomDamage:1));

            CardManagerList.AllCards.Add(new Card("Lina", "Laguna Blade", "Destroy an enemy unit.",
                 "Sprites/Cards/Lina4", "Sounds/Cards/StartOrder/LinaLagunaBlade", Color.red,
                 1, 1, destroyCardPoints: -1));

            CardManagerList.AllCards.Add(new Card("Luna", "Lucent Beam", "Damage enemy unit by 2 and stun it.",
                "Sprites/Cards/Luna1", "Sounds/Cards/StartOrder/LunaLucentBeam", Color.blue,
                7, 7, damage:2, stunOther:true));

            CardManagerList.AllCards.Add(new Card("Luna", "Moon Glaives", "At the end of the your turn, deal 1 damage to a random enemy unit 2 times.",
                "Sprites/Cards/Luna2", "Sounds/Cards/StartOrder/LunaMoonGlaives", Color.blue,
                 6, 6, endTurnQuantity:2, endTurnRandomDamage:1));

            CardManagerList.AllCards.Add(new Card("Luna", "Eclipse", "Damage 5 enemy units by 2.",
                "Sprites/Cards/Luna3", "Sounds/Cards/StartOrder/LunaEclipse", Color.blue,
                8, 8, damage: 2, rangeDamage: 2)) ;

            CardManagerList.AllCards.Add(new Card("Templar Assasin", "Refraction", "Boost allied unit by 8. Shield.",
                 "Sprites/Cards/TemplarAssasin1", "Sounds/Cards/StartOrder/TemplarAssasinRefraction", Color.magenta,
                 3, 3, boost: 8, isSelfShielded: true));

            CardManagerList.AllCards.Add(new Card("Templar Assasin", "Meld", "Damage enemy unit by 5. Shield.",
                "Sprites/Cards/TemplarAssasin2", "Sounds/Cards/StartOrder/TemplarAssasinMeld", Color.magenta,
                7, 7, damage: 5, isSelfShielded: true));

            CardManagerList.AllCards.Add(new Card("Axe", "Berserker's Call", "Boost enemy unit by 3.",
                 "Sprites/Cards/Axe1", "Sounds/Cards/StartOrder/AxeBerserker'sCall", Color.red,
                 14, 13, damage: -3));

            CardManagerList.AllCards.Add(new Card("Axe", "CullingBlade", "Destroy an enemy unit with 4 or less points.",
                "Sprites/Cards/Axe3", "Sounds/Cards/StartOrder/AxeCullingBlade", Color.red,
                14, 13, destroyCardPoints:4));

            CardManagerList.AllCards.Add(new Card("Centaur Warrunner", "Double Edge", "Damage self by 3.",
                 "Sprites/Cards/Centaur1", "Sounds/Cards/StartOrder/CentaurDoubleEdge", Color.gray,
                 20, 16, selfDamage:3));

            CardManagerList.AllCards.Add(new Card("Centaur Warrunner", "Retaliate", "Whenever this unit takes damage, damage to the card that dealt 1 damage",
                "Sprites/Cards/Centaur2", "Sounds/Cards/StartOrder/CentaurRetaliate", Color.gray,
                10, 8, returnDamageValue: 1));

            CardManagerList.AllCards.Add(new Card("Huskar", "Life Break", "Damage self and enemy unit by 7.",
                "Sprites/Cards/Huskar1", "Sounds/Cards/StartOrder/HuskarLifeBreak", Color.yellow,
                19, 17, damage: 7, selfDamage: 7, addictionWithEnemyField: true));

            CardManagerList.AllCards.Add(new Card("Windranger", "Powershot", "Damage enemy unit by 5 and units near by 1.",
                "Sprites/Cards/Windranger1", "Sounds/Cards/StartOrder/WindrangerPowershot", Color.green,
                4, 4, damage: 5, rangeDamage: 1, changeDamage: -4));

            CardManagerList.AllCards.Add(new Card("Windranger", "Windrun", "Draw 1 card",
                "Sprites/Cards/Windranger2", "Sounds/Cards/StartOrder/WindrangerWindrun", Color.green,
                3, 3, drawCardCount: 1));

            CardManagerList.AllCards.Add(new Card("Kunkka", "Tidebringer", "Damage enemy unit by 1, and units near by 5.",
                "Sprites/Cards/Kunkka1", "Sounds/Cards/StartOrder/KunkkaTidebringer", Color.blue,
                5, 5, damage: 1, rangeDamage: 1, changeDamage: 4));

            CardManagerList.AllCards.Add(new Card("Earthshaker", "Fissure", "Damage 3 enemy units by 3 then stun them.",
                "Sprites/Cards/Earthshaker1", "Sounds/Cards/StartOrder/EarthshakerFissure", Color.yellow,
                6, 6, damage: 3, rangeDamage: 1, stunOther: true));

            CardManagerList.AllCards.Add(new Card("Earthshaker", "Echo Slam", "Damage all enemy units by 2.",
                "Sprites/Cards/Earthshaker2", "Sounds/Cards/StartOrder/EarthshakerEchoSlam", Color.yellow,
                4, 4, damage: 2, rangeDamage: -1));

            CardManagerList.AllCards.Add(new Card("Chen", "Hand Of God", "Give all allied units 2 endurance.",
                "Sprites/Cards/Chen1", "Sounds/Cards/StartOrder/ChenHandOfGod", Color.yellow,
                3, 3, boost: 0, rangeBoost: -1, enduranceOrBleedingOther:2, isEnemyTargetEnduranceOrBleeding: false));

            CardManagerList.AllCards.Add(new Card("Chen", "Divine Favor", "At the end of the your turn, deal 1 boost to a random 2 allied units.",
                "Sprites/Cards/Chen2", "Sounds/Cards/StartOrder/ChenDivineFavor", Color.yellow,
                5, 5, endTurnQuantity: 2, endTurnRandomBoost: 1));

            CardManagerList.AllCards.Add(new Card("Sniper", "Assasinate", "Damage enemy unit by 17.",
                "Sprites/Cards/Sniper1", "Sounds/Cards/StartOrder/SniperAssasinate", Color.yellow,
                3, 3, damage: 17));

            CardManagerList.AllCards.Add(new Card("Bane", "BrainSap", "Damage enemy unit and boost self by 4.",
                "Sprites/Cards/Bane1", "Sounds/Cards/StartOrder/BaneBrainSap", Color.black,
                4, 4, damage: 4, selfBoost: 4, addictionWithEnemyField: true));

            CardManagerList.AllCards.Add(new Card("Zeus", "Arc Lightning", "Damage enemy unit by 2, and near units by 1.",
                "Sprites/Cards/Zeus1", "Sounds/Cards/StartOrder/ZeusArcLightning", Color.blue,
                3, 3, damage: 2, rangeDamage: 1, changeDamage: -1));

            CardManagerList.AllCards.Add(new Card("Zeus", "Lightning Bolt", "Damage enemy unit by 5.",
                "Sprites/Cards/Zeus2", "Sounds/Cards/StartOrder/ZeusLightningBolt", Color.blue,
                3, 3, damage: 5));

            CardManagerList.AllCards.Add(new Card("Abaddon", "Mist Coil", "Boost allied unit by 8 and damage self by 4.",
                "Sprites/Cards/Abaddon1", "Sounds/Cards/StartOrder/AbaddonMistCoil", Color.gray,
                8, 8, boost: 8, selfDamage: 4, addictionWithSelfField: true));

            CardManagerList.AllCards.Add(new Card("Abaddon", "Aphotic Shield", "Boost allied unit by 2 and give it shield. Shield.",
            "Sprites/Cards/Abaddon2", "Sounds/Cards/StartOrder/AbaddonAphoticShield", Color.gray,
                6, 6, boost: 2, shieldOther: true));

            CardManagerList.AllCards.Add(new Card("Abaddon", "BorrowedTime", "Whenever this unit takes damage, boost him by damage value",
            "Sprites/Cards/Abaddon3", "Sounds/Cards/StartOrder/AbaddonBorrowedTime", Color.gray,
                 2, 2, healDamageValue: -1));

            CardManagerList.AllCards.Add(new Card("Chaos Knight", "Phantasm", "Spawn 3 your illusion units near.",
                "Sprites/Cards/ChaosKnight1", "Sounds/Cards/StartOrder/ChaosKnightPhantasm", Color.red,
                6, 6, summonCardCount: 3, summonCardNumber: -1));

            CardManagerList.AllCards.Add(new Card("Chaos Knight", "Chaos Bolt", "Damage enemy unit by 2, stun him and spawn 1 your illusion unit near.",
                "Sprites/Cards/ChaosKnight2", "Sounds/Cards/StartOrder/ChaosKnightChaosBolt", Color.red,
                6, 6, damage:2,addictionWithEnemyField: true, summonCardCount: 1, summonCardNumber: -1, stunOther: true));

            CardManagerList.AllCards.Add(new Card("Lycan", "Summon Wolves", "Spawn 2 units Wolves near.",
                "Sprites/Cards/Lycan1", "Sounds/Cards/StartOrder/LycanSummonWolves", Color.gray,
                11, 11, summonCardCount: 2, summonCardNumber: 0));

            CardManagerList.AllCards.Add(new Card("Lycan", "Feral Impulce", "At the end of the your turn, deal 2 boost to a random allied unit.",
                "Sprites/Cards/Lycan2", "Sounds/Cards/StartOrder/LycanFeralImpulce", Color.gray,
                6, 6, endTurnQuantity: 1, endTurnRandomBoost: 2));

            CardManagerList.AllCards.Add(new Card("Lycan", "Shape Shift", "Transformation this unit to Lycan Wolf ",
                "Sprites/Cards/Lycan3", "Sounds/Cards/StartOrder/LycanShapeShift", Color.gray,
                1, 1, transformationNumber: 0));

            CardManagerList.AllCards.Add(new Card("Riki", "Tricks of the Trade", "Damage 3 enemy units by 4. Invisibility. Invulnerability.",
                "Sprites/Cards/Riki1", "Sounds/Cards/StartOrder/RikiTricksOfTheTrade", Color.magenta,
                1, 1, damage: 4, rangeDamage: 1, invisibility: true, invulnerability: true));

            CardManagerList.AllCards.Add(new Card("Riki", "Cloak And Dagger", "Damage enemy unit by 12. Invisibility.",
                "Sprites/Cards/Riki2", "Sounds/Cards/StartOrder/RikiCloakAndDagger", Color.magenta,
                1, 1, damage: 12, invisibility: true));

            CardManagerList.AllCards.Add(new Card("Juggernaut", "Omnislash", "Damage 5 enemy units by 2. Invulnerability.",
                "Sprites/Cards/Juggernaut1", "Sounds/Cards/StartOrder/JuggernautOmnislash", Color.green,
                5, 5, damage: 2, rangeDamage: 2, invulnerability: true));

            CardManagerList.AllCards.Add(new Card("Juggernaut", "Blade Dance", "At the end of the your turn, deal 4 damage to a random enemy unit.",
                "Sprites/Cards/Juggernaut2", "Sounds/Cards/StartOrder/JuggernautBladeDance", Color.green,
                1, 1, endTurnQuantity: 1, endTurnRandomDamage: 4));

            CardManagerList.AllCards.Add(new Card("Juggernaut", "Healing Ward", "Spawn Healing Ward to the left of this unit.",
                "Sprites/Cards/Juggernaut3", "Sounds/Cards/StartOrder/JuggernautHealingWard", Color.green,
                 3, 3, summonCardCount:1, summonCardNumber:1));

            CardManagerList.AllCards.Add(new Card("Naga Siren", "Mirror Image", "Spawn 2 your illusion units copy near.",
                "Sprites/Cards/NagaSiren1", "Sounds/Cards/StartOrder/NagaSirenMirrorImage", Color.cyan,
                7, 7, summonCardCount: 2, summonCardNumber: -1));

            CardManagerList.AllCards.Add(new Card("Naga Siren", "Song of the Siren", "Stun all enemy units. Invulnerability.",
                "Sprites/Cards/NagaSiren2", "Sounds/Cards/StartOrder/NagaSirenSongOfTheSiren", Color.cyan,
                8, 8, rangeDamage: -1, stunOther: true, invulnerability: true));

            CardManagerList.AllCards.Add(new Card("Terrorblade", "Conjure Image", "Spawn 1 your illusion unit copy near. At the end of the your turn, deal 1 damage to a random enemy unit.",
                "Sprites/Cards/Terrorblade1", "Sounds/Cards/StartOrder/TerrorbladeConjureImage", Color.blue,
                3, 3, endTurnQuantity:1, endTurnRandomDamage:1 ,summonCardCount:1, summonCardNumber:-1));

            CardManagerList.AllCards.Add(new Card("Terrorblade", "Sunder", "Swap points with enemy unit.",
                "Sprites/Cards/Terrorblade2", "Sounds/Cards/StartOrder/TerrorbladeSunder", Color.blue,
                8, 8, swapPoints:true));

            CardManagerList.AllCards.Add(new Card("Bloodseeker", "Bloodrage", "Give self 3 bleeding.",
                "Sprites/Cards/Bloodseeker1", "Sounds/Cards/StartOrder/BloodseekerBloodrage", Color.red,
                11, 11, enduranceOrBleedingSelf:-3));

            CardManagerList.AllCards.Add(new Card("Bloodseeker", "Rupture", "Give an enemy unit 7 bleeding.",
                "Sprites/Cards/Bloodseeker2", "Sounds/Cards/StartOrder/BloodseekerRupture", Color.red,
                8, 8, enduranceOrBleedingOther: -7, isEnemyTargetEnduranceOrBleeding: true));


            //Transformation

            CardManagerList.TransformationCards.Add(new Card("Lycan", "Wolf Form", "At the end of the your turn, deal 2 Damage near units.",
                "Sprites/Cards/LycanWolf", "Sounds/Cards/StartOrder/LycanWolf", Color.gray,
                20, 20, endTurnQuantity:1, endTurnNearDamage:2));

            //SUMMONS

            CardManagerList.SummonCards.Add(new Card("Wolf", "Summon", "Nothing",
                "Sprites/Cards/Wolf", "Sounds/Cards/StartOrder/Wolf", Color.gray,
                2, 2));

            CardManagerList.SummonCards.Add(new Card("HealingWard", "Ward", "At the end of the your turn, boost 2 near units by 2",
                "Sprites/Cards/HealingWard", "Sounds/Cards/StartOrder/HealingWard", Color.green,
                1, 1, endTurnQuantity:1, endTurnNearBoost:2));

            CardManagerList.IsAddCardInGame = true;
        }
    }
}
