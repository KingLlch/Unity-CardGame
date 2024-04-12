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

        Debug.Log(startCard.Name + " �������� ���� " + card.Name + " � ������� " + value + "\n" + (card.Points - value) + " => " + card.Points);

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

    public List<CardInfoScript> ReturnNearCard()
    {
        if ((SiblingIndex + 1 < transform.parent.childCount) && (SiblingIndex - 1 >= 0))
        {
            return new List<CardInfoScript> { transform.parent.GetChild(SiblingIndex + 1).GetComponent<CardInfoScript>(), transform.parent.GetChild(SiblingIndex - 1).GetComponent<CardInfoScript>() };
        }

        else if (SiblingIndex + 1 < transform.parent.childCount)
        {
            return new List<CardInfoScript> { transform.parent.GetChild(SiblingIndex + 1).GetComponent<CardInfoScript>() };
        }
        else if (SiblingIndex - 1 >= 0)
        {
            return new List<CardInfoScript> { transform.parent.GetChild(SiblingIndex - 1).GetComponent<CardInfoScript>() }; 
        }

        else return null;
    }
}
