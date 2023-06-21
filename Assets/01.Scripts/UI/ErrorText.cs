using UnityEngine;
using Febucci.UI;
using DG.Tweening;
public class ErrorText : MonoBehaviour{
    [SerializeField] private TextAnimator _textAnimator;
    [SerializeField] private TextAnimatorPlayer _textAnimatorPlayer;

    public void ShowingSequence(string text){
        _textAnimatorPlayer.ShowText($"<shake>{text}<shake>");
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(2f);
        //sequence.AppendCallback(()=>_textAnimatorPlayer.StopShowingText());
    }
}