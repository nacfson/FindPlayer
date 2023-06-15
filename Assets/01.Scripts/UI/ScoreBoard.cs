using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class ScoreBoard : MonoBehaviour{
    private TMP_Text _playerNameText;
    private TMP_Text _killCountText;
    private TMP_Text _scoreText;
    private string _playerName;

    public int score;

    private void Awake() {
        _playerNameText = transform.Find("PlayerNameText").GetComponent<TMP_Text>();
        _killCountText = transform.Find("KillCountText").GetComponent<TMP_Text>();
        _scoreText = transform.Find("ScoreText").GetComponent<TMP_Text>();
    }

    public void SetUI(Player player){
        _playerName = player.NickName;
        _playerNameText.SetText($"닉네임: {_playerName}");
        Hashtable hashTable = player.CustomProperties;

        score = (int)hashTable["SCORE"];
        _killCountText.SetText($"킬: {hashTable["KILLCOUNT"]}");
        _scoreText.SetText($"점수: {score}");
    }

    public void WinGame() {
        _playerNameText.SetText($"1등: <rainb>{_playerName}<rainb>");
    }
}