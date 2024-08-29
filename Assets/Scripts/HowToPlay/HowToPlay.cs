using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : MonoBehaviour
{
    private static HowToPlay _instance;

    public static HowToPlay Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<HowToPlay>();
            }

            return _instance;
        }
    }

    public bool IsHowToPlay;
    [HideInInspector] public GameObject[] HowToPlayDeckList;
    [HideInInspector] public GameObject[] HowToPlayGameList;
    private GameObject HowToPlayGameFon;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void HowToPlayDeckBuild(GameObject[] list)
    {
        if (IsHowToPlay)
        {
            HowToPlayDeckList = list;
            StartCoroutine(HowToPlayDeckBuildCoroutine());
        }
    }

    private IEnumerator HowToPlayDeckBuildCoroutine()
    {
        int number = 0;
        HowToPlayDeckList[0].SetActive(true);

        while (number < HowToPlayDeckList.Length)
        {
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            number++;
            NextPanelHowToPlay(HowToPlayDeckList, number);
            yield return new WaitForSeconds(1f);
        }

        StopCoroutine(HowToPlayDeckBuildCoroutine());
    }

    public void HowToPlayGame(GameObject[] list, GameObject fon)
    {
        if (IsHowToPlay)
        {
            HowToPlayGameFon = fon;
            HowToPlayGameList = list;
            StartCoroutine(HowToPlayGameCoroutine());
        }
    }

    private IEnumerator HowToPlayGameCoroutine()
    {
        HowToPlayGameFon.SetActive(true);

        int number = 0;
        HowToPlayGameList[0].SetActive(true);

        while (number < HowToPlayGameList.Length)
        {
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
            number++;
            NextPanelHowToPlay(HowToPlayGameList, number);
            yield return new WaitForSeconds(1f);
        }

        HowToPlayGameFon.SetActive(false);
        GameManager.Instance.StartTurnCoroutine();
        IsHowToPlay = false;
        StopCoroutine(HowToPlayGameCoroutine());
    }

    private void NextPanelHowToPlay(GameObject[] panels,int number)
    {
        panels[number - 1].SetActive(false);
        if (number < panels.Length)
        panels[number].SetActive(true);
    }
}
