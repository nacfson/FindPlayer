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

    public float maxDistance = 10f; // �ִ� �Ÿ�
    public float minVolume = 0.1f; // �ּ� �Ҹ� ũ��
    public float maxVolume = 1f; // �ִ� �Ҹ� ũ��

    private AudioSource _audioSource;
    private PhotonView _PV;
    private AgentAnimator _agentAnimator;

    private void Awake() {
        _PV = GetComponent<PhotonView>();
        _audioSource = _PV.GetComponent<AudioSource>();
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
        // RPC�� ����Ͽ� �ٸ� �÷��̾�Ե� �Ҹ��� ����

        //�׷��� �÷��̾ �Ÿ��� ���� �Ҹ��� ũ�Ⱑ �ٸ��� ���� �޾ƾߵǴµ� �̷��� �Ǹ� �׳� ���� �Ҹ� �״�� ���� �ް� ��
        if (others) {
            photonView.RPC("PlaySoundRPC", RpcTarget.All,clipName,transform.position);
        }
    }
    
    [PunRPC]
    public void PlaySoundRPC(string clipName,Vector3 startPos) {
        _audioSource.clip = _audioSO.GetAudioClipByName(clipName);


        _audioSource.spatialBlend = 1f;
        float distance = Vector3.Distance(transform.position, startPos);
        float volume = Mathf.Lerp(maxVolume, minVolume, distance / maxDistance);
        _audioSource.volume = volume;
        _audioSource.Play();
    }

}