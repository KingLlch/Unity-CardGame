using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CardMove : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    private Camera _mainCamera;
    private Vector3 _offset;
    private GameObject _mainCanvas;

    public bool IsDraggable;

    [HideInInspector] public int StartSiblingIndex;
    [HideInInspector] public int SiblingIndex;

    [HideInInspector] public Transform CurrentCardParentTransform;
    [HideInInspector] public Transform FutureCardParentTransform;

    [HideInInspector] public UnityEvent ChangeCardPosition;
    [HideInInspector] public UnityEvent HideEmptyCard;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _mainCanvas = GameObject.Find("UI/MainCanvas");
    }

    public void OnBeginDrag(PointerEventData pointer)
    {
        _offset = transform.position - _mainCamera.ScreenToWorldPoint(new Vector3(pointer.position.x, pointer.position.y, 0));

        CurrentCardParentTransform = transform.parent;
        FutureCardParentTransform = transform.parent;

        IsDraggable = ((CurrentCardParentTransform.GetComponent<DropField>().TypeField == TypeField.SELF_HAND) && (GameManager.Instance.IsPlayerTurn) && (!GameManager.Instance.IsChoosing) && (!GameManager.Instance.IsHandCardPlaying));

        if (!IsDraggable) return;

        GameManager.Instance.IsDrag = true;

        StartSiblingIndex = transform.GetSiblingIndex();
        transform.SetParent(CurrentCardParentTransform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData pointer)
    {
        if (!IsDraggable) return;

        transform.position = (_mainCamera.ScreenToWorldPoint(new Vector3(pointer.position.x, pointer.position.y, 0)) + _offset);
        ChangePosition();
    }

    public void OnEndDrag(PointerEventData pointer)
    {
        if (!IsDraggable) return;

        GameManager.Instance.IsDrag = false;

        HideEmptyCard.Invoke();

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        if (!pointer.pointerCurrentRaycast.gameObject.GetComponent<DropField>())
        {
            transform.SetParent(CurrentCardParentTransform);
            transform.SetSiblingIndex(StartSiblingIndex);
        }

        ChangeCardPosition.RemoveAllListeners();
        HideEmptyCard.RemoveAllListeners();
    }

    private void ChangePosition()
    {
        int newIndex = FutureCardParentTransform.childCount;

        for (int i = 0; i < FutureCardParentTransform.childCount; i++)
        {
            if (transform.position.x < FutureCardParentTransform.GetChild(i).position.x)
            {
                newIndex = i;

                if (FutureCardParentTransform.transform.GetSiblingIndex() < newIndex) newIndex--;
                break;
            }
        }

        SiblingIndex = newIndex;
        ChangeCardPosition.Invoke();
    }

    public void EnemyMoveToField(Transform field)
    {
        if (field.childCount > 0)
            transform.DOMove(field.GetChild(field.childCount - 1).position + new Vector3(50, 0, 0), 0.5f);
        else transform.DOMove(field.position, 0.5f);
    }

    public void PlayerMoveToField(DropField field, Transform emptyHandCard, bool isInvisibility = false)
    {
        transform.GetComponent<CardInfoScript>().IsAnimationCard = true;
        transform.position = new Vector3(emptyHandCard.transform.position.x, emptyHandCard.transform.position.y, 9);

        if (!isInvisibility)
            transform.DOMove(field.EmptyTableCard.position, 0.5f);
        else
            transform.DOMove(field.EmptyEnemyTableCard.position, 0.5f);
    }

    public void MoveTopHierarchy()
    {
        transform.SetParent(_mainCanvas.transform);
        transform.SetAsLastSibling();
    }

    public void MoveBackHierarchy()
    {
        transform.SetParent(CurrentCardParentTransform);
        transform.SetSiblingIndex(SiblingIndex);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            CardView.Instance.CardViewObject.SetActive(true);
            CardView.Instance.ShowCard(transform.GetComponent<CardInfoScript>());
        }
    }
}
