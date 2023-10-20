using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class AnimatedLabelPrefabContoller : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI mText;
    Camera _cam;
    RectTransform _rect;

    void Awake()
    {
        _cam = Camera.main;
        _rect = GetComponent<RectTransform>();
    }
    public void ShowToProgressbar(string text, Vector3 position) => Show(text, position, ProgressManager.Instance.GetTweenTargetPosition());

    public void Show(string text, Vector3 position, Color color)
    {
        mText.color = color;
        ShowToProgressbar(text, position);
    }
    public void Show(string text, Vector3 position, Vector3 target)
    {
        mText.text = text;
        _rect.position = _cam.WorldToScreenPoint(position);
        transform.DOMove(target, 1f).OnComplete(() => Destroy(gameObject));
    }
    public void ShowDefault(string text, Vector3 position)
    {
        mText.text = text;
        _rect.position = _cam.WorldToScreenPoint(position);
        Vector3 target = new Vector3(_rect.position.x, _rect.position.y + 50, _rect.position.z);
        transform.DOMove(target, 1f).OnComplete(() => Destroy(gameObject));
    }

}
