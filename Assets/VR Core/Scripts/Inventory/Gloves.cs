using System.Collections.Generic;
using UnityEngine;
using BNG;

namespace Zengo.Inventory
{
    public class Gloves : Item
    {
        public enum GloveType
        {
            Base,
            Rubber,
            Merged,
            Special,
            Normal
        }

        public GloveType gloveType;
        public HandModelSelector hms;

        private static List<Gloves> equippedGloves = new List<Gloves>();

        private void Start()
        {
            if (hms == null)
            {
                hms = FindObjectOfType<HandModelSelector>();
            }

            if (hms == null)
            {
                Debug.LogWarning("No Hand Model Selector Found in Scene. Will not be able to switch hand models");
            }
        }

        private void OnDisable()
        {
            if(hms != null)
            {
                hms.ChangeHandsModel(0, false);
            }
        }

        public override void EquipItem()
        {
            base.EquipItem();
            equippedGloves.Add(this);
            UpdateHandModel();
        }

        public override void RemoveItem()
        {
            base.RemoveItem();
            equippedGloves.Remove(this);
            UpdateHandModel();
        }

        private void UpdateHandModel()
        {
            if (hms != null)
            {
                int modelId = GetModelIdForCurrentGloves();
                hms.ChangeHandsModel(modelId, false);
            }
            else
            {
                Debug.LogWarning("Cannot change hand model: HandModelSelector not found");
            }
        }

        private int GetModelIdForCurrentGloves()
        {
            bool hasRubber = equippedGloves.Exists(g => g.gloveType == GloveType.Rubber);
            bool hasMerged = equippedGloves.Exists(g => g.gloveType == GloveType.Merged);
            bool hasSpecial = equippedGloves.Exists(g => g.gloveType == GloveType.Special);

            if (hasMerged && hasSpecial)
                return 2; // merged and special
            else if (hasRubber && hasSpecial)
                return 1; // rubber glove and special
            else if (hasMerged && hasSpecial)
                return 2; // merged and special
            else if (hasSpecial && !hasRubber && !hasMerged)
                return 3; // special + bare (base)
            else if (hasMerged)
                return 5; // merged glove only (prioritized over rubber)
            else if (hasRubber)
                return 4; // rubber glove only
            else
                return 0; // base hand (bare)
        }
    }
}