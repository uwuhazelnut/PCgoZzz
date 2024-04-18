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

        private System.Timers.Timer timer;
        private TimeSpan remainingTime;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SetTimer_Click(object sender, RoutedEventArgs e)
        {
            // Parse hours and minutes from TextBoxes
            if (int.TryParse(hoursTextBox.Text, out int hours) &&
                int.TryParse(minutesTextBox.Text, out int minutes))
            {
                // Calculate total remaining time
                remainingTime = TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes);

                // Update timer label
                UpdateTimerLabel();
            }
            else
            {
                MessageBox.Show("Invalid input. Please enter valid integer values for hours and minutes.");
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // Create or update the timer
            if (timer == null)
            {
                timer = new System.Timers.Timer(1000); // Interval in milliseconds
                timer.Elapsed += Timer_Elapsed;
            }
            else
            {
                timer.Stop();
            }

            // Start the timer
            timer.Start();

            // Disable Set Timer button and enable Stop button
            startButton.IsEnabled = false;
            stopButton.IsEnabled = true;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            // Stop the timer
            if (timer != null)
            {
                timer.Stop();
            }

            // Enable Set Timer button and disable Stop button
            startButton.IsEnabled = true;
            stopButton.IsEnabled = false;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Decrease remaining time by one second
            remainingTime = remainingTime.Subtract(TimeSpan.FromSeconds(1));

            // Update timer label
            Dispatcher.Invoke(() => UpdateTimerLabel());

            // Check if timer reached 0
            if (remainingTime <= TimeSpan.Zero)
            {
                // Stop the timer
                timer.Stop();

                // Do something when the timer reaches 0
                // Put the PC to sleep
                SetSuspendState(false, false, false);

                // Enable Set Timer button and disable Stop button
                Dispatcher.Invoke(() =>
                {
                    startButton.IsEnabled = true;
                    stopButton.IsEnabled = false;
                });
            }
        }

        private void UpdateTimerLabel()
        {
            // Update timer label text to display remaining time
            timerLabel.Content = $"{remainingTime.Hours:D2}:{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}";
        }
    }
}
