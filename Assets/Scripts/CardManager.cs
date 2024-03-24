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
        CardManagerList.AllCards.Add(new Card("q", "...", 1, 1, "Sprites/Cards/1"));
        CardManagerList.AllCards.Add(new Card("w", "...", 10, 10, "Sprites/Cards/2"));
        CardManagerList.AllCards.Add(new Card("e", "...", 9, 9, "Sprites/Cards/3"));
        CardManagerList.AllCards.Add(new Card("r", "...", 2, 2, "Sprites/Cards/4"));
        CardManagerList.AllCards.Add(new Card("t", "...", 3, 3, "Sprites/Cards/1"));
    }
}
