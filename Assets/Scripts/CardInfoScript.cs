using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Linq;
using System.Diagnostics;

public class CardInfoScript : MonoBehaviour
{
    public Card SelfCard;
    public GameObject DescriptionObject;
    public Image Image;
    public TextMeshProUGUI Point;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;

    private void Start()
    {
        //ShowCardInfo(CardManagerList.AllCards[transform.GetSiblingIndex()]);
    }

    public void ShowCardInfo(Card card)
    {
        SelfCard = card;

        Image.sprite = card.Image;
        Image.preserveAspect = true;
        Point.text = card.Points.ToString();
        Name.text = card.Name.ToString();
        Description.text = card.Description.ToString();
    }

    public void HideCardInfo(Card card)
    {
        SelfCard = card;

        Image.sprite = null;
        Image.preserveAspect = false;
        Point.text = null;
        Name.text = null;
        Description.text = null;
    }


    public void ShowDescription()
    {
       // if ()
        //{
            DescriptionObject.SetActive(true);
            DescriptionObject.transform.SetParent(transform.parent.parent);
        //}
    }

    public void HideDescription()
    {
        DescriptionObject.SetActive(false);
        DescriptionObject.transform.SetParent(transform);
    }
}
