using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePanelUI : MonoBehaviour{
    private TMP_Text _provocationText;
    private TMP_Text _rankText;
    private TMP_Text _scoreText;

    void Awake(){
        _provocationText = transform.Find("ProvocationText").GetComponent<TMP_Text>();
        _rankText = transform.Find("RankText").GetComponent<TMP_Text>();
        _scoreText = transform.Find("ScoreText").GetComponent<TMP_Text>();
    }

    public void SetRankText(int playerRank, int maxRank){
        string result = $"<bounce>#{playerRank} / maxRank<bounce>";
        _rankText.SetText(result);
    }

    public void SetProvocationText(string result){
        _provocationText.SetText(result);
    }

    public void SetScoreText(int killCount,int score){
        string result = $"처치 : {killCount} 점수 : {score}";
        _scoreText.SetText(result);
    }
}