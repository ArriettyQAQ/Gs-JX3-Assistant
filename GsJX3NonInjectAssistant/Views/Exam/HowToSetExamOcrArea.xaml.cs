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
using System.Windows.Shapes;
using System.Windows.Forms;


namespace GsJX3NonInjectAssistant.Views.Exam
{
    /// <summary>
    /// Interaction logic for HowToSetExamOcrArea.xaml
    /// </summary>
    public partial class HowToSetExamOcrArea : Window
    {
        public HowToSetExamOcrArea()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Close();
        }

    }
}
