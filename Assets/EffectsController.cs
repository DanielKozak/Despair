using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EffectsController : Singleton<EffectsController>
{

    [SerializeField] ParticleSystem _effectShipDebris;


    public void ShowShipDebris(Vector3 position, Vector3 up)
    {
        _effectShipDebris.transform.position = position;
        _effectShipDebris.transform.up = up;
        _effectShipDebris.gameObject.SetActive(true);
        _effectShipDebris.Play();
        DOVirtual.DelayedCall(_effectShipDebris.main.duration, () => _effectShipDebris.gameObject.SetActive(false));
    }
}
