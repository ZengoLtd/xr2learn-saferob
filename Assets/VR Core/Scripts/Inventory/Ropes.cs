namespace Zengo.Inventory
{
    public class Ropes : Item
    {
        public int allowedRopeCount;

        private void Start()
        {

        }

        public override void EquipItem()
        {
            base.EquipItem();

        }

        public int GetAllowedRopeCount()
        {
            return allowedRopeCount;
        }
    }
}

