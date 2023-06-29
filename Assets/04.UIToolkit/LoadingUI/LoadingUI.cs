using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadingUI : MonoBehaviour{
    private UIDocument _document;
    public event Action<float> Callback;

    private VisualElement _loadingUI;
    private ProgressBar _bar;
    private VisualElement _character;
    [SerializeField] private float _timerMinValue = 10f;

    private static LoadingUI _instance;
    public static LoadingUI Instance{
        get{
            return _instance;
        }
    }

    private void Awake() {
        if(_instance == null){
            _instance = this;
        }
        else{
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    

    private void OnEnable() {
        _document = GetComponent<UIDocument>();
        VisualElement root = _document.rootVisualElement;

        VisualElement parent = root.Q<VisualElement>("VisualElement");

        _loadingUI = parent.Q<VisualElement>("LoadingUI");
        _character = _loadingUI.Q<VisualElement>("Character");
        _bar = _loadingUI.Q<ProgressBar>("LoadingBar");
    }


    public void SetLoadingBarValue(float value){
        _bar.value = value * 100f;
    }

    public void LoadLevel(int sceneIndex, bool autoLoad = true, bool loadingAsync = true,Action Callback = null){
        PhotonNetwork.AutomaticallySyncScene = autoLoad;

        if(loadingAsync){
            StartCoroutine(AsyncLoadCor(sceneIndex,Callback));
        }
        else{
            StartCoroutine(RestartGameCor(sceneIndex,Callback));
        }
    }

    private IEnumerator RestartGameCor(int sceneIndex,Action Callback){
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        while(!asyncLoad.isDone){
            yield return null;
        }

        PhotonNetwork.LoadLevel(sceneIndex);
        RoomManager.Instance.RestartGame();
    }

    private IEnumerator AsyncLoadCor(int sceneIndex,Action Callback){
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        float timer = 0f;

        while(!asyncLoad.isDone || timer < _timerMinValue){
            _loadingUI.AddToClassList("active");
            timer += Time.deltaTime;

            float percent = asyncLoad.progress < timer / _timerMinValue ? asyncLoad.progress : timer / _timerMinValue;
            percent = easeOutExpo(percent);

            float smoothPercent= Mathf.Lerp(0f,percent,Time.deltaTime * 8f);
            _character.style.left = percent * _bar.resolvedStyle.width;

            SetLoadingBarValue(percent);
            Debug.Log($"Percent: {percent}");

            yield return null;
        }
        _loadingUI.RemoveFromClassList("active");

        PhotonNetwork.LoadLevel(sceneIndex);
        Callback?.Invoke();
    }
    
    float easeInBounce(float x) {
        return 1 - easeOutBounce(1 - x);
    }

    float easeOutBounce(float x) {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (x < 1 / d1) {
            return n1 * x * x;
        } else if (x < 2 / d1) {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        } else if (x < 2.5 / d1) {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        } else {
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }
    float easeOutExpo(float x){
        return x == 1 ? 1 : 1 - MathF.Pow(2, -10 * x);
    }
}