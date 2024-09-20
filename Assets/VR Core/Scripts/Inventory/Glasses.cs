using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zengo.Inventory
{
    public class Glasses : Item
    {
        public override void EquipItem()
        {
           base.EquipItem();
           if(NetworkDataHolder.Instance != null){
                NetworkDataHolder.Instance.AddOrUpdateData("Glasses", true);
           }
        }
         public override void RemoveItem()
        {
           base.RemoveItem();
           if(NetworkDataHolder.Instance != null){
                NetworkDataHolder.Instance.AddOrUpdateData("Glasses", false);
           }
        }
    }
}

