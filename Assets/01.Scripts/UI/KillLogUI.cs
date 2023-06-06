using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;
public class KillLogUI: MonoBehaviour {
    [SerializeField] private TMP_Text _killLogText;

    public void SetUI(Player killer, Player deader) {
        string killerValue = killer.NickName;
        string deaderValue = deader.NickName;

        string result = $"{killerValue} Killed {deaderValue}";
        _killLogText.SetText(result);
    }
    [ContextMenu("ShowingSequence")]
    public void ShowingSequence() {
        gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMoveX(-500f, 1.5f).SetEase(Ease.OutExpo));
        sequence.AppendInterval(3f);
        sequence.AppendCallback(() => Destroy(this.gameObject));
    }



}

