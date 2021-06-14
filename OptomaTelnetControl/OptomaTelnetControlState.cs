using System.Xml.Serialization;

namespace OptomaTelnetControl
{
    public class OptomaTelnetControlState
    {
        [XmlElement("CurrentZoom")]
        public int CurrentZoom { get; set; } = 0;


        [XmlElement("CurrentFocus")]
        public int CurrentFocus { get; set; } = 0;


        [XmlElement("CurrentHorizontalLensShift")]
        public int CurrentHorizontalLensShift { get; set; } = 0;


        [XmlElement("CurrentVerticalLensShift")]
        public int CurrentVerticalLensShift { get; set; } = 0;
    }
}
