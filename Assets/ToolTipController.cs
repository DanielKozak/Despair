using System.Text;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class ToolTipController : Singleton<ToolTipController>
{
    [SerializeField] GameObject _panel;
    [SerializeField] TMP_Text _nameLabel;
    [SerializeField] TMP_Text _descriptionLabel;

    RectTransform _rect;
    string cyancolor = "#00FAE0";

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    public void Show(AbilitySO ability, bool isOTU = false, int numUses = 0)
    {
        _panel.SetActive(true);
        _nameLabel.text = ability.NameKey;
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"<color=white>{ability.DescriptionKey} </color>\n");
        foreach (UpgradeableParam effect in ability.uParamList)
        {
            sb.AppendLine($"<color={cyancolor}>{effect.Description} is </color><color=green>{effect.value}</color>");
        }

        if (isOTU)
        {
            sb.AppendLine($"<color=white>Uses Remaining: {numUses}</color>");
        }
        _descriptionLabel.text = sb.ToString();
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);

    }

    public void Hide()
    {
        _panel.SetActive(false);
    }


}
