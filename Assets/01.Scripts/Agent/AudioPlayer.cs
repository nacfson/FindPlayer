using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour {
    protected AudioSource _audioSource;
    [SerializeField]
    protected float _pitchRandomness = 0.2f;
    protected float _basePitch;
    protected PhotonView _PV;

    protected virtual void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start() {
        _basePitch = _audioSource.pitch;
    }
    //Ŭ���� ������ġ�� ����ϴ� �Լ�
    protected void PlayClipWithVariablePitch(AudioClip clip) {
        float randomPitch = Random.Range(-_pitchRandomness, _pitchRandomness);
        _audioSource.pitch = _basePitch + randomPitch;
        PlayClip(clip);
    }
    //��ġ �������� �׳� ����ϴ� �Լ�
    protected void PlayClip(AudioClip clip) {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}