using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Alternet.UI.Integration.VisualStudio.Services;
using Microsoft.VisualStudio.Shell;
using Serilog;

namespace Alternet.UI.Integration.VisualStudio.Views
{
    public partial class AlternetUIPreviewer : UserControl, IDisposable
    {
        private bool disposedValue;

        public AlternetUIPreviewer()
        {
            InitializeComponent();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
