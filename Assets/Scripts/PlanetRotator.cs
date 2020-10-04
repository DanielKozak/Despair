using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotator : MonoBehaviour
{
    public float rotSpeed;
    private void Start()
    {
        rotSpeed = Random.Range(-3f, 3f);
    }
    void Update()
    {
        //float angle = transform.rotation.eulerAngles.z;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + (rotSpeed * Time.deltaTime));
        //transform.Rotate(Vector3.forward, angle + (rotSpeed * Time.deltaTime));
    }
}
