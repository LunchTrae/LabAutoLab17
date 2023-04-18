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
            cboDevice.SelectedIndex = 0;
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
                //device.Write("F0X"); // DC Volts
                //device.Write("B1X"); // Readings from data store
                //device.Write("I100X"); // Data store of n
                //device.Write("")
                //device.Write("G5X"); // Buffer readings without prefixes and without buffer locations
            }
            else
            {
                
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
            lstData.Items.Add(device.ReadString());
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
