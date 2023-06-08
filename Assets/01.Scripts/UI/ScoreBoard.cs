using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class ScoreBoard : MonoBehaviour{
    private TMP_Text _playerNameText;
    private TMP_Text _killCountText;
    private TMP_Text _scoreText;

    private void Awake() {
        _playerNameText = transform.Find("PlayerNameText").GetComponent<TMP_Text>();
        _killCountText = transform.Find("KillCountText").GetComponent<TMP_Text>();
        _scoreText = transform.Find("ScoreText").GetComponent<TMP_Text>();
    }

    public void SetUI(Player player){
        _playerNameText.SetText(player.NickName);
        Hashtable hashTable = player.CustomProperties;

        _killCountText.SetText(hashTable["KILLCOUNT"].ToString());
        _scoreText.SetText(hashTable["SCORE"].ToString());
    }
}