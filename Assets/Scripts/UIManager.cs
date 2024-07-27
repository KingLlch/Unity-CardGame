using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject EndGamePanelWin;
    public GameObject EndGamePanelLose;
    public GameObject EndGamePanelDraw;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        _playerPointsTMPro = GameObject.Find("UI/MainCanvas/RightUI/Points/PlayerAllPointsImage/PlayerAllPoints").GetComponent<TextMeshProUGUI>();
        _enemyPointsTMPro = GameObject.Find("UI/MainCanvas/RightUI/Points/EnemyAllPointsImage/EnemyAllPoints").GetComponent<TextMeshProUGUI>();

        _playerDeckTMPro = GameObject.Find("UI/MainCanvas/RightUI/EnemyDeck/DeckCountText").GetComponent<TextMeshProUGUI>();
        _enemyDeckTMPro = GameObject.Find("UI/MainCanvas/RightUI/PlayerDeck/DeckCountText").GetComponent<TextMeshProUGUI>();

        _imageTurnTime[0] = GameObject.Find("UI/MainCanvas/RightUI/EndTurnButton/ImagesTurnTime/ImageTurnTime").GetComponent<Image>();
        _imageTurnTime[1] = GameObject.Find("UI/MainCanvas/RightUI/EndTurnButton/ImagesTurnTime/ImageTurnTime1").GetComponent<Image>();

        _endTurnButton = GameObject.Find("UI/MainCanvas/RightUI/EndTurnButton").GetComponent<Button>();

        _line = GameObject.Find("UI/MainCanvas/Line").GetComponent<LineRenderer>();
    }

    public void ChangeEndTurnButtonInteractable(bool isInteractable)
    {
        _endTurnButton.interactable = isInteractable;
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
}
