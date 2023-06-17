using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
[RequireComponent(typeof(AudioSource))]
public class UISoundManager : MonoBehaviour {
    private AudioSource _audioSource;
    public static UISoundManager Instance;

    [SerializeField] private AudioClip _clickClip;
    [SerializeField] private AudioClip _startClip;
    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
        _audioSource = GetComponent<AudioSource>();
    }
    public void PlayClickAudio(Action AudioCallback = null) {
        PlayClip(_clickClip,AudioCallback);
    }
    public void PlayClip(AudioClip clip,Action AudioCallback = null) {
        _audioSource.Stop();
        _audioSource.clip = clip;
        _audioSource.Play();
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(clip.length + 0.2f);
        sequence.AppendCallback(() => {
            AudioCallback?.Invoke();
        });
    }
}