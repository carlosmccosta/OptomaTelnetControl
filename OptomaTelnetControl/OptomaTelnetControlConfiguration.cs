using System.Xml.Serialization;

namespace OptomaTelnetControl
{
    public class OptomaTelnetControlConfiguration
    {
        [XmlElement("Hostname")]
        public string Hostname { get; set; } = "192.168.0.100";


        [XmlElement("Port")]
        public int Port { get; set; } = 23;


        [XmlElement("ProjectorId")]
        public int ProjectorId { get; set; } = 0;


        [XmlElement("NumberOfRequestsForHorizontalLensShiftReset")]
        public int NumberOfRequestsForHorizontalLensShiftReset { get; set; } = 100;


        [XmlElement("NumberOfRequestsForVerticalLensShiftReset")]
        public int NumberOfRequestsForVerticalLensShiftReset { get; set; } = 200;


        [XmlElement("NumberOfRequestsForFocusReset")]
        public int NumberOfRequestsForFocusReset { get; set; } = 30;


        [XmlElement("NumberOfRequestsForZoomReset")]
        public int NumberOfRequestsForZoomReset { get; set; } = 30;
    }
}
