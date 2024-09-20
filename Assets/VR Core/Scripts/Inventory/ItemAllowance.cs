using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zengo.Inventory
{
    [Serializable]
    public class AllowTypes
    {
        public List<ItemType> type = new List<ItemType>();
        public List<ItemTypeSO> optimalItems = new List<ItemTypeSO>();
        public List<ItemTypeSO> notoptimalItems = new List<ItemTypeSO>();
        public List<ItemTypeSO> badItems = new List<ItemTypeSO>();

    }

    [CreateAssetMenu(fileName = "ItemAllowances", menuName = "Item/Allowed", order = 1)]
    public class ItemAllowance : ScriptableObject
    {

        public List<AllowTypes> allowedItemTypes = new List<AllowTypes>();


    }
}
   
