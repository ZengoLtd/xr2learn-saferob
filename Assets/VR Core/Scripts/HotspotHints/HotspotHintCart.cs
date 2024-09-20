using UnityEngine.Localization;
using UnityEngine;

public class HotspotHintCart : HotspotHint
{
    public LocalizedString alreadyConnectedText;

    public GameObject cartModell;

    public override void ShowHotspot()
    {
        if (cartModell != null)
        {
            if (cartModell.activeSelf)
            {
                HotspotHintSystem.Instance.ShowHotspotHint(alreadyConnectedText);
            }
            else
            {
                HotspotHintSystem.Instance.ShowHotspotHint(hotspotText);
            }
        }

    }
}
