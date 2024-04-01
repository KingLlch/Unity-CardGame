using System.Collections.Generic;
using UnityEngine;

public struct Card
{
    public string Name;
    public string Description;

    public int MaxPoints;
    public int Points;
   
    public Sprite Image;
    public AudioClip Sound;

    public int Boost;
    public int Damage;
    public int SelfBoost;
    public int SelfDamage;

    public Card(string name, string description, 
        int maxPoints, int points, 
        string spritePath, string soundPath, 
        int boost = 0, 
        int damage = 0,
        int selfBoost = 0,
        int selfDamage = 0)
    {
        Name = name;
        Description = description;
        MaxPoints = maxPoints;
        Points = points;
        Image = Resources.Load<Sprite>(spritePath);

        Sound = Resources.Load<AudioClip>(soundPath);

        Boost = boost;
        Damage = damage;

        SelfBoost = selfBoost;
        SelfDamage = selfDamage;
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
        CardManagerList.AllCards.Add(new Card("Lina", "Damage enemy card by 3", 10, 10, "Sprites/Cards/1", "Sounds/Cards/1", 0,3));
        CardManagerList.AllCards.Add(new Card("Lina: Fire", "Damage enemy card by 5", 3, 3, "Sprites/Cards/2", "Sounds/Cards/2", 0,5));
        CardManagerList.AllCards.Add(new Card("Marci: Strength", "Boost friendly card by 5", 4, 4, "Sprites/Cards/3", "Sounds/Cards/3", 5,0));
        CardManagerList.AllCards.Add(new Card("Marci", "Boost enemy card by 5", 12, 12, "Sprites/Cards/4", "Sounds/Cards/4", 0,-5));
        CardManagerList.AllCards.Add(new Card("Templar Assasin", "Boost friendly card by 2", 5, 5, "Sprites/Cards/5", "Sounds/Cards/5", 2,0));
        CardManagerList.AllCards.Add(new Card("Luna", "Damage enemy card by 2", 3, 3, "Sprites/Cards/6", "Sounds/Cards/6", 0,2));
    }
}
