using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }

            return _instance;
        }
    }

    private TextMeshProUGUI _playerPointsTMPro;
    private TextMeshProUGUI _enemyPointsTMPro;
    private TextMeshProUGUI _playerDeckTMPro;
    private TextMeshProUGUI _enemyDeckTMPro;

    private Image[] _imageTurnTime = new Image[2];
    private LineRenderer _line;
    private Button _endTurnButton;

    public GameObject EndGamePanel;
    public TextMeshProUGUI EndGameText;

    public bool IsPause;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        _playerPointsTMPro = GameObject.Find("UI/MainCanvas/RightUI/Points/PlayerAllPointsImage/PlayerAllPoints").GetComponent<TextMeshProUGUI>();
        _enemyPointsTMPro = GameObject.Find("UI/MainCanvas/RightUI/Points/EnemyAllPointsImage/EnemyAllPoints").GetComponent<TextMeshProUGUI>();

        _playerDeckTMPro = GameObject.Find("UI/MainCanvas/RightUI/PlayerDeck/DeckCountText").GetComponent<TextMeshProUGUI>();
        _enemyDeckTMPro = GameObject.Find("UI/MainCanvas/RightUI/EnemyDeck/DeckCountText").GetComponent<TextMeshProUGUI>();

        _imageTurnTime[0] = GameObject.Find("UI/MainCanvas/RightUI/EndTurnButton/ImagesTurnTime/ImageTurnTime").GetComponent<Image>();
        _imageTurnTime[1] = GameObject.Find("UI/MainCanvas/RightUI/EndTurnButton/ImagesTurnTime/ImageTurnTime1").GetComponent<Image>();

        _endTurnButton = GameObject.Find("UI/MainCanvas/RightUI/EndTurnButton").GetComponent<Button>();

        _line = GameObject.Find("UI/MainCanvas/Line").GetComponent<LineRenderer>();
    }

    public void ChangeEndTurnButtonInteractable(bool isInteractable)
    {
        _endTurnButton.interactable = isInteractable;
    }

    public bool ReturnEndTurnButtonInteractable()
    {
        return _endTurnButton.interactable;
    }

    public void ChangeDeckCount(Game currentGame)
    {
        _playerDeckTMPro.text = currentGame.PlayerDeck.Count.ToString();
        _enemyDeckTMPro.text = currentGame.EnemyDeck.Count.ToString();
    }

    public void ChangePoints(int playerPoints, int enemyPoints)
    {
        _playerPointsTMPro.text = playerPoints.ToString();
        _enemyPointsTMPro.text = enemyPoints.ToString();

        if (playerPoints > enemyPoints)
        {
            _enemyPointsTMPro.color = Color.black;
            _enemyPointsTMPro.fontSize = 36;
            _playerPointsTMPro.color = Color.red;
            _playerPointsTMPro.fontSize = 50;
        }

        else if (enemyPoints > playerPoints) 
        {
            _playerPointsTMPro.color = Color.black;
            _playerPointsTMPro.fontSize = 36;
            _enemyPointsTMPro.color = Color.red;
            _enemyPointsTMPro.fontSize = 50;
        }

        else
        {
            _playerPointsTMPro.color = Color.black;
            _playerPointsTMPro.fontSize = 36;
            _enemyPointsTMPro.color = Color.black;
            _enemyPointsTMPro.fontSize = 36;
        }
    }

    public void ChangeWick(int currentTime)
    {
        _imageTurnTime[0].fillAmount = (float)currentTime / GameManager.Instance.TurnDuration;
        _imageTurnTime[1].fillAmount = (float)currentTime / GameManager.Instance.TurnDuration;
    }

    public void ChangeLineColor(Color firstColor, Color secondColor)
    {
        _line.startColor = firstColor;
        _line.endColor = secondColor;
    }

    public void ChangeLinePosition(int point, Vector3 position)
    {
        _line.SetPosition(point, position);
    }

    public void CheckColorPointsCard(CardInfoScript card)
    {
        if (card.SelfCard.BaseCard.Points == card.SelfCard.BaseCard.MaxPoints)
        {
            card.Point.colorGradient = new VertexGradient(Color.white, Color.white, Color.white, Color.white);
        }

        else if (card.SelfCard.BaseCard.Points < card.SelfCard.BaseCard.MaxPoints)
        {
            card.Point.colorGradient = new VertexGradient(Color.red, Color.red, Color.white, Color.white);
        }

        else
        {
            card.Point.colorGradient = new VertexGradient(Color.green, Color.green, Color.white, Color.white);
        }
    }

    public void CheckBleeding(CardInfoScript card)
    {
        if (card.SelfCard.StatusEffects.SelfEnduranceOrBleeding < 0)
        {
            card.BleedingPanel.SetActive(true);
            card.BleedingPanel.GetComponent<Image>().color = Color.red;
            card.BleedingPanelText.text = (-card.SelfCard.StatusEffects.SelfEnduranceOrBleeding).ToString();
        }

        else if (card.SelfCard.StatusEffects.SelfEnduranceOrBleeding > 0)
        {
            card.BleedingPanel.SetActive(true);
            card.BleedingPanel.GetComponent<Image>().color = Color.green;
            card.BleedingPanelText.text = card.SelfCard.StatusEffects.SelfEnduranceOrBleeding.ToString();
        }

        else
        {
            card.BleedingPanel.SetActive(false);
        }
    }

    public void EndGame(int playerPoints, int enemyPoint)
    {
        StopAllCoroutines();
        EndGamePanel.SetActive(true);

        if (playerPoints < enemyPoint)
        {
            EndGameText.text = "You Lose";
        }

        else if (playerPoints > enemyPoint)
        {
            EndGameText.text = "You Win";
        }

        else
        {
            EndGameText.text = "Draw";
        }
    }

    public void Pause()
    {
        IsPause = true;
        EndGamePanel.SetActive(true);
        EndGameText.text = "Pause";
    }
    public void UnPause()
    {
        IsPause = false;
        EndGamePanel.SetActive(false);
    }
}
