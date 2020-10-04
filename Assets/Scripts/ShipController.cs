//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShipController : MonoBehaviour
{
    public enum Status
    {
        Arriving, Orbiting, Departing, Dead
    }
    public Status CurrentStatus = Status.Arriving;
    public enum ShipTask
    {
        FTL, Scanning, Repairs, Fighting, Resting, Frozen
    }

    private ShipTask _curTask = ShipTask.FTL;

    public ShipTask CurrentTask
    {
        get { return _curTask; }
        set
        {
            _curTask = value;
            myOverlay.GetComponent<ShipUIOverlay>().UpdateTaskText();
            taskCounter = 0;
        }
    }
    float taskCounter = 0;

    //GAME PARAMS   
    public float HP = 100;
    public float Despair = 0;
    public float Intel = 0;

    public bool isFrozen;
    public bool isStunned;


    public string ShipName;

    public float tortureTime = 0f;

    public float RepairMod = 1f;
    public float IntelMod = 1f;
    public float DespairMod = 1f;

    public float FightMod = 1f;


    public int OrbitalSpeed = 20;
    public float OrbitHeight;
    public float OrbitInsertionAngle;
    public Vector3 OrbitInsertionPoint;
    public SpriteRenderer sprite;

    public TrailRenderer FTLTrail;
    Vector3[] path;
    int currentPositionIndex;
    int pathPrecision = 360;
    private Dictionary<ShipTask, float> TaskWeights = new Dictionary<ShipTask, float>();
    int clockwise;
    int dir;

    GameObject myOverlay;

    public GameObject timerCircle;

    public void onGameOver(object sender, System.EventArgs e)
    {
        Depart();
    }

    void Start()
    {
        GameManager.OnGameOver += onGameOver;
        path = OrbitRenderer.GetPoints(OrbitHeight, pathPrecision);
        TaskWeights.Add(ShipTask.Scanning, 0.5f);
        TaskWeights.Add(ShipTask.Repairs, 0f);
        TaskWeights.Add(ShipTask.Fighting, 0f);
        TaskWeights.Add(ShipTask.Resting, 0f);
        CurrentPositionAngle = OrbitInsertionAngle;
        OrbitInsertionPoint = OrbitRenderer.GetPoint(OrbitHeight, OrbitInsertionAngle);

        dir = Random.Range(0f, 1f) < 0.5f ? 1 : -1;
        //if (dir > 0) gameObject.transform.localScale = new Vector3(1, -1, 1);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);
        minDelta = Random.Range(0.2f, 0.8f);
    }
    public void SetOverlay(GameObject overlay)
    {
        myOverlay = overlay;
    }


    // Update is called once per frame
    void Update()
    {
        tortureTime += Time.deltaTime;
        switch (CurrentStatus)
        {
            case Status.Arriving:
                UpdateArrivingPosition();
                break;
            case Status.Orbiting:
                UpdateOrbitalPosition();
                CheckAndUpdateParams();
                CheckAndUpdateTask();
                break;
            case Status.Departing:
                UpdateDepartingPosition();
                CurrentTask = ShipTask.FTL;
                break;
            case Status.Dead:
                //UpdateOrbitalPosition();
                break;
        }
        /*if (Input.GetKeyUp(KeyCode.F1))
        {
            Depart();
        }*/

    }


    public float CurrentPositionAngle;
    void UpdateOrbitalPosition()
    {
        CurrentPositionAngle += ((20 - OrbitHeight) / 2f) * Time.deltaTime * dir;
        transform.position = OrbitRenderer.GetPoint(OrbitHeight, CurrentPositionAngle);
        int mod = dir > 0 ? -1 : 1;
        transform.up = mod * new Vector3(transform.position.y, -1 * transform.position.x, 0).normalized;
        //sprite.transform.rotation =  (transform.position - GameManager.Instance.Planet.transform.position)
    }
    float FTLSpeed = 50f;

    float c = 0;

    bool isArrivingStarted = false;
    IEnumerator ArrivingCoroutine()
    {
        isArrivingStarted = true;
        AudioManager.PlaySound("boost");
        yield return new WaitForSeconds(0.2f);
        CurrentStatus = Status.Orbiting;

        StartCoroutine(TickResources());
        GetComponentInChildren<OrbitRenderer>().Show(OrbitHeight);
    }

    bool isFlipped;
    bool playSound = false;
    void UpdateArrivingPosition()
    {
        if (isArrivingStarted) return;
        if (Vector3.Distance(transform.position, Vector3.zero) <= OrbitHeight)
        {
            StartCoroutine(ArrivingCoroutine());
            return;
        }

        Vector3 l_dir = (OrbitInsertionPoint - transform.position).normalized;
        float dist = Vector3.Distance(transform.position, OrbitInsertionPoint);

        if (dist <= 10)
        {
            sprite.DOColor(new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f), 1f);
            if (!playSound)
            {
                AudioManager.PlaySound("");
                playSound = true;

            }
        }


        if (dist <= 1)
        {
            //Debug.Log($" up.z {transform.up} |rot {transform.rotation.eulerAngles} |  result {new Vector3(OrbitInsertionPoint.y, -1 * OrbitInsertionPoint.x, 0)}");
            //Vector3 v = new Vector3(OrbitInsertionPoint.y, -1 * OrbitInsertionPoint.x, 0);
            int mod = dir > 0 ? -1 : 1;

            FTLTrail.gameObject.SetActive(false);
            transform.up = Vector3.LerpUnclamped(l_dir, mod * new Vector3(OrbitInsertionPoint.y, -1 * OrbitInsertionPoint.x, 0), (1 - dist)).normalized;
            //transform.DORotate(new Vector3(OrbitInsertionPoint.y, -1 * OrbitInsertionPoint.x, 0), 1, RotateMode.FastBeyond360);

            //if(dist<=2) transform.DORotate(new Vector3(OrbitInsertionPoint.y, -1 * OrbitInsertionPoint.x, 0))


        }
        else transform.up = l_dir;
        transform.position += l_dir * FTLSpeed * Time.deltaTime;
        FTLSpeed = dist / 0.5f + 2;
    }
    public int ShipType;

    public void EndGame()
    {
        if (tortureTime > GameManager.Instance.LongestTortureTime)
        {
            GameManager.Instance.LongestTortureTime = tortureTime;
            GameManager.Instance.LongestTortureName = ShipName;
            GameManager.Instance.LongestTortureType = ShipType;

            PlayerPrefs.SetString("_torturedName", ShipName);
            PlayerPrefs.GetFloat("_torturedTime", tortureTime);
            PlayerPrefs.GetInt("_torturedtype", ShipType);
        }

        GameManager.Instance.UpdateInformationGathered(Mathf.FloorToInt(Intel * IntelMod));
        GameManager.Instance.ShipCount--;
        if (GameManager.Instance.ShipCount == 0)
            GameManager.Instance.ExitToMenu();

    }
    void UpdateDepartingPosition()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) >= 100)
        {
            Destroy(myOverlay);
            Destroy(gameObject);

            GameManager.Instance.UpdateInformationGathered(Mathf.FloorToInt(Intel * IntelMod));

            return;
        }
        Vector3 dir = this.dir > 0 ? transform.up : -transform.up;

        float dist = Vector3.Distance(transform.position, DepartingPosition);


        transform.position += -dir * FTLSpeed * Time.deltaTime;
        FTLSpeed = dist / 0.5f;

    }
    private float wIntel;
    private float wRep;
    private float wFight;
    private float wChill;


    private float minDelta = 1;
    private float maxDelta = 10;

    private float critHP = 40;
    private float switchWeightHP;
    private float switchWeightDesp;


    void CheckAndUpdateTask()
    {
        wIntel = TaskWeights[ShipTask.Scanning];
        wRep = TaskWeights[ShipTask.Repairs];
        wFight = TaskWeights[ShipTask.Fighting];
        wChill = TaskWeights[ShipTask.Resting];

        float nHp = HP / 100f;
        float nDp = Despair / 100f;

        if (isBoarded) TaskWeights[ShipTask.Fighting] = 2;
        else TaskWeights[ShipTask.Fighting] = 0;


        TaskWeights[ShipTask.Scanning] = 0.1f;

        //switchWeightHP = HP < critHP ? 1 : (taskCounter - minDelta) / (maxDelta - minDelta);
        //switchWeightDesp = HP < critHP ? 0 : (taskCounter - minDelta) / (maxDelta - minDelta);


        TaskWeights[ShipTask.Repairs] = ((1 - nHp) * ((1 - nHp) / (1 - critHP / 100f)));// * switchWeightHP;
        if (isFrozen) TaskWeights[ShipTask.Repairs] = 0;
        TaskWeights[ShipTask.Resting] = (nDp);// * switchWeightDesp;
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
            if (HP <= 100) TaskWeights[ShipTask.Scanning] = 0;
            else
            {
                Depart();

                StopCoroutine(TickResources());
            }

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
            Despair += 50;
            myOverlay.GetComponent<ShipUIOverlay>().UpdateTaskText("Stunned, resting");
            StopCoroutine(TickResources());
            return;
        }
        if (Despair <= 100 && isStunned)
        {
            isStunned = false;
            StartCoroutine(TickResources());
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
                    Intel += 1 * IntelMod;
                    if (Intel >= 100)
                    {
                        Depart();
                    }
                    if (Intel >= 80 && !isShakingIntel)
                    {
                        myOverlay.GetComponent<ShipUIOverlay>().IntelBar.transform.DOScale(1.2f, 2f).SetLoops(-1).SetEase(Ease.InOutCirc);
                        StartCoroutine(PingRoutine());
                        isShakingIntel = true;
                    }
                    if (Intel <= 80 && isShakingIntel)
                    {
                        myOverlay.GetComponent<ShipUIOverlay>().IntelBar.transform.DOKill(true);
                        StopCoroutine(PingRoutine());
                        isShakingIntel = false;
                    }

                    break;
                case ShipTask.Repairs:
                    HP += 1 * RepairMod;

                    break;
                case ShipTask.Fighting:
                    if (enemyHP > 0)
                    {
                        float x = Random.Range(1, 3) * FightMod;
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
                    Despair -= (2 * DespairMod);
                    break;
            }
            Despair -= (Random.Range(-2, +2) * DespairMod);
            if (Despair < 0) Despair = 0;
            int deltaScore = Mathf.FloorToInt(Despair / 10f);
            if (deltaScore > 0)
            {
                ProgressManager.Instance.Score += deltaScore;
                InterfaceManager.Instance.ShowAnimatedLabel(GameManager.Instance.DespairColor, $"+{deltaScore}", transform.position);
            }

            yield return new WaitForSeconds(1f);
        }
    }


    public void Damage(int dmg)
    {
        HP -= dmg;
        if (HP <= 0) DieHP();
    }
    void DieHP()
    {
        GetComponentInChildren<OrbitRenderer>().Hide();
        StopCoroutine(PingRoutine());
        Destroy(myOverlay);
        Destroy(gameObject);
        //Boom

    }
    Vector3 DepartingPosition;
    void Depart()
    {
        StopCoroutine(PingRoutine());

        StopCoroutine(TickResources());
        FTLTrail.gameObject.SetActive(true);
        DepartingPosition = transform.position;
        CurrentStatus = Status.Departing;
        GetComponentInChildren<OrbitRenderer>().Hide();

    }

    public bool isBoarded = false;
    public IEnumerator PhysicsRoutine(float duration)
    {
        ShipTask cStatus = CurrentTask;
        CurrentTask = ShipTask.Frozen;
        isFrozen = true;
        var overlay = myOverlay.GetComponent<ShipUIOverlay>();
        Debug.Log("Before routine");
        overlay.StartCoroutine(overlay.TimerMaskCoroutine(duration));
        yield return new WaitForSeconds(duration);
        CurrentTask = cStatus;
        isFrozen = false;
    }

    public IEnumerator InterferenceRoutine(float duration)
    {

        IntelMod *= 0.5f;
        var overlay = myOverlay.GetComponent<ShipUIOverlay>();
        overlay.StartCoroutine(overlay.TimerMaskCoroutine(duration));
        yield return new WaitForSeconds(duration);
        IntelMod *= 2f;
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
