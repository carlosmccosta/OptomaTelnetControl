using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace OptomaTelnetControl
{
    public class OptomaControl : IDisposable
    {
        private bool _disposed = false;


        public TcpClient TcpClient { get; set; }
        public Configuration Configuration { get; set; } = new Configuration();
        public State State { get; set; } = new State();
        public StateProfiles Profiles { get; set; } = new StateProfiles();


        public OptomaControl() { }


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (TcpClient != null)
                    {
                        if (TcpClient.Connected)
                        {
                            PowerOff();
                            Thread.Sleep(1000);
                        }
                        TcpClient.Dispose();
                    }
                }

                TcpClient = null;
                _disposed = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public void LoadXmls(string folder = "xml/")
        {
            LoadConfiguration(folder);
            LoadState(folder);
            LoadProfiles(folder);
        }


        public void SaveXmls(string folder = "xml/")
        {
            SaveConfiguration(folder);
            SaveState(folder);
            SaveProfiles(folder);
        }


        public void LoadConfiguration(string folder = "xml/", string filename = "Configuration.xml")
        {
            Configuration = Tools.LoadFromXml<Configuration>(folder + filename);
            Configuration.SetValuesToValidRanges();
        }


        public void SaveConfiguration(string folder = "xml/", string filename = "Configuration.xml")
        {
            Configuration.SetValuesToValidRanges();
            Tools.SaveToXml(folder + filename, Configuration);
        }


        public void LoadState(string folder = "xml/", string filename = "State.xml")
        {
            State = Tools.LoadFromXml<State>(folder + filename);
            State.SetValuesToValidRanges(Configuration);
        }


        public void SaveState(string folder = "xml/", string filename = "State.xml")
        {
            State.SetValuesToValidRanges(Configuration);
            Tools.SaveToXml(folder + filename, State);
        }


        public void LoadProfiles(string folder = "xml/", string filename = "Profiles.xml")
        {
            Profiles = Tools.LoadFromXml<StateProfiles>(folder + filename);
            Profiles.SetValuesToValidRanges(Configuration);
        }


        public void SaveProfiles(string folder = "xml/", string filename = "Profiles.xml")
        {
            Profiles.SetValuesToValidRanges(Configuration);
            Tools.SaveToXml(folder + filename, Profiles);
        }


        public bool Connect(string hostname, int port)
        {
            Configuration.Hostname = hostname;
            Configuration.Port = port;
            return Connect();
        }


        public bool Connect()
        {
            if (TcpClient is null)
            {
                TcpClient = new TcpClient();
            }

            TcpClient.Connect(Configuration.Hostname, Configuration.Port);
            
            if (TcpClient.Connected)
            {
                PowerOn();
                SetMainVideoSource(Configuration.MainVideoSourceId);
                return true;
            }

            return false;
        }


        public bool isConnected()
        {
            if (TcpClient != null && TcpClient.Connected)
                return true;

            return false;
        }


        //Returns before finishing power on
        public bool PowerOn()
        {
            return SendWriteCommand(Configuration.ProjectorId, "00", "1");
        }


        //Returns before finishing power off
        public bool PowerOff()
        {
            return SendWriteCommand(Configuration.ProjectorId, "00", "2");
        }


        public bool SetMainVideoSource(int source)
        {
            if (source == 1 || source == 2 || source == 5 || source == 18 || source == 21 || source == 22)
                return SendWriteCommand(Configuration.ProjectorId, "12 ", source.ToString());
            return false;
        }


        public bool SetMainVideoSourceToHdmi()
        {
            return SendWriteCommand(Configuration.ProjectorId, "12 ", "1");
        }


        public bool SetMainVideoSourceToDvi()
        {
            return SendWriteCommand(Configuration.ProjectorId, "12 ", "2");
        }


        public bool SetMainVideoSourceToVga()
        {
            return SendWriteCommand(Configuration.ProjectorId, "12 ", "5");
        }


        public bool SetMainVideoSourceToNetwork()
        {
            return SendWriteCommand(Configuration.ProjectorId, "12 ", "18");
        }

        public bool SetMainVideoSourceToHdBaseT()
        {
            return SendWriteCommand(Configuration.ProjectorId, "12 ", "21");
        }


        public bool SetMainVideoSourceTo3GSDI()
        {
            return SendWriteCommand(Configuration.ProjectorId, "12 ", "22");
        }


        public bool LockLens(bool state)
        {
            return SendWriteCommand(Configuration.ProjectorId, "349", state ? "1" : "2");
        }


        public bool ResetVerticalLensShiftToBottom()
        {
            return MoveLensVerticalShiftDown(Configuration.NumberOfRequestsForGoingFromBottomToTopLensShift);
        }


        public bool ResetHorizontalLensShiftToLeft()
        {
            return MoveLensHorizontalShiftLeft(Configuration.NumberOfRequestsForGoingFromLeftToRightLensShift);
        }


        public bool ResetLensShiftToBottomLeft()
        {
            return MoveLensVerticalShiftDown(Configuration.NumberOfRequestsForGoingFromBottomToTopLensShift) &&
                MoveLensHorizontalShiftLeft(Configuration.NumberOfRequestsForGoingFromLeftToRightLensShift);
        }


        public bool ResetFocusToClosestProjectionDistance()
        {
            return DecreaseFocusToLowerProjectionDistance(Configuration.NumberOfRequestsForGoingFromFocusAtMinimumToMaximum);
        }


        public bool ResetZoomToSmallestProjectionSize()
        {
            return DecreaseZoomForLowerProjectionSize(Configuration.NumberOfRequestsForGoingFromZoomAtMinimumToMaximum);
        }


        //Returns before finishing lens motions (less accurate than ResetLensShiftToBottomLeft -> will descalibrate your profiles)
        //For recalibrating your profiles, move the lens to the bottom left and set the lens shift values in State.xml to 0
        public bool RecalibrateLensShiftToCenter()
        {
            LockLens(false);
            bool status = SendWriteCommand(Configuration.ProjectorId, "525", "1");
            if (status)
            {
                State.HorizontalLensShift = Configuration.NumberOfRequestsForGoingFromLeftToRightLensShift / 2;
                State.VerticalLensShift = Configuration.NumberOfRequestsForGoingFromBottomToTopLensShift / 2;
            }
            LockLens(true);
            return status;
        }


        public bool SaveLensShiftPosition(int profileNumber = 1)
        {
            LockLens(false);
            bool status = SendWriteCommand(Configuration.ProjectorId, "360", profileNumber.ToString());
            LockLens(true);
            return status;
        }


        //Returns before finishing lens motions
        public bool ApplyLensShiftPosition(int profileNumber = 1)
        {
            if (profileNumber >= 1 && profileNumber <= 5)
            {
                if (profileNumber != State.LensMemoryProfile)
                {
                    LockLens(false);
                    bool status = SendWriteCommand(Configuration.ProjectorId, "359", profileNumber.ToString());
                    if (status)
                    {
                        State.LensMemoryProfile = profileNumber;
                    }
                    LockLens(true);
                    return status;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }


        public bool IncreaseFocusToHigherProjectionDistance(int numberOfTimes = 1)
        {
            for (int i = 0; i < numberOfTimes; ++i)
            {
                if (SendWriteCommand(Configuration.ProjectorId, "308", "2"))
                {
                    if (State.Focus < Configuration.NumberOfRequestsForGoingFromFocusAtMinimumToMaximum)
                    {
                        State.Focus += 1;
                    }
                    SaveState();
                }
                else
                {
                    return false;
                }
            }
            return true;
        }


        public bool DecreaseFocusToLowerProjectionDistance(int numberOfTimes = 1)
        {
            for (int i = 0; i < numberOfTimes; ++i)
            {
                if (SendWriteCommand(Configuration.ProjectorId, "308", "1"))
                {
                    if (State.Focus > 0)
                    {
                        State.Focus -= 1;
                    }
                    SaveState();
                }
                else
                {
                    return false;
                }
            }
            return true;
        }


        public bool IncreaseZoomForHigherProjectionSize(int numberOfTimes = 1)
        {
            for (int i = 0; i < numberOfTimes; ++i)
            {
                if (SendWriteCommand(Configuration.ProjectorId, "307", "2"))
                {
                    if (State.Zoom < Configuration.NumberOfRequestsForGoingFromZoomAtMinimumToMaximum)
                    {
                        State.Zoom += 1;
                    }
                    SaveState();
                }
                else
                {
                    return false;
                }
            }
            return true;
        }


        public bool DecreaseZoomForLowerProjectionSize(int numberOfTimes = 1)
        {
            for (int i = 0; i < numberOfTimes; ++i)
            {
                if (SendWriteCommand(Configuration.ProjectorId, "307", "1"))
                {
                    if (State.Zoom > 0)
                    {
                        State.Zoom -= 1;
                    }
                    SaveState();
                }
                else
                {
                    return false;
                }
            }
            return true;
        }


        public bool MoveLensVerticalShiftUp(int numberOfTimes = 1)
        {
            for (int i = 0; i < numberOfTimes; ++i)
            {
                if (SendWriteCommand(Configuration.ProjectorId, "84 ", "3"))
                {
                    if (State.VerticalLensShift < Configuration.NumberOfRequestsForGoingFromBottomToTopLensShift)
                    {
                        State.VerticalLensShift += 1;
                    }
                    SaveState();
                }
                else
                {
                    return false;
                }
            }
            return true;
        }


        public bool MoveLensVerticalShiftDown(int numberOfTimes = 1)
        {
            for (int i = 0; i < numberOfTimes; ++i)
            {
                if (SendWriteCommand(Configuration.ProjectorId, "84 ", "4"))
                {
                    if (State.VerticalLensShift > 0)
                    {
                        State.VerticalLensShift -= 1;
                    }
                    SaveState();
                }
                else
                {
                    return false;
                }
            }
            return true;
        }


        public bool MoveLensHorizontalShiftRight(int numberOfTimes = 1)
        {
            for (int i = 0; i < numberOfTimes; ++i)
            {
                if (SendWriteCommand(Configuration.ProjectorId, "84 ", "6"))
                {
                    if (State.HorizontalLensShift < Configuration.NumberOfRequestsForGoingFromLeftToRightLensShift)
                    {
                        State.HorizontalLensShift += 1;
                    }
                    SaveState();
                }
                else
                {
                    return false;
                }
            }
            return true;
        }


        public bool MoveLensHorizontalShiftLeft(int numberOfTimes = 1)
        {
            for (int i = 0; i < numberOfTimes; ++i)
            {
                if (SendWriteCommand(Configuration.ProjectorId, "84 ", "5"))
                {
                    if (State.HorizontalLensShift > 0)
                    {
                        State.HorizontalLensShift -= 1;
                    }
                    SaveState();
                }
                else
                {
                    return false;
                }
            }
            return true;
        }


        public bool ActivateStoredProfile()
        {
            return ActivateStoredProfile(Profiles.ActiveProfileIndex);
        }


        public bool ActivateStoredProfile(int profileNumber)
        {
            if (profileNumber >= 0 && profileNumber < Profiles.ProfilesList.Count)
            {
                if (ActivateProfile(Profiles.ProfilesList[profileNumber]))
                {
                    Profiles.ActiveProfileIndex = profileNumber;
                    SaveProfiles();
                }
            }
            return false;
        }


        public bool ActivateProfile(State profile)
        {
            int zoomDifference = profile.Zoom - State.Zoom;
            int focusDifference = profile.Focus - State.Focus;
            int horizontalLensShiftDifference = profile.HorizontalLensShift - State.HorizontalLensShift;
            int verticalLensShiftDifference = profile.VerticalLensShift - State.VerticalLensShift;

            bool zoomStatus = true, focusStatus = true, lensShiftStatus = true, horizontalLensShiftStatus = true, verticalLensShift = true;

            if (zoomDifference > 0)
            {
                zoomStatus = IncreaseZoomForHigherProjectionSize(zoomDifference);
            }
            else if (zoomDifference < 0)
            {
                zoomStatus = DecreaseZoomForLowerProjectionSize(-zoomDifference);
            }

            if (focusDifference > 0)
            {
                focusStatus = IncreaseFocusToHigherProjectionDistance(focusDifference);
            }
            else if (focusDifference < 0)
            {
                focusStatus = DecreaseFocusToLowerProjectionDistance(-focusDifference);
            }

            if (profile.LensMemoryProfile >= 1 && profile.LensMemoryProfile <= 5)
            {
                lensShiftStatus = ApplyLensShiftPosition(profile.LensMemoryProfile);
            }
            else
            {
                if (horizontalLensShiftDifference > 0)
                {
                    horizontalLensShiftStatus = MoveLensHorizontalShiftRight(horizontalLensShiftDifference);
                }
                else if (horizontalLensShiftDifference < 0)
                {
                    horizontalLensShiftStatus = MoveLensHorizontalShiftLeft(-horizontalLensShiftDifference);
                }

                if (verticalLensShiftDifference > 0)
                {
                    verticalLensShift = MoveLensVerticalShiftUp(verticalLensShiftDifference);
                }
                else if (verticalLensShiftDifference < 0)
                {
                    verticalLensShift = MoveLensVerticalShiftDown(-verticalLensShiftDifference);
                }
            }

            return zoomStatus && focusStatus && lensShiftStatus && horizontalLensShiftStatus && verticalLensShift;
        }


        public bool SendWriteCommand(string projectorId, string commandId, string parameter)
        {
            if (TcpClient.Connected)
            {
                StringBuilder command = new StringBuilder();
                command.Append('~');
                command.Append(projectorId);
                command.Append(commandId);
                command.Append(' ');
                command.Append(parameter);
                command.Append("\r");

                byte[] commandBytes = Encoding.ASCII.GetBytes(command.ToString());

                NetworkStream networkStream = TcpClient.GetStream();
                var writer = new StreamWriter(networkStream, Encoding.ASCII);
                var reader = new StreamReader(networkStream, Encoding.ASCII);

                networkStream.Write(commandBytes, 0, commandBytes.Length);

                char[] responseBytes = new char[2]; // response should be "P\r"
                int bytesRead = 0;

                while (bytesRead < 2)
                {
                    try
                    {
                        bytesRead += reader.Read(responseBytes, 0, responseBytes.Length);
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(10);
                    }
                }

                return true;
            }

            return false;
        }
    }
}
