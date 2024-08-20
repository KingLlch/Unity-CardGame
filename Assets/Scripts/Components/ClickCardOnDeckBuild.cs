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
                card.ImageEdge1.color = Color.red;
                IsInDeck = true;
            }
            else if (!IsMainCard)
            {
                DeckBuildManager.Instance.RemoveCard(CardInfoScript, gameObject);

                CardInfoScript.ImageEdge1.color = Color.white;
                IsInDeck = false;
            }
        }

        else if (eventData.button == PointerEventData.InputButton.Right && !IsInDeck)
        {
            CardView.Instance.CardViewObject.SetActive(true);
            CardView.Instance.ShowCard(card);
        }
    }
}
