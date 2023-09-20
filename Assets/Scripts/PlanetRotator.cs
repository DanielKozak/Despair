using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotator : MonoBehaviour
{
    public bool useScaledDeltaTime = true;
    float rotSpeed;
    private void Start()
    {
        rotSpeed = Random.Range(-3f, 3f);
    }
    void Update()
    {
        float time = useScaledDeltaTime ? Time.deltaTime : Time.unscaledDeltaTime;
        transform.Rotate(Vector3.forward, rotSpeed * time);
        // transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + (rotSpeed * Time.deltaTime));
    }
}
