using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ChoseCard : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public UnityEvent<CardInfoScript> IChoseCard = new UnityEvent<CardInfoScript>();

    private void Awake()
    {
        IChoseCard.AddListener(GameManager.Instance.ChoseCard);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.GetComponent<CardInfoScript>())
        {
            IChoseCard.Invoke(eventData.pointerCurrentRaycast.gameObject.GetComponent<CardInfoScript>());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CardInfoScript card = transform.GetComponent<CardInfoScript>();
        card.ImageEdge1.color = Color.red;

        if (GameManager.Instance.StartChoseCard.SelfCard.RangeBoost > 0 || GameManager.Instance.StartChoseCard.SelfCard.RangeDamage > 0)
        {
            card.CheckSiblingIndex();

            if (CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeBoost, true) != null)
            {
                for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeBoost, true).Count; i++)
                {
                    CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeBoost, true)[i].ImageEdge1.color = Color.red;
                }
            }

            if (CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeBoost, false) != null)
            {
                for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeBoost, false).Count; i++)
                {
                    CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeBoost, false)[i].ImageEdge1.color = Color.red;
                }
            }

            if (CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeDamage, true) != null)
            {
                for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeDamage, true).Count; i++)
                {
                    CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeDamage, true)[i].ImageEdge1.color = Color.red;
                }
            }

            if (CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeDamage, false) != null)
            {
                for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeDamage, false).Count; i++)
                {
                    CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeDamage, false)[i].ImageEdge1.color = Color.red;
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CardInfoScript card = transform.GetComponent<CardInfoScript>();
        card.ImageEdge1.color = Color.white;

        if (GameManager.Instance.StartChoseCard.SelfCard.RangeBoost > 0 || GameManager.Instance.StartChoseCard.SelfCard.RangeDamage > 0)
        {
            card.CheckSiblingIndex();

            if (CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeBoost, true) != null)
            {
                for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeBoost, true).Count; i++)
                {
                    CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeBoost, true)[i].ImageEdge1.color = Color.white;
                }
            }

            if (CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeBoost, false) != null)
            {
                for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeBoost, false).Count; i++)
                {
                    CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeBoost, false)[i].ImageEdge1.color = Color.white;
                }
            }

            if (CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeDamage, true) != null)
            {
                for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeDamage, true).Count; i++)
                {
                    CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeDamage, true)[i].ImageEdge1.color = Color.white;
                }
            }

            if (CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeDamage, false) != null)
            {
                for (int i = 0; i < CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeDamage, false).Count; i++)
                {
                    CardMechanics.Instance.ReturnNearCard(card, GameManager.Instance.StartChoseCard.SelfCard.RangeDamage, false)[i].ImageEdge1.color = Color.white;
                }
            }
        }
    }
}
