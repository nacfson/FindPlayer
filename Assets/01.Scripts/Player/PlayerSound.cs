using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerSound : MonoBehaviourPun{
    public AudioClip soundClip;
    public float maxDistance = 10f;

    private AudioSource audioSource;
    private AgentAnimator _agentAnimator;

    [SerializeField] private AudioClip _walkSound;


    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
    }
    private void Start() {
        _agentAnimator.OnWalkTrigger += PlayWalkSound;
    }

    private void PlaySound(bool others = true) {
        audioSource.clip = soundClip;
        audioSource.Play();

        // RPC를 사용하여 다른 플레이어에게도 소리를 전달
        if (others) {
            photonView.RPC("PlaySoundRPC", RpcTarget.Others);
        }
    }
    public void PlayWalkSound() {
        PlaySound();
    }

    [PunRPC]
    private void PlaySoundRPC() {
        audioSource.clip = soundClip;
        audioSource.Play();
    }
}