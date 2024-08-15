using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _deploymentPlayerAudioSource;
    [SerializeField] private AudioSource _deploymentEnemyAudioSource;
    [SerializeField] private AudioSource _startPlayerTargetAudioSource;
    [SerializeField] private AudioSource _startEnemyTargetAudioSource;
    [SerializeField] private AudioSource _endTurnAudioSource;

    private void Awake()
    {
        GameManager.Instance.PlayerDropCardEvent.AddListener(PlayerDeploymentSound);
        GameManager.Instance.EnemyDropCardEvent.AddListener(EnemyDeploymentSound);

        GameManager.Instance.PlayerOrderCard.AddListener(PlayerStartOrderSound);
        GameManager.Instance.EnemyOrderCardEvent.AddListener(EnemyStartOrderSound);

        CardMechanics.Instance.EndTurnCardEvent.AddListener(EndTurnSound);
    }

    private void PlayerDeploymentSound(CardInfoScript card)
    {
        _deploymentPlayerAudioSource.clip = Resources.Load<AudioClip>("Sounds/Cards/Deployment/" + card.SelfCard.Name + Random.Range(0, 6));
        _deploymentPlayerAudioSource.Play();
    }

    private void EnemyDeploymentSound(CardInfoScript card)
    {
        _deploymentEnemyAudioSource.clip = Resources.Load<AudioClip>("Sounds/Cards/Deployment/" + card.SelfCard.Name + Random.Range(0, 6));
        _deploymentEnemyAudioSource.Play();
    }

    private void PlayerStartOrderSound(CardInfoScript card)
    {
        _startPlayerTargetAudioSource.clip = card.SelfCard.StartOrderSound;
        _startPlayerTargetAudioSource.Play();
    }

    private void EnemyStartOrderSound(CardInfoScript card)
    {
        _startEnemyTargetAudioSource.clip = card.SelfCard.StartOrderSound;
        _startEnemyTargetAudioSource.Play();
    }

    private void EndTurnSound(CardInfoScript card)
    {
        _endTurnAudioSource.clip = card.SelfCard.StartOrderSound;
        _endTurnAudioSource.Play();
    }

}
