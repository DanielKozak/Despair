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
        Moving, Intel, Repair, Fight, Chill
    }
    public ShipTask CurrentTask = ShipTask.Moving;
    float taskCounter = 0;

    //GAME PARAMS   
    public float HP = 100;
    public float Despair = 0;
    public float Intel = 0;


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
    Vector3[] path;
    int currentPositionIndex;
    int pathPrecision = 360;
    private Dictionary<ShipTask, float> TaskWeights = new Dictionary<ShipTask, float>();
    int clockwise;
    int dir;

    GameObject myOverlay;

    void Start()
    {
        path = OrbitRenderer.GetPoints(OrbitHeight, pathPrecision);
        TaskWeights.Add(ShipTask.Intel, 0.5f);
        TaskWeights.Add(ShipTask.Repair, 0f);
        TaskWeights.Add(ShipTask.Fight, 0f);
        TaskWeights.Add(ShipTask.Chill, 0f);
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
                CurrentTask = ShipTask.Moving;
                break;
            case Status.Dead:
                //UpdateOrbitalPosition();
                break;
        }
        if (Input.GetKeyUp(KeyCode.F1))
        {
            Depart();
        }

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
        yield return new WaitForSeconds(0.5f);
        CurrentStatus = Status.Orbiting;//TODO PSSHHH

        StartCoroutine(TickResources());
        GetComponentInChildren<OrbitRenderer>().Show(OrbitHeight);
    }

    bool isFlipped;
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
        }


        if (dist <= 1)
        {
            //Debug.Log($" up.z {transform.up} |rot {transform.rotation.eulerAngles} |  result {new Vector3(OrbitInsertionPoint.y, -1 * OrbitInsertionPoint.x, 0)}");
            //Vector3 v = new Vector3(OrbitInsertionPoint.y, -1 * OrbitInsertionPoint.x, 0);
            int mod = dir > 0 ? -1 : 1;
            transform.up = Vector3.LerpUnclamped(l_dir, mod * new Vector3(OrbitInsertionPoint.y, -1 * OrbitInsertionPoint.x, 0), (1 - dist)).normalized;
            //transform.DORotate(new Vector3(OrbitInsertionPoint.y, -1 * OrbitInsertionPoint.x, 0), 1, RotateMode.FastBeyond360);

            //if(dist<=2) transform.DORotate(new Vector3(OrbitInsertionPoint.y, -1 * OrbitInsertionPoint.x, 0))


        }
        else transform.up = l_dir;
        transform.position += l_dir * FTLSpeed * Time.deltaTime;
        FTLSpeed = dist / 0.5f + 2;
    }
    void UpdateDepartingPosition()
    {
        if (Vector3.Distance(transform.position, Vector3.zero) >= 100)
        {
            Destroy(myOverlay);
            Destroy(gameObject);
            Debug.Log("GAME OVER");
            CurrentStatus = Status.Dead;
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


    private float minDelta;
    private float repDelta;
    private float chillDelta;
    void CheckAndUpdateTask()
    {
        wIntel = TaskWeights[ShipTask.Intel];
        wRep = TaskWeights[ShipTask.Repair];
        wFight = TaskWeights[ShipTask.Fight];
        wChill = TaskWeights[ShipTask.Chill];
        if (isBoarded) TaskWeights[ShipTask.Fight] = 1;
        else TaskWeights[ShipTask.Fight] = 0;


        TaskWeights[ShipTask.Intel] = 0.5f;
        TaskWeights[ShipTask.Repair] = (1 - HP / 100f);
        TaskWeights[ShipTask.Chill] = (Despair / 100f);


        Vector4 vector = new Vector4(TaskWeights[ShipTask.Intel], TaskWeights[ShipTask.Fight],
            TaskWeights[ShipTask.Repair], TaskWeights[ShipTask.Chill]);
        vector.Normalize();
        TaskWeights[ShipTask.Fight] = vector.y;
        TaskWeights[ShipTask.Intel] = vector.x;
        TaskWeights[ShipTask.Repair] = vector.z;
        TaskWeights[ShipTask.Chill] = vector.w;




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
            if (HP <= 100) TaskWeights[ShipTask.Intel] = 0;
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
            DieDespair();
            StopCoroutine(TickResources());
            return;
        }
    }

    IEnumerator TickResources()
    {
        while (true)
        {
            Debug.Log("TICK");
            switch (CurrentTask)
            {
                case ShipTask.Intel:
                    Intel += 1 * IntelMod;
                    if (Intel >= 100)
                    {
                        Depart();
                    }
                    break;
                case ShipTask.Repair:
                    HP += 1 * RepairMod;

                    break;
                case ShipTask.Fight:
                    float x = 1 * FightMod;
                    HP -= x;
                    Despair += x * DespairMod;
                    break;
                case ShipTask.Chill:
                    Despair -= (4 * DespairMod);
                    break;
            }
            Despair -= (1 * DespairMod);
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
    }
    void DieDespair()
    {
        GetComponentInChildren<OrbitRenderer>().Hide();

    }
    Vector3 DepartingPosition;
    void Depart()
    {
        DepartingPosition = transform.position;
        CurrentStatus = Status.Departing;
        GetComponentInChildren<OrbitRenderer>().Hide();

    }

    public bool isBoarded = false;
    public void AddFight(string text)
    {
        isBoarded = true;
    }
}
