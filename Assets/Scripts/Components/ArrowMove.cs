using DG.Tweening;
using System.Collections;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public bool Vertical;

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
                transform.DOMoveY(transform.position.y + 15, 0.5f);
                yield return new WaitForSeconds(1f);
                transform.DOMoveY(transform.position.y - 15, 0.5f);
                yield return new WaitForSeconds(0.5f);
            }

            else
            {
                transform.DOMoveX(transform.position.x + 15, 0.5f);
                yield return new WaitForSeconds(1f);
                transform.DOMoveX(transform.position.x - 15, 0.5f);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
