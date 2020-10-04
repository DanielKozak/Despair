using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularHealthBarController : MonoBehaviour
{
    public enum Positions
    {
        Left, Bottom, Right
    }
    public Positions myPosition;
    public SpriteRenderer Bg;
    public SpriteRenderer Fill;

    public Transform Mask;
    public Color color;

    float zmin, zmax;
    private void Start()
    {
        Bg.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        Fill.color = color;
        switch (myPosition)
        {
            case Positions.Left:
                zmin = 84.66f;
                zmax = 1.35f;
                break;
            case Positions.Bottom:
                zmin = -85.2f;
                zmax = -0.92f;
                break;
            case Positions.Right:
                zmin = -83.2f;
                zmax = -1.2f;
                break;
        }
    }

    public void SetNormalizedValue(float value)
    {
        if (value < 0f) value = 0f;
        if (value > 1f) value = 1f;

        Mask.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(zmin, zmax, value));
    }
}
