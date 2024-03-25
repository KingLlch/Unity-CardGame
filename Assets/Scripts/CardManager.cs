using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Card
{
    public string Name;
    public string Description;
    public int MaxPoints;
    public int Points;
    public Sprite Image;

    public Card(string name, string description, int maxPoints, int points, string spritePath)
    {
        Name = name;
        Description = description;
        MaxPoints = maxPoints;
        Points = points;
        Image = Resources.Load<Sprite>(spritePath);
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
        CardManagerList.AllCards.Add(new Card("Lina", "Fire Girl", 10, 10, "Sprites/Cards/1"));
        CardManagerList.AllCards.Add(new Card("Lina: Fire", "Fire Girl", 11, 11, "Sprites/Cards/2"));
        CardManagerList.AllCards.Add(new Card("Marci: Strength", "Anime Girl", 9, 9, "Sprites/Cards/3"));
        CardManagerList.AllCards.Add(new Card("Marci", "Anime Girl", 2, 2, "Sprites/Cards/4"));
        CardManagerList.AllCards.Add(new Card("Templar Assasin", "Assasin Girl", 5, 5, "Sprites/Cards/5"));
        CardManagerList.AllCards.Add(new Card("Luna", "Moon Girl", 3, 3, "Sprites/Cards/6"));
    }
}
