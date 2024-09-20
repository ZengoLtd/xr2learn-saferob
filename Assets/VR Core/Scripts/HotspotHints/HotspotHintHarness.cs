

using UnityEngine.Localization;

public class HotspotHintHarness : HotspotHint
{

    public LocalizedString alreadyConnectedText;

    RopeConnector ropeConnector;

    private void OnEnable()
    {
        ropeConnector = GetComponentInParent<RopeConnector>();
    }

    public override void ShowHotspot()
    {
        if (ropeConnector != null)
        {
            if ((bool)ropeConnector.ropeConnected.state)
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
