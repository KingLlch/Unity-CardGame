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

public struct Spawns
{
    public int SpawnCardNumber;
    public int SpawnCardCount;
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
    public Spawns Summons;
    public DrawCard DrawCard;
    public UniqueMechanics UniqueMechanics;
    public StatusEffects StatusEffects;

    public Card(string name, string secondName, string description, string spritePath, string startOrderSoundPath, Color color, int maxPoints, int points,

        int boost = 0, int rangeBoost = 0, int changeBoost = 0, int damage = 0, int rangeDamage = 0, int changeDamage = 0, 
        int selfBoost = 0, int selfDamage = 0, bool addictionWithSelfField = false, bool addictionWithEnemyField = false,

        int endTurnQuantity = 0, int endTurnRandomBoost = 0, int endTurnRandomDamage = 0, 
        int endTurnSelfBoost = 0, int endTurnSelfDamage = 0, int endTurnNearBoost = 0, int endTurnNearDamage = 0,

        int spawnCardCount = 0, int spawnCardNumber = 0, 

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


        Summons = new Spawns();
        Summons.SpawnCardNumber = spawnCardNumber;
        Summons.SpawnCardCount = spawnCardCount;


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

            CardManagerList.AllCards.Add(new Card("Lina", "Dragon Slave", "<color=red>Damage</color> 3 enemy unit by 3.",
                "Sprites/Cards/Lina1", "Sounds/Cards/StartOrder/LinaDragonSlave", Color.red,
                5, 5, damage:3, rangeDamage: 1));

            CardManagerList.AllCards.Add(new Card("Lina", "Light Strike Array", "<color=red>Damage</color> enemy unit by 3 and <color=yellow>stun</color>  its.",
                 "Sprites/Cards/Lina2", "Sounds/Cards/StartOrder/LinaLightStrikeArray", Color.red,
                 6, 6, damage:3, stunOther:true));

            CardManagerList.AllCards.Add(new Card("Lina", "Fiery Soul", "At the end of the your turn, deal 1 <color=red>damage</color> to a random enemy unit 3 <color=red>times</color>.",
                 "Sprites/Cards/Lina3", "Sounds/Cards/StartOrder/LinaFierySoul", Color.red,
                 2, 2, endTurnQuantity:3, endTurnRandomDamage:1));

            CardManagerList.AllCards.Add(new Card("Lina", "Laguna Blade", "<color=red>Destroy</color> an enemy unit.",
                 "Sprites/Cards/Lina4", "Sounds/Cards/StartOrder/LinaLagunaBlade", Color.red,
                 1, 1, destroyCardPoints: -1));

            CardManagerList.AllCards.Add(new Card("Luna", "Lucent Beam", "<color=red>Damage</color> enemy unit by 2 and <color=yellow>stun</color> it.",
                "Sprites/Cards/Luna1", "Sounds/Cards/StartOrder/LunaLucentBeam", Color.blue,
                7, 7, damage:2, stunOther:true));

            CardManagerList.AllCards.Add(new Card("Luna", "Moon Glaives", "At the end of the your turn, deal 1 <color=red>damage</color> to a random enemy unit 2 <color=red>times</color>.",
                "Sprites/Cards/Luna2", "Sounds/Cards/StartOrder/LunaMoonGlaives", Color.blue,
                 6, 6, endTurnQuantity:2, endTurnRandomDamage:1));

            CardManagerList.AllCards.Add(new Card("Luna", "Eclipse", "<color=red>Damage</color> 5 enemy units by 2.",
                "Sprites/Cards/Luna3", "Sounds/Cards/StartOrder/LunaEclipse", Color.blue,
                8, 8, damage: 2, rangeDamage: 2)) ;

            CardManagerList.AllCards.Add(new Card("Templar Assasin", "Refraction", "<color=green>Boost</color> allied unit by 8. <color=red>Shield</color>.",
                 "Sprites/Cards/TemplarAssasin1", "Sounds/Cards/StartOrder/TemplarAssasinRefraction", Color.magenta,
                 3, 3, boost: 8, isSelfShielded: true));

            CardManagerList.AllCards.Add(new Card("Templar Assasin", "Meld", "<color=red>Damage</color> enemy unit by 5. <color=red>Shield</color>.",
                "Sprites/Cards/TemplarAssasin2", "Sounds/Cards/StartOrder/TemplarAssasinMeld", Color.magenta,
                7, 7, damage: 5, isSelfShielded: true));

            CardManagerList.AllCards.Add(new Card("Axe", "Berserker's Call", "<color=green>Boost</color> enemy unit by 3.",
                 "Sprites/Cards/Axe1", "Sounds/Cards/StartOrder/AxeBerserker'sCall", Color.red,
                 14, 13, damage: -3));

            CardManagerList.AllCards.Add(new Card("Axe", "CullingBlade", "<color=red>Destroy</color> an enemy unit with 4 or less points.",
                "Sprites/Cards/Axe3", "Sounds/Cards/StartOrder/AxeCullingBlade", Color.red,
                14, 13, destroyCardPoints:4));

            CardManagerList.AllCards.Add(new Card("Centaur Warrunner", "Double Edge", "<color=red>Damage</color> self by 3.",
                 "Sprites/Cards/Centaur1", "Sounds/Cards/StartOrder/CentaurDoubleEdge", Color.gray,
                 20, 16, selfDamage:3));

            CardManagerList.AllCards.Add(new Card("Centaur Warrunner", "Retaliate", "Whenever this unit takes damage, damage to the card that dealt 1 damage",
                "Sprites/Cards/Centaur2", "Sounds/Cards/StartOrder/CentaurRetaliate", Color.gray,
                10, 8, returnDamageValue: 1));

            CardManagerList.AllCards.Add(new Card("Huskar", "Life Break", "<color=red>Damage</color> self and enemy unit by 7.",
                "Sprites/Cards/Huskar1", "Sounds/Cards/StartOrder/HuskarLifeBreak", Color.yellow,
                19, 17, damage: 7, selfDamage: 7, addictionWithEnemyField: true));

            CardManagerList.AllCards.Add(new Card("Windranger", "Powershot", "<color=red>Damage</color> enemy unit by 5 and units near by 1.",
                "Sprites/Cards/Windranger1", "Sounds/Cards/StartOrder/WindrangerPowershot", Color.green,
                4, 4, damage: 5, rangeDamage: 1, changeDamage: -4));

            CardManagerList.AllCards.Add(new Card("Windranger", "Windrun", "<color=red>Draw</color> 1 card",
                "Sprites/Cards/Windranger2", "Sounds/Cards/StartOrder/WindrangerWindrun", Color.green,
                3, 3, drawCardCount: 1));

            CardManagerList.AllCards.Add(new Card("Kunkka", "Tidebringer", "<color=red>Damage</color> enemy unit by 1, and units near by 5.",
                "Sprites/Cards/Kunkka1", "Sounds/Cards/StartOrder/KunkkaTidebringer", Color.blue,
                5, 5, damage: 1, rangeDamage: 1, changeDamage: 4));

            CardManagerList.AllCards.Add(new Card("Earthshaker", "Fissure", "<color=red>Damage</color> 3 enemy units by 3 then <color=yellow>stun</color> them.",
                "Sprites/Cards/Earthshaker1", "Sounds/Cards/StartOrder/EarthshakerFissure", Color.yellow,
                6, 6, damage: 3, rangeDamage: 1, stunOther: true));

            CardManagerList.AllCards.Add(new Card("Earthshaker", "Echo Slam", "<color=red>Damage</color> all enemy units by 2.",
                "Sprites/Cards/Earthshaker2", "Sounds/Cards/StartOrder/EarthshakerEchoSlam", Color.yellow,
                4, 4, damage: 2, rangeDamage: -1));

            CardManagerList.AllCards.Add(new Card("Chen", "Hand Of God", "<color=green>Boost</color> all allied units by 1 and give them 2 <color=green>endurance</color>.",
                "Sprites/Cards/Chen1", "Sounds/Cards/StartOrder/ChenHandOfGod", Color.yellow,
                3, 3, boost: 1, rangeBoost: -1, enduranceOrBleedingOther:2, isEnemyTargetEnduranceOrBleeding: false));

            CardManagerList.AllCards.Add(new Card("Chen", "Divine Favor", "At the end of the your turn, deal 1 <color=green>boost</color> to a random 2 allied units.",
                "Sprites/Cards/Chen2", "Sounds/Cards/StartOrder/ChenDivineFavor", Color.yellow,
                5, 5, endTurnQuantity: 2, endTurnRandomBoost: 1));

            CardManagerList.AllCards.Add(new Card("Sniper", "Assasinate", "<color=red>Damage</color> enemy unit by 17.",
                "Sprites/Cards/Sniper1", "Sounds/Cards/StartOrder/SniperAssasinate", Color.yellow,
                3, 3, damage: 17));

            CardManagerList.AllCards.Add(new Card("Bane", "BrainSap", "<color=red>Damage</color> enemy unit and <color=green>boost</color> self by 4.",
                "Sprites/Cards/Bane1", "Sounds/Cards/StartOrder/BaneBrainSap", Color.black,
                4, 4, damage: 4, selfBoost: 4, addictionWithEnemyField: true));

            CardManagerList.AllCards.Add(new Card("Zeus", "Arc Lightning", "<color=red>Damage</color> enemy unit by 2, and near units by 1.",
                "Sprites/Cards/Zeus1", "Sounds/Cards/StartOrder/ZeusArcLightning", Color.blue,
                3, 3, damage: 2, rangeDamage: 1, changeDamage: -1));

            CardManagerList.AllCards.Add(new Card("Zeus", "Lightning Bolt", "<color=red>Damage</color> enemy unit by 5.",
                "Sprites/Cards/Zeus2", "Sounds/Cards/StartOrder/ZeusLightningBolt", Color.blue,
                3, 3, damage: 5));

            CardManagerList.AllCards.Add(new Card("Abaddon", "Mist Coil", "<color=green>Boost</color> allied unit by 8 and <color=red>damage</color> self by 4.",
                "Sprites/Cards/Abaddon1", "Sounds/Cards/StartOrder/AbaddonMistCoil", Color.gray,
                8, 8, boost: 8, selfDamage: 4, addictionWithSelfField: true));

            CardManagerList.AllCards.Add(new Card("Abaddon", "Aphotic Shield", "<color=green>Boost</color> allied unit by 2 and give it <color=red>shield</color>. <color=red>Shield</color>.",
            "Sprites/Cards/Abaddon2", "Sounds/Cards/StartOrder/AbaddonAphoticShield", Color.gray,
                6, 6, boost: 2, shieldOther: true));

            CardManagerList.AllCards.Add(new Card("Abaddon", "BorrowedTime", "Whenever this unit takes <color=red>damage</color>, <color=green>boost</color> him by damage value",
            "Sprites/Cards/Abaddon3", "Sounds/Cards/StartOrder/AbaddonBorrowedTime", Color.gray,
                 2, 2, healDamageValue: -1));

            CardManagerList.AllCards.Add(new Card("Chaos Knight", "Phantasm", "<color=blue>Spawn</color> 3 your <color=blue>illusions</color> units near.",
                "Sprites/Cards/ChaosKnight1", "Sounds/Cards/StartOrder/ChaosKnightPhantasm", Color.red,
                6, 6, spawnCardCount: 3, spawnCardNumber: -1));

            CardManagerList.AllCards.Add(new Card("Chaos Knight", "Chaos Bolt", "<color=red>Damage</color> enemy unit by 2, <color=yellow>stun</color> him and <color=blue>spawn</color> 1 your <color=blue>illusion</color> unit near.",
                "Sprites/Cards/ChaosKnight2", "Sounds/Cards/StartOrder/ChaosKnightChaosBolt", Color.red,
                6, 6, damage:2,addictionWithEnemyField: true, spawnCardCount: 1, spawnCardNumber: -1, stunOther: true));

            CardManagerList.AllCards.Add(new Card("Lycan", "Summon Wolves", "<color=blue>Spawn</color> 2 units Wolves near.",
                "Sprites/Cards/Lycan1", "Sounds/Cards/StartOrder/LycanSummonWolves", Color.gray,
                11, 11, spawnCardCount: 2, spawnCardNumber: 0));

            CardManagerList.AllCards.Add(new Card("Lycan", "Feral Impulce", "At the end of the your turn, deal 2 <color=green>boost</color> to a random allied unit.",
                "Sprites/Cards/Lycan2", "Sounds/Cards/StartOrder/LycanFeralImpulce", Color.gray,
                6, 6, endTurnQuantity: 1, endTurnRandomBoost: 2));

            CardManagerList.AllCards.Add(new Card("Lycan", "Shapeshift", "<color=red>Transformate</color> this unit to Lycan Wolf ",
                "Sprites/Cards/Lycan3", "Sounds/Cards/StartOrder/LycanShapeshift", Color.gray,
                1, 1, transformationNumber: 0));

            CardManagerList.AllCards.Add(new Card("Riki", "Tricks of the Trade", "<color=red>Damage</color> 3 enemy units by 4. <color=purple>Invisibility</color>. <color=orange>Invulnerability</color>.",
                "Sprites/Cards/Riki1", "Sounds/Cards/StartOrder/RikiTricksOfTheTrade", Color.magenta,
                1, 1, damage: 4, rangeDamage: 1, invisibility: true, invulnerability: true));

            CardManagerList.AllCards.Add(new Card("Riki", "Cloak And Dagger", "<color=red>Damage</color> enemy unit by 12. <color=purple>Invisibility</color>.",
                "Sprites/Cards/Riki2", "Sounds/Cards/StartOrder/RikiCloakAndDagger", Color.magenta,
                1, 1, damage: 12, invisibility: true));

            CardManagerList.AllCards.Add(new Card("Juggernaut", "Omnislash", "<color=red>Damage</color> 5 enemy units by 2. <color=orange>Invulnerability</color>.",
                "Sprites/Cards/Juggernaut1", "Sounds/Cards/StartOrder/JuggernautOmnislash", Color.green,
                5, 5, damage: 2, rangeDamage: 2, invulnerability: true));

            CardManagerList.AllCards.Add(new Card("Juggernaut", "Blade Dance", "At the end of the your turn, deal 4 <color=red>damage</color> to a random enemy unit 1 <color=red>times</color>.",
                "Sprites/Cards/Juggernaut2", "Sounds/Cards/StartOrder/JuggernautBladeDance", Color.green,
                1, 1, endTurnQuantity: 1, endTurnRandomDamage: 4));

            CardManagerList.AllCards.Add(new Card("Juggernaut", "Healing Ward", "<color=blue>Spawn</color> Healing Ward to the left of this unit.",
                "Sprites/Cards/Juggernaut3", "Sounds/Cards/StartOrder/JuggernautHealingWard", Color.green,
                 3, 3, spawnCardCount:1, spawnCardNumber:1));

            CardManagerList.AllCards.Add(new Card("Naga Siren", "Mirror Image", "<color=blue>Spawn</color> 2 your <color=blue>illusions</color> units near.",
                "Sprites/Cards/NagaSiren1", "Sounds/Cards/StartOrder/NagaSirenMirrorImage", Color.cyan,
                7, 7, spawnCardCount: 2, spawnCardNumber: -1));

            CardManagerList.AllCards.Add(new Card("Naga Siren", "Song of the Siren", "<color=yellow>Stun</color> all enemy units. <color=orange>Invulnerability</color>.",
                "Sprites/Cards/NagaSiren2", "Sounds/Cards/StartOrder/NagaSirenSongOfTheSiren", Color.cyan,
                8, 8, rangeDamage: -1, stunOther: true, invulnerability: true));

            CardManagerList.AllCards.Add(new Card("Terrorblade", "Conjure Image", "<color=blue>Spawn</color> 1 your <color=blue>illusion</color> unit near. At the end of the your turn, deal 1 damage to a random enemy unit 1 <color=red>times</color>.",
                "Sprites/Cards/Terrorblade1", "Sounds/Cards/StartOrder/TerrorbladeConjureImage", Color.blue,
                3, 3, endTurnQuantity:1, endTurnRandomDamage:1 ,spawnCardCount:1, spawnCardNumber:-1));

            CardManagerList.AllCards.Add(new Card("Terrorblade", "Sunder", "<color=red>Swap points</color> with unit.",
                "Sprites/Cards/Terrorblade2", "Sounds/Cards/StartOrder/TerrorbladeSunder", Color.blue,
                8, 8, swapPoints:true));

            CardManagerList.AllCards.Add(new Card("Bloodseeker", "Bloodrage", "Give self 3 <color=red>bleeding</color>.",
                "Sprites/Cards/Bloodseeker1", "Sounds/Cards/StartOrder/BloodseekerBloodrage", Color.red,
                11, 11, enduranceOrBleedingSelf:-3));

            CardManagerList.AllCards.Add(new Card("Bloodseeker", "Rupture", "Give an enemy unit 7 <color=red>bleeding</color>.",
                "Sprites/Cards/Bloodseeker2", "Sounds/Cards/StartOrder/BloodseekerRupture", Color.red,
                8, 8, enduranceOrBleedingOther: -7, isEnemyTargetEnduranceOrBleeding: true));


            //Transformation

            CardManagerList.TransformationCards.Add(new Card("Lycan", "Wolf Form", "At the end of the your turn, deal 2 <color=red>Damage</color> near units.",
                "Sprites/Cards/LycanWolf", "Sounds/Cards/StartOrder/LycanWolf", Color.gray,
                20, 20, endTurnQuantity:1, endTurnNearDamage:2));

            //SUMMONS

            CardManagerList.SummonCards.Add(new Card("Wolf", "Summon", "Nothing",
                "Sprites/Cards/Wolf", "Sounds/Cards/StartOrder/Wolf", Color.gray,
                2, 2));

            CardManagerList.SummonCards.Add(new Card("HealingWard", "Ward", "At the end of the your turn, <color=green>boost</color> 2 near units by 2",
                "Sprites/Cards/HealingWard", "Sounds/Cards/StartOrder/HealingWard", Color.green,
                1, 1, endTurnQuantity:1, endTurnNearBoost:2));

            CardManagerList.IsAddCardInGame = true;
        }
    }
}
