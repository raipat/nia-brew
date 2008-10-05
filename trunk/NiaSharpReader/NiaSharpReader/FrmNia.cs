using System;
using System.Windows.Forms;

// Specific to hid calls
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

// For ZedGraph
using System.Drawing;
using ZedGraph;

namespace NiaReader
{
    public partial class FrmNia : Form
    {
        #region Declarations
        /// <summary>
        /// Device management helper class
        /// </summary>
        private GenericHid.DeviceManagement DeviceManagement = new GenericHid.DeviceManagement();
        /// <summary>
        /// Hid helper class
        /// </summary>
        private GenericHid.Hid MyHid = new GenericHid.Hid();
        /// <summary>
        /// Needed to get a handle on hid devices
        /// </summary>
        private SafeFileHandle hidHandle;
        private SafeFileHandle readHandle;
        private SafeFileHandle writeHandle;
        /// <summary>
        /// Needed for ondevicechange notifications
        /// </summary>
        private IntPtr deviceNotificationHandle;
        /// <summary>
        /// Hid path to the NIA
        /// </summary>
        private string NiaPathName = string.Empty;
        /// <summary>
        /// If the NIA was detected
        /// </summary>
        private bool NiaDetected = false;
        #endregion

        #region Init/Dispose Actions
        public FrmNia()
        {
            InitializeComponent();
			this.Text       += string.Concat(" : v", this.ProductVersion);
            this.Disposed   += new EventHandler(FrmNia_Disposed);
            this.timer.Tick += new EventHandler(timer_Tick);

            if (!FindNia())
                MessageBox.Show("NIA is not connected.", "Error");

            this.timer.Interval = 1;
        }
        void FrmNia_Disposed(object sender, EventArgs e)
        {
            DisposeNIA();
        }
        #endregion

        #region Form Actions
		private void ReadSyncButton_Click(object sender, EventArgs e)
		{
            if (!NiaDetected)
            {
                if (!FindNia())
                {
                    MessageBox.Show("NIA is not connected.", "Error");
                    return;
                }
            }
			
            DataListBox.Items.Clear();				
            timer.Enabled = true;
		}
        private void StopButton_Click(object sender, EventArgs e)
        {
			timer.Enabled = false;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            ReadFromNiaSync();

            if (DataListBox.Items.Count >= 0)
                DataListBox.SelectedIndex = (DataListBox.Items.Count-1);

            if (DataListBox.Items.Count > 200)
                DataListBox.Items.RemoveAt(0);
        }
        #endregion

        #region Find/Close NIA
        public bool FindNia()
        {
            bool success = false;

            try
            {
                // Get the guid for the system hid class
                Guid hidGuid = Guid.Empty;
                GenericHid.Hid.HidD_GetHidGuid(ref hidGuid);

                // Find all devices of type hid
                string[] deviceCollection = new String[128];
                bool devicesFound = DeviceManagement.FindDeviceFromGuid(hidGuid, ref deviceCollection);
                
                // Did we find any hid devices ?            
                if (devicesFound)
                {
                    int memberIndex = 0;
                    do
                    {
                        // try to get a handle on the current hid device in the list
						hidHandle = GenericHid.FileIO.CreateFile(deviceCollection[memberIndex], 0, GenericHid.FileIO.FILE_SHARE_READ | GenericHid.FileIO.FILE_SHARE_WRITE, null, GenericHid.FileIO.OPEN_EXISTING, 0, 0);

                        if (!hidHandle.IsInvalid)
                        {                            
                            // Set the Size property of DeviceAttributes to the number of bytes in the structure.
                            MyHid.DeviceAttributes.Size = Marshal.SizeOf(MyHid.DeviceAttributes);

                            // try to get the hid's information
							success = GenericHid.Hid.HidD_GetAttributes(hidHandle, ref MyHid.DeviceAttributes);
                            if (success)
                            {
                                if ((MyHid.DeviceAttributes.VendorID == 4660) & (MyHid.DeviceAttributes.ProductID == 0))
                                {
                                    NiaDetected = true;

                                    // Save the DevicePathName for OnDeviceChange().
                                    NiaPathName = deviceCollection[memberIndex];
                                }
                                else
                                {
                                    NiaDetected = false;
                                    hidHandle.Close();
                                }
                            }
                            else
                            {
                                NiaDetected = false;
                                hidHandle.Close();
                            }
                        }

                        //  Keep looking until we find the device or there are no devices left to examine.
                        memberIndex = memberIndex + 1;

                    } while (!((NiaDetected | (memberIndex == deviceCollection.Length))));

                    // Did we find a NIA ?
                    if (NiaDetected)
                    {
                        // The device was detected.
                        // Register to receive notifications if the device is removed or attached.
                        success = DeviceManagement.RegisterForDeviceNotifications(this.NiaPathName, this.Handle, hidGuid, ref deviceNotificationHandle);

                        if (success)
                        {
                            //  Get handles to use in requesting Input and Output reports.
							readHandle = GenericHid.FileIO.CreateFile(this.NiaPathName, GenericHid.FileIO.GENERIC_READ, GenericHid.FileIO.FILE_SHARE_READ | GenericHid.FileIO.FILE_SHARE_WRITE, null, GenericHid.FileIO.OPEN_EXISTING, GenericHid.FileIO.FILE_FLAG_OVERLAPPED, 0);

                            if (!readHandle.IsInvalid)
                            {
								writeHandle = GenericHid.FileIO.CreateFile(this.NiaPathName, GenericHid.FileIO.GENERIC_WRITE, GenericHid.FileIO.FILE_SHARE_READ | GenericHid.FileIO.FILE_SHARE_WRITE, null, GenericHid.FileIO.OPEN_EXISTING, 0, 0);
                                MyHid.FlushQueue(readHandle);
                                
                                ZedInit();
                            }
                        }
                    }
                }

                return NiaDetected;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
		private void DisposeNIA()
		{
			try
			{
				//  Close open handles to the device.
				if (!(hidHandle == null))
				{
					if (!(hidHandle.IsInvalid))
					{
						hidHandle.Close();
					}
				}

				if (!(readHandle == null))
				{
					if (!(readHandle.IsInvalid))
					{
						readHandle.Close();
					}
				}

				if (!(writeHandle == null))
				{
					if (!(writeHandle.IsInvalid))
					{
						writeHandle.Close();
					}
				}

				//  Stop receiving notifications.
				DeviceManagement.StopReceivingDeviceNotifications(deviceNotificationHandle);

				// Stop the timer
				timer.Enabled = false;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
        #endregion

        #region Read from NIA
		/// <summary>
		/// Something is fishy here since time is negative sometimes ! threading ?
		/// </summary>
		public static int last = 0;
		private void ShowMS(int now)
		{			
			int current = (now - last);
			SampleRateLabel.Text = string.Format("{0} ms", current);		
			last = now;			
		}
		///  <summary>
		///  Retrieves Input report data and status information.
		///  Used for asynchronous reads from the device.  
		///  </summary>
		private void ReadFromNiaSync()
        {
            Byte[] inputReportBuffer = new Byte[56];
            Boolean success = false;

			GenericHid.Hid.InputReportViaInterruptTransfer myInputReport = new GenericHid.Hid.InputReportViaInterruptTransfer();
            myInputReport.Read(hidHandle, readHandle, writeHandle, ref NiaDetected, ref inputReportBuffer, ref success);

			ShowMS(DateTime.Now.Millisecond);
			       
			if (success)
                Interpret(inputReportBuffer);
        }
        #endregion

        #region Interpret Data
		private static long timerValue = 0;
        private void Interpret(Byte[] data)
        {
			// interpretation taken from niawiimote hack						
			long validPackets = data[55];
			long packetTimer  = data[54] * 256 + data[53] - validPackets;
		
			for (long index = 0; index <= (validPackets - 1); index++)
			{
				long timerPosition = (packetTimer + index);
                long rawData = data[index * 3 + 1] * 1 + data[index * 3 + 2] * 256 + data[index * 3 + 3] * 65535;

				if (timerValue > timerPosition)
				{
                    DataListBox.Items.Add("Timer Reset");
                    UpdateZedGraph(0, 0);
				}
				timerValue = timerPosition;

                // Max value detected 16776960(0xFFFF00)/2 = 8388480(0x7FFF80)
                rawData = (rawData - 8388480);             

				string output = string.Concat("Timer: ", timerPosition, "\tData: ", rawData);
                DataListBox.Items.Add(output);
                UpdateZedGraph(timerPosition, rawData);
			}
        }
        #endregion

        #region ZedGraph
        private void ZedInit()
        {
            GraphPane myPane = zedGraphControl.GraphPane;

            myPane.Title.Text       = "Dynamic NIA Data";
            myPane.XAxis.Title.Text = "TimeLine";
            myPane.YAxis.Title.Text = "Sample Value";

            // 1200:50ms, 2400:25ms, 4800:12.5ms, 9600:6.25ms, 19200:3.125ms, 38400:1.5625ms
            // Save 1200 points.  At 50 ms sample rate, this is one minute
            // The RollingPointPairList is an efficient storage class that always
            // keeps a rolling set of point data without needing to shift any data values
            RollingPointPairList list1 = new RollingPointPairList(9600);

            // Initially, a curve is added with no data points (list is empty)
            // Color is blue, and there will be no symbols
            LineItem curve1 = myPane.AddCurve("NIA Input", list1, Color.Red, SymbolType.None);
            curve1.Line.IsOptimizedDraw = true;

            // Scale XAxis : Time
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = 65535; //65528 (0xFFF8): max value detected for timer packet
            
            // Scale YAxis: Value
			myPane.YAxis.Scale.Min = -10000000;
            myPane.YAxis.Scale.Max = 10000000; //16776960 (0xFFFF00): max value detected for data packet;

            // Scale the axes
            zedGraphControl.AxisChange();

            
        }
        private void UpdateZedGraph(double time, double value)
        {
            // Make sure that the curvelist has at least one curve
            if (zedGraphControl.GraphPane.CurveList.Count <= 0)
                return;

            // Get the first CurveItem in the graph
            LineItem curve1 = zedGraphControl.GraphPane.CurveList[0] as LineItem;
            if (curve1 == null)
                return;
            curve1.Line.IsOptimizedDraw = true;

            // Get the PointPairList
            IPointListEdit list1 = curve1.Points as IPointListEdit;
            // If this is null, it means the reference at curve.Points does not
            // support IPointListEdit, so we won't be able to modify it
            if (list1 == null)
                return;

			// remove the backtrail
			if (time == 0)
			{
				list1.Clear();
				zedGraphControl.Invalidate();
				return;
			}
            
            // Add points to the chart
            list1.Add(time, value);

            // Force a redraw
            zedGraphControl.Invalidate();
        }
        #endregion

        #region OnDeviceChange Event Handlers
        ///  <summary>
        ///  Called when a WM_DEVICECHANGE message has arrived,
        ///  indicating that a device has been attached or removed.
        ///  </summary>
        ///  <param name="m"> a message with information about the device </param>
        /// robert : Not yet working
        public void OnDeviceChange(Message m)
        {
            try
            {
				if ((m.WParam.ToInt32() == GenericHid.DeviceManagement.DBT_DEVICEARRIVAL))
                {
                    //  Find out if it's the device we're communicating with.
                    if (!this.NiaDetected && DeviceManagement.DeviceNameMatch(m, this.NiaPathName))
                    {
                        MessageBox.Show("NIA has been connected.");
                    }
                }
				else if ((m.WParam.ToInt32() == GenericHid.DeviceManagement.DBT_DEVICEREMOVECOMPLETE))
                {
                    //  Find out if it's the device we're communicating with.

					if (this.NiaDetected && DeviceManagement.DeviceNameMatch(m, this.NiaPathName))
                    {
                        MessageBox.Show("NIA has been disconnected.");

                        //  Set MyDeviceDetected False so on the next data-transfer attempt,
                        //  FindTheHid() will be called to look for the device 
                        //  and get a new handle.
                        this.NiaDetected = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        ///  <summary>
        ///   Overrides WndProc to enable checking for and handling WM_DEVICECHANGE messages.
        ///  </summary>
        ///  <param name="m"> a Windows Message </param>
        protected override void WndProc(ref Message m)
        {
            try
            {
                //  The OnDeviceChange routine processes WM_DEVICECHANGE messages.
				if (m.Msg == GenericHid.DeviceManagement.WM_DEVICECHANGE)
                {
                    OnDeviceChange(m);
                }

                //  Let the base form process the message.
                base.WndProc(ref m);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
        #endregion
    }
}
