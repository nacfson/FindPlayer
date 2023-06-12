using System.Runtime.CompilerServices;
using UnityEngine;
using DG.Tweening;
using Photon.Realtime;
using Photon.Pun;
using System.Collections.Generic;

public class EndPanelUI : MonoBehaviour {
    [SerializeField] private Vector3 _targetOffset;
    [SerializeField] private ScoreBoard _scoreBoard;

    private List<ScoreBoard> _scoreBoardList = new List<ScoreBoard>();

    public void AddScoreBoardItem(Player player){
        ScoreBoard item = Instantiate<ScoreBoard>(_scoreBoard,this.transform);
        item.SetUI(player);
        _scoreBoardList.Add(item);
    }

    [ContextMenu("ShowingSequence")]
    public void ShowingSequence(float delay =  0f) {

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(delay);
        sequence.Append(transform.DOLocalMove(_targetOffset, 0.3f)).SetEase(Ease.OutBounce);
        sequence.AppendCallback(()=>{
            foreach(Player player in PhotonNetwork.PlayerList){
                AddScoreBoardItem(player);
            }

            ScoreBoard bestPlayer = null;
            foreach(ScoreBoard sb in _scoreBoardList) {
                if(bestPlayer == null) {
                    bestPlayer = sb;
                    continue;
                }
                if(sb.score > bestPlayer.score) {
                    bestPlayer = sb;
                }
            }
            bestPlayer.WinGame();
        });
    }
}