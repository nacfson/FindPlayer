using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;   
using DG.Tweening;
using Febucci.UI;

public class InformationPanel : MonoBehaviour {
    private TMP_Text _informationText;
    private TextAnimator _textAnimator;
    private TextAnimatorPlayer _textAnimatorPlayer;
    private void Awake() {
        _informationText = transform.Find("InformationText").GetComponent<TMP_Text>();
        _textAnimator = _informationText.transform.GetComponent<TextAnimator>();
        _textAnimatorPlayer = _informationText.transform.GetComponent<TextAnimatorPlayer>();
    }

    public void ShowingSequence(string text,float showing) {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMoveX(800f,1f));
        sequence.AppendCallback(() => {
            _informationText.SetText(text);
            _textAnimatorPlayer.ShowText(text);

            StartCoroutine(DelayCallback(showing, () => Destroy(this.gameObject)));
        });
    }
    IEnumerator DelayCallback(float delay, Action action = null) {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }


}