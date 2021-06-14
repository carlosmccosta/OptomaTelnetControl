using System.Xml.Serialization;

namespace OptomaTelnetControl
{
    public class State
    {
        [XmlElement("Zoom")]
        public int Zoom { get; set; } = 0;


        [XmlElement("Focus")]
        public int Focus { get; set; } = 0;


        [XmlElement("HorizontalLensShift")]
        public int HorizontalLensShift { get; set; } = 0;


        [XmlElement("VerticalLensShift")]
        public int VerticalLensShift { get; set; } = 0;

        //If between 1 and 5, it will be used the projector on-board profile for lens shift, instead of HorizontalLensShift and VerticalLensShift
        [XmlElement("LensMemoryProfile")]
        public int LensMemoryProfile { get; set; } = -1;


        public void SetValuesToValidRanges(Configuration configuration)
        {
            if (Zoom < 0)
                Zoom = 0;

            if (Zoom > configuration.NumberOfRequestsForGoingFromZoomAtMinimumToMaximum)
                Zoom = configuration.NumberOfRequestsForGoingFromZoomAtMinimumToMaximum;

            if (Focus < 0)
                Focus = 0;

            if (Focus > configuration.NumberOfRequestsForGoingFromFocusAtMinimumToMaximum)
                Focus = configuration.NumberOfRequestsForGoingFromFocusAtMinimumToMaximum;

            if (HorizontalLensShift < 0)
                HorizontalLensShift = 0;

            if (HorizontalLensShift > configuration.NumberOfRequestsForGoingFromLeftToRightLensShift)
                HorizontalLensShift = configuration.NumberOfRequestsForGoingFromLeftToRightLensShift;

            if (VerticalLensShift < 0)
                VerticalLensShift = 0;

            if (VerticalLensShift > configuration.NumberOfRequestsForGoingFromBottomToTopLensShift)
                VerticalLensShift = configuration.NumberOfRequestsForGoingFromBottomToTopLensShift;
        }
    }
}
