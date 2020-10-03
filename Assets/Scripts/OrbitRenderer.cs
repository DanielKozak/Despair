using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(LineRenderer))]
public class OrbitRenderer : MonoBehaviour
{
    public int precision = 720;

    LineRenderer orbitLine;
    // Start is called before the first frame update
    public void Show(float radius)
    {
        Color s = new Color(0, 0.4f, 1, 0);
        Color f = new Color(0, 0.4f, 1, 1);
        orbitLine = GetComponent<LineRenderer>();
        var points = GetPoints(radius, precision);
        orbitLine.positionCount = points.Length;
        orbitLine.SetPositions(points);
        orbitLine.DOColor(new Color2(s, s), new Color2(f, f), 1f);
    }

    public void Hide()
    {
        Color s = new Color(0, 0.4f, 1, 0);
        Color f = new Color(0, 0.4f, 1, 1);
        orbitLine.DOColor(new Color2(f, f), new Color2(s, s), 1f);

    }


    public static Vector3[] GetPoints(float radius, int precision)
    {
        Vector3[] result = new Vector3[precision];
        for (int i = 0; i < precision; i++)
        {
            float angle = ((float)i / (float)precision) * 360f;
            float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);
            result[i] = new Vector3(x, y, 0);
        }
        result[precision - 1] = result[0];
        Debug.Log(result.Length);

        return result;

    }

    public static Vector3 GetPoint(float radius, float angle)
    {
        float x = radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = radius * Mathf.Sin(angle * Mathf.Deg2Rad);

        return new Vector3(x, y, 0);

    }
}
