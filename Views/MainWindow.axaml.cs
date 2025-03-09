using ApproximationByBezier.ViewModels;
using Avalonia.Controls;
using Avalonia.Input;
using System;
using ReactiveUI;

namespace ApproximationByBezier.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WhenAnyValue(t => t.DataContext).Subscribe(d => vm = d as MainWindowViewModel);
        }

        private MainWindowViewModel? vm; 

        private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            vm?.CalculateApproximationCommand.Execute();
        }
    }
}