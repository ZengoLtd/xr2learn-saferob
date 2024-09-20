using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class HotspotHint : MonoBehaviour
{
    public LocalizedString hotspotText;
    protected int state;


    public virtual void ShowHotspot()
    {
        HotspotHintSystem.Instance.ShowHotspotHint(hotspotText);
    }

    public virtual void HideHotspot()
    {
        HotspotHintSystem.Instance.ClearHint();
    }
}
