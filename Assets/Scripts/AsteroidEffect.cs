using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit;
using DG.Tweening;

public class AsteroidEffect : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private List<Sprite> asteroids;
    Vector3 _target;
    SpriteRenderer spriteRenderer;
    float _damage;
    Camera _cam;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = asteroids?.GetRandom();
        _cam = Camera.main;
        Debug.Log($"MeteorEffect Awake");

    }

    public void Init(Vector3 target, float damage)
    {
        _target = target;
        dir = target - transform.position;
        _damage = damage;
        Debug.Log($"MeteorEffect init");
    }
    Vector3 dir;

    bool reached = false;
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;//= Vector3.MoveTowards(transform.position, _target, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _target) < 1f && !reached)
        {
            Debug.Log($"MeteorEffect destroy");
            spriteRenderer.sortingOrder = -5;
            spriteRenderer.DOFade(0f, 2f);

            Destroy(gameObject, 5f);
            reached = true;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Asteroid hit {other.gameObject.name}");
        other.GetComponent<ShipController>().Damage(Mathf.FloorToInt(_damage));
        _cam.DOShakePosition(0.2f, 1, 50);


        EffectsController.Instance.ShowShipDebris(other.ClosestPoint(transform.position), dir);
        GetComponentInChildren<TrailRenderer>().transform.SetParent(null);
        Destroy(gameObject);


    }
}
