﻿using System;
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
using MColor = System.Windows.Media.Color;
using DColor = System.Drawing.Color;

using GsJX3NonInjectAssistant;
using GsJX3NonInjectAssistant.Classes.HID.Display;
using GsJX3NonInjectAssistant.Classes.HID.Mouse;
using GsJX3NonInjectAssistant.Classes.Features.Exam;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Data;

namespace GsJX3NonInjectAssistant.Views.Exam
{
    /// <summary>
    /// Interaction logic for Exam.xaml
    /// </summary>
    public partial class Exam : Page
    {

        private ExamController examController;
        private IDisplayHelper displayHelper;
        private IQAProvider qAProvider;
        private IMouseReader mouseReader;
        private System.Timers.Timer timer_captureScreen = new System.Timers.Timer(1000);
        Bitmap CapturedScreen;

        public Exam()
        {
            InitializeComponent();
            displayHelper = new DisplayHelper_GDI();
            qAProvider = new QAProvider_LocalJSON();
            examController = new ExamController(displayHelper, qAProvider);
            timer_captureScreen.Elapsed += Timer_captureScreen_Elapsed;
            timer_captureScreen.AutoReset = true;
            //timer_captureScreen.Start();
        }

        private void Timer_captureScreen_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (examController.ScreenCaptureConfiguration.Size != Common.NullSize)
            {
                Task.Run(TakeScreenShot);
            }
        }

        public void TakeScreenShot()
        {
            Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                CapturedScreen = displayHelper.CaptureScreen(examController.ScreenCaptureConfiguration);
                image_preview.Source = Common.BitmapToImageSource(CapturedScreen);
                TriggerOCR();
                Search();
            }));

        }



        private void button_selectArea_Click(object sender, RoutedEventArgs e)
        {
            button_selectArea.IsEnabled = false;

            label_selectedArea_TL.Content = "设置文字识别框 左上角 坐标";

            IMouseReader mouseReader_TL = new MouseReader_MouseKeyHook();
            examController.ScreenCaptureConfiguration.TopLeft = Common.NullPoint;
            mouseReader_TL.GetCursorPosition((System.Drawing.Point TL, int mouseButton) =>
            {
                examController.ScreenCaptureConfiguration.TopLeft = TL;
                label_selectedArea_TL.Content = $"左上{examController.ScreenCaptureConfiguration.TopLeft.ToString()}";

                label_selectedArea_BR.Content = "设置文字识别框 右下角 坐标";

                IMouseReader mouseReader_BR = new MouseReader_MouseKeyHook();
                examController.ScreenCaptureConfiguration.BottomRight = Common.NullPoint;
                mouseReader_BR.GetCursorPosition((System.Drawing.Point BR, int mouseButton) =>
                {
                    examController.ScreenCaptureConfiguration.BottomRight = BR;

                    Console.WriteLine(examController.ScreenCaptureConfiguration.Size);

                    label_selectedArea_TL.Foreground = new SolidColorBrush(MColor.FromRgb(0,0,0));
                    label_selectedArea_BR.Content = $"右下{examController.ScreenCaptureConfiguration.BottomRight.ToString()}";

                    button_selectArea.IsEnabled = true;
                    button_ocr.IsEnabled = true;

                });

            });

        }

        private async void Search()
        {
            if (textBox_KW.Text == "") return;
            List<string> keywords = textBox_KW.Text.Split(' ').ToList();
            try
            {
                List<QuestionAndAnswer> matchedQAs = await examController.Search(keywords);
                listBox.ItemsSource = matchedQAs;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void TriggerOCR()
        {
            if (examController.ScreenCaptureConfiguration.Size == Common.NullSize) return;
            if (CapturedScreen == null) return;

                List<string> lines = examController.RunOCR(CapturedScreen);

                List<string> keywords = new List<string>();
                Random random = new Random();
                
                foreach (var line in lines)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        int rInt = random.Next(0, line.Length - 2);
                        keywords.Add(line.Substring(rInt, 2));
                    }

                }

                textBox_KW.Text = String.Join(" ", keywords.ToArray());

        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }



        private void button_ocr_Click(object sender, RoutedEventArgs e)
        {
            timer_captureScreen.Start();
        }

        private void button_search_Click(object sender, RoutedEventArgs e)
        {
            timer_captureScreen.Stop();
        }
    }
}