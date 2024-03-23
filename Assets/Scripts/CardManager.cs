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

    public Card(string name, string description, int maxPoints, int points, string imagePath)
    {
        Name = name;
        Description = description;
        MaxPoints = maxPoints;
        Points = points;
        Image = Resources.Load<Sprite>(imagePath);
    }
}

public static class CardManagerList
{
    public static List<Card> AllCards;
} 

public class CardManager : MonoBehaviour
{
    private void Awake()
    {
        CardManagerList.AllCards.Add(new Card("q", "...", 1, 1, "Images/Cards/1"));
        CardManagerList.AllCards.Add(new Card("w", "...", 10, 10, "Images/Cards/2"));
        CardManagerList.AllCards.Add(new Card("e", "...", 9, 9, "Images/Cards/3"));
        CardManagerList.AllCards.Add(new Card("r", "...", 2, 2, "Images/Cards/4"));
        CardManagerList.AllCards.Add(new Card("t", "...", 3, 3, "Images/Cards/1"));
    }
}
