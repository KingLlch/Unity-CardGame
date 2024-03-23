using UnityEngine;
using UnityEngine.EventSystems;

public class Table : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private Transform _emptyTableCard;
    private CardMove card;
    private Hand hand;

    [HideInInspector] public Transform EmptyCardTableParentTransform;

    private void Awake()
    {
        _emptyTableCard = GameObject.Find("EmptyTableCard").transform;
        hand = GameObject.FindAnyObjectByType<Hand>();
    }

    private void ChangeCardPosition()
    {
        _emptyTableCard.transform.SetSiblingIndex(card.SiblingIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        card = eventData.pointerDrag.GetComponent<CardMove>();
        card.ChangeCardPosition.AddListener(ChangeCardPosition);

        _emptyTableCard.SetParent(transform);
        card.FutureCardParentTransform = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;
        card.FutureCardParentTransform = hand.transform;
        HideEmptyCard();
    }

    public void OnDrop(PointerEventData eventData)
    {
        hand.HideEmptyCard();

        if (card.FutureCardParentTransform == transform)
        {
            card.transform.SetParent(transform);
            HideEmptyCard();
        }

        card.transform.SetSiblingIndex(card.SiblingIndex);
    }

    public void HideEmptyCard()
    {
        _emptyTableCard.SetParent(null);
        _emptyTableCard.transform.position = new Vector2(1000, 0);
    }
}
