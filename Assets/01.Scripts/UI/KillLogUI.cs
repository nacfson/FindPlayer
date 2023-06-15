using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;
using UnityEngine.UI;
using Febucci.UI;

public class KillLogUI: MonoBehaviour {
    [SerializeField] private TMP_Text _killLogText;
    [SerializeField] private TextAnimatorPlayer _textAnimatorPlayer;
    [SerializeField] private TextAnimator _textAnimator;
    [SerializeField] private Image _image;

    public void SetUI(Player killer, Player deader) {
        string killerValue = killer.NickName;
        string deaderValue = deader.NickName;

        string result = $"{killerValue} Killed {deaderValue}";
        _killLogText.SetText(result);
        // _killLogText.ForceMeshUpdate();
        // Vector2 textSize = _killLogText.GetRenderedValues(false);
        // _image.GetComponent<RectTransform>().sizeDelta = new Vector2(textSize.x, textSize.y);
    }

    [ContextMenu("ShowingSequence")]
    public void ShowingSequence() {
        gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMoveX(-800f, 3f).SetEase(Ease.OutExpo));
        sequence.AppendCallback(() => {
            _textAnimatorPlayer.ShowText(_killLogText.text);
        });
        sequence.AppendInterval(3f);
        sequence.AppendCallback(() => Destroy(this.gameObject));
    }
}

