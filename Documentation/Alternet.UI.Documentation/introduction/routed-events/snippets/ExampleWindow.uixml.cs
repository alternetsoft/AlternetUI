using Alternet.UI;
using System;

namespace Alternet.UI.Documentation.Examples
{
    public partial class MainWindow : Window
    {
        SampleControl control;

        public MainWindow()
        {
            InitializeComponent();

            control = new SampleControl();
            control.Parent = this;
            #region AddHandlerPlusEquals
            control.Tap += Control_Tap;
            #endregion

        }

        private void SampleAddEvent()
        {
            #region AddHandlerCode
            control.AddHandler(SampleControl.TapEvent, new RoutedEventHandler(Control_Tap));
            #endregion
        }

        #region CSharpCreation2
        private void Control_Tap(object sender, RoutedEventArgs e)
        {
            Application.Log("Tap handled");
            e.Handled = true;
        }
        #endregion

        public void Example1()
        {
        }
    }

    #region CSharpCreation
    public class SampleControl : Control
    {
        public static readonly RoutedEvent TapEvent = EventManager.RegisterRoutedEvent(
            "Tap", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SampleControl));

        // Provide CLR accessors for the event
        public event RoutedEventHandler Tap
        {
            add { AddHandler(TapEvent, value); }
            remove { RemoveHandler(TapEvent, value); }
        }
    }
    #endregion
}