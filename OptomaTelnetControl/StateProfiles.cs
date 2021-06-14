using System.Collections.Generic;
using System.Xml.Serialization;

namespace OptomaTelnetControl
{
    public class StateProfiles
    {
        [XmlArray("ProfilesList")]
        [XmlArrayItem("State", Type = typeof(State))]
        public List<State> ProfilesList { get; set; } = new List<State>();


        [XmlElement("ActiveProfileIndex")]
        public int ActiveProfileIndex { get; set; } = 0;


        public void SetValuesToValidRanges(Configuration configuration)
        {
            foreach (State state in ProfilesList)
            {
                state.SetValuesToValidRanges(configuration);
            }
        }
    }
}
