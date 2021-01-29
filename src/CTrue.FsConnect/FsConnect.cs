using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.FlightSimulator.SimConnect;
using System.Diagnostics;

namespace CTrue.FsConnect
{
    /// <inheritdoc />
    public class FsConnect : IFsConnect
    {
        private SimConnect _simConnect = null;

        private EventWaitHandle _simConnectEventHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
        private Thread _simConnectReceiveThread = null;
        private bool _connected;

        #region Simconnect structures

        public enum Definitions
        {
            PlaneInfo = 0,
            PlaneCommit = 1,
            PlaneRotate = 2,
            NearbyObjects = 3,
            TowPlane = 4,
            Airport = 5,
            SystemEvents = 6,
        }

        #endregion

        /// <inheritdoc />
        public bool Connected
        {
            get => _connected;
            private set
            {
                if (_connected != value)
                {
                    _connected = value;
                    ConnectionChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <inheritdoc />
        /// MY DOCUMENTS IS DANGEROUS METHOD!!!
        public SimConnectFileLocation SimConnectFileLocation { get; set; } = SimConnectFileLocation.Local;

        /// <inheritdoc />
        public event EventHandler ConnectionChanged;

        /// <inheritdoc />
        public event EventHandler<FsDataReceivedEventArgs> FsDataReceived;

        public event EventHandler<AirportDataReceivedEventArgs> AirportDataReceived;

        public event EventHandler<ObjectAddremoveEventReceivedEventArgs> ObjectAddremoveEventReceived;

        /// <inheritdoc />
        public event EventHandler<FsErrorEventArgs> FsError;

        /// <inheritdoc />
        public void Connect(string applicationName, uint configIndex = 0)
        {
            try
            {
                _simConnect = new SimConnect(applicationName, IntPtr.Zero, 0, _simConnectEventHandle, configIndex);
            }
            catch (Exception e)
            {
                _simConnect = null;
                throw new Exception("Could not connect to Flight Simulator: " + e.Message, e);
            }

            _simConnectReceiveThread = new Thread(new ThreadStart(SimConnect_MessageReceiveThreadHandler));
            _simConnectReceiveThread.IsBackground = true;
            _simConnectReceiveThread.Start();

            _simConnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(SimConnect_OnRecvOpen);
            _simConnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(SimConnect_OnRecvQuit);

            _simConnect.OnRecvException += new SimConnect.RecvExceptionEventHandler(SimConnect_OnRecvException);
            _simConnect.OnRecvSimobjectDataBytype += new SimConnect.RecvSimobjectDataBytypeEventHandler(SimConnect_OnRecvSimobjectDataBytype);
            _simConnect.OnRecvAirportList += new SimConnect.RecvAirportListEventHandler(SimConnect_OnRecvAirportListEventHandler);
            _simConnect.OnRecvEventObjectAddremove += new SimConnect.RecvEventObjectAddremoveEventHandler(SimConnect_OnRecvEventObjectAddremoveEventHandler); 

            _simConnect.SubscribeToSystemEvent(Definitions.SystemEvents, "ObjectAdded");
        }

        /// <inheritdoc />
        public void Connect(string applicationName, string hostName, uint port, SimConnectProtocol protocol)
        {
            if (applicationName == null) throw new ArgumentNullException(nameof(applicationName));

            CreateSimConnectConfigFile(hostName, port, protocol);

            Connect(applicationName);
        }

        /// <inheritdoc />
        public void Disconnect()
        {
            if (!Connected) return;

            try
            {
                _simConnectReceiveThread.Abort();
                _simConnectReceiveThread.Join();

                _simConnect.OnRecvOpen -= new SimConnect.RecvOpenEventHandler(SimConnect_OnRecvOpen);
                _simConnect.OnRecvQuit -= new SimConnect.RecvQuitEventHandler(SimConnect_OnRecvQuit);
                _simConnect.OnRecvException -= new SimConnect.RecvExceptionEventHandler(SimConnect_OnRecvException);
                _simConnect.OnRecvSimobjectDataBytype -= new SimConnect.RecvSimobjectDataBytypeEventHandler(SimConnect_OnRecvSimobjectDataBytype);
                _simConnect.OnRecvAirportList -= new SimConnect.RecvAirportListEventHandler(SimConnect_OnRecvAirportListEventHandler);
                _simConnect.OnRecvEventObjectAddremove -= new SimConnect.RecvEventObjectAddremoveEventHandler(SimConnect_OnRecvEventObjectAddremoveEventHandler);

                _simConnect.UnsubscribeFromSystemEvent(Definitions.SystemEvents);

                _simConnect.Dispose();
            }
            catch (Exception e)
            {
            }
            finally
            {
                _simConnectReceiveThread = null;
                _simConnect = null;
                Connected = false;
            }
        }

        /// <inheritdoc />
        public void RegisterDataDefinition<T>(Enum id, List<SimProperty> definition) where T : struct
        {
            foreach (var item in definition)
            {
                _simConnect.AddToDataDefinition(id, item.Name, item.Unit, item.DataType, 0.0f, SimConnect.SIMCONNECT_UNUSED);
            }

            _simConnect.RegisterDataDefineStruct<T>(id);
        }

        /// <inheritdoc />
        public void RequestData(Enum requestId, Enum defineId, uint radius = 0, SIMCONNECT_SIMOBJECT_TYPE type = SIMCONNECT_SIMOBJECT_TYPE.USER)
        {
            try
            {
                _simConnect?.RequestDataOnSimObjectType(requestId, defineId, radius, type);
            } catch (Exception e) {
                Disconnect();
            }
        }

        /// <inheritdoc />
        public void UpdateData<T>(Enum id, T data, uint ObjID = 1)
        {
            _simConnect?.SetDataOnSimObject(id, ObjID, SIMCONNECT_DATA_SET_FLAG.DEFAULT, data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="duration"></param>
        public void SetText(string text, int duration)
        {
            _simConnect.Text(SIMCONNECT_TEXT_TYPE.PRINT_BLACK, duration, Definitions.PlaneInfo, text);
        }

        private void SimConnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
        {
            Connected = true;
        }

        private void SimConnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
        {
            Disconnect();
        }

        private void SimConnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
        {
            SIMCONNECT_EXCEPTION eException = (SIMCONNECT_EXCEPTION)data.dwException;

            FsError?.Invoke(this, new FsErrorEventArgs()
            {
                Exception = data.dwException,
                ExceptionDescription = eException.ToString(),
                SendID = data.dwSendID,
                Index = data.dwIndex
            });
        }

        private void SimConnect_OnRecvSimobjectDataBytype(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
        {
            FsDataReceived?.Invoke(this, new FsDataReceivedEventArgs()
            {
                RequestId = data.dwRequestID,
                Data = data.dwData[0],
                ObjectID = data.dwObjectID
            });
        }

        private void SimConnect_OnRecvAirportListEventHandler(SimConnect sender, SIMCONNECT_RECV_AIRPORT_LIST data)
        {
            AirportDataReceived?.Invoke(this, new AirportDataReceivedEventArgs()
            {
                RequestId = data.dwRequestID,
                Data = data.rgData[0],
                ObjectID = data.dwID
            });
        }

        private void SimConnect_OnRecvEventObjectAddremoveEventHandler(SimConnect sender, SIMCONNECT_RECV_EVENT recEvent)
        {
            ObjectAddremoveEventReceived?.Invoke(this, new ObjectAddremoveEventReceivedEventArgs()
            {
                RequestId = recEvent.uEventID,
                Data = recEvent.dwData,
                ObjectID = recEvent.dwID
            });
        }

        private void SimConnect_MessageReceiveThreadHandler()
        {
            while (true)
            {
                _simConnectEventHandle.WaitOne();

                try
                {
                    _simConnect?.ReceiveMessage();
                }
                catch (Exception e)
                {
                }
            }
        }

        private void CreateSimConnectConfigFile(string hostName, uint port, SimConnectProtocol protocol)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                string protocolString = "Ipv4";

                switch (protocol)
                {
                    case SimConnectProtocol.Pipe:
                        protocolString = "Pipe";
                        break;
                    case SimConnectProtocol.Ipv4:
                        protocolString = "Ipv4";
                        break;
                    case SimConnectProtocol.Ipv6:
                        protocolString = "Ipv6";
                        break;
                }

                sb.AppendLine("[SimConnect]");
                sb.AppendLine("Protocol=" + protocolString);
                sb.AppendLine($"Address={hostName}");
                sb.AppendLine($"Port={port}");

                string directory = "";
                if (SimConnectFileLocation == SimConnectFileLocation.Local)
                {
                    directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                    string docDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    // DELETE UNWANTED CFG FILE
                    try
                    {
                        if (File.Exists(Path.Combine(docDirectory, "SimConnect.cfg")) &&
                        sb.ToString() == File.ReadAllText(Path.Combine(docDirectory, "SimConnect.cfg"))) {
                            File.Delete(Path.Combine(docDirectory, "SimConnect.cfg"));
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Failed to delete SimConnect.cfg from My Documents folder");
                    }
                }
                else
                {
                    directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                string fileName = Path.Combine(directory, "SimConnect.cfg");

                File.WriteAllText(fileName, sb.ToString());
            }
            catch (Exception e)
            {
                throw new Exception("Could not create SimConnect.cfg file: " + e.Message, e);
            }
        }

        //public void AICreateSimulatedObject(string szContainerTitle, SIMCONNECT_DATA_INITPOSITION InitPos, Enum RequestID);
        //public void AIRemoveObject(uint ObjectID, Enum RequestID);

        // instance.SetAircraftFlightPlan(*objID, 1000, "C:\\Users\\Jacques\\Desktop\\EGCCLFPG")
        // instance.LoadParkedATCAircraft("Boeing 747-8i Asobo", "G-420", "EGCC", 100)

        //hr = SimConnect_SubscribeToSystemEvent(hSimConnect, EVENT_ADDED_AIRCRAFT, "ObjectAdded");
        //hr = SimConnect_SubscribeToSystemEvent(hSimConnect, EVENT_REMOVED_AIRCRAFT, "ObjectRemoved");
        // AI TRAFFIC STATE
        // 0: Sleep 1: Zombie 2: Waypoint 3: Takeoff 4: Landing 5: Taxi 6: Working 7: Waiting

        public void CreateEnrouteATCAircraft(string szFlightPlanPath, string title, double position, Enum RequestID)
        {
            try
            {
                _simConnect?.AICreateEnrouteATCAircraft(title, "TUG", 9999, szFlightPlanPath, position, false, RequestID);
            }
            catch (Exception e)
            {
                Disconnect();
            }
        }

        public void CreateNonATCAircraft(SIMCONNECT_DATA_INITPOSITION InitPos, string title, Enum RequestID)
        {
            try
            {
                _simConnect?.AICreateNonATCAircraft(title, "TUG", InitPos, RequestID);
            }
            catch (Exception e)
            {
                Disconnect();
            }
        }

        public void LoadParkedATCAircraft(string icao, string title, Enum RequestID)
        {
            try
            {
                _simConnect?.AICreateParkedATCAircraft(title, "TUG", icao, RequestID);
            }
            catch (Exception e)
            {
                Disconnect();
            }
        }

        public void AIReleaseControl(uint ObjectID, Enum RequestID)
        {
            try
            {
                _simConnect?.AIReleaseControl(ObjectID, RequestID);
            }
            catch (Exception e)
            {
                Disconnect();
            }
        }

        public void AISetAircraftFlightPlan(uint ObjectID, string szFlightPlanPath, Enum RequestID)
        {
            try
            {
                _simConnect?.AISetAircraftFlightPlan(ObjectID, szFlightPlanPath, RequestID);
            }
            catch (Exception e)
            {
                Disconnect();
            }
        }

        // 23-01-2021 THIS REQUEST IS BROKEN
        public void RequestFacilitiesList(Enum RequestID) {
            try
            {
                _simConnect?.RequestFacilitiesList(SIMCONNECT_FACILITY_LIST_TYPE.AIRPORT, RequestID);
            }
            catch (Exception e)
            {
                Disconnect();
            }

            //protected void raise_OnRecvAirportList(SimConnect value0, SIMCONNECT_RECV_AIRPORT_LIST value1);
        }

        // To detect redundant calls
        private bool _disposed = false;

        /// <summary>
        /// Disconnects and disposes the client.
        /// </summary>
        public void Dispose() => Dispose(true);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // Dispose managed state (managed objects).
                _simConnect?.Dispose();
            }

            _disposed = true;
        }

    }
}
