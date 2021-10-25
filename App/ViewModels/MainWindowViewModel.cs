using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using System.Windows.Input;
using Avalonia.Diagnostics;
using Avalonia.Logging;
using ReactiveUI;

namespace AntUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string testButtonText;

        public string TestButtonText
        {
            get => testButtonText;
            set => this.RaiseAndSetIfChanged(ref testButtonText, value);
        }
        
        public MainWindowViewModel()
        {
            testButtonText = "Test Button";
        }

        public void OnTestClick(string parameter)
        {
            TestButtonText = "HAHAHA I'M A TEST BUTTON :D";
        }
    }
}
