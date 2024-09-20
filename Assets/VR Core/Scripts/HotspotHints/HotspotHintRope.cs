using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class HotspotHintRope : HotspotHint
{
    public LocalizedString hasNoRopeText;
    public LocalizedString alreadyConnectedText;

    public override void ShowHotspot()
    {
        if (RopeManagerBlock.Instance.FreeRopesAmount() == 0)
        {
            HotspotHintSystem.Instance.ShowHotspotHint(hasNoRopeText);
        }
        if (RopeManagerBlock.Instance.FreeRopesAmount() > 0)
        {
            HotspotHintSystem.Instance.ShowHotspotHint(hotspotText);
        }
        if (GetComponentInParent<RopeConnector>() != null)
        {
            if ((bool)GetComponentInParent<RopeConnector>().ropeConnected.state)
            {
                HotspotHintSystem.Instance.ShowHotspotHint(alreadyConnectedText);
            }
        }

    }
}
