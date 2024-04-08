using System.Collections.Generic;
using UnityEngine;

public struct Card
{
    public string Name;
    public string SecondName;
    public string Description;

    public int MaxPoints;
    public int Points;
   
    public Sprite Image;
    public AudioClip DeploymentSound;
    public AudioClip StartOrderSound;
    public Color ColorTheme;

    public int Boost;
    public int Damage;
    public int SelfBoost;
    public int SelfDamage;

    public bool EndTurnAction;
    public int EndTurnDamage;
    public int EndTurnBoost;

    public Card(string name, string secondName, string description, 
        string spritePath, string deploymentSoundPath, string startOrderSoundPath, Color color,
        int maxPoints, int points, 
        int boost = 0, 
        int damage = 0,
        int selfBoost = 0,
        int selfDamage = 0,
        bool endTurnAction = false, int endTurnDamage = 0, int endTurnBoost = 0)
    {
        Name = name;
        SecondName = secondName;
        Description = description;
        MaxPoints = maxPoints;
        Points = points;

        Image = Resources.Load<Sprite>(spritePath);
        DeploymentSound = Resources.Load<AudioClip>(deploymentSoundPath);
        StartOrderSound = Resources.Load<AudioClip>(startOrderSoundPath);
        ColorTheme = color;

        Boost = boost;
        Damage = damage;

        SelfBoost = selfBoost;
        SelfDamage = selfDamage;

        EndTurnAction = endTurnAction;
        EndTurnDamage = endTurnDamage;
        EndTurnBoost = endTurnBoost;
    }
}

public static class CardManagerList
{
    public static List<Card> AllCards = new List<Card>();
}

public class CardManager : MonoBehaviour
{
    private void Awake()
    {
        CardManagerList.AllCards.Add(new Card("Lina", "Dragon Slave", "Damage enemy card by 3",
            "Sprites/Cards/Lina1", "Sounds/Cards/Deployment/Lina1", "Sounds/Cards/StartOrder/LinaDragonSlave", Color.red, 
            5, 5, 0, 3));
        CardManagerList.AllCards.Add(new Card("Lina", "Light Strike Array", "Damage enemy card by 2",
             "Sprites/Cards/Lina2", "Sounds/Cards/Deployment/Lina2" , "Sounds/Cards/StartOrder/LinaLightStrikeArray", Color.red,
             3, 3, 0, 2));
        CardManagerList.AllCards.Add(new Card("Lina", "Fiery Soul", "At the end of the turn, deal 1 damage to a random enemy unit.",
             "Sprites/Cards/Lina3", "Sounds/Cards/Deployment/Lina3", "Sounds/Cards/StartOrder/LinaFierySoul", Color.red, 
             3, 3, 0, 0, 0, 0, true , 1, 0));
        CardManagerList.AllCards.Add(new Card("Lina", "Laguna Blade", "Damage enemy card by 10",
             "Sprites/Cards/Lina4", "Sounds/Cards/Deployment/Lina4", "Sounds/Cards/StartOrder/LinaLagunaBlade", Color.red, 
             1, 1, 0, 10));

        CardManagerList.AllCards.Add(new Card("Luna", "Lucent Beam", "Damage enemy card by 2",
            "Sprites/Cards/Luna1", "Sounds/Cards/Deployment/Luna1", "Sounds/Cards/StartOrder/LunaLucentBeam", Color.blue,
            4, 4, 0, 2));
        CardManagerList.AllCards.Add(new Card("Templar Assasin", "Refraction", "Boost friendly card by 6",
             "Sprites/Cards/TemplarAssasin1", "Sounds/Cards/Deployment/TemplarAssasin1", "Sounds/Cards/StartOrder/TemplarAssasinRefraction", Color.magenta,
             1, 1, 6, 0));
        CardManagerList.AllCards.Add(new Card("Axe", "Berserker's Call", "Boost enemy card by 3",
             "Sprites/Cards/Axe1", "Sounds/Cards/Deployment/Axe1", "Sounds/Cards/StartOrder/AxeBerserker'sCall", Color.red,
             10, 10, 0, -3));
        CardManagerList.AllCards.Add(new Card("Centaur Warrunner", "Double Edge", "Damage self by 3",
             "Sprites/Cards/Centaur1", "Sounds/Cards/Deployment/Centaur1", "Sounds/Cards/StartOrder/CentaurDoubleEdge", Color.gray,
             12, 12, 0, 0, 0, 3));
    }
}
