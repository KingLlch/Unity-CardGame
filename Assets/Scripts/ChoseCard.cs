using System.Drawing;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ChoseCard : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public UnityEvent<CardInfoScript> IChoseCard;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        IChoseCard.AddListener(_gameManager.ChoseCard);
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
        transform.GetComponent<CardInfoScript>().ImageEdge1.color = UnityEngine.Color.cyan;

        if (_gameManager.StartChoseCard.SelfCard.RangeBoost > 0 || _gameManager.StartChoseCard.SelfCard.RangeDamage > 0)
        {
            transform.GetComponent<CardInfoScript>().CheckSiblingIndex();

            if (transform.GetComponent<CardInfoScript>().ReturnRightNearCard(_gameManager.StartChoseCard.SelfCard.RangeBoost) != null)
            {
                for (int i = 0; i < transform.GetComponent<CardInfoScript>().ReturnRightNearCard(_gameManager.StartChoseCard.SelfCard.RangeBoost).Count; i++)
                {
                    transform.GetComponent<CardInfoScript>().ReturnRightNearCard(_gameManager.StartChoseCard.SelfCard.RangeBoost)[i].ImageEdge1.color = UnityEngine.Color.cyan;
                }
            }

            if (transform.GetComponent<CardInfoScript>().ReturnLeftNearCard(_gameManager.StartChoseCard.SelfCard.RangeBoost) != null)
            {
                for (int i = 0; i < transform.GetComponent<CardInfoScript>().ReturnLeftNearCard(_gameManager.StartChoseCard.SelfCard.RangeBoost).Count; i++)
                {
                    transform.GetComponent<CardInfoScript>().ReturnLeftNearCard(_gameManager.StartChoseCard.SelfCard.RangeBoost)[i].ImageEdge1.color = UnityEngine.Color.cyan;
                }
            }

            if (transform.GetComponent<CardInfoScript>().ReturnRightNearCard(_gameManager.StartChoseCard.SelfCard.RangeDamage) != null)
            {
                for (int i = 0; i < transform.GetComponent<CardInfoScript>().ReturnRightNearCard(_gameManager.StartChoseCard.SelfCard.RangeDamage).Count; i++)
                {
                    transform.GetComponent<CardInfoScript>().ReturnRightNearCard(_gameManager.StartChoseCard.SelfCard.RangeDamage)[i].ImageEdge1.color = UnityEngine.Color.cyan;
                }
            }

            if (transform.GetComponent<CardInfoScript>().ReturnLeftNearCard(_gameManager.StartChoseCard.SelfCard.RangeDamage) != null)
            {
                for (int i = 0; i < transform.GetComponent<CardInfoScript>().ReturnLeftNearCard(_gameManager.StartChoseCard.SelfCard.RangeDamage).Count; i++)
                {
                    transform.GetComponent<CardInfoScript>().ReturnLeftNearCard(_gameManager.StartChoseCard.SelfCard.RangeDamage)[i].ImageEdge1.color = UnityEngine.Color.cyan;
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetComponent<CardInfoScript>().ImageEdge1.color = UnityEngine.Color.white;

        if (_gameManager.StartChoseCard.SelfCard.RangeBoost > 0 || _gameManager.StartChoseCard.SelfCard.RangeDamage > 0)
        {
            transform.GetComponent<CardInfoScript>().CheckSiblingIndex();

            if (transform.GetComponent<CardInfoScript>().ReturnRightNearCard(_gameManager.StartChoseCard.SelfCard.RangeBoost) != null)
            {
                for (int i = 0; i < transform.GetComponent<CardInfoScript>().ReturnRightNearCard(_gameManager.StartChoseCard.SelfCard.RangeBoost).Count; i++)
                {
                    transform.GetComponent<CardInfoScript>().ReturnRightNearCard(_gameManager.StartChoseCard.SelfCard.RangeBoost)[i].ImageEdge1.color = UnityEngine.Color.white;
                }
            }

            if (transform.GetComponent<CardInfoScript>().ReturnLeftNearCard(_gameManager.StartChoseCard.SelfCard.RangeBoost) != null)
            {
                for (int i = 0; i < transform.GetComponent<CardInfoScript>().ReturnLeftNearCard(_gameManager.StartChoseCard.SelfCard.RangeBoost).Count; i++)
                {
                    transform.GetComponent<CardInfoScript>().ReturnLeftNearCard(_gameManager.StartChoseCard.SelfCard.RangeBoost)[i].ImageEdge1.color = UnityEngine.Color.white;
                }
            }

            if (transform.GetComponent<CardInfoScript>().ReturnRightNearCard(_gameManager.StartChoseCard.SelfCard.RangeDamage) != null)
            {
                for (int i = 0; i < transform.GetComponent<CardInfoScript>().ReturnRightNearCard(_gameManager.StartChoseCard.SelfCard.RangeDamage).Count; i++)
                {
                    transform.GetComponent<CardInfoScript>().ReturnRightNearCard(_gameManager.StartChoseCard.SelfCard.RangeDamage)[i].ImageEdge1.color = UnityEngine.Color.white;
                }
            }

            if (transform.GetComponent<CardInfoScript>().ReturnLeftNearCard(_gameManager.StartChoseCard.SelfCard.RangeDamage) != null)
            {
                for (int i = 0; i < transform.GetComponent<CardInfoScript>().ReturnLeftNearCard(_gameManager.StartChoseCard.SelfCard.RangeDamage).Count; i++)
                {
                    transform.GetComponent<CardInfoScript>().ReturnLeftNearCard(_gameManager.StartChoseCard.SelfCard.RangeDamage)[i].ImageEdge1.color = UnityEngine.Color.white;
                }
            }
        }
    }
}
