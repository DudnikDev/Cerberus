using System.Collections.Generic;
using CRepublic.Magic.Logic.Structure.Slots.Items;
using Newtonsoft.Json;

namespace CRepublic.Magic.Logic.Structure.Slots
{
    internal class Calendar
    {
        [JsonProperty("events")]  internal List<Event> Events = new List<Event>();
    }
}
