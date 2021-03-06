using System.Collections.Generic;

namespace CRepublic.Magic.Logic.Structure.Slots.Items
{
    internal class List_Regions
    {
        internal List<Level> Level = new List<Level>();

        public List_Regions(Level Player)
        {
            if (Player != null)
                this.Level.Add(Player);
        }

        internal void Remove(Level Player)
        {
            if (Player != null)
            {
                if (this.Level.Contains(Player))
                {
                    this.Level.Remove(Player);
                }
                //else
                //this.Devices.Remove(Device.Socket.Handle);
            }
        }
    }
}