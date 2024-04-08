using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private DropField _playerTable;
    private GameManager _gameManager;

    private AudioSource _deploymentAudioSource;
    private AudioSource _startTarhetAudioSource;

    private void Awake()
    {
        _gameManager = GameObject.Find("Main Camera/GameManager").GetComponent<GameManager>();
        _playerTable = GameObject.Find("UI/MainCanvas/PlayerTable/TableLayout").GetComponent<DropField>();

        _deploymentAudioSource = GameObject.Find("Main Camera/SoundManager/DeploymentAudioSource").GetComponent<AudioSource>();
        _startTarhetAudioSource = GameObject.Find("Main Camera/SoundManager/StartTargetAudioSource").GetComponent<AudioSource>();

        _playerTable.DropCard.AddListener(DeploymentSound);
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
