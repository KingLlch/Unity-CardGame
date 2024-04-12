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
        transform.GetComponent<CardInfoScript>().ImageEdge.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetComponent<CardInfoScript>().ImageEdge.color = transform.GetComponent<CardInfoScript>().SelfCard.ColorTheme;
    }
}
