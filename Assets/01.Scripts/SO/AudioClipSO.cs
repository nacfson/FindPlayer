using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Audio{
    public string clipName;
    public AudioClip clip;
}
[CreateAssetMenu(menuName = "SO/AudioList")]
public class AudioClipSO : ScriptableObject{
    public List<Audio> audios = new List<Audio>();

    public AudioClip GetAudioClipByName(string clipName){
        foreach(var a in audios){
            if(a.clipName == clipName){
                return a.clip;
            }
        }
        Debug.LogError($"Can't Find AudioClip By ClipName: {clipName}");
        return null;
    }

}