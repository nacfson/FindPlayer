using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;

[RequireComponent(typeof(PhotonView))]
public class ItemObject : MonoBehaviour{
    [HideInInspector] public Item item;
    private Transform _getTransform;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] protected ParticleSystem _useParticle;
    protected PhotonView _PV;

    protected ParticleSystem _particle;
    private GameObject _character;

    private void Awake() {
        _getTransform = this.transform;
        _particle = GetComponent<ParticleSystem>();
        _character = transform.Find("Character").gameObject;
        item = GetComponent<Item>();
        _PV = GetComponent<PhotonView>();
        StartCoroutine(CheckColliderCor());
    }
    IEnumerator CheckColliderCor(){
        while (true) {
            if (RoomManager.Instance.CurrentState == GAME_STATE.LOADING) yield return null;
            Collider[] cols = Physics.OverlapSphere(_getTransform.position, 0.8f, _layerMask);

            if (cols.Length > 0) {
                foreach (Collider col in cols) {
                    if (col.transform.parent.TryGetComponent<AgentItem>(out AgentItem agentItem)) {
                        agentItem.UseItem(() => item.UseItem(agentItem));
                        ParticleSystem p = Instantiate<ParticleSystem>(_useParticle, transform.position, Quaternion.identity);
                        p.Play();
                        Destroy(p.gameObject, 1f);
                        Sequence sequence = DOTween.Sequence();
                        sequence.Append(_character.transform.DOScale(Vector3.zero, 1f).SetEase(Ease.OutCirc));
                        sequence.AppendCallback(() => Destroy(_character.gameObject));
                        _character = null;
                        _particle.Stop();
                        StopAllCoroutines();
                    }
                }
            }
            if (_character != null) {
                _character.transform.Rotate(Vector3.up * 60f * Time.deltaTime);
            }
            yield return null;
        }
    }
    //private void OnDrawGizmos() {
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawRay(_getTransform.position, Vector3.up * 10f);
    //}
}