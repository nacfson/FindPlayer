using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(AudioSource))]
public class PlayerSound : MonoBehaviourPun{
    [SerializeField] private AudioClipSO _audioSO;
    [SerializeField] private string _walkClipName;
    [SerializeField] private string _invisibleClipName;

    public float maxDistance = 10f;

    private AudioSource _audioSource;
    private AgentAnimator _agentAnimator;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
        _agentAnimator = transform.Find("Visual").GetComponent<AgentAnimator>();
    }

    private void Start() {
        _agentAnimator.OnWalkTrigger += PlayWalkSound;
    }
    
    public void PlayWalkSound() {
        PlaySound(_walkClipName);
    }
    public void PlayInvisibleSound(){
        PlaySound(_invisibleClipName);
    }

    public void PlaySound(string clipName,bool others = true) {
        _audioSource.clip = _audioSO.GetAudioClipByName(clipName);
        _audioSource.Play();
        //Debug.LogError(_audioSource.clip.name);
        // RPC를 사용하여 다른 플레이어에게도 소리를 전달

        //그런데 플레이어가 거리에 따라 소리의 크기가 다르게 전달 받아야되는데 이렇게 되면 그냥 기존 소리 그대로 전달 받게 됨
        if (others) {
            photonView.RPC("PlaySoundRPC", RpcTarget.All,clipName);
        }
    }
    
    [PunRPC]
    public void PlaySoundRPC(string clipName) {
        _audioSource.clip = _audioSO.GetAudioClipByName(clipName);
        _audioSource.Play();
    }

}