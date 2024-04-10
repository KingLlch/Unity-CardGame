using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private GameManager _gameManager;

    private AudioSource _deploymentAudioSource;
    private AudioSource _startTarhetAudioSource;

    private void Awake()
    {
        _gameManager = GameObject.Find("Main Camera/GameManager").GetComponent<GameManager>();

        _deploymentAudioSource = GameObject.Find("Main Camera/SoundManager/DeploymentAudioSource").GetComponent<AudioSource>();
        _startTarhetAudioSource = GameObject.Find("Main Camera/SoundManager/StartTargetAudioSource").GetComponent<AudioSource>();

        _gameManager.PlayerDropCardEvent.AddListener(DeploymentSound);
        _gameManager.EnemyDropCardEvent.AddListener(DeploymentSound);

        _gameManager.OrderCard.AddListener(StartOrderSound);
        _gameManager.EnemyDropCardEvent.AddListener(StartOrderSound);
    }

    private void DeploymentSound(CardInfoScript card)
    {
        _deploymentAudioSource.clip = card.SelfCard.DeploymentSound;
        _deploymentAudioSource.Play();
    }

    private void StartOrderSound(CardInfoScript card)
    {
        _startTarhetAudioSource.clip = card.SelfCard.StartOrderSound;
        _startTarhetAudioSource.Play();
    }

}
