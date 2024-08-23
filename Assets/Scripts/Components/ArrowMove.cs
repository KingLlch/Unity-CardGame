using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    public bool Vertical;

    public float TimeMoveArrow = 0.25f;
    public float TimeStopArrow = 0.5f;

    public float DistanceArrowMove = 15;

    private void Start()
    {
        if (Vertical)
            StartCoroutine(Move(Vertical));
        else
            StartCoroutine(Move(Vertical));
    }

    public IEnumerator Move(bool isVertical)
    {
        while (true)
        {
            if (isVertical)
            {
                transform.DOMoveY(transform.position.y + DistanceArrowMove, TimeMoveArrow);
                yield return new WaitForSeconds(TimeStopArrow);
                transform.DOMoveY(transform.position.y - DistanceArrowMove, TimeMoveArrow);
                yield return new WaitForSeconds(TimeStopArrow);
            }

            else
            {
                transform.DOMoveX(transform.position.x + DistanceArrowMove, TimeMoveArrow);
                yield return new WaitForSeconds(TimeStopArrow);
                transform.DOMoveX(transform.position.x - DistanceArrowMove, TimeMoveArrow);
                yield return new WaitForSeconds(TimeStopArrow);
            }
        }
    }
}
