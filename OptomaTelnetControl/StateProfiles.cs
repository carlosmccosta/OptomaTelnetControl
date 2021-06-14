using System.Collections.Generic;
using System.Xml.Serialization;

namespace OptomaTelnetControl
{
    public class StateProfiles
    {
        [XmlArray("ProfilesList")]
        [XmlArrayItem("State", Type = typeof(OptomaTelnetControlState))]
        public List<OptomaTelnetControlState> ProfilesList { get; set; } = new List<OptomaTelnetControlState>();


        [XmlElement("ActiveProfileIndex")]
        public int ActiveProfileIndex { get; set; } = 0;
    }
}
