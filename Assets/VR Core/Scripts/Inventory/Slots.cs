using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zengo.Inventory
{
    public class Slots : MonoBehaviour
    {
        public List<ItemType> allowedItemTypes = new List<ItemType>();

        Image image;

        private void Start()
        {
            image = GetComponent<Image>();
            if (image != null)
            {
                image.enabled = false;
            }
        }

        public void ToggleEquipment()
        {
            if (image != null)
            {
                image.enabled = !image.enabled;
            }
        }

    }
}
