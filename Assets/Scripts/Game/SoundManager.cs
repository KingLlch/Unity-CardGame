using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();
            }

            return _instance;
        }
    }

    [SerializeField] private AudioSource _deploymentPlayerAudioSource;
    [SerializeField] private AudioSource _deploymentEnemyAudioSource;
    [SerializeField] private AudioSource _startPlayerTargetAudioSource;
    [SerializeField] private AudioSource _startEnemyTargetAudioSource;
    [SerializeField] private AudioSource _endTurnAudioSource;
    [SerializeField] private AudioSource _timerAudioSource;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void PlayerDeploymentSound(CardInfoScript card)
    {
        _deploymentPlayerAudioSource.clip = Resources.Load<AudioClip>("Sounds/Cards/Deployment/" + card.SelfCard.BaseCard.Name + Random.Range(0, 6));
        _deploymentPlayerAudioSource.Play();
    }

    public void EnemyDeploymentSound(CardInfoScript card)
    {
        _deploymentEnemyAudioSource.clip = Resources.Load<AudioClip>("Sounds/Cards/Deployment/" + card.SelfCard.BaseCard.Name + Random.Range(0, 6));
        _deploymentEnemyAudioSource.Play();
    }

    public void PlayerStartEffectSound(CardInfoScript card)
    {
        _startPlayerTargetAudioSource.clip = card.SelfCard.BaseCard.CardPlaySound;
        _startPlayerTargetAudioSource.Play();
    }

    public void EnemyStartEffectSound(CardInfoScript card)
    {
        _startEnemyTargetAudioSource.clip = card.SelfCard.BaseCard.CardPlaySound;
        _startEnemyTargetAudioSource.Play();
    }

    public void EndTurnSound(CardInfoScript card)
    {
        _endTurnAudioSource.clip = card.SelfCard.BaseCard.CardPlaySound;
        _endTurnAudioSource.Play();
    }

    public void TimerSound(CardInfoScript card)
    {
        _timerAudioSource.clip = card.SelfCard.BaseCard.CardTimerSound;
        _timerAudioSource.Play();
    }
}
