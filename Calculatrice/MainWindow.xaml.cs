using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Calculatrice
{
    public partial class MainWindow
    {
        private string _firstNumber;
        private string _secondNumber;
        private string _op;
        private double _result;
        private bool _isSecond;
        private bool _opPressed;

        public MainWindow()
        {
            InitializeComponent();
            _isSecond = false;
            _opPressed = false;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            IconHelper.RemoveIcon(this);
        }

        private void TxtName_OnLoaded(object sender, RoutedEventArgs e)
        {
            Calculation.Text = null;
            Result.Text = "0";
        }

        private void Number(object sender, EventArgs e)
        {
            Result.FontSize = 27;
            ButtonStatus(true);
            switch (_isSecond)
            {
                case true:
                    _secondNumber += ((Button)sender).Content.ToString();
                    Result.Text = _secondNumber;
                    break;
                default:
                    _firstNumber += ((Button)sender).Content.ToString();
                    _result = Convert.ToDouble(_firstNumber,CultureInfo.GetCultureInfo("fr-FR"));
                    Result.Text = _firstNumber;
                    break;
            }
        }

        private void Float(object sender, EventArgs e)
        {
            switch (_isSecond)
            {
                case true:
                    _secondNumber += ((Button)sender).Content.ToString();
                    Result.Text = _secondNumber;
                    break;
                default:
                    _firstNumber += ((Button)sender).Content.ToString();
                    _result = Convert.ToDouble(_firstNumber,CultureInfo.GetCultureInfo("fr-FR"));
                    Result.Text = _firstNumber;
                    break;
            }
        }

        private void Operator(object sender, EventArgs e)
        {
            _secondNumber = "";
            _isSecond = true;
            _opPressed = true;
            _op = ((Button)sender).Content.ToString();
            _firstNumber = Convert.ToString(_result,CultureInfo.GetCultureInfo("fr-FR"));
            Calculation.Text = _result + _op;
        }

        private void DoCalculation()
        {
            switch (_isSecond)
            {
                case true when _opPressed && string.IsNullOrEmpty(_secondNumber): //no SecondNumber + "+" => Equal
                    switch (_op)
                    {
                        case "+":
                            Calculation.Text = _result + _op + _result + "=";
                            _result += _result;
                            break;
                        case "-":
                            Calculation.Text = _result + _op + _result + "=";
                            _result -= _result;
                            break;
                        case "x":
                            Calculation.Text = _result + _op + _result + "=";
                            _result *= _result;
                            break;
                        case "/":
                            switch (_secondNumber)
                            {
                                case "0":
                                    Result.Text = "Impossible de diviser par 0";
                                    break;
                                default:
                                    Calculation.Text = _result + _op + _result + "=";
                                    _result /= _result;
                                    break;
                            }
                            break;
                    }
                    break;
                case true when string.IsNullOrEmpty(_secondNumber):  //no SecondNumber => Multiple Equal
                    switch (_op)
                    {
                        case "+":
                            Calculation.Text = _result + _op + _firstNumber + "=";
                            _result += Convert.ToDouble(_firstNumber,CultureInfo.GetCultureInfo("fr-FR"));
                            break;
                        case "-":
                            Calculation.Text = _result + _op + _firstNumber + "=";
                            _result -= Convert.ToDouble(_firstNumber,CultureInfo.GetCultureInfo("fr-FR"));
                            break;
                        case "x":
                            Calculation.Text = _result + _op + _firstNumber + "=";
                            _result *= Convert.ToDouble(_firstNumber,CultureInfo.GetCultureInfo("fr-FR"));
                            break;
                        case "/":
                            switch (_secondNumber)
                            {
                                case "0":
                                    Result.Text = "Impossible de diviser par 0";
                                    break;
                                default:
                                    Calculation.Text = _result + _op + _firstNumber + "=";
                                    _result /= Convert.ToDouble(_firstNumber,CultureInfo.GetCultureInfo("fr-FR"));
                                    break;
                            }
                            break;
                    }
                    break;
                default: // FirstNumber + SecondNumber => Equal
                    switch (_op)
                    {
                        case "+":
                            Calculation.Text = _result + _op + _secondNumber + "=";
                            _result += Convert.ToDouble(_secondNumber,CultureInfo.GetCultureInfo("fr-FR"));
                            break;
                        case "-":
                            Calculation.Text = _result + _op + _secondNumber + "=";
                            _result -= Convert.ToDouble(_secondNumber,CultureInfo.GetCultureInfo("fr-FR"));
                            break;
                        case "x":
                            Calculation.Text = _result + _op + _secondNumber + "=";
                            _result *= Convert.ToDouble(_secondNumber,CultureInfo.GetCultureInfo("fr-FR"));
                            break;
                        case "/":
                            switch (_secondNumber)
                            {
                                case "0":
                                    Result.FontSize = 20;
                                    Result.Text = "Impossible de diviser par 0";
                                    break;
                                default:
                                    Calculation.Text = _result + _op + _secondNumber + "=";
                                    _result /= Convert.ToDouble(_secondNumber,CultureInfo.GetCultureInfo("fr-FR"));
                                    break;
                            }
                            break;
                    }
                    break;
            }
        }

        private void Equal(object sender, EventArgs e)
        {
            switch (_secondNumber)
            {
                case "0":
                    ButtonStatus(false);
                    Clear();
                    Result.Text = "Impossible de diviser par 0";
                    Result.FontSize = 20;
                    _opPressed = false;
                    break;
                default:
                    DoCalculation();
                    Result.Text = _result.ToString(CultureInfo.GetCultureInfo("fr-FR"));
                    _opPressed = false;
                    break;
            }
        }

        private void Clear(object sender, EventArgs e)
        {
            _firstNumber = "";
            _op = "";
            _secondNumber = "";
            _result = 0;
            _isSecond = false;
            Calculation.Text = null;
            Result.Text = _result.ToString(CultureInfo.GetCultureInfo("fr-FR"));
        }

        private void Clear()
        {
            _firstNumber = "";
            _op = "";
            _secondNumber = "";
            _result = 0;
            _isSecond = false;
            Calculation.Text = null;
            Result.Text = _result.ToString(CultureInfo.GetCultureInfo("fr-FR"));
        }

        private void ButtonStatus(bool status)
        {
            BtnAddition.IsEnabled = status;
            BtnSubstraction.IsEnabled = status;
            BtnMultiplication.IsEnabled = status;
            BtnDivision.IsEnabled = status;
        }

        private void DebugValues()
        {
            Debug.WriteLine("First: " + _firstNumber);
            Debug.WriteLine("Op: " + _op);
            Debug.WriteLine("Second: " + _secondNumber);
            Debug.WriteLine("Result: " + _result);
            Debug.WriteLine("isSecond: " + _isSecond);
            Debug.WriteLine("opPressed: " + _opPressed+"\n");
        }
    }

    public static class IconHelper
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int width,
            int height, uint flags);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_DLGMODALFRAME = 0x0001;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_FRAMECHANGED = 0x0020;
        private const uint WM_SETICON = 0x0080;

        public static void RemoveIcon(Window window)
        {
            // Get this window's handle
            IntPtr hwnd = new WindowInteropHelper(window).Handle;

            // Change the extended window style to not show a window icon
            int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);

            // Update the window's non-client area to reflect the changes
            SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
        }
    }
}