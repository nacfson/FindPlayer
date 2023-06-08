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
        _playerNameText.SetText(_playerName);
        Hashtable hashTable = player.CustomProperties;

        score = (int)hashTable["SCORE"];
        _killCountText.SetText(hashTable["KILLCOUNT"].ToString());
        _scoreText.SetText(score.ToString());
    }

    public void WinGame() {
        _playerNameText.SetText($"<rainb>{_playerName}<rainb>");
    }
}