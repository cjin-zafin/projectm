﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace metro
{

    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class HLETwo : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _text;
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Text"));
            }
        }


        public HLETwo()
        {
            InitializeComponent();
        }
    }
}
