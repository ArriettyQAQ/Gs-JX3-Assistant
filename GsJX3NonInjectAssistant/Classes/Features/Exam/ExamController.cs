﻿using GsJX3NonInjectAssistant.Classes.HID.Display;
using GsJX3NonInjectAssistant.Classes.HID.Mouse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace GsJX3NonInjectAssistant.Classes.Features.Exam
{

    class ExamController
    {

        public ScreenCaptureConfiguration ScreenCaptureConfiguration = new ScreenCaptureConfiguration();

        private IDisplayHelper displayHelper;
        private IQAProvider qAProvider;

        public ExamController(IDisplayHelper displayHelper, IQAProvider qAProvider)
        {
            this.displayHelper = displayHelper;
            this.qAProvider = qAProvider;
        }

        public async Task<List<QuestionAndAnswer>> Search(string question)
        {
            return await qAProvider.SearchAsync(question);
        }

        public string RunOCR(Bitmap bitmap_screenshot)
        {
            try
            {
                string question = "";

                byte[] screenshot = Common.BitmapToByteArray(bitmap_screenshot);

                // initialize TesseractEngine
                using TesseractEngine engine = new TesseractEngine(@"./tessdata", "chi_sim", EngineMode.Default);

                // set image source
                using Pix img = Pix.LoadFromMemory(screenshot);
                using Page page = engine.Process(img);


                string text = page.GetText();
                using (var iter = page.GetIterator())
                {
                    iter.Begin();
                    string line = "";
                    do
                    {
                        do
                        {
                            do
                            {
                                line = "";
                                do
                                {
                                    string keyword = iter.GetText(PageIteratorLevel.Word);
                                    line += keyword;

                                } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                line = line.Trim();
                                if (line.Length > 0)
                                {
                                    question += line;
                                }

                            } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                        } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                    } while (iter.Next(PageIteratorLevel.Block));
                }

                return question;

            }
            catch (Exception e)
            {
                if (e.ToString().Contains("DllNotFoundException"))
                {
                    System.Diagnostics.Process.Start("https://aka.ms/vs/16/release/vc_redist.x64.exe");
                    throw new Exception("这台电脑没有安装文字识别所需的运行环境。请安装自动下载的运行环境后重试");
                }
                else
                {
                    Trace.TraceError(e.ToString());
                    Console.WriteLine("Unexpected Error: " + e.Message);
                    Console.WriteLine("Details: ");
                    Console.WriteLine(e.ToString());
                    throw new Exception(e.ToString());
                }
            }

        }


    }
}
