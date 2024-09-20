using System.Collections.Generic;
using UnityEngine;

public class TeleportDestinationMarkerWithBadLocations : TeleportDestinationMarker
{
    private bool dangerousTeleport = false;
    public List<BNG.TeleportDestination> BadDestinations = new List<BNG.TeleportDestination>();

    AlertBoxMessages alertBoxMessages;

    private void Start()
    {
        alertBoxMessages = GetComponent<AlertBoxMessages>();
    }

    public bool DangerousTeleport
    {
        get { return dangerousTeleport; }
        set { dangerousTeleport = value; }
    }

    void SetDangerousTeleport(bool value)
    {
        foreach (BNG.TeleportDestination destination in Destinations)
        {
            if (destination?.GetComponent<TeleportDestinationMarkerWithBadLocations>() != null)
            {
                destination.GetComponent<TeleportDestinationMarkerWithBadLocations>().DangerousTeleport = false;
            }
        }
        foreach (BNG.TeleportDestination baddestination in BadDestinations)
        {
            if (baddestination?.GetComponent<TeleportDestinationMarkerWithBadLocations>() != null)
            {
                baddestination.GetComponent<TeleportDestinationMarkerWithBadLocations>().DangerousTeleport = value;
            }
        }
    }

    public override void ShowDestinations()
    {
        // Hide all teleporters first
        HideAllTeleporters();

        // Show only the teleporters in the Destinations list
        foreach (BNG.TeleportDestination destination in Destinations)
        {
            if (destination?.GetComponent<TeleportDestinationMarker>() != null)
            {
                destination.GetComponent<TeleportDestinationMarker>().EnableTeleporter();
            }
        }

        foreach (BNG.TeleportDestination destination in Destinations)
        {
            if (destination?.GetComponent<TeleportDestinationMarkerWithBadLocations>() != null)
            {
                destination.GetComponent<TeleportDestinationMarkerWithBadLocations>().dangerousTeleport = false;
            }
        }

        // Show the bad destinations and set them as dangerous
        foreach (BNG.TeleportDestination baddestination in BadDestinations)
        {
            if (baddestination?.GetComponent<TeleportDestinationMarkerWithBadLocations>() != null)
            {
                var badMarker = baddestination.GetComponent<TeleportDestinationMarkerWithBadLocations>();
                badMarker.EnableTeleporter();
                badMarker.DangerousTeleport = true;
            }
        }
    }

    private void HideAllTeleporters()
    {
        // Get all teleporters in the scene
        TeleportDestinationMarker[] allTeleporters = FindObjectsOfType<TeleportDestinationMarker>();

        // Disable all teleporters
        foreach (TeleportDestinationMarker teleporter in allTeleporters)
        {
            teleporter.DisableTeleporter();
        }
    }

    public override void ResetLastTarget()
    {
        base.ResetLastTarget();
    }

    public override void SetAsLastTarget()
    {
        base.SetAsLastTarget();
        if (dangerousTeleport)
        {
            if(alertBoxMessages != null)
            {
                alertBoxMessages.ShowErrorMessage();
            }
        }
    }
}