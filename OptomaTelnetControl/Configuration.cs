using System.Xml.Serialization;

namespace OptomaTelnetControl
{
    public class Configuration
    {
        [XmlElement("Hostname")]
        public string Hostname { get; set; } = "192.168.0.100";


        [XmlElement("Port")]
        public int Port { get; set; } = 23;


        [XmlElement("ProjectorId")]
        public string ProjectorId { get; set; } = "00";


        /// <summary>
        /// 1 -> Hdmi
        /// 2 -> Dvi
        /// 5 -> Vga
        /// 18 -> Network
        /// 21 -> HdBaseT
        /// 22 -> 3GSDI
        /// </summary>
        [XmlElement("MainVideoSourceId")]
        public int MainVideoSourceId { get; set; } = 1;


        [XmlElement("NumberOfRequestsForGoingFromLeftToRightLensShift")]
        public int NumberOfRequestsForGoingFromLeftToRightLensShift { get; set; } = 1550;


        [XmlElement("NumberOfRequestsForGoingFromBottomToTopLensShift")]
        public int NumberOfRequestsForGoingFromBottomToTopLensShift { get; set; } = 2910;


        [XmlElement("NumberOfRequestsForGoingFromFocusAtMinimumToMaximum")]
        public int NumberOfRequestsForGoingFromFocusAtMinimumToMaximum { get; set; } = 27;


        [XmlElement("NumberOfRequestsForGoingFromZoomAtMinimumToMaximum")]
        public int NumberOfRequestsForGoingFromZoomAtMinimumToMaximum { get; set; } = 35;


        public void SetValuesToValidRanges()
        {
            if (Hostname == null || Hostname.Length == 0)
                Hostname = "192.168.0.100";

            if (Port < 0)
                Port = 0;

            if (ProjectorId == null || ProjectorId.Length == 0)
                ProjectorId = "00";

            if (NumberOfRequestsForGoingFromLeftToRightLensShift < 1)
                NumberOfRequestsForGoingFromLeftToRightLensShift = 1;

            if (NumberOfRequestsForGoingFromBottomToTopLensShift < 1)
                NumberOfRequestsForGoingFromBottomToTopLensShift = 1;

            if (NumberOfRequestsForGoingFromFocusAtMinimumToMaximum < 1)
                NumberOfRequestsForGoingFromFocusAtMinimumToMaximum = 1;

            if (NumberOfRequestsForGoingFromZoomAtMinimumToMaximum < 1)
                NumberOfRequestsForGoingFromZoomAtMinimumToMaximum = 1;

            if (!(MainVideoSourceId == 1 || MainVideoSourceId == 2 || MainVideoSourceId == 5 || MainVideoSourceId == 18 || MainVideoSourceId == 21 || MainVideoSourceId == 22))
                MainVideoSourceId = 1;
        }
    }
}
