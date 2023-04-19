using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NationalInstruments.NI4882;

namespace Lab17
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Device device;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Root_Loaded(object sender, RoutedEventArgs e)
        {
            // Setup GPIB Combo Box
            for(int i = 0; i < 100; i++)
            {
                cboGPIB.Items.Add("GPIB" + i.ToString());
            }
            cboGPIB.SelectedIndex = 0;

            // Setup Device Address Combo Box
            for(int i = 0; i <=30; i++)
            {
                cboDevice.Items.Add(i.ToString());
            }
            cboDevice.SelectedIndex = 16;
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            // Component Control
            btnOpen.IsEnabled = false;
            btnClose.IsEnabled = true;
            rbn196.IsEnabled = false;
            rbn2000.IsEnabled = false;
            cboDevice.IsEnabled = false;
            cboGPIB.IsEnabled = false;
            btnRead.IsEnabled = true;

            // Device Setup
            int boardNumber = Convert.ToInt32(cboGPIB.SelectedIndex);
            int boardAddress = Convert.ToInt32(cboDevice.SelectedIndex);
            device = new Device(boardNumber, (byte)boardAddress);

            if(rbn196.IsChecked == true)
            {
                device.Write("L0X");    // Default Conditions:     Start from factory default conditions
                device.Write("F0X");    // Function Mode:          DC Volts
                device.Write("R0X");    // Range:                  Auto
                device.Write("S3X");    // Rate:                   6 1/2 d
                device.Write("G2X");    // Data Format:            Buffer readings with prefixes and buffer locations
                device.Write("Q50X");   // Data Store Interval:    50ms
                device.Write("T4X");    // Trigger Mode:           Continuous on X
                device.Write("I100X");  // Data Store Size:        Data store of 100
                device.Write("B1X");    // Reading Mode:           Readings from data store
            }
            else
            {
                device.Write("*RST");
                device.Write(":SENSE:FUNC \"VOLT:DC\"");
                device.Write(":SENSE:VOLT:DC:RANG:AUTO ON");
                device.Write(":TRAC:POIN 100");
                device.Write(":TRIG:COUN 100");
                device.Write(":TRIG:DEL 0.05");
                device.Write(":TRIG:SOUR BUS");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            btnOpen.IsEnabled = true;
            btnClose.IsEnabled = false;
            rbn196.IsEnabled = true;
            rbn2000.IsEnabled = true;
            cboDevice.IsEnabled = true;
            cboGPIB.IsEnabled = true;
            btnRead.IsEnabled = false;
            device.Dispose();
        }

        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rbn196.IsChecked == true)
                {
                    device.Write("I0X");
                    device.Write("I100X");
                    string inData = device.ReadString();
                    inData = inData + device.ReadString();
                    inData = inData + device.ReadString();

                    string[] dataPoints = inData.Split(',');

                    for (int i = 0; i < dataPoints.Length; i = i + 2)
                    {
                        dataPoints[i] = dataPoints[i] + "," + dataPoints[i + 1];
                        lstData.Items.Add(dataPoints[i]);
                    }
                    lstData.SelectedIndex = lstData.Items.Count - 1;
                    lstData.ScrollIntoView(lstData.Items[lstData.Items.Count - 1]);
                }
                else
                {
                    lstData.Items.Add(device.ReadString());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lstData.Items.Clear();
        }

        private void Root_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            device.Dispose();
        }
    }
}
