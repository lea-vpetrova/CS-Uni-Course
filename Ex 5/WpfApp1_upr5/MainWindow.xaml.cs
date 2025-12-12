using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1_upr5
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnStartAsync_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtCountTo.Text, out int countTo) || countTo <= 0)
            {
                MessageBox.Show("Please enter a valid positive number to count to.");
                return;
            }

            if (!int.TryParse(txtDelay.Text, out int awaitMillisec) || awaitMillisec < 0)
            {
                MessageBox.Show("Please enter a valid delay (milliseconds).");
                return;
            }

            btnStartAsync.IsEnabled = false;

            lblBefore.Content = "Start: " + DateTime.Now.ToLongTimeString();
            lblAfter.Content = "";
            lblNumber.Content = "";

            await SlowWorkAsync(countTo, awaitMillisec);

            lblAfter.Content = "End: " + DateTime.Now.ToLongTimeString();
            btnStartAsync.IsEnabled = true;
        }

        private void btnStartSync_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtCountTo.Text, out int countTo) || countTo <= 0)
            {
                MessageBox.Show("Please enter a valid positive number to count to.");
                return;
            }

            if (!int.TryParse(txtDelay.Text, out int sleepMillisec) || sleepMillisec < 0)
            {
                MessageBox.Show("Please enter a valid delay (milliseconds).");
                return;
            }

            lblSyncMsg.Content = "Sync button clicked – UI will freeze now...";
            lblSyncMsg.Visibility = Visibility.Visible;

            btnStartSync.IsEnabled = false;
            btnStartSync.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render);

            lblBefore.Content = "Start: " + DateTime.Now.ToLongTimeString();
            lblAfter.Content = "";
            lblNumber.Content = "";

            SlowWorkSync(countTo, sleepMillisec);

            lblAfter.Content = "End: " + DateTime.Now.ToLongTimeString();
            lblSyncMsg.Visibility = Visibility.Collapsed;
            btnStartSync.IsEnabled = true;
        }

        private async Task SlowWorkAsync(int countTo, int awaitMillisec)
        {
            for (int i = 1; i <= countTo; i++)
            {
                lblNumber.Content = i.ToString();
                await Task.Delay(awaitMillisec);
            }
        }

        private void SlowWorkSync(int countTo, int sleepMillisec)
        {
            for (int i = 1; i <= countTo; i++)
            {
                lblNumber.Content = i.ToString();
                Thread.Sleep(sleepMillisec); 
            }
        }
    }
}
