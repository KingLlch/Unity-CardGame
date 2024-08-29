using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public struct BaseCard
{
    public string Name;
    public string AbilityName;

    public string DescriptionEng;
    public string DescriptionRu;
    public string DescriptionUk;

    public int MaxPoints;
    public int Points;

    public Texture ImageTexture;
    public Sprite Sprite;

    public AudioClip CardPlaySound;
    public AudioClip CardTimerSound;
    public Color ColorTheme;

    public bool isDestroyed;
    public int ArmorPoints;
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

    public int Timer;
    public bool TimerNoMoreActions;

    public int ArmorOther;
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
    public Spawns Spawns;
    public DrawCard DrawCard;
    public UniqueMechanics UniqueMechanics;
    public StatusEffects StatusEffects;

    public Card(string name, string secondName, string descriptionEng, string descriptionRu, string descriptionUk, string spritePath, string startOrderSoundPath, Color color, int maxPoints, int points, string timerSoundPath = null, int armor = 0,

        int boost = 0, int rangeBoost = 0, int changeBoost = 0, int damage = 0, int nearDamage = 0, int changeDamage = 0, 
        int selfBoost = 0, int selfDamage = 0, bool addictionWithSelfField = false, bool addictionWithEnemyField = false,

        int endTurnCount = 0, int endTurnRandomBoost = 0, int endTurnRandomDamage = 0, 
        int endTurnSelfBoost = 0, int endTurnSelfDamage = 0, int endTurnNearBoost = 0, int endTurnNearDamage = 0, int timer = 0, bool timerEndTurnNoMoreAction = false, int armorOther = 0,

        int spawnCardCount = 0, int spawnCardNumber = 0, 

        int drawCardCount = 0,

        bool shieldOther = false, bool stunOther = false, bool invisibility = false, bool invulnerability = false, bool isSelfShielded = false,
        int enduranceOrBleedingSelf = 0, int enduranceOrBleedingOther = 0, bool isEnemyTargetEnduranceOrBleeding = false,

        int destroyCardPoints = 0, bool swapPoints = false, int transformationNumber = -1, int returnDamageValue = 0, int healDamageValue = 0)

    {

        BaseCard = new BaseCard
        {
            Name = name,
            AbilityName = secondName,

            DescriptionEng = descriptionEng,
            DescriptionRu = descriptionRu,
            DescriptionUk = descriptionUk,

            MaxPoints = maxPoints,
            Points = points,

            ImageTexture = Resources.Load<Texture>(spritePath),
            Sprite = Resources.Load<Sprite>(spritePath),

            CardPlaySound = Resources.Load<AudioClip>(startOrderSoundPath),
            CardTimerSound = Resources.Load<AudioClip>(timerSoundPath),
            ColorTheme = color,
            ArmorPoints = armor
        };


        BoostOrDamage = new BoostOrDamage
        {
            Boost = boost,
            NearBoost = rangeBoost,
            ChangeNearBoost = changeBoost,
            SelfBoost = selfBoost,

            Damage = damage,
            NearDamage = nearDamage,
            ChangeNearDamage = changeDamage,
            SelfDamage = selfDamage,

            AddictionWithAlliedField = addictionWithSelfField,
            AddictionWithEnemyField = addictionWithEnemyField
        };


        EndTurnActions = new EndTurnActions
        {
            EndTurnActionCount = endTurnCount,
            EndTurnRandomDamage = endTurnRandomDamage,
            EndTurnRandomBoost = endTurnRandomBoost,

            EndTurnSelfBoost = endTurnSelfBoost,
            EndTurnSelfDamage = endTurnSelfDamage,
            EndTurnNearBoost = endTurnNearBoost,
            EndTurnNearDamage = endTurnNearDamage,
            Timer = timer,
            TimerNoMoreActions = timerEndTurnNoMoreAction,

            ArmorOther = armorOther
        };


        Spawns = new Spawns
        {
            SpawnCardNumber = spawnCardNumber,
            SpawnCardCount = spawnCardCount
        };


        DrawCard = new DrawCard
        {
            DrawCardCount = drawCardCount
        };


        StatusEffects = new StatusEffects
        {
            IsShieldOther = shieldOther,
            IsStunOther = stunOther,
            IsInvisibility = invisibility,
            IsInvulnerability = invulnerability,
            IsSelfShielded = isSelfShielded,

            SelfEnduranceOrBleeding = enduranceOrBleedingSelf,
            EnduranceOrBleedingOther = enduranceOrBleedingOther,
            IsEnemyTargetEnduranceOrBleeding = isEnemyTargetEnduranceOrBleeding
        };


        UniqueMechanics = new UniqueMechanics
        {
            DestroyCardPoints = destroyCardPoints,
            SwapPoints = swapPoints,
            TransformationNumber = transformationNumber,
            ReturnDamageValue = returnDamageValue,
            HealDamageValue = healDamageValue
        };
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

            CardManagerList.DebugCards.Add(new Card("Debug", "Debug", "Debug", "Debug", "Debug",
                "Sprites/Cards/Lina1", "Sounds/Cards/StartOrder/LinaDragonSlave", Color.white,
                10, 10));

            //Game

            CardManagerList.AllCards.Add(new Card("Lina", "Dragon Slave", "<color=red>Damage</color> 3 near enemy unit by 3.", 
                "Нанесите 3 <color=red>урона</color> 3 рядом стоящим вражеским отрядам.", 
                "Нанесіть 3 <color=red>шкоди</color> 3 поруч ворожим загонам, що стоять поруч.",
                "Sprites/Cards/Lina1", "Sounds/Cards/StartOrder/LinaDragonSlave", Color.red,
                5, 5, damage:3, nearDamage: 1)); //14

            CardManagerList.AllCards.Add(new Card("Lina", "Light Strike Array", "<color=red>Damage</color> enemy unit by 3 and <color=yellow>stun</color> its.",
                "Нанесите 3 <color=red>урона</color> вражескому отряду и <color=yellow>оглушите</color> его.",
                "Завдайте 3 <color=red>шкоди</color> ворожій одиниці і <color=yellow>зашокируйте</color> її.",
                "Sprites/Cards/Lina2", "Sounds/Cards/StartOrder/LinaLightStrikeArray", Color.red,
                6, 6, damage: 3, stunOther: true)); //9 + stun

            CardManagerList.AllCards.Add(new Card("Lina", "Fiery Soul", "At the end of the your turn, deal 1 <color=red>damage</color> to a random enemy unit, repeat 3 <color=red>times</color>.",
                "В конце вашего хода нанесите 1 <color=red>урон</color> случайной вражеской единице, повторите 3 <color=red>раза</color>.",
                "В кінці вашого ходу завдайте 1 <color=red>шкоду</color> випадковій ворожій одиниці, повторіть 3 <color=red>рази</color>.",
                "Sprites/Cards/Lina3", "Sounds/Cards/StartOrder/LinaFierySoul", Color.red,
                2, 2, endTurnCount: 3, endTurnRandomDamage: 1)); //2 + 3 at end turn

            CardManagerList.AllCards.Add(new Card("Lina", "Laguna Blade", "<color=red>Destroy</color> an enemy unit.",
                "<color=red>Уничтожьте</color> вражеский отряд.",
                "<color=red>Знищте</color> ворожий загін.",
                "Sprites/Cards/Lina4", "Sounds/Cards/StartOrder/LinaLagunaBlade", Color.red,
                1, 1, destroyCardPoints: -1)); //1 + destroy

            CardManagerList.AllCards.Add(new Card("Luna", "Lucent Beam", "<color=red>Damage</color> enemy unit by 2 and <color=yellow>stun</color> it.",
                "Нанесите 2 <color=red>урона</color> вражескому отряду и <color=yellow>оглушите</color> ее.",
                "Завдайте 2 <color=red>шкоди</color> ворожиму загіну і <color=yellow>зашокируйте</color> її.",
                "Sprites/Cards/Luna1", "Sounds/Cards/StartOrder/LunaLucentBeam", Color.blue,
                7, 7, damage: 2, stunOther: true)); //9 + stun

            CardManagerList.AllCards.Add(new Card("Luna", "Moon Glaives", "At the end of the your turn, deal 1 <color=red>damage</color> to a random enemy unit, repeat 2 <color=red>times</color>.",
                "В конце вашего хода нанесите 1 <color=red>урон</color> случайному вражескому отряду, повторите 2 <color=red>раза</color>.",
                "В кінці вашого ходу завдайте 1 <color=red>шкоду</color> випадковому ворожому загіну, повторіть 2 <color=red>рази</color>.",
                "Sprites/Cards/Luna2", "Sounds/Cards/StartOrder/LunaMoonGlaives", Color.blue,
                6, 6, endTurnCount: 2, endTurnRandomDamage: 1)); //6 + 2 end turn

            CardManagerList.AllCards.Add(new Card("Luna", "Eclipse", "<color=red>Damage</color> 5 near enemy units by 2.",
                "Нанесите 2 <color=red>урона</color> 5 вражеским отрядам, находящимся рядом.",
                "Завдайте 2 <color=red>шкоди</color> 5 ворожим загінам поруч.",
                "Sprites/Cards/Luna3", "Sounds/Cards/StartOrder/LunaEclipse", Color.blue,
                8, 8, damage: 2, nearDamage: 2)); // 18

            CardManagerList.AllCards.Add(new Card("Templar Assasin", "Refraction", "<color=green>Boost</color> allied unit by 8. <color=red>Shield</color>.",
                "<color=green>Усильте</color> союзный отряд на 8. <color=red>Щит</color>.",
                "<color=green>Підсильте</color> союзний загін на 8. <color=red>Щит</color>.",
                "Sprites/Cards/TemplarAssasin1", "Sounds/Cards/StartOrder/TemplarAssasinRefraction", Color.magenta,
                3, 3, boost: 8, isSelfShielded: true)); // 11 + selfShield

            CardManagerList.AllCards.Add(new Card("Templar Assasin", "Meld", "<color=red>Damage</color> enemy unit by 5. <color=red>Shield</color>.",
                "Нанесите 5 <color=red>урона</color> вражескому отряду. <color=red>Щит</color>.",
                "Завдайте 5 <color=red>шкоди</color> ворожому загіну. <color=red>Щит</color>.",
                "Sprites/Cards/TemplarAssasin2", "Sounds/Cards/StartOrder/TemplarAssasinMeld", Color.magenta,
                7, 7, damage: 5, isSelfShielded: true)); // 12 + selfShield

            CardManagerList.AllCards.Add(new Card("Axe", "Berserker's Call", "<color=green>Boost</color> enemy unit by 3.",
                "<color=green>Усильте</color> вражеский отряд на 3.",
                "<color=green>Підсильте</color> союзний загін на 3.",
                "Sprites/Cards/Axe1", "Sounds/Cards/StartOrder/AxeBerserker'sCall", Color.red,
                14, 13, armor: 4, damage: -3)); // 10

            CardManagerList.AllCards.Add(new Card("Axe", "Culling Blade", "<color=red>Destroy</color> an enemy unit with 4 or less points.",
                "<color=red>Уничтожьте</color> вражеский отряд с 4 или менее очками.",
                "<color=red>Знищте</color> ворожий загін з 4 або менше очками.",
                "Sprites/Cards/Axe3", "Sounds/Cards/StartOrder/AxeCullingBlade", Color.red,
                14, 13, armor: 2, destroyCardPoints: 4)); // 13 + 4

            CardManagerList.AllCards.Add(new Card("Centaur Warrunner", "Double Edge", "<color=red>Damage</color> self by 5.",
                "Нанесите себе 5 <color=red>урона</color>.",
                "Завдайте собі 5 <color=red>шкоди</color>.",
                "Sprites/Cards/Centaur1", "Sounds/Cards/StartOrder/CentaurDoubleEdge", Color.gray,
                20, 16, armor: 2, selfDamage: 5)); //13

            CardManagerList.AllCards.Add(new Card("Centaur Warrunner", "Retaliate", "Whenever this unit takes damage, damage to the card that dealt 1 damage",
                "Каждый раз, когда этот отряд получает урон, нанесите 1 <color=red>урон</color> карте, которая нанесла урон.",
                "Щоразу, коли цей загін отримує шкоду, завдайте 1 <color=red>шкоду</color> карті, яка завдала шкоду.",
                "Sprites/Cards/Centaur2", "Sounds/Cards/StartOrder/CentaurRetaliate", Color.gray,
                10, 8, armor: 6, returnDamageValue: 1)); //8

            CardManagerList.AllCards.Add(new Card("Huskar", "Life Break", "<color=red>Damage</color> self and enemy unit by 7.",
                "Нанесите себе и вражескому отряду по 7 <color=red>урона</color>.",
                "Завдайте собі і ворожому загіну по 7 <color=red>шкоди</color>.",
                "Sprites/Cards/Huskar1", "Sounds/Cards/StartOrder/HuskarLifeBreak", Color.yellow,
                19, 17, damage: 7, selfDamage: 7, addictionWithEnemyField: true)); //17

            CardManagerList.AllCards.Add(new Card("Huskar", "Burning Spear", "At the end of the your turn, deal 1 <color=red>damage</color> to a random enemy unit, repeat 3 <color=red>times</color>.",
                "В конце вашего хода нанесите 1 <color=red>урон</color> случайному вражескому отряду, повторите 3 <color=red>раза</color>.",
                "В кінці вашого ходу завдайте 1 <color=red>шкоду</color> випадковому ворожому загіну, повторіть 3 <color=red>рази</color>.",
                "Sprites/Cards/Huskar2", "Sounds/Cards/StartOrder/HuskarBurningSpear", Color.yellow,
                8, 8, endTurnCount: 3, endTurnRandomDamage: 1)); //9 + 3 end turn

            CardManagerList.AllCards.Add(new Card("Huskar", "Berserkers Blood", "At the end of the your turn, <color=red>boost</color> self by 1, repeat 1 <color=red>times</color>.",
                "<color=red>Увеличьте</color> себя на 1 в конце вашего хода, повторите 1 <color=red>раз</color>.",
                "<color=red>Збільшіть</color> себе на 1 в кінці вашого ходу, повторіть 1 <color=red>раз</color>.",
                "Sprites/Cards/Huskar3", "Sounds/Cards/StartOrder/BerserkersBlood", Color.yellow,
                19, 4, endTurnCount: 1, endTurnSelfBoost: 1)); //4 + 1 end turn

            CardManagerList.AllCards.Add(new Card("Windranger", "Powershot", "<color=red>Damage</color> enemy unit by 5 and units near by 1.",
                "<color=red>Повредите</color> вражеский отряд на 5 и отряды рядом на 1.",
                "<color=red>Завдайте шкоди</color> ворожому загону на 5 і загону поруч на 1.",
                "Sprites/Cards/Windranger1", "Sounds/Cards/StartOrder/WindrangerPowershot", Color.green,
                4, 4, damage: 5, nearDamage: 1, changeDamage: -4)); // 11

            CardManagerList.AllCards.Add(new Card("Windranger", "Windrun", "<color=red>Draw</color> 1 card",
                "<color=red>Возьмите</color> 1 карту",
                "<color=red>Візьміть</color> 1 карту",
                "Sprites/Cards/Windranger2", "Sounds/Cards/StartOrder/WindrangerWindrun", Color.green,
                3, 3, drawCardCount: 1)); // 3 + card

            CardManagerList.AllCards.Add(new Card("Kunkka", "Tidebringer", "<color=red>Damage</color> enemy unit by 1, and units near by 5.",
                "<color=red>Повредите</color> вражеский отряд на 1, и отряды рядом на 5.",
                "<color=red>Завдайте шкоди</color> ворожому загону на 1 і загону поруч на 5.",
                "Sprites/Cards/Kunkka1", "Sounds/Cards/StartOrder/KunkkaTidebringer", Color.blue,
                5, 5, damage: 1, nearDamage: 5, changeDamage: 4)); // 16

            CardManagerList.AllCards.Add(new Card("Kunkka", "Ghost Ship", "<color=blue>Spawn</color> Ghost Ship to the left of this unit.",
                "<color=blue>Призовите</color> Призрачный Корабль слева от этого отряда.",
                "<color=blue>Закличте</color> Примарський Корабель ліворуч від цього загону.",
                "Sprites/Cards/Kunkka2", "Sounds/Cards/StartOrder/KunkkaGhostShip", Color.blue,
                7, 7, spawnCardCount: 1, spawnCardNumber: 2)); // 12 + 1 end turn

            CardManagerList.AllCards.Add(new Card("Earthshaker", "Fissure", "<color=red>Damage</color> enemy unit by 3 and near by 2, then <color=yellow>stun</color> them.",
                "<color=red>Повредите</color> вражеский отряд на 3 и отряды рядом на 2, затем <color=yellow>оглушите</color> их.",
                "<color=red>Завдайте шкоди</color> ворожому загону на 3 і загону поруч на 2, потім <color=yellow>стуньте</color> їх.",
                "Sprites/Cards/Earthshaker1", "Sounds/Cards/StartOrder/EarthshakerFissure", Color.yellow,
                6, 6, damage: 3, nearDamage: 2, changeDamage: -1, stunOther: true)); //13

            CardManagerList.AllCards.Add(new Card("Earthshaker", "Echo Slam", "<color=red>Damage</color> all enemy units by 2.",
                "<color=red>Повредите</color> все вражеские отряды на 2.",
                "<color=red>Завдайте шкоди</color> всім ворожим загонам на 2.",
                "Sprites/Cards/Earthshaker2", "Sounds/Cards/StartOrder/EarthshakerEchoSlam", Color.yellow,
                4, 4, damage: 2, nearDamage: -1)); //4 + allDamage 2

            CardManagerList.AllCards.Add(new Card("Chen", "Hand Of God", "<color=green>Boost</color> all allied units by 1 and give them 2 <color=green>endurance</color>.",
                "<color=green>Усильте</color> все союзные отряды на 1 и дайте им 2 <color=green>выносливости</color>.",
                "<color=green>Підсильте</color> всі союзні загони на 1 і надайте їм 2 <color=green>витривалості</color>.",
                "Sprites/Cards/Chen1", "Sounds/Cards/StartOrder/ChenHandOfGod", Color.yellow,
                3, 3, boost: 1, rangeBoost: -1, enduranceOrBleedingOther: 2, isEnemyTargetEnduranceOrBleeding: false)); //3 + allEnd 2

            CardManagerList.AllCards.Add(new Card("Chen", "Divine Favor", "At the end of the your turn, deal 1 <color=green>boost</color> to a random allied unit, repeat 2 <color=red>times</color>.",
                "В конце вашего хода дайте 1 <color=green>усиление</color> случайному союзному отряду, повторите 2 <color=red>раза</color>.",
                "В кінці вашого ходу надайте 1 <color=green>підсилення</color> випадковому союзному загону, повторіть 2 <color=red>рази</color>.",
                "Sprites/Cards/Chen2", "Sounds/Cards/StartOrder/ChenDivineFavor", Color.yellow,
                5, 5, endTurnCount: 2, endTurnRandomBoost: 1)); //5 + 2endTurn

            CardManagerList.AllCards.Add(new Card("Sniper", "Assasinate", "<color=red>Damage</color> enemy unit by 11.",
                "<color=red>Повредите</color> вражеский отряд на 11.",
                "<color=red>Завдайте шкоди</color> ворожому загону на 11.",
                "Sprites/Cards/Sniper1", "Sounds/Cards/StartOrder/SniperAssasinate", Color.yellow,
                3, 3, damage: 11)); // 14

            CardManagerList.AllCards.Add(new Card("Sniper", "Shrapnel", "At the end of the your turn, deal 1 <color=red>damage</color> to a random enemy unit, repeat 2 <color=red>times</color>.",
                "В конце вашего хода нанесите 1 <color=red>урон</color> случайному вражескому отряду, повторите 2 <color=red>раза</color>.",
                "В кінці вашого ходу завдайте 1 <color=red>шкоду</color> випадковому ворожому загону, повторіть 2 <color=red>рази</color>.",
                "Sprites/Cards/Sniper2", "Sounds/Cards/StartOrder/SniperAssasinate", Color.yellow,
                4, 4, endTurnCount: 2, endTurnRandomDamage: 1)); // 4 + 2 end turn

            CardManagerList.AllCards.Add(new Card("Bane", "BrainSap", "<color=red>Damage</color> enemy unit and <color=green>boost</color> self by 4.",
                "<color=red>Повредите</color> вражеский отряд и <color=green>усильте</color> себя на 4.",
                "<color=red>Завдайте шкоди</color> ворожому загону і <color=green>підсильте</color> себе на 4.",
                "Sprites/Cards/Bane1", "Sounds/Cards/StartOrder/BaneBrainSap", Color.magenta,
                4, 4, damage: 4, selfBoost: 4, addictionWithEnemyField: true)); // 12

            CardManagerList.AllCards.Add(new Card("Zeus", "Arc Lightning", "<color=red>Damage</color> enemy unit by 3, and near units by 2, and near by 1.",
                "Нанесите 3 <color=red>урона</color> вражескому отряду и 2 <color=red>урона</color> ближайшим отрядам, и 1 <color=red>урон</color> ближайшим отрядам.",
                "Завдайте 3 <color=red>шкоди</color> ворожому загону і 2 <color=red>шкоди</color> найближчим загонам, і 1 <color=red>шкоду</color> найближчим загiнам.",
                "Sprites/Cards/Zeus1", "Sounds/Cards/StartOrder/ZeusArcLightning", Color.blue,
                3, 3, damage: 3, nearDamage: 2, changeDamage: -1)); //12

            CardManagerList.AllCards.Add(new Card("Zeus", "Lightning Bolt", "<color=red>Damage</color> enemy unit by 5 and <color=yellow>stun</color> it.",
                "Нанесите 5 <color=red>урона</color> вражескому отряду и <color=yellow>оглушите</color> его.",
                "Завдайте 5 <color=red>шкоди</color> ворожому загону і <color=yellow>оглушіть</color> його.",
                "Sprites/Cards/Zeus2", "Sounds/Cards/StartOrder/ZeusLightningBolt", Color.blue,
                6, 6, damage: 5, stunOther: true)); //11 

            CardManagerList.AllCards.Add(new Card("Zeus", "Thundergod's Wrath", "<color=red>Damage</color> all enemy units by 3.",
                "Нанесите 3 <color=red>урона</color> всем вражеским отрядам.",
                "Завдайте 3 <color=red>шкоди</color> всім ворожим загонам.",
                "Sprites/Cards/Zeus3", "Sounds/Cards/StartOrder/ZeusThundergod'sWrath", Color.blue,
                 1, 1, damage: 3, nearDamage: -1)); //11

            CardManagerList.AllCards.Add(new Card("Abaddon", "Mist Coil", "<color=green>Boost</color> allied unit by 8 and <color=red>damage</color> self by 4.",
                "<color=green>Усильте</color> союзный отряд на 8 и нанесите 4 <color=red>урона</color> себе.",
                "<color=green>Підсильте</color> союзний загiн на 8 і завдайте 4 <color=red>шкоди</color> собі.",
                "Sprites/Cards/Abaddon1", "Sounds/Cards/StartOrder/AbaddonMistCoil", Color.gray,
                8, 8, boost: 8, selfDamage: 4, addictionWithSelfField: true)); //12

            CardManagerList.AllCards.Add(new Card("Abaddon", "Aphotic Shield", "<color=green>Boost</color> allied unit by 2 and give it <color=red>shield</color>. <color=red>Shield</color>.",
                "<color=green>Усильте</color> союзный отряд на 2 и дайте ему <color=red>щит</color>.",
                "<color=green>Підсильте</color> союзний загiн на 2 і надайте йому <color=red>щит</color>.",
                "Sprites/Cards/Abaddon2", "Sounds/Cards/StartOrder/AbaddonAphoticShield", Color.gray,
                8, 8, boost: 2, shieldOther: true)); //10 + shieldOther

            CardManagerList.AllCards.Add(new Card("Abaddon", "BorrowedTime", "Whenever this unit takes <color=red>damage</color>, <color=green>boost</color> him by damage value",
                "Каждый раз, когда этот отряд получает <color=red>урон</color>, <color=green>усильте</color> его на значение урона.",
                "Щоразу, коли цей загін отримує <color=red>шкоду</color>, <color=green>підсильте</color> його на значення шкоди.",
                "Sprites/Cards/Abaddon3", "Sounds/Cards/StartOrder/AbaddonBorrowedTime", Color.gray,
                 1, 1, healDamageValue: -1)); //1

            CardManagerList.AllCards.Add(new Card("Chaos Knight", "Phantasm", "<color=blue>Spawn</color> 3 your <color=blue>illusions</color> units near.",
                "<color=blue>Создайте</color> 3 ваших <color=blue>иллюзии</color> рядом.",
                "<color=blue>Створіть</color> 3 ваших <color=blue>ілюзії</color> поряд.",
                "Sprites/Cards/ChaosKnight1", "Sounds/Cards/StartOrder/ChaosKnightPhantasm", Color.red,
                5, 5, spawnCardCount: 3, spawnCardNumber: -1)); // 20

            CardManagerList.AllCards.Add(new Card("Chaos Knight", "Chaos Bolt", "<color=red>Damage</color> enemy unit by 2, <color=yellow>stun</color> him and <color=blue>spawn</color> 1 your <color=blue>illusion</color> unit near.",
                "Нанесите 2 <color=red>урона</color> вражескому отряду, <color=yellow>оглушите</color> его и <color=blue>создайте</color> 1 вашу <color=blue>иллюзию</color> рядом.",
                "Завдайте 2 <color=red>шкоди</color> ворожому загону, <color=yellow>оглушіть</color> його і <color=blue>створіть</color> 1 вашу <color=blue>ілюзію</color> поряд.",
                "Sprites/Cards/ChaosKnight2", "Sounds/Cards/StartOrder/ChaosKnightChaosBolt", Color.red,
                6, 6, damage: 2, addictionWithEnemyField: true, spawnCardCount: 1, spawnCardNumber: -1, stunOther: true)); //12+2+stunOther

            CardManagerList.AllCards.Add(new Card("Chaos Knight", "Chaos Strike", "At the end of the your turn, deal 1 <color=red>damage</color> to a random enemy unit and <color=green>boost</color> self, repeat 1 <color=red>times</color>.",
                "В конце вашего хода нанесите 1 <color=red>урон</color> случайному вражескому отряду и <color=green>усильте</color> себя, повторите 1 <color=red>раз</color>.",
                "В кінці вашого ходу завдайте 1 <color=red>шкоду</color> випадковому ворожому загону і <color=green>підсильте</color> себе, повторіть 1 <color=red>раз</color>.",
                "Sprites/Cards/ChaosKnight3", "Sounds/Cards/StartOrder/ChaosKnightChaosStrike", Color.red,
                3, 3, endTurnCount: 1, endTurnRandomDamage: 1, endTurnSelfBoost: 1)); // 3 + 2 end turn

            CardManagerList.AllCards.Add(new Card("Lycan", "Summon Wolves", "<color=blue>Spawn</color> 2 units Wolves near.",
                "Создайте 2 отряда волков рядом.",
                "Створіть 2 загони вовків поряд.",
                "Sprites/Cards/Lycan1", "Sounds/Cards/StartOrder/LycanSummonWolves", Color.gray,
                10, 10, spawnCardCount: 2, spawnCardNumber: 0)); //14

            CardManagerList.AllCards.Add(new Card("Lycan", "Feral Impulce", "At the end of the your turn, deal 2 <color=green>boost</color> to a random allied unit, repeat 1 <color=red>times</color>.",
                "В конце вашего хода <color=green>усильте</color> случайный союзный отряд на 2, повторите 1 <color=red>раз</color>.",
                "В кінці вашого ходу <color=green>підсильте</color> випадковий союзний загiн на 2, повторіть 1 <color=red>раз</color>.",
                "Sprites/Cards/Lycan2", "Sounds/Cards/StartOrder/LycanFeralImpulce", Color.gray,
                6, 6, endTurnCount: 1, endTurnRandomBoost: 2)); // 6 + 2 end turn

            CardManagerList.AllCards.Add(new Card("Lycan", "Shapeshift", "<color=red>Transformate</color> this unit to Lycan Wolf ",
                "Трансформируйте этот отряд в волка Ликана.",
                "Трансформуйте цей загін у вовка Ликана.",
                "Sprites/Cards/Lycan3", "Sounds/Cards/StartOrder/LycanShapeshift", Color.gray,
                1, 1, transformationNumber: 0));//20 - 4 end turn

            CardManagerList.AllCards.Add(new Card("Riki", "Tricks of the Trade", "<color=red>Damage</color> 3 enemy units by 4. <color=purple>Invisibility</color>. <color=orange>Invulnerability</color>.",
                "Нанесите 4 <color=red>урона</color> 3 вражеским отрядам. <color=purple>Невидимость</color>. <color=orange>Неуязвимость</color>.",
                "Завдайте 4 <color=red>шкоди</color> 3 ворожим загонам. <color=purple>Невидимість</color>. <color=orange>Незахищеність</color>.",
                "Sprites/Cards/Riki1", "Sounds/Cards/StartOrder/RikiTricksOfTheTrade", Color.magenta,
                2, 2, damage: 4, nearDamage: 1, invisibility: true, invulnerability: true)); // -2 + 12

            CardManagerList.AllCards.Add(new Card("Riki", "Cloak And Dagger", "<color=red>Damage</color> enemy unit by 12. <color=purple>Invisibility</color>.",
                "Нанесите 12 <color=red>урона</color> вражескому отряду. <color=purple>Невидимость</color>.",
                "Завдайте 12 <color=red>шкоди</color> ворожому загону. <color=purple>Невидимість</color>.",
                "Sprites/Cards/Riki2", "Sounds/Cards/StartOrder/RikiCloakAndDagger", Color.magenta,
                1, 1, damage: 12, invisibility: true)); // -1 + 12

            CardManagerList.AllCards.Add(new Card("Juggernaut", "Omnislash", "<color=red>Damage</color> enemy units by 5 and near by 3 and near by 1. <color=orange>Invulnerability</color>.",
                "Нанесите 5 <color=red>урона</color> вражеским отрядам, 3 <color=red>урона</color> соседним отрядам и 1 <color=red>урон</color> соседнему отряду. <color=orange>Неуязвимость</color>.",
                "Завдайте 5 <color=red>шкоди</color> ворожим загонам, 3 <color=red>шкоди</color> сусіднім загонам і 1 <color=red>шкоду</color> сусідньому загону. <color=orange>Незахищеність</color>.",
                "Sprites/Cards/Juggernaut1", "Sounds/Cards/StartOrder/JuggernautOmnislash", Color.green,
                2, 2, damage: 5, nearDamage: 2, changeDamage: -2, invulnerability: true)); //14

            CardManagerList.AllCards.Add(new Card("Juggernaut", "Blade Dance", "At the end of the your turn, deal 4 <color=red>damage</color> to a random enemy unit, repeat 1 <color=red>times</color>.",
                "В конце вашего хода нанесите 4 <color=red>урона</color> случайному вражескому отряду, повторите 1 <color=red>раз</color>.",
                "В кінці вашого ходу завдайте 4 <color=red>шкоди</color> випадковому ворожому загону, повторіть 1 <color=red>раз</color>.",
                "Sprites/Cards/Juggernaut2", "Sounds/Cards/StartOrder/JuggernautBladeDance", Color.green,
                1, 1, endTurnCount: 1, endTurnRandomDamage: 4)); //1 + 4 end turn

            CardManagerList.AllCards.Add(new Card("Juggernaut", "Healing Ward", "<color=blue>Spawn</color> Healing Ward to the left of this unit.",
                "Создайте Healing Ward слева от этого отряда.",
                "Створіть Healing Ward зліва від цього загону.",
                "Sprites/Cards/Juggernaut3", "Sounds/Cards/StartOrder/JuggernautHealingWard", Color.green,
                 2, 2, spawnCardCount: 1, spawnCardNumber: 1)); //2 + 1 + 4 end turn

            CardManagerList.AllCards.Add(new Card("Naga Siren", "Mirror Image", "<color=blue>Spawn</color> 2 your <color=blue>illusions</color> units near.",
                "Создайте 2 ваши <color=blue>иллюзии</color> рядом.",
                "Створіть 2 ваші <color=blue>ілюзії</color> поряд.",
                "Sprites/Cards/NagaSiren1", "Sounds/Cards/StartOrder/NagaSirenMirrorImage", Color.cyan,
                7, 7, spawnCardCount: 2, spawnCardNumber: -1)); // 21

            CardManagerList.AllCards.Add(new Card("Naga Siren", "Song of the Siren", "<color=yellow>Stun</color> all enemy units. <color=orange>Invulnerability</color>.",
                "Оглушите все вражеские отряды. <color=orange>Неуязвимость</color>.",
                "Оглушіть всі ворожі загони. <color=orange>Незахищеність</color>.",
                "Sprites/Cards/NagaSiren2", "Sounds/Cards/StartOrder/NagaSirenSongOfTheSiren", Color.cyan,
                12, 12, nearDamage: -1, stunOther: true, invulnerability: true)); //12 + stun

            CardManagerList.AllCards.Add(new Card("Terrorblade", "Conjure Image", "<color=blue>Spawn</color> 1 your <color=blue>illusion</color> unit near. At the end of the your turn, deal 1 damage to a random enemy unit, repeat 1 <color=red>times</color>.",
                "Создайте 1 вашу <color=blue>иллюзию</color> рядом. В конце вашего хода нанесите 1 <color=red>урон</color> случайному вражескому отряду, повторите 1 <color=red>раз</color>.",
                "Створіть 1 вашу <color=blue>ілюзію</color> поряд. В кінці вашого ходу завдайте 1 <color=red>шкоду</color> випадковому ворожому загону, повторіть 1 <color=red>раз</color>.",
                "Sprites/Cards/Terrorblade1", "Sounds/Cards/StartOrder/TerrorbladeConjureImage", Color.blue,
                3, 3, endTurnCount: 1, endTurnRandomDamage: 1, spawnCardCount: 1, spawnCardNumber: -1)); // 6 + 2 end turn

            CardManagerList.AllCards.Add(new Card("Terrorblade", "Sunder", "<color=red>Swap points</color> with unit.",
                "Поменяйте очки с отрядом.",
                "Обміняйте очки з загоном.",
                "Sprites/Cards/Terrorblade2", "Sounds/Cards/StartOrder/TerrorbladeSunder", Color.blue,
                8, 8, swapPoints: true)); // 8 + swap

            CardManagerList.AllCards.Add(new Card("Bloodseeker", "Bloodrage", "Give self 3 <color=red>bleeding</color>.",
                "Дайте себе 3 <color=red>кровотечения</color>.",
                "Надайте собі 3 <color=red>кровотечі</color>.",
                "Sprites/Cards/Bloodseeker1", "Sounds/Cards/StartOrder/BloodseekerBloodrage", Color.red,
                12, 12, enduranceOrBleedingSelf: -3)); //12 - 3 bleed

            CardManagerList.AllCards.Add(new Card("Bloodseeker", "Rupture", "Give an enemy unit 7 <color=red>bleeding</color>.",
                "Дайте вражескому отряду 7 <color=red>кровотечений</color>.",
                "Надайте ворожому загону 7 <color=red>кровотечі</color>.",
                "Sprites/Cards/Bloodseeker2", "Sounds/Cards/StartOrder/BloodseekerRupture", Color.red,
                 8, 8, enduranceOrBleedingOther: -7, isEnemyTargetEnduranceOrBleeding: true)); //8 + 7 bleed

            CardManagerList.AllCards.Add(new Card("Hoodwink", "Bushwhack", "<color=red>Damage</color> 3 near enemy units by 2 then <color=yellow>stun</color> them.",
                "Нанесите 2 <color=red>урона</color> 3 соседним вражеским отрядам, затем <color=yellow>оглушите</color> их.",
                "Завдайте 2 <color=red>шкоди</color> 3 сусіднім ворожим загонам, а потім <color=yellow>засліпіть</color> їх.",
                "Sprites/Cards/Hoodwink1", "Sounds/Cards/StartOrder/HoodwinkBushwhack", Color.green,
                6, 6, damage: 2, nearDamage: 1, stunOther: true)); //12

            CardManagerList.AllCards.Add(new Card("Hoodwink", "Sharpshooter", "<color=blue>Timer</color> 4: at the end of your turn, deal 4 <color=red>damage</color> to a random enemy unit 1 <color=red>times</color>.",
                "Таймер 4: в конце вашего хода нанесите 4 <color=red>урона</color> случайному вражескому отряду 1 <color=red>раз</color>.",
                "Таймер 4: в кінці вашого ходу завдайте 4 <color=red>шкоди</color> випадковому ворожому загону 1 <color=red>раз</color>.",
                "Sprites/Cards/Hoodwink2", "Sounds/Cards/StartOrder/HoodwinkSharpshooter", Color.green,
                3, 3, timerSoundPath: "Sounds/Cards/StartOrder/HoodwinkSharpshooterTimer",
                timer: 4, endTurnCount: 1, endTurnRandomDamage: 4)); //3 + 4 end turn

            CardManagerList.AllCards.Add(new Card("Sven", "Storm Hammer", "<color=red>Damage</color> enemy unit by 2 and <color=yellow>stun</color> then.",
                "Нанесите 2 <color=red>урона</color> вражескому отряду и <color=yellow>оглушите</color> его.",
                "Завдайте 2 <color=red>шкоди</color> ворожому загону і <color=yellow>засліпіть</color> його.",
                "Sprites/Cards/Sven1", "Sounds/Cards/StartOrder/SvenStormHammer", Color.gray,
                8, 8, damage: 2, stunOther: true, armor: 6)); //10

            CardManagerList.AllCards.Add(new Card("Sven", "Warcry", "Give all allied units 3 armor.",
                "Дайте всем союзным отрядам 3 брони.",
                "Надайте всім союзним загонам 3 броні.",
                "Sprites/Cards/Sven2", "Sounds/Cards/StartOrder/SvenWarcry", Color.gray,
                7, 7, armor: 3, rangeBoost: -1, armorOther: 3)); //

            CardManagerList.AllCards.Add(new Card("Sven", "God's Strength", "<color=blue>Timer</color> 3: <color=green>boost</color> self by self max point",
                "Таймер 3: <color=green>усильте</color> себя на максимальное количество очков.",
                "Таймер 3: <color=green>підсильте</color> себе на максимальну кількість очок.",
                "Sprites/Cards/Sven3", "Sounds/Cards/StartOrder/SvenGod'sStrength", Color.gray,
                8, 8, timerSoundPath: "Sounds/Cards/StartOrder/SvenGod'sStrengthTimer",
                armor: 2, timer: 3, endTurnCount: 1, endTurnSelfBoost: 8, timerEndTurnNoMoreAction: true)); //


            // Transformation

            CardManagerList.TransformationCards.Add(new Card("Lycan", "Wolf Form", "At the end of your turn, deal 2 <color=red>Damage</color> to nearby units.",
                "В конце вашего хода нанесите 2 <color=red>урона</color> соседним отрядам.",
                "В кінці вашого ходу завдайте 2 <color=red>шкоди</color> сусіднім загонам.",
                "LycanWolf", "Sounds/Cards/StartOrder/LycanWolf", Color.gray,
                20, 20, endTurnCount: 1, endTurnNearDamage: 2));

            // SUMMONS

            CardManagerList.SummonCards.Add(new Card("Wolf", "Summon", "Nothing",
                "Ничего",
                "Нічого",
                "Wolf", "Sounds/Cards/StartOrder/Wolf", Color.gray,
                2, 2));

            CardManagerList.SummonCards.Add(new Card("Healing Ward", "Ward", "At the end of your turn, <color=green>boost</color> 2 nearby units by 2",
                "В конце вашего хода <color=green>усильте</color> 2 соседних отряда на 2.",
                "В кінці вашого ходу <color=green>підсильте</color> 2 сусідні загони на 2.",
                "HealingWard", "Sounds/Cards/StartOrder/HealingWard", Color.green,
                1, 1, endTurnCount: 1, endTurnNearBoost: 2));

            CardManagerList.SummonCards.Add(new Card("Ghost Ship", "Ship", "At the end of your turn, deal 2 <color=red>damage</color> to a random enemy unit and 1 damage to yourself 1 <color=red>times</color>.",
                "В конце вашего хода нанесите 2 <color=red>урона</color> случайному вражескому отряду и 1 <color=red>урон</color> себе 1 <color=red>раз</color>.",
                "В кінці вашого ходу завдайте 2 <color=red>шкоди</color> випадковому ворожому загону і 1 <color=red>шкоду</color> собі 1 <color=red>раз</color>.",
                "GhostShip", "Sounds/Cards/StartOrder/GhostShip", Color.blue,
                5, 5, endTurnCount: 1, endTurnRandomDamage: 2, endTurnSelfDamage: 1));

            CardManagerList.IsAddCardInGame = true;
        }
    }
}
