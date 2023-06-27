using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SitPlayer : MonoBehaviour{
    private WayPoint _wayPoint;
    [SerializeField] private TMP_Text _nickNameTxt;


    public bool IsEnabled;
    public WayPoint WayPoint{
        get{
            return _wayPoint;
        }
        set{
            _wayPoint = value;
        }
    }

    public void SetNickName(string nickName){
        _nickNameTxt.SetText(nickName);
    }

}