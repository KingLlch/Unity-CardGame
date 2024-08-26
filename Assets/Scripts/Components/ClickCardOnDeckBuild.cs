using UnityEngine;
using UnityEngine.EventSystems;

public class ClickCardOnDeckBuild : MonoBehaviour, IPointerClickHandler
{
    public bool IsMainCard;
    public bool IsInDeck;

    public CardInfoScript CardInfoScript;

    public void OnPointerClick(PointerEventData eventData)
    {
        CardInfoScript card = transform.GetComponent<CardInfoScript>();

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!IsInDeck && IsMainCard)
            {

                DeckBuildManager.Instance.AddCard(card);
                CardInDeck(card);
            }
            else if (!IsMainCard)
            {
                DeckBuildManager.Instance.RemoveCard(CardInfoScript, gameObject);
                CardRemoveFromDeck(CardInfoScript);
            }
        }

        else if (eventData.button == PointerEventData.InputButton.Right && !IsInDeck)
        {
            CardView.Instance.CardViewObject.SetActive(true);
            CardView.Instance.ShowCard(card);
        }

    }
    public void CardInDeck(CardInfoScript card)
    {
        card.ImageEdge1.color = Color.red;
        IsInDeck = true;
    }

    public void CardRemoveFromDeck(CardInfoScript card)
    {
        CardInfoScript.ImageEdge1.color = Color.white;
        card.GetComponent<ClickCardOnDeckBuild>().IsInDeck = false;
        IsInDeck = false;
    }
}
