using System;
using System.Net.Sockets;

namespace OptomaTelnetControl
{
    public class OptomaTelnetControl : IDisposable
    {
        private bool _disposed = false;

        public TcpClient TcpClient { get; set; }
        public OptomaTelnetControlConfiguration Configuration { get; set; } = new OptomaTelnetControlConfiguration();
        public OptomaTelnetControlState State { get; set; } = new OptomaTelnetControlState();
        public StateProfiles Profiles { get; set; } = new StateProfiles();


        public OptomaTelnetControl() { }


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    TcpClient?.Dispose();
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


        public void LoadXmls()
        {
            LoadConfiguration();
            LoadState();
            LoadProfiles();
        }


        public void SaveXmls()
        {
            SaveConfiguration();
            SaveState();
            SaveProfiles();
        }


        public void LoadConfiguration(string filePath = "xml/Configuration.xml")
        {
            Configuration = Tools.LoadFromXml<OptomaTelnetControlConfiguration>(filePath);
        }


        public void SaveConfiguration(string filePath = "xml/Configuration.xml")
        {
            Tools.SaveToXml(filePath, Configuration);
        }


        public void LoadState(string filePath = "xml/State.xml")
        {
            State = Tools.LoadFromXml<OptomaTelnetControlState>(filePath);
        }


        public void SaveState(string filePath = "xml/State.xml")
        {
            Tools.SaveToXml(filePath, State);
        }


        public void LoadProfiles(string filePath = "xml/Profiles.xml")
        {
            Profiles = Tools.LoadFromXml<StateProfiles>(filePath);
        }


        public void SaveProfiles(string filePath = "xml/Profiles.xml")
        {
            Tools.SaveToXml(filePath, Profiles);
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
                
                return true;
            }

            return false;
        }


        public bool PowerOn()
        {
            return false;
        }


        public bool PowerOff()
        {
            return false;
        }


        public bool LockLens(bool state)
        {
            return false;
        }


        public bool ResetLensShift()
        {
            return false;
        }


        public bool CenterLensShift()
        {
            return false;
        }


        public bool SaveLensShiftPosition(int profileNumber = 1)
        {
            return false;
        }


        public bool LoadLensShiftPosition(int profileNumber = 1)
        {
            return false;
        }


        public bool IncreaseFocus(int numberOfTimes = 1)
        {
            return false;
        }


        public bool DecreaseFocus(int numberOfTimes = 1)
        {
            return false;
        }


        public bool IncreaseZoom(int numberOfTimes = 1)
        {
            return false;
        }


        public bool DecreaseZoom(int numberOfTimes = 1)
        {
            return false;
        }


        public bool IncreaseLensVerticalShift(int numberOfTimes = 1)
        {
            return false;
        }


        public bool DecreaseLensVerticalShift(int numberOfTimes = 1)
        {
            return false;
        }


        public bool IncreaseLensHorizontalShift(int numberOfTimes = 1)
        {
            return false;
        }


        public bool DecreaseLensHorizontalShift(int numberOfTimes = 1)
        {
            return false;
        }


        public bool ActivateStoredProfile(int profileNumber)
        {
            if (Profiles.ActiveProfileIndex < Profiles.ProfilesList.Count)
                return ActivateProfile(Profiles.ProfilesList[Profiles.ActiveProfileIndex]);
            return false;
        }


        public bool ActivateProfile(OptomaTelnetControlState profile)
        {
            return false;
        }
    }
}
