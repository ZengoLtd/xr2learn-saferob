using System;
using UnityEngine.Localization;

namespace Zengo.Inventory  
{
    [Serializable]
    public class ItemModel
    {
        public ItemType type;
        public LocalizedString name;
        public LocalizedString description;
    }
}

