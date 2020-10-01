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

using SharpDX.XInput;
using System.Threading;

using System.Timers;

namespace XInputExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// 2020/10/01
    /// 
    /// Adapted from SharpDX sample 
    /// https://github.com/sharpdx/SharpDX-Samples/tree/master/Desktop/XInput/XGamepadApp
    /// 
    /// SharpDX.XInput is installed through nuget package manager
    /// 
    /// </summary>

    public partial class MainWindow : Window
    {
        public TextBlock txb;
        System.Timers.Timer aTimer;
        Controller controller = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 10;
            bool controllerExists = LookForController();

            if( controllerExists )
            {
                aTimer.Enabled = true;
            }
            else
            {
                Tx_Box.Text = "no xbox controller connected";
            }
        }


        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                Dispatcher.Invoke(() => {
                    // Code causing the exception or requires UI thread access
                    // https://stackoverflow.com/questions/21306810/the-calling-thread-cannot-access-this-object-because-a-different-thread-owns-it
                    UpdateInfo();
                });
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.Message);
            }
        }


        private void UpdateInfo()
        {
            ControlUpdate();
        }


        private bool LookForController()
        {
            // MessageBox.Show("Start XGamepadApp");

            // Initialize XInput
            var controllers = new[] { new Controller(UserIndex.One), new Controller(UserIndex.Two), new Controller(UserIndex.Three), new Controller(UserIndex.Four) };


            // Get 1st controller available
            foreach (var selectControler in controllers)
            {
                if (selectControler.IsConnected)
                {
                    controller = selectControler;
                    break;
                }
            }

            if (controller == null)
            {
                MessageBox.Show("No XInput controller installed");
                return false;
            }

            // MessageBox.Show("Found a XInput controller available " + controller.ToString() );
            return true;
        }


        private void ControlUpdate()
        {
            var state = controller.GetState();

            Vibration vibration = new Vibration();

            vibration.RightMotorSpeed = (ushort)(255 * state.Gamepad.LeftTrigger);
            vibration.LeftMotorSpeed = (ushort)(255 * state.Gamepad.RightTrigger);
            controller.SetVibration(vibration);

            Tx_Box.Text =state.Gamepad.ToString()  ;

            //  "\r\n" + 

            /*
            if (previousState.PacketNumber != state.PacketNumber)
            {
                previousState = state;
            }
            */
        }

    }
}
