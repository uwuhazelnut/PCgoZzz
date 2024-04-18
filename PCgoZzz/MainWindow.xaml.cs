using System;
using System.Collections.Generic;
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
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace PCgoZzz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Import the SetSuspendState function from kernel32.dll
        [DllImport("powrprof.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        static extern uint SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);

        private DateTime lastInputTime;

        public MainWindow()
        {
            InitializeComponent();

            lastInputTime = DateTime.Now;

            // Set up a timer to periodically check idle time
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Check every minute
            timer.Tick += Timer_Tick;
            timer.Start();

            // Subscribe to user input events
            InputManager.Current.PreProcessInput += OnUserInput;
        }

        private void OnUserInput(object sender, PreProcessInputEventArgs e)
        {
            // Update the last input time whenever a user input event occurs
            lastInputTime = DateTime.Now;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Check if the PC has been idle for one hour
            TimeSpan idleTime = DateTime.Now - lastInputTime;
            if (idleTime.TotalSeconds >= 5)
            {
                // Execute the desired function when the PC has been idle for one hour
                ExecuteFunction();
            }

            if (idleTime.TotalHours >= 1)
            {
                // Execute the desired function when the PC has been idle for one hour
                ExecuteFunction();
            }

            // Update the label's content to display the idle time
            LblIdleTime.Content = $"Idle Time: {idleTime.Hours:D2}:{idleTime.Minutes:D2}:{idleTime.Seconds:D2}";
        }

        private void ExecuteFunction()
        {
            // Add code here to execute the desired function
            MessageBox.Show("I LOVE HAZEL!");
        }

        private void BtnZzz_Click(object sender, RoutedEventArgs e)
        {
            // Put the PC to sleep
            SetSuspendState(false, true, false);
        }
    }
}
