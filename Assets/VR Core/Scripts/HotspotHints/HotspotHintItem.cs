using UnityEngine.Localization;

public class HotspotHintItem : HotspotHint
{
    Zengo.Inventory.Item item;

    private void Awake()
    {
        item = GetComponent<Zengo.Inventory.Item>();
    }


    public override void ShowHotspot()
    {
        if (item != null)
        {
            // Create a new LocalizedString that will combine the text from the item name and the hotspotText.
            LocalizedString combinedText = new LocalizedString();
            // Assign the localized values (assuming the format is something like "{0} {1}" in the table)
            combinedText.Arguments = new object[] { item.model.name, hotspotText };

            // Show the combined text
            HotspotHintSystem.Instance.ShowHotspotHint(combinedText);
        }
    }

}
