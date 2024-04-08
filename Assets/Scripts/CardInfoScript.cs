using TMPro;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

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

    public bool IsHideCard;

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

    }

    public void HideCardInfo(Card card)
    {
        SelfCard = card;
        CardBack.SetActive(true);
        IsHideCard = true;
    }


    public void ShowDescription()
    {
        if (!IsHideCard)
        {
            if (!_gameManager.IsDrag)
            {
                DescriptionObject.SetActive(true);
                DescriptionObject.transform.SetParent(transform.parent.parent);
            }
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

}
