﻿using SimpleTimer.Clocks;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleTimer.ClockUserControls
{
    /// <summary>
    /// Interaction logic for ClockUserCtrl.xaml
    /// </summary>
    public partial class ClockUserCtrl : UserControl, IClockUserCtrl, IUserInterface
    {
        string _windowTitle;
        public string WindowTitle { get => _windowTitle; set { _windowTitle = value; OnPropertyChanged(nameof(WindowTitle)); } }
        public static string AppVersion
        {
            get
            {
                var version = typeof(MainWindow).Assembly.GetName().Version;
                return $"v{version.Major}.{version.Minor}";
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event EventHandler<UIEventArgs> UiEventHappened;
        IClockViewModel _vm;
        public ClockUserCtrl(string uidAppend = "")
        {
            InitializeComponent();
            TxtTime.Uid += uidAppend;
            BtnStart.Uid += uidAppend;
            BtnReset.Uid += uidAppend;
            this.Uid += uidAppend;
        }

        public void SetViewModel(IClockViewModel vm)
        {
            _vm = vm;
            DataContext = _vm;
        }

        private void OnUiEventHappened(UIEventArgs e)
        {
            var handler = UiEventHappened;
            handler?.Invoke(this, e);
        }

        #region IUserInterface
        public void ChangeWindowTitle(string title)
        {
            WindowTitle = title;
        }
        public void BtnStartFocus()
        {
            BtnStart.Focus();
        }
        public void TextFocus()
        {
            TxtTime.Focus();
        }
        public bool IsTextFocused()
        {
            return TxtTime.IsFocused;
        }
        public void ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon)
        {
            MessageBox.Show(messageBoxText, caption, button, icon);
        }
        #endregion

        #region WindowEvents
        public void WindowNumberKeyDown(KeyEventArgs e)
        {
            OnUiEventHappened(new UIEventArgs(UIEventArgs.UIEventType.WindowNumberKeyDown));
        }
        public void WindowShiftEnterKeyDown(ExecutedRoutedEventArgs e)
        {
            OnUiEventHappened(new UIEventArgs(UIEventArgs.UIEventType.WindowShiftEnterKeyDown));
        }

        public void WindowBackspaceKeyDown(KeyboardEventArgs e)
        {
            OnUiEventHappened(new UIEventArgs(UIEventArgs.UIEventType.WindowBackspaceKeyDown));
        }

        public void SwitchedToAnotherTab()
        {
            OnUiEventHappened(new UIEventArgs(UIEventArgs.UIEventType.TabLostFocus));
        }
        #endregion

        #region UI events
        private void TxtTime_GotFocus(object sender, RoutedEventArgs e)
        {
            OnUiEventHappened(new UIEventArgs(UIEventArgs.UIEventType.TextGotFocus));
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            OnUiEventHappened(new UIEventArgs(UIEventArgs.UIEventType.BtnStartClicked));
        }
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            OnUiEventHappened(new UIEventArgs(UIEventArgs.UIEventType.BtnResetClicked));
        }
        
        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _vm?.Dispose();
                }
                disposedValue = true;
            }
        }

        ~ClockUserCtrl()
        {
            Dispose(false);
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        
    }
}
