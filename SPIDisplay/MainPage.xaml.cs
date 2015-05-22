/*
    Copyright(c) Microsoft Open Technologies, Inc. All rights reserved.

    The MIT License(MIT)

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files(the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions :

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE.
*/
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Enumeration;
using Windows.Devices.Spi;
using Windows.Devices.Gpio;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml;
using ili9340;

namespace SPIDisplay
{

    public sealed partial class MainPage : Page
    {
        private CWaveShare32b waveShare = new CWaveShare32b();

        private IAsyncAction workItemThread;
        private DispatcherTimer timerProducer;
        private DispatcherTimer timerConsumer;
        private Random rnd = new Random();
        private uint xi = 20;
        private uint yi = 20;
        private uint ci = 0;
        private bool inv = false;
        private int rot = 0;

        public MainPage()
        {
            this.InitializeComponent();

            /* Register for the unloaded event so we can clean up upon exit */
            Unloaded += MainPage_Unloaded;

            /* Initialize GPIO, SPI, and the display */
            InitAll();

        }

        /* Initialize GPIO, SPI, and the display */
        private async void InitAll()
        {
            try
            {
                await waveShare.InitAll();                      
            }
            /* If initialization fails, display the exception and stop running */
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return;
            }


            //waveShare.ClearScreen((int)rnd.Next(0, 65535));



            //waveShare.DisplayHelloText();

            //waveShare.DisplayUpdate();
            ci = (uint)rnd.Next(0, 65535);
            //waveShare.FillScreen(ci);
            //waveShare.FillScreenTEMP(ci);
            waveShare.ClearScreen(ci);

            // Timer to change screen 

            //waveShare.DisplayHelloText();
            /*waveShare.DrawFillRectTEMP((uint)rnd.Next(0, 300), (uint)rnd.Next(0, 300), (uint)rnd.Next(10, 50), (uint)rnd.Next(10, 50), ci);
            waveShare.DrawPixelTEMP((uint)rnd.Next(0, 100), (uint)rnd.Next(0, 100), ci);
            waveShare.DrawFastHLineTEMP(10, 10, 100, ci);
            waveShare.DrawFastVLineTEMP(10, 10, 100, ci);
            waveShare.DisplayUpdate();*/



            // Something to check the queue and consume it
            timerConsumer = new DispatcherTimer();
            timerConsumer.Interval = TimeSpan.FromMilliseconds(1);
            timerConsumer.Tick += Timer_TickConsumer;
            timerConsumer.Start();

            // Something to produce the graphic
            timerProducer = new DispatcherTimer();
            timerProducer.Interval = TimeSpan.FromMilliseconds(100);
            timerProducer.Tick += Timer_TickProducer;
            timerProducer.Start();





            // waveShare.ClearScreen((int)rnd.Next(0, 65535));
            //waveShare.DrawRect(100, 100, 100, 50, (int)rnd.Next(0, 65535));

            /*for (yi = 20; yi < CWaveShare32b.SCREEN_HEIGHT_PX; yi++)
            {
                for (xi = 20; xi < CWaveShare32b.SCREEN_WIDTH_PX; xi++)
                {

                    await Task.Delay(120);
                    waveShare.DrawPixel((short)xi, (short)yi, (int)ci);

                }
            }*/
            //waveShare.DrawRect(100, 100, 100, 50, (int)rnd.Next(0, 65535));
        }


        private void Timer_TickConsumer(object sender, object e)
        {
            waveShare.DisplayUpdate();
        }

        private void Timer_TickProducer(object sender, object e)
        {
            //waveShare.ClearScreen(ci);
            //waveShare.ClearScreen((int)rnd.Next(0, 65535));
            int screenWidth = (int)waveShare.screenwidth;
            int screenHeight = (int)waveShare.screenheight;
            //waveShare.DisplayHelloText();
            //waveShare.DrawFillRectTEMP((uint)rnd.Next(0, screenWidth), (uint)rnd.Next(0, screenHeight), (uint)rnd.Next(10, 50), (uint)rnd.Next(10, 50), ci);
            //waveShare.DrawPixelTEMP((uint)rnd.Next(0, screenWidth), (uint)rnd.Next(0, screenHeight), ci);

            waveShare.DrawFillRectTEMP(5, 5, (uint)(screenWidth-10), (uint)rnd.Next(15, (screenHeight/3)-10), ci);

            //waveShare.DrawFastHLineTEMP(50, 50, 100, ci);
            //waveShare.DrawFastVLineTEMP(50, 50, 100, ci);
            //waveShare.DisplayUpdate();
            /*
            waveShare.DrawPixel((uint)rnd.Next(170,200), (uint)rnd.Next(170, 200), ci);

            waveShare.DrawFastHLine(10, 10, 100, ci);
            waveShare.DrawFastHLine(30, 30, 100, ci);
            waveShare.DrawFastHLine(50, 50, 100, ci);
            waveShare.DrawFastVLine(10, 10, 100, ci);
            waveShare.DrawFastVLine(30, 30, 100, ci);
            waveShare.DrawFastVLine(50, 50, 100, ci);
            //waveShare.DrawPixel(xi, yi, ci);

            waveShare.DrawFillRect(70, 70, 50, 50, ci);
            */
            //waveShare.DrawText(30, 30, "X", ci);

            xi++;
            if (xi>CWaveShare32b.SCREEN_WIDTH_PX)
            {
                xi = 0;
                yi++;
                if (yi> CWaveShare32b.SCREEN_HEIGHT_PX)
                {
                    yi = 0;
                    
                }
            }
            ci = (uint)rnd.Next(0, 65535);

            if (xi % 10==0)
            {
                inv = !inv;
                rot++;
                if (rot>3) { rot = 0; }
                waveShare.InvertDisplay(inv);
                waveShare.RotateScreen(rot);
            }

            
        }

        


        private void MainPage_Unloaded(object sender, object args)
        {
            /* Cleanup */
            waveShare.Dispose(sender, args);
            Debug.WriteLine("unloaded");
        }
    }
}
