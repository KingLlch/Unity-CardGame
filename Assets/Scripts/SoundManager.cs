using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource _deploymentAudioSource;
    private AudioSource _deploymentEnemyAudioSource;
    private AudioSource _startTarhetAudioSource;
    private AudioSource _startEnemyTarhetAudioSource;

    private void Awake()
    {
        _deploymentAudioSource = GameObject.Find("Main Camera/SoundManager/DeploymentAudioSource").GetComponent<AudioSource>();
        _deploymentEnemyAudioSource = GameObject.Find("Main Camera/SoundManager/EnemyDeploymentAudioSource").GetComponent<AudioSource>();

        _startTarhetAudioSource = GameObject.Find("Main Camera/SoundManager/StartTargetAudioSource").GetComponent<AudioSource>();
        _startEnemyTarhetAudioSource = GameObject.Find("Main Camera/SoundManager/EnemyStartTargetAudioSource").GetComponent<AudioSource>();

        GameManager.Instance.PlayerDropCardEvent.AddListener(DeploymentSound);
        GameManager.Instance.EnemyDropCardEvent.AddListener(EnemyDeploymentSound);

        GameManager.Instance.OrderCard.AddListener(StartOrderSound);
        GameManager.Instance.EnemyDropCardEvent.AddListener(EnemyStartOrderSound);
    }

    private void DeploymentSound(CardInfoScript card)
    {
        _deploymentAudioSource.clip = Resources.Load<AudioClip>("Sounds/Cards/Deployment/" + card.SelfCard.Name + Random.Range(0, 6));
        _deploymentAudioSource.Play();
    }

    private void EnemyDeploymentSound(CardInfoScript card)
    {
        _deploymentEnemyAudioSource.clip = Resources.Load<AudioClip>("Sounds/Cards/Deployment/" + card.SelfCard.Name + Random.Range(0, 6));
        _deploymentEnemyAudioSource.Play();
    }

    private void StartOrderSound(CardInfoScript card)
    {
        _startTarhetAudioSource.clip = card.SelfCard.StartOrderSound;
        _startTarhetAudioSource.Play();
    }

    private void EnemyStartOrderSound(CardInfoScript card)
    {
        _startEnemyTarhetAudioSource.clip = card.SelfCard.StartOrderSound;
        _startEnemyTarhetAudioSource.Play();
    }

}
