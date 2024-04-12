using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private GameManager _gameManager;

    private AudioSource _deploymentAudioSource;
    private AudioSource _deploymentEnemyAudioSource;
    private AudioSource _startTarhetAudioSource;
    private AudioSource _startEnemyTarhetAudioSource;

    private void Awake()
    {
        _gameManager = GameObject.Find("Main Camera/GameManager").GetComponent<GameManager>();

        _deploymentAudioSource = GameObject.Find("Main Camera/SoundManager/DeploymentAudioSource").GetComponent<AudioSource>();
        _deploymentEnemyAudioSource = GameObject.Find("Main Camera/SoundManager/EnemyDeploymentAudioSource").GetComponent<AudioSource>();

        _startTarhetAudioSource = GameObject.Find("Main Camera/SoundManager/StartTargetAudioSource").GetComponent<AudioSource>();
        _startEnemyTarhetAudioSource = GameObject.Find("Main Camera/SoundManager/EnemyStartTargetAudioSource").GetComponent<AudioSource>();

        _gameManager.PlayerDropCardEvent.AddListener(DeploymentSound);
        _gameManager.EnemyDropCardEvent.AddListener(EnemyDeploymentSound);

        _gameManager.OrderCard.AddListener(StartOrderSound);
        _gameManager.EnemyDropCardEvent.AddListener(EnemyStartOrderSound);
    }

    private void DeploymentSound(CardInfoScript card)
    {
        _deploymentAudioSource.clip = Resources.Load<AudioClip>("Sounds/Cards/Deployment/" + card.SelfCard.Name + Random.Range(0,6));
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
