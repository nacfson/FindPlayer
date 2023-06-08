using System.Runtime.CompilerServices;
using UnityEngine;
using DG.Tweening;
using Photon.Realtime;
using Photon.Pun;
public class EndPanelUI : MonoBehaviour {
    [SerializeField] private Vector3 _targetOffset;
    [SerializeField] private ScoreBoard _scoreBoard;

    public void AddScoreBoardItem(Player player){
        ScoreBoard item = Instantiate<ScoreBoard>(_scoreBoard,this.transform);
        item.SetUI(player);
    }
    [ContextMenu("ShowingSequence")]
    public void ShowingSequence() {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(_targetOffset, 1f)).SetEase(Ease.OutBounce);
        sequence.AppendCallback(()=>{
            foreach(Player player in PhotonNetwork.PlayerList){
                AddScoreBoardItem(player);
            }
        });
    }
}