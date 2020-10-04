using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Text))]
public class AnimatedLabelPrefabContoller : MonoBehaviour
{

    public Text mText;

    public void Show(Color color, string text, Vector3 position)
    {
        mText.text = text;
        mText.color = color;
        transform.position = position;

        Sequence s = DOTween.Sequence();
        //s.SetEase(Ease.out);
        s.Append(transform.DOMoveY(transform.position.y + 3f, 1f));
        s.Insert(0, mText.DOColor(new Color(mText.color.r, mText.color.g, mText.color.b, 0.0f), 1f));
        s.OnComplete(Kill);

    }

    void Kill()
    {
        Destroy(gameObject);
    }

}
