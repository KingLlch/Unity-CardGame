using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardInfoScript : MonoBehaviour
{
    public Card SelfCard;

    public GameObject DescriptionObject;
    public GameObject CardBack;

    public Image Image;
    public Image ImageEdge;

    public TextMeshProUGUI Point;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI SecondName;
    public TextMeshProUGUI Description;

    public int SiblingIndex;
    public bool IsHideCard;
    public bool IsAnimationCard;

    public Shadow ShadowCard;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    public int ShowPoints(Card card)
    {
        return card.Points;
    }

    public void ChangePoints(ref Card card, int value, Card startCard)
    {
        card.Points += value;

        Debug.Log(startCard.Name + " изменила силу " + card.Name + " в размере " + value + "\n" + (card.Points - value) + " => " + card.Points);

        ShowPointsUI(card);
    }

    public void ShowCardInfo(Card card)
    {
        SelfCard = card;

        CardBack.SetActive(false);
        IsHideCard = false;

        Image.sprite = card.Image;
        Image.preserveAspect = true;
        Point.text = card.Points.ToString();
        Name.text = card.Name.ToString();
        SecondName.text = card.SecondName.ToString();
        Description.text = card.Description.ToString();

        Name.colorGradient = new VertexGradient(card.ColorTheme, card.ColorTheme, Color.black, Color.black);
        SecondName.colorGradient = new VertexGradient(card.ColorTheme, card.ColorTheme, Color.black, Color.black);
        Description.colorGradient = new VertexGradient(card.ColorTheme, card.ColorTheme, Color.black, Color.black);
        ImageEdge.color = card.ColorTheme;

        ShadowCard.enabled = true;

    }

    public void HideCardInfo(Card card)
    {
        SelfCard = card;
        CardBack.SetActive(true);
        IsHideCard = true;
        ShadowCard.enabled = false;
    }


    public void ShowDescription()
    {
        if ((!IsHideCard) && (!_gameManager.IsDrag) && (!IsAnimationCard))
        {
            DescriptionObject.SetActive(true);
            DescriptionObject.transform.SetParent(transform.parent.parent);
        }
    }

    public void HideDescription()
    {
        DescriptionObject.SetActive(false);
        DescriptionObject.transform.SetParent(transform);
    }

    public void ShowPointsUI(Card card)
    {
        Point.text = card.Points.ToString();
    }

    public void CheckSiblingIndex()
    {
        SiblingIndex = transform.GetSiblingIndex();
    }
    public List<CardInfoScript> ReturnRightNearCard(int range = 1)
    {
        List<CardInfoScript> RightNearCard = new List<CardInfoScript>();

        for (int i = 1; i <= range; i++)
        {
            if (SiblingIndex + i < transform.parent.childCount)
            {
                RightNearCard.Add(transform.parent.GetChild(SiblingIndex + i).GetComponent<CardInfoScript>());
            }
        }

        if (RightNearCard.Count != 0) return RightNearCard;
        else return null;
    }

    public List<CardInfoScript> ReturnLeftNearCard(int range = 1)
    {
        List<CardInfoScript> LeftNearCard = new List<CardInfoScript>();

        for (int i = 1; i <= range; i++)
        {
            if (SiblingIndex - i >= 0)
            {
                LeftNearCard.Add(transform.parent.GetChild(SiblingIndex - i).GetComponent<CardInfoScript>());
            }
        }

        if (LeftNearCard.Count != 0) return LeftNearCard;
        else return null;
    }

    /*public List<CardInfoScript> ReturnNearCard(int range = 1)
    {
        List<CardInfoScript> NearCard = new List<CardInfoScript>();

        for (int i = 1; i <= range; i++)
        {
            if (SiblingIndex + i < transform.parent.childCount)
            {
                NearCard.Add(transform.parent.GetChild(SiblingIndex + i).GetComponent<CardInfoScript>());
            }
        }

        for (int i = 1; i <= range; i++)
        {
            if (SiblingIndex - i >= 0)
            {
                NearCard.Add(transform.parent.GetChild(SiblingIndex - i).GetComponent<CardInfoScript>());
            }
        }

        if (NearCard.Count != 0) return NearCard;
        else return null;
    }*/
}
