using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Zengo.Inventory
{
    [CreateAssetMenu(fileName = "SafeRobItemAllowances", menuName = "Item/SafeRob Allowed", order = 1)]
    public class SafeRobItemAllowance : ItemAllowance
    {

        public LocalizedString messageError;
        public LocalizedString messageSuccess;
        public int errorPoints;
    }
}

