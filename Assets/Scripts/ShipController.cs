//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using ProceduralToolkit;
using System;

public class ShipController : MonoBehaviour
{
    EventChannelDespairScoreTick _eventChannelDespairScoreTick;
    EventChannelScanInfo _eventChannelScanInfo;
    EventChannelGameFlow _eventChannelGameFlow;


    ShipOrbitalStatus CurrentStatus = ShipOrbitalStatus.Arriving;
    ShipTask _curTask = ShipTask.FTL;

    public ShipTask CurrentTask
    {
        get { return _curTask; }
        set
        {
            _curTask = value;
            // if (isStunned) myOverlay.GetComponent<ShipUIOverlay>().UpdateTaskText("Stunned, resting");
            // else myOverlay.GetComponent<ShipUIOverlay>().UpdateTaskText();
            // if (isStunned) _overlay.SetTaskLabel("Stunned");
            if (isStunned) _overlay.SetStatusLabel("Stunned");
            else if (isFrozen) _overlay.SetStatusLabel("Frozen");
            else _overlay.SetStatusLabel("");


            _overlay.SetTaskLabel($"{_curTask}");

            taskCounter = 0;
        }
    }

    public float HP
    {
        get => _hp; set
        {
            _hp = Mathf.Clamp(value, 0, 100f);
            _overlay.SetHPLabel(Mathf.FloorToInt(_hp).ToString());
        }
    }
    public float Despair
    {
        get => _despair; set
        {
            _despair = Mathf.Clamp(value, 0, 100f);
            _overlay.SetDespairLabel(Mathf.FloorToInt(_despair).ToString());

        }
    }
    public float Intel
    {
        get => _intel; set
        {
            _intel = Mathf.Clamp(value, 0, 100f);
            _overlay.SetInfoLabel(Mathf.FloorToInt(_intel).ToString());
        }
    }

    ShipTypeSO _typeData;

    float taskCounter = 0;

    //GAME PARAMS   
    float _hp = 100;
    float _despair = 0;
    float _intel = 0;
    string _shipName;

    bool isFrozen;
    bool isStunned;


    float FTLSpeed = 50f;

    float _orbitHeight;
    float _orbitInsertionAngle;
    Vector3 OrbitInsertionPoint;
    SpriteRenderer _spriteRenderer;

    [SerializeField] TrailRenderer FTLTrail;
    private Dictionary<ShipTask, float> TaskWeights = new Dictionary<ShipTask, float>();
    int dir;

    GameObject myOverlay;

    public float CurrentPositionAngle;


    void Subscribe()
    {
        _eventChannelDespairScoreTick = RuntimeEventChannelContainer.Instance.EventChannelDespairScoreTickInstance;
        _eventChannelScanInfo = RuntimeEventChannelContainer.Instance.EventChannelScanInfoInstance;
        _eventChannelGameFlow = RuntimeEventChannelContainer.Instance.EventChannelGameFlowInstance;

        _eventChannelGameFlow.OnTriggerColonisationEvent += Callback_OnTriggerColonisationEvent;
    }

    void UnSubscribe()
    {
        _eventChannelGameFlow.OnTriggerColonisationEvent -= Callback_OnTriggerColonisationEvent;
    }

    private void Callback_OnTriggerColonisationEvent()
    {
        Depart();
    }
    internal void Init(ShipTypeSO typeSO, string shipName, float radius, float angle)
    {
        _typeData = typeSO;
        _shipName = shipName;
        _orbitHeight = radius;
        _orbitInsertionAngle = angle;

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        TaskWeights.Add(ShipTask.Scanning, 0.5f);
        TaskWeights.Add(ShipTask.Repairs, 0f);
        TaskWeights.Add(ShipTask.Fighting, 0f);
        TaskWeights.Add(ShipTask.Resting, 0f);

        CurrentPositionAngle = _orbitInsertionAngle;
        OrbitInsertionPoint = OrbitRenderer.GetPoint(_orbitHeight, _orbitInsertionAngle);
        dir = UnityEngine.Random.Range(0f, 1f) < 0.5f ? 1 : -1;
        // sprite.color = sprite.color.WithA(0f);

        transform.position = OrbitRenderer.GetPoint(_orbitHeight, _orbitInsertionAngle).normalized * 50f;


        Subscribe();
        // StartCoroutine(ArrivingCoroutine());
        _spriteRenderer.DOFade(1f, 1f);


    }

    public void SetOverlay(GameObject overlay)
    {
        myOverlay = overlay;
    }
    UI_DBG_ShipOverlay _overlay;
    public void SetDBGOverlay(UI_DBG_ShipOverlay dbgOverlay)
    {
        _overlay ??= dbgOverlay;
        _overlay.SetNameLabel(_shipName);
    }

    float timer = 1f;
    float cTimer = 0f;

    void Update()
    {
        switch (CurrentStatus)
        {
            case ShipOrbitalStatus.Arriving:
                UpdateArrivingPosition();
                break;
            case ShipOrbitalStatus.Orbiting:
                UpdateOrbitalPosition();
                CheckAndUpdateParams();
                break;
            case ShipOrbitalStatus.Departing:
                UpdateDepartingPosition();
                CurrentTask = ShipTask.FTL;
                break;
        }

        cTimer += Time.deltaTime;
        if (cTimer > timer)
        {
            cTimer = cTimer - timer;
            UpdateAI();
        }

    }

    void UpdateAI()
    {
        if (CurrentStatus == ShipOrbitalStatus.Orbiting)
        {
            CheckAndUpdateTask();
        }
    }


    void UpdateOrbitalPosition()
    {
        CurrentPositionAngle += ((20 - _orbitHeight) / 2f) * Time.deltaTime * dir;
        transform.position = OrbitRenderer.GetPoint(_orbitHeight, CurrentPositionAngle);
        int mod = dir > 0 ? -1 : 1;
        transform.up = mod * new Vector3(transform.position.y, -1 * transform.position.x, 0).normalized;
        //sprite.transform.rotation =  (transform.position - GameManager.Instance.Planet.transform.position)
    }
    IEnumerator BoostCoroutine()
    {
        AudioManager.PlaySound("boost");
        FTLTrail.gameObject.SetActive(false);
        // Debug.Log("boost rootine");
        int mod = dir > 0 ? -1 : 1;
        transform.DOLocalRotate(mod * new Vector3(transform.position.y, -1 * transform.position.x, 0).normalized, 0.2f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.2f);
        CurrentStatus = ShipOrbitalStatus.Orbiting;
        // transform.up = mod * new Vector3(transform.position.y, -1 * transform.position.x, 0).normalized;

        StartCoroutine(TickResources());
        GetComponentInChildren<OrbitRenderer>().Show(_orbitHeight);
    }

    Coroutine boostRoutine;
    void UpdateArrivingPosition()
    {
        if (boostRoutine != null) return;
        if (Vector3.Distance(transform.position, Vector3.zero) <= _orbitHeight)
        {

            boostRoutine = StartCoroutine(BoostCoroutine());
            return;
        }

        Vector3 l_dir = (OrbitInsertionPoint - transform.position).normalized;
        float dist = Vector3.Distance(transform.position, OrbitInsertionPoint);


        transform.up = l_dir;
        transform.position += l_dir * FTLSpeed * Time.deltaTime;
        FTLSpeed = dist * 2f + 2;


    }

    // public void CompareShipData()
    // {
    //     _eventChannelScanInfo.RaiseOnScanInfoLevelChangedEvent(Mathf.FloorToInt(Intel * TypeData.IntelMod));

    //     if (tortureTime > GameManager.Instance.LongestTortureTime)
    //     {
    //         GameManager.Instance.LongestTortureTime = tortureTime;
    //         GameManager.Instance.LongestTortureName = ShipName;
    //         GameManager.Instance.LongestTortureType = (int)TypeData.typeID;

    //         PlayerPrefs.SetString("_torturedName", ShipName);
    //         PlayerPrefs.SetFloat("_torturedTime", tortureTime);
    //         PlayerPrefs.SetInt("_torturedtype", (int)TypeData.typeID);
    //     }
    // }
    void UpdateDepartingPosition()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) >= 100)
        {
            // Destroy(myOverlay);
            Destroy(gameObject);

            // CompareShipData();

            GameManager.Instance.ShipCount--;
            if (GameManager.Instance.ShipCount == 0)
            {
                _eventChannelGameFlow.RaiseOnTriggerColonisationEvent();
                Debug.Log("GameOver");
            }
            return;
        }
        Vector3 dir = this.dir > 0 ? transform.up : -transform.up;

        float dist = Vector3.Distance(transform.position, DepartingPosition);


        transform.position += -dir * FTLSpeed * Time.deltaTime;
        FTLSpeed = dist / 0.5f;

    }

    private float critHP = 40;


    void CheckAndUpdateTask()
    {

        float nHp = HP / 100f;
        float nDp = Despair / 100f;

        if (isBoarded) TaskWeights[ShipTask.Fighting] = 2;
        else TaskWeights[ShipTask.Fighting] = 0;


        TaskWeights[ShipTask.Scanning] = 0.1f;

        //switchWeightHP = HP < critHP ? 1 : (taskCounter - minDelta) / (maxDelta - minDelta);
        //switchWeightDesp = HP < critHP ? 0 : (taskCounter - minDelta) / (maxDelta - minDelta);


        TaskWeights[ShipTask.Repairs] = (1 - nHp) * ((1 - nHp) / (1 - critHP / 100f));// * switchWeightHP;
        if (isFrozen) TaskWeights[ShipTask.Repairs] = 0;
        TaskWeights[ShipTask.Resting] = nDp;// * switchWeightDesp;
        if (isStunned) TaskWeights[ShipTask.Resting] = 1000;
        //TaskWeights[ShipTask.Repair] = HP < critHP ? 1 : TaskWeights[ShipTask.Repair];




        float maxWeight = -1f;
        ShipTask nextTask = CurrentTask;
        foreach (var weight in TaskWeights)
        {
            if (weight.Value > maxWeight)
            {
                maxWeight = weight.Value;
                nextTask = weight.Key;
            }
        }
        if (nextTask != CurrentTask)
        {
            CurrentTask = nextTask;
            taskCounter = 0f;
        }
        else
        {
            taskCounter += Time.deltaTime;
        }
    }

    void CheckAndUpdateParams()
    {

        if (Intel >= 100)
        {
            Depart();

            /*  if (HP <= 100) TaskWeights[ShipTask.Scanning] = 0;
              else
              {

                  //StopCoroutine(TickResources());
              }
              */
            return;
        }
        if (HP <= 0)
        {
            DieHP();
            StopCoroutine(TickResources());
            return;
        }
        if (Despair >= 100)
        {
            isStunned = true;

            //StopCoroutine(TickResources());
            return;
        }
        if (Despair <= 100 && isStunned)
        {
            isStunned = false;
            //StartCoroutine(TickResources());
            return;
        }
    }

    public float enemyHP;

    public bool isShakingIntel = false;
    IEnumerator TickResources()
    {
        while (true)
        {
            switch (CurrentTask)
            {
                case ShipTask.Scanning:
                    Intel += 1 * _typeData.IntelMod;

                    if (Intel >= 80 && !isShakingIntel)
                    {
                        // myOverlay.GetComponent<ShipUIOverlay>().IntelBar.transform.DOScale(1.2f, 2f).SetLoops(-1).SetEase(Ease.InOutCirc);
                        StartCoroutine(PingRoutine());
                        isShakingIntel = true;
                    }
                    if (Intel <= 80 && isShakingIntel)
                    {
                        // myOverlay.GetComponent<ShipUIOverlay>().IntelBar.transform.DOKill(true);
                        StopCoroutine(PingRoutine());
                        isShakingIntel = false;
                    }

                    break;
                case ShipTask.Repairs:
                    HP += 1 * _typeData.RepairMod;

                    break;
                case ShipTask.Fighting:
                    if (enemyHP > 0)
                    {
                        float x = UnityEngine.Random.Range(1, 3) * _typeData.FightMod;
                        HP -= x;
                        enemyHP -= x;
                    }
                    else
                    {
                        TaskWeights[ShipTask.Fighting] = 0;
                        isBoarded = false;
                    }

                    break;
                case ShipTask.Resting:
                    Despair -= (2 * _typeData.DespairMod);
                    break;
            }
            Despair -= UnityEngine.Random.Range(0, +2) * _typeData.DespairMod;
            if (Despair < 0) Despair = 0;
            int deltaScore = Mathf.FloorToInt(Despair / 10f);
            if (deltaScore > 0)
            {
                _eventChannelDespairScoreTick.RaiseOnDespairTickEvent(deltaScore);
                InterfaceManager.Instance.ShowAnimatedLabel(Color.white, $"+{deltaScore}", transform.position); //TODO: mvoe to events
            }

            yield return new WaitForSeconds(1f);
        }
    }


    public void Damage(int dmg)
    {
        HP -= dmg;


        Color c = _spriteRenderer.color;
        _spriteRenderer.color = Color.yellow;
        _spriteRenderer.DOColor(c, 0.2f);
        if (HP <= 0) DieHP();
    }
    void DieHP()
    {
        GetComponentInChildren<OrbitRenderer>().Hide();
        StopCoroutine(PingRoutine());
        Destroy(_overlay.gameObject);
        Destroy(gameObject);
        //Boom
    }
    Vector3 DepartingPosition;
    void Depart()
    {

        StopAllCoroutines();
        // StopCoroutine(PingRoutine());

        // StopCoroutine(TickResources());
        FTLTrail.gameObject.SetActive(true);
        DepartingPosition = transform.position;
        CurrentStatus = ShipOrbitalStatus.Departing;
        GetComponentInChildren<OrbitRenderer>().Hide();
        UnSubscribe();

    }

    public bool isBoarded = false;
    public IEnumerator PhysicsRoutine(float duration)
    {
        isFrozen = true;
        // var overlay = myOverlay.GetComponent<ShipUIOverlay>();
        // Debug.Log("Before frozen routine");
        // overlay.StartCoroutine(overlay.TimerMaskCoroutine(duration));
        yield return new WaitForSeconds(duration);
        Debug.Log("after frozen routine");

        isFrozen = false;
    }

    public bool isJammed = false;
    public IEnumerator InterferenceRoutine(float duration)
    {
        isJammed = true;
        _typeData.IntelMod *= 0.1f;
        // var overlay = myOverlay.GetComponent<ShipUIOverlay>();
        // overlay.StartCoroutine(overlay.TimerMaskCoroutine(duration));
        yield return new WaitForSeconds(duration);
        _typeData.IntelMod *= 10f;
        isJammed = false;

    }

    public IEnumerator PingRoutine()
    {
        while (true)
        {
            AudioManager.PlaySound("ping");
            yield return new WaitForSeconds(3f);
        }
    }


}
public enum ShipTask
{
    FTL, Scanning, Repairs, Fighting, Resting, Frozen
}
public enum ShipOrbitalStatus
{
    Arriving, Orbiting, Departing
}