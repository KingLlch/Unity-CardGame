using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ChoseCard : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public UnityEvent<CardInfoScript> IChoseCard;

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
        transform.GetComponent<CardInfoScript>().ImageEdge1.color = Color.red;

        if (GameManager.Instance.StartChoseCard.SelfCard.RangeBoost > 0 || GameManager.Instance.StartChoseCard.SelfCard.RangeDamage > 0)
        {
            transform.GetComponent<CardInfoScript>().CheckSiblingIndex();

            if (transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeBoost) != null)
            {
                for (int i = 0; i < transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeBoost).Count; i++)
                {
                    transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeBoost)[i].ImageEdge1.color = Color.red;
                }
            }

            if (transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeBoost) != null)
            {
                for (int i = 0; i < transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeBoost).Count; i++)
                {
                    transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeBoost)[i].ImageEdge1.color = Color.red;
                }
            }

            if (transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeDamage) != null)
            {
                for (int i = 0; i < transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeDamage).Count; i++)
                {
                    transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeDamage)[i].ImageEdge1.color = Color.red;
                }
            }

            if (transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeDamage) != null)
            {
                for (int i = 0; i < transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeDamage).Count; i++)
                {
                    transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeDamage)[i].ImageEdge1.color = Color.red;
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetComponent<CardInfoScript>().ImageEdge1.color = Color.white;

        if (GameManager.Instance.StartChoseCard.SelfCard.RangeBoost > 0 || GameManager.Instance.StartChoseCard.SelfCard.RangeDamage > 0)
        {
            transform.GetComponent<CardInfoScript>().CheckSiblingIndex();

            if (transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeBoost) != null)
            {
                for (int i = 0; i < transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeBoost).Count; i++)
                {
                    transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeBoost)[i].ImageEdge1.color = Color.white;
                }
            }

            if (transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeBoost) != null)
            {
                for (int i = 0; i < transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeBoost).Count; i++)
                {
                    transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeBoost)[i].ImageEdge1.color = Color.white;
                }
            }

            if (transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeDamage) != null)
            {
                for (int i = 0; i < transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeDamage).Count; i++)
                {
                    transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeDamage)[i].ImageEdge1.color = Color.white;
                }
            }

            if (transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeDamage) != null)
            {
                for (int i = 0; i < transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeDamage).Count; i++)
                {
                    transform.GetComponent<CardInfoScript>().ReturnNearCard(GameManager.Instance.StartChoseCard.SelfCard.RangeDamage)[i].ImageEdge1.color = Color.white;
                }
            }
        }
    }
}
