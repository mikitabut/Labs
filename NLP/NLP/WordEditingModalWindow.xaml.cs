﻿using System.Windows;

namespace NLP
{
    /// <summary>
    /// Interaction logic for WordEditingModalWindow.xaml
    /// </summary>
    public partial class WordEditingModalWindow : Window
    {
        public WordEditingModalWindow() => InitializeComponent();

        private void OK_Click(object sender, RoutedEventArgs e) => DialogResult = true;
    }
}
