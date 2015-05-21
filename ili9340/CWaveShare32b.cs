using ili9340;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace ili9340
{
    public class CWaveShare32b : Cili9340
    {
        private const Int32 PB1_PIN = 18;
        private const Int32 PB2_PIN = 23;
        private const Int32 PB3_PIN = 24;

        private GpioPin pushButton1;        // Buttons on display
        private GpioPin pushButton2;
        private GpioPin pushButton3;

        public GpioController IoController { get; private set; }

        public CWaveShare32b():base() { }

        public void InitAll()
        {
            try
            {
                base.InitAll();

                 InitGpioButtons();
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private  void InitGpioButtons()
        {
            try
            {
                Debug.WriteLine("start of InitGpioButtons");
                

                IoController = GpioController.GetDefault(); /* Get the default GPIO controller on the system */

                 // Init the button
                pushButton1 = IoController.OpenPin(PB1_PIN);
                pushButton1.SetDriveMode(GpioPinDriveMode.Input);
                pushButton1.ValueChanged += PushButton1_ValueChanged;

                pushButton2 = IoController.OpenPin(PB2_PIN);
                pushButton2.SetDriveMode(GpioPinDriveMode.Input);
                pushButton2.ValueChanged += PushButton2_ValueChanged;

                pushButton3 = IoController.OpenPin(PB3_PIN);
                pushButton3.SetDriveMode(GpioPinDriveMode.Input);
                pushButton3.ValueChanged += PushButton3_ValueChanged;

                Debug.WriteLine("End of InitGpioButtons");
            }
            /* If initialization fails, throw an exception */
            catch (Exception ex)
            {
                throw new Exception("InitGpioButtons initialization failed "+ex.Message, ex);
            }
        }




        private void PushButton3_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            Debug.WriteLine("Push Button 3 changed!"); ;
        }

        private void PushButton2_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            Debug.WriteLine("Push Button 2 changed!");
        }

        private void PushButton1_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            Debug.WriteLine("Push Button 1 changed!");
        }


        public void Dispose(object sender, object args)
        {
            base.Dispose(sender, args);

            pushButton1.Dispose();
            pushButton2.Dispose();
            pushButton3.Dispose();

            Debug.WriteLine("wave unloaded");
        }
    }
}
