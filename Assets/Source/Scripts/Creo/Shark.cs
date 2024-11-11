using Cinemachine;
using DG.Tweening;
using Kuhpik;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Shark : MonoBehaviour
{
    [SerializeField] Transform RaftRotation;
    [SerializeField] Transform Destination;
    [SerializeField] float Speed;
    private bool go;
    [SerializeField] GameObject LosePanel;
    [SerializeField] ParticleSystem StunPS;
    [SerializeField] Transform newShipDestination;
    [SerializeField] Transform sharkJumpPosition;
    [SerializeField] Transform sharkJawsPosition;
    [SerializeField] ParticleSystem HitPS;
    [SerializeField] ParticleSystem SplashPS;
    [SerializeField] Transform shipJumpPosition;

    private void Start()
    {
        Speed = (transform.position - Destination.position).magnitude / 2;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            go = true;
        }
        if (go)
        {
            transform.position = Vector3.MoveTowards(transform.position, Destination.position, Speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Raft"))
        {
            go = false;
            transform.DOJump(sharkJumpPosition.position, 5, 1, 1f).OnComplete(() => { go = true;SplashPS.Play();});
            //other.transform.parent.DORotate(RaftRotation.eulerAngles,0.4f);
            var coll=other.GetComponents<Collider>();
            foreach (Collider c in coll)
            {
                c.enabled= false;
            }
            Bootstrap.Instance.ChangeGameState(GameStateID.Game);
            DOVirtual.DelayedCall(0.3f, () => {
                LosePanel.SetActive(true); LosePanel.transform.DOScale(1, 0.4f);
            });
        }
        if (other.CompareTag("Ship"))
        {
            go = false;
            GetComponent<Collider>().enabled = false;
            transform.DOJump(shipJumpPosition.position, 2, 1, .6f).OnComplete(() => {
                Destination.position = transform.position;
                HitPS.Play();
                transform.DOMoveY(-1.1f, 0.5f).OnComplete(() => {
                    SplashPS.Play();
                    transform.DORotate(new Vector3(0, 0, 180), 0.4f);
                    StunPS.Play();
                });
            });
            other.GetComponent<BoatComponent>().SetSpeed();
            //other.GetComponent<BoatComponent>().SetDestination(newShipDestination);
        }
        if (other.CompareTag("Player"))
        {
            Speed = 10;
            GameObject.Find("DefaultVC").GetComponent<CinemachineVirtualCamera>().Follow = null;
            transform.LookAt(Destination.position);
            DOVirtual.DelayedCall(0.15f, () =>
            {
                other.transform.SetParent(sharkJawsPosition);
                other.transform.position = sharkJawsPosition.position;
                other.transform.rotation = sharkJawsPosition.rotation;
                GameObject.FindGameObjectWithTag("Raft").transform.parent.DORotate(RaftRotation.eulerAngles, 0.4f);
            });
        }
    }
}
