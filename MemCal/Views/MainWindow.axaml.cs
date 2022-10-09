namespace MemCal.Views;

using Avalonia.Controls;
using Avalonia.Input;
using MemCal.ViewModels;
using MemCal.Models.DataTypes.Enums;

/// <summary>
/// Code-behind from the MainWindow the app launches to.
/// </summary>
public partial class MainWindow : Window
{
    // CONSTRUCTORS
    // ============

    /// <summary>
    /// Creates a MainWindow.
    /// </summary>
    public MainWindow()
    {
        this.InitializeComponent();
    }



    // METHODS
    // ============

    /// <summary>
    /// Handles keyboard key presses.
    /// </summary>
    /// <param name="sender">The element that sent this command.</param>
    /// <param name="e">The key event parameters.</param>
    private void KeyDownHandler(object sender, KeyEventArgs e)
    {
        var vm = (MainWindowViewModel?)this.DataContext;
        if (vm != null)
        {
            switch (e.Key)
            {
                case Key.D1:
                case Key.NumPad1:
                    vm.InputNumber(1);
                    break;
                case Key.D2:
                case Key.NumPad2:
                    vm.InputNumber(2);
                    break;
                case Key.D3:
                case Key.NumPad3:
                    vm.InputNumber(3);
                    break;
                case Key.D4:
                case Key.NumPad4:
                    vm.InputNumber(4);
                    break;
                case Key.NumPad5:
                    vm.InputNumber(5);
                    break;
                case Key.D6:
                case Key.NumPad6:
                    vm.InputNumber(6);
                    break;
                case Key.D7:
                case Key.NumPad7:
                    vm.InputNumber(7);
                    break;
                case Key.D8:
                case Key.NumPad8:
                    vm.InputNumber(8);
                    break;
                case Key.D9:
                case Key.NumPad9:
                    vm.InputNumber(9);
                    break;
                case Key.D0:
                case Key.NumPad0:
                    vm.InputNumber(0);
                    break;
                case Key.D5:
                    if (e.KeyModifiers == KeyModifiers.Shift)
                    {
                        vm.InputPercent();
                    }
                    else
                    {
                        vm.InputNumber(5);
                    }

                    break;
                case Key.Add:
                case Key.OemPlus:
                    vm.InputOperator(Operation.Addition);
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    vm.InputOperator(Operation.Subtraction);
                    break;
                case Key.Multiply:
                    vm.InputOperator(Operation.Multiplication);
                    break;
                case Key.Divide:
                    vm.InputOperator(Operation.Division);
                    break;
                case Key.Decimal:
                    vm.InputDecimal();
                    break;
                case Key.Return:
                    vm.Calculate();
                    break;
            }
        }
    }

    /// <summary>
    /// After reset view focus.
    /// </summary>
    private void ResetViewFocus()
    {
        this.ResultOutput.Focus();
    }
}