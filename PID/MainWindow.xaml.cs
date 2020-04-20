using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using PID.DifferentialEquations;
using PID.EquationsSolver;
using PID.HelperClasses.Checker;
using PID.HelperClasses.Timer;
using PID.Objects;

namespace PID
{

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public delegate void SimulationDelegateType(double inputSignalValue, double loadTorqueValue, double time, double dt, int numberOfIteration, double sineShift);
        SimulationDelegateType DelegateSimulationMode;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Canvas1CoefficientAxisX = new ObservableCollection<double>();
            Canvas1CoefficientAxisY = new ObservableCollection<double>();
            Canvas2CoefficientAxisX = new ObservableCollection<double>();
            Canvas2CoefficientAxisY = new ObservableCollection<double>();

            SecondaryAxisCoefficient1 = new ObservableCollection<double>();
            SecondaryAxisCoefficient2 = new ObservableCollection<double>();
            SecondaryAxisCoefficient3 = new ObservableCollection<double>();
            SecondaryAxisCoefficient4 = new ObservableCollection<double>();

            AxisMaxValueCoefficient = new ObservableCollection<double>();
            Axis34ValueCoefficient = new ObservableCollection<double>();
            Axis12ValueCoefficient = new ObservableCollection<double>();
            Axis14ValueCoefficient = new ObservableCollection<double>();

            ButtonStart.IsEnabled = false;
            ButtonStop.IsEnabled = false;
            ButtonReset.IsEnabled = false;
            ButtonSaveSettings.IsEnabled = false;

            currentDifferentialEquation = new EquationI(electricMotor);
            speedDifferentialEquation = new EquationOmega(electricMotor);

            DataList = new List<string>();

            RadioButtonOpenLoop.IsChecked = true;
            RadioButtonManualControl.IsChecked = true;

        }

        #region Definition of NotfyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        void RaisedPropertyChanged(string controllerName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(controllerName));
        }
        #endregion

        #region Property definitions
        private double _InputSignalValue;
        public double InputSignalValue
        {
            get
            {
                return _InputSignalValue;
            }
            set
            {
                _InputSignalValue = value;
                RaisedPropertyChanged("InputSignalValue");

            }
        }

        private double _ControllerSignalValue;
        public double ControllerSignalValue
        {
            get
            {
                return _ControllerSignalValue;
            }
            set
            {
                _ControllerSignalValue = value;
                RaisedPropertyChanged("ControllerSignalValue");

            }
        }

        private double _OutputSignalValue;
        public double OutputSignalValue
        {
            get
            {
                return _OutputSignalValue;
            }
            set
            {
                _OutputSignalValue = value;
                RaisedPropertyChanged("OutputSignalValue");

            }
        }
        private double _Error;
        public double Error
        {
            get
            {
                return _Error;
            }
            set
            {
                _Error = value;
                RaisedPropertyChanged("Error");

            }
        }

        private double _AxisTimeValue0;
        public double AxisTimeValue0
        {
            get
            {
                return _AxisTimeValue0;
            }
            set
            {
                _AxisTimeValue0 = value;
                RaisedPropertyChanged("AxisTimeValue0");

            }
        }

        private double _AxisTimeValue1;
        public double AxisTimeValue1
        {
            get
            {
                return _AxisTimeValue1;
            }
            set
            {
                _AxisTimeValue1 = value;
                RaisedPropertyChanged("AxisTimeValue1");

            }
        }

        private double _AxisTimeValue2;
        public double AxisTimeValue2
        {
            get
            {
                return _AxisTimeValue2;
            }
            set
            {
                _AxisTimeValue2 = value;
                RaisedPropertyChanged("AxisTimeValue2");

            }
        }

        private double _AxisTimeValue3;
        public double AxisTimeValue3
        {
            get
            {
                return _AxisTimeValue3;
            }
            set
            {
                _AxisTimeValue3 = value;
                RaisedPropertyChanged("AxisTimeValue3");

            }
        }

        private double _AxisTimeValue4;
        public double AxisTimeValue4
        {
            get
            {
                return _AxisTimeValue4;
            }
            set
            {
                _AxisTimeValue4 = value;
                RaisedPropertyChanged("AxisTimeValue4");

            }
        }
        private double _AxisTimeValueEnd;
        public double AxisTimeValueEnd
        {
            get
            {
                return _AxisTimeValueEnd;
            }
            set
            {
                _AxisTimeValueEnd = value;
                RaisedPropertyChanged("AxisTimeValueEnd");

            }
        }

        private double _MaxOutputControllerValue;
        public double MaxOutputControllerValue
        {
            get
            {
                return _MaxOutputControllerValue;
            }
            set
            {
                _MaxOutputControllerValue = value;
                RaisedPropertyChanged("MaxOutputControllerValue");

            }
        }



        private double _MaxInputSignalValue;
        public double MaxInputSignalValue
        {
            get
            {
                return _MaxInputSignalValue;
            }
            set
            {
                _MaxInputSignalValue = value;
                RaisedPropertyChanged("MaxInputSignalValue");
            }
        }

        private double _SliderTick;
        public double SliderTick
        {
            get
            {
                return _SliderTick;
            }
            set
            {
                _SliderTick = value;
                RaisedPropertyChanged("SliderTick");
            }
        }

        private double _MaxOutputSignalValue;
        public double MaxOutputSignalValue
        {
            get
            {
                return _MaxOutputSignalValue;
            }
            set
            {
                _MaxOutputSignalValue = value;
                RaisedPropertyChanged("MaxOutputSignalValue");

            }
        }

        private double _LoadTorqueValue;
        public double LoadTorqueValue
        {
            get
            {
                return _LoadTorqueValue;
            }
            set
            {
                _LoadTorqueValue = value;
                RaisedPropertyChanged("LoadTorqueValue");
            }
        }


        #endregion

        #region Definition of observable collections
        public ObservableCollection<double> Canvas1CoefficientAxisX { get; set; }
        public ObservableCollection<double> Canvas1CoefficientAxisY { get; set; }
        public ObservableCollection<double> Canvas2CoefficientAxisX { get; set; }
        public ObservableCollection<double> Canvas2CoefficientAxisY { get; set; }

        public ObservableCollection<double> SecondaryAxisCoefficient1 { get; set; } 
        public ObservableCollection<double> SecondaryAxisCoefficient2 { get; set; }
        public ObservableCollection<double> SecondaryAxisCoefficient3 { get; set; }
        public ObservableCollection<double> SecondaryAxisCoefficient4 { get; set; }

        public ObservableCollection<double> AxisMaxValueCoefficient { get; set; } 
        public ObservableCollection<double> Axis34ValueCoefficient { get; set; }  
        public ObservableCollection<double> Axis12ValueCoefficient { get; set; }
        public ObservableCollection<double> Axis14ValueCoefficient { get; set; }

        #endregion

        #region Definition of PID components and the PID controller

        EmelentP P = new EmelentP(30);
        ElementI I = new ElementI(0);
        ElementD D = new ElementD(0);

        Controller PiD = new Controller();


        #endregion

        #region Timer definition

        double time;
        double dt = 0.001;
        TranslateTransform leftShift;

        #endregion

        #region Definition of variables

        double chartShift;
        double inputValue;
        double currentValue;
        double omegaValue;
        int iteration;

        bool boolOpenLoopControl;
        bool boolManualControl;

        double sineAmplitudeValue;
        double sineFrequencyValue;
        double sineMeanValue;

        int horizontalAxisLength;


        int canvasHeightValue;

        double scaleInputChartCoefficientY;
        double scaleControllerChartCoefficientY;
        double scaleOutputChartCoefficientY;
        double shiftInputChartCoefficientY;
        double shiftControllerChartCoefficientY;
        double shiftOutputChartCoefficientY;

        #endregion

        #region Definition of MicroTimer

        MicroTimer MTimer2;

        #endregion

        #region Motor definition
        Engine electricMotor = new Engine("Basic electric engine 1.0", 22.581d, 0.0281d, 0.0221d, 1.25d, 1.25d);
        #endregion

        #region Definiton of RK4 solver and equations
        EquationI currentDifferentialEquation;
        EquationOmega speedDifferentialEquation;

        RK4 equationsSolver = new RK4();
        #endregion

        #region Definition of data list
        List<string> DataList;
        #endregion

        private void ButtonChartsInitializationClick(object sender, RoutedEventArgs e)
        {
            const double margin = 30;
            canvasHeightValue = (int)(GridPIDSignal.ActualHeight - 2 * margin);

            horizontalAxisLength = (int)(GridInputSignal.ActualWidth - (30 + 40) - 8);

            Canvas1CoefficientAxisY.Add(0); //x1
            Canvas1CoefficientAxisY.Add(0); //y1
            Canvas1CoefficientAxisY.Add(0); //x2
            Canvas1CoefficientAxisY.Add(canvasHeightValue); //x2

            Canvas1CoefficientAxisX.Add(0); //x1
            Canvas1CoefficientAxisX.Add(0); //y1
            Canvas1CoefficientAxisX.Add(horizontalAxisLength); //x2
            Canvas1CoefficientAxisX.Add(0); //y2

            Canvas2CoefficientAxisY.Add(0); //x1
            Canvas2CoefficientAxisY.Add(0); //y1
            Canvas2CoefficientAxisY.Add(0); //x2
            Canvas2CoefficientAxisY.Add(canvasHeightValue); //y2

            Canvas2CoefficientAxisX.Add(0); //x1
            Canvas2CoefficientAxisX.Add(0); //y1
            Canvas2CoefficientAxisX.Add(horizontalAxisLength); //x2
            Canvas2CoefficientAxisX.Add(0); //y2

            SecondaryAxisCoefficient1.Add((horizontalAxisLength) / 5);
            SecondaryAxisCoefficient1.Add(0);
            SecondaryAxisCoefficient1.Add((horizontalAxisLength) / 5);
            SecondaryAxisCoefficient1.Add(canvasHeightValue);

            SecondaryAxisCoefficient2.Add(((horizontalAxisLength) / 5) * 2);
            SecondaryAxisCoefficient2.Add(0);
            SecondaryAxisCoefficient2.Add(((horizontalAxisLength) / 5) * 2);
            SecondaryAxisCoefficient2.Add(canvasHeightValue);

            SecondaryAxisCoefficient3.Add(((horizontalAxisLength) / 5) * 3);
            SecondaryAxisCoefficient3.Add(0);
            SecondaryAxisCoefficient3.Add(((horizontalAxisLength) / 5) * 3);
            SecondaryAxisCoefficient3.Add(canvasHeightValue);

            SecondaryAxisCoefficient4.Add(((horizontalAxisLength) / 5) * 4);
            SecondaryAxisCoefficient4.Add(0);
            SecondaryAxisCoefficient4.Add(((horizontalAxisLength) / 5) * 4);
            SecondaryAxisCoefficient4.Add(canvasHeightValue);

            AxisMaxValueCoefficient.Add(0);
            AxisMaxValueCoefficient.Add(canvasHeightValue);
            AxisMaxValueCoefficient.Add(horizontalAxisLength);
            AxisMaxValueCoefficient.Add(canvasHeightValue);

            Axis34ValueCoefficient.Add(0);
            Axis34ValueCoefficient.Add(canvasHeightValue / 4.0 * 3.0);
            Axis34ValueCoefficient.Add(horizontalAxisLength);
            Axis34ValueCoefficient.Add(canvasHeightValue / 4.0 * 3.0);

            Axis12ValueCoefficient.Add(0);
            Axis12ValueCoefficient.Add(canvasHeightValue / 4.0 * 2.0);
            Axis12ValueCoefficient.Add(horizontalAxisLength);
            Axis12ValueCoefficient.Add(canvasHeightValue / 4.0 * 2.0);

            Axis14ValueCoefficient.Add(0);
            Axis14ValueCoefficient.Add(canvasHeightValue / 4.0);
            Axis14ValueCoefficient.Add(horizontalAxisLength);
            Axis14ValueCoefficient.Add(canvasHeightValue / 4.0);

            PolylineInputSignal.Stroke = Brushes.Red;
            PolylineInputSignal.StrokeThickness = 1;

            PolylinePIDSignal.Stroke = Brushes.Green;
            PolylinePIDSignal.StrokeThickness = 1;

            PolylineOutputSignal.Stroke = Brushes.Blue;
            PolylineOutputSignal.StrokeThickness = 1;

            ButtonChartsInitialization.IsEnabled = false;
            ButtonSaveSettings.IsEnabled = true;

        }
        private void MTimerTick2(object sender, MicroTimerEventArgs timerEventArgs)
        {
            double sineShift;

            if (boolManualControl)
            {
                Dispatcher.Invoke(delegate { inputValue = Slider.Value; });
                sineShift = 0;
            }
            else
            {
                inputValue = sineMeanValue + sineAmplitudeValue * Math.Sin((time * 2 * Math.PI * sineFrequencyValue));
                sineShift = sineMeanValue;
            }

            Dispatcher.Invoke(delegate { LoadTorqueValue = SliderLoadTorqueValue.Value; });
                DelegateSimulationMode(inputValue, LoadTorqueValue, time, dt, iteration, sineShift);

            iteration++;
            time += dt;
        }
        private void ButtonStartClick(object sender, RoutedEventArgs e)
        {
            ButtonStart.IsEnabled = false;
            ButtonStop.IsEnabled = true;
            ButtonReset.IsEnabled = false;
            ButtonSaveToFile.IsEnabled = false;
            ButtonSaveSettings.IsEnabled = false;
            ButtonInformationAboutAuthor.IsEnabled = false;

            SetControlsAvailabilityOpenClosedControlMode(false);
            SetControlsAvailabilityInputSignal(false);
            SetControlsAvailabilityManualInputSignal(false);
            SetControlsAvailabilitySinusoidalInputSignal(false);
            SetControlsAvailabilityPIDController(false);
            SetControlsAvailabilityEngine(false);
            SetControlsAvailabilityLoadTorque(false);
            TextBoxOutputChartMaxValue.IsEnabled = false;

            leftShift = new TranslateTransform();

            MTimer2 = new MicroTimer();
            MTimer2.Interval = 10000; //(1000 us = 1ms)
            MTimer2.MicroTimerElapsed += MTimerTick2;

            MTimer2.Start();

        }
        private void ButtonStopClick(object sender, RoutedEventArgs e)
        {
            MTimer2.Stop();
            ButtonStop.IsEnabled = false;
            ButtonReset.IsEnabled = true;
            ButtonSaveToFile.IsEnabled = true;
            ButtonInformationAboutAuthor.IsEnabled = true;

            SetControlsAvailabilityOpenClosedControlMode(true);
            SetControlsAvailabilityInputSignal(true);
            SetControlsAvailabilityEngine(true);
            SetControlsAvailabilityLoadTorque(true);

            if (boolManualControl)
                SetControlsAvailabilityManualInputSignal(true);
            else
                SetControlsAvailabilitySinusoidalInputSignal(true);
            if (boolOpenLoopControl == false)
                SetControlsAvailabilityPIDController(true);

            TextBoxOutputChartMaxValue.IsEnabled = true;
        }
        private void SetControlsAvailabilityOpenClosedControlMode(bool value)
        {
            RadioButtonOpenLoop.IsEnabled = value;
            RadioButtonClosedLoop.IsEnabled = value;
        }
        private void SetControlsAvailabilityInputSignal(bool value)
        {
            RadioButtonManualControl.IsEnabled = value;
            RadioButtonSinusControl.IsEnabled = value;
        }
        private void SetControlsAvailabilityManualInputSignal(bool value)
        {
            TextBoxMaxInputValueForControl.IsEnabled = value;
            TextBoxSliderTick.IsEnabled = value;
        }
        private void SetControlsAvailabilitySinusoidalInputSignal(bool vale)
        {
            TextBoxSineAmplitudeValue.IsEnabled = vale;
            TextBoxSineFrequencyValue.IsEnabled = vale;
            TextBoxSineMeanValue.IsEnabled = vale;
        }
        private void SetControlsAvailabilityPIDController(bool value)
        {
            TextBox_PID_P_Value.IsEnabled = value;
            TextBox_PID_I_Value.IsEnabled = value;
            TextBox_PID_D_Value.IsEnabled = value;
            TextBox_PID_U_Max_Value.IsEnabled = value;
        }
        private void SetControlsAvailabilityLoadTorque(bool value)
        {
            TextBoxLoadMomentMaxValue.IsEnabled = value;
            TextBoxLoadMomentSliderTick.IsEnabled = value;
        }
        private void SetControlsAvailabilityEngine(bool value)
        {
            TextBoxEngineRValue.IsEnabled = value;
            TextBoxEngineLValue.IsEnabled = value;
            TextBoxEngineJValue.IsEnabled = value;
            TextBoxEngineKeValue.IsEnabled = value;
            TextBoxEngineKmValue.IsEnabled = value;
        }
        private void ButtonSaveSettingsClick(object sender, RoutedEventArgs e)
        {
            int numberOfOperations = 0;
            int numberOfSuccessfulOperations = 0;

            double pGainValue = 0;
            double iGainValue = 0;
            double dGainValue = 0;
            bool isPGainValueChanged = false;
            bool isIGainValueChanged = false;
            bool isDGainValueChanged = false;
            bool isAllGainOfPIDChanged = false;
            bool isCorrectlyAllPIDGainParsed = false;

            double engineValueR = 0;
            double engineValueL = 0;
            double engineValueJ = 0;
            double engineValueKe = 0;
            double engineValueKm = 0;
            bool isEngineRValueChanged = false;
            bool isEngineLValueChanged = false;
            bool isEngineJValueChanged = false;
            bool isEngineKeValueChanged = false;
            bool isEngineKmValueChanged = false;
            bool isAllEngineValueChanged = false;
            bool isCorrectlyAllEngineValueParsed = false;

            double sineAmplitudeValue = 0;
            double sineFrequencyChanged = 0;
            double sineMeanValue = 0;
            bool isAmplitudeValueChanged = false;
            bool isFrequencyValueChanged = false;
            bool isMeanValueChanged = false;
            bool isAllSineValueChanged = false;
            bool isCorrectlyAllSineValueParsed = false;

            double maxInputSignalValueManual = 0;
            double sliderInputSignalValue = 0;
            bool isInputSignalValueManualChanged = false;
            bool isSliderInputSingalValueChanged = false;
            bool isAllInputSignalValueChanged = false;
            bool isSliderTickInputSignalValueGreaterThanZero = false;
            bool isSliderTickInputSignalValueLesserThanInputSignalValue = false;
            bool isAllInputSignalManualValueParsed = false;

            double maxPIDSignalValue = 0;
            bool isPIDSignalValueChanged = false;

            double maxOutputSignalValue = 0;
            bool isOutputSignalValueChanged = false;
            bool isOutputSignalValueParsed = false;

            double maxLoadTorqueSignalValue = 0;
            double tickLoadTorqueValue = 0;
            bool isLoadTorqueSignalValueChanged = false;
            bool isTickLoadTorqueValueChanged = false;
            bool isAllLoadTorqueValuesChanged = false;
            bool isTickLoadTorqueGreaterThanZero = false;
            bool isTickLoadTorqueLesserThanMaxValueOfLoadTorque = false;
            bool isAllLoadTorqueValuesParsed = false;

            numberOfOperations++;
            isPGainValueChanged = NumberChecker.TryDoubleParse(TextBox_PID_P_Value.Text, ref pGainValue);
            isIGainValueChanged = NumberChecker.TryDoubleParse(TextBox_PID_I_Value.Text, ref iGainValue);
            isDGainValueChanged = NumberChecker.TryDoubleParse(TextBox_PID_D_Value.Text, ref dGainValue);

            if (isPGainValueChanged && isIGainValueChanged && isDGainValueChanged)
                isAllGainOfPIDChanged = true;
            else
            {
                MessageBox.Show("The entered PID controller parameter values must be floating point numbers!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (isAllGainOfPIDChanged)
            {
                if (pGainValue >= 0 && iGainValue >= 0 && dGainValue >= 0)
                    isCorrectlyAllPIDGainParsed = true;
                else
                    MessageBox.Show("The entered PID controller parameter values must be greater than zero!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (isCorrectlyAllPIDGainParsed)
            {
                P.SetParametr(pGainValue);
                I.SetParametr(iGainValue);
                D.SetParametr(dGainValue);
                numberOfSuccessfulOperations++;
            }

            numberOfOperations++;
            isEngineRValueChanged = NumberChecker.TryDoubleParse(TextBoxEngineRValue.Text, ref engineValueR);
            isEngineLValueChanged = NumberChecker.TryDoubleParse(TextBoxEngineLValue.Text, ref engineValueL);
            isEngineJValueChanged = NumberChecker.TryDoubleParse(TextBoxEngineJValue.Text, ref engineValueJ);
            isEngineKeValueChanged = NumberChecker.TryDoubleParse(TextBoxEngineKeValue.Text, ref engineValueKe);
            isEngineKmValueChanged = NumberChecker.TryDoubleParse(TextBoxEngineKmValue.Text, ref engineValueKm);

            if (isEngineRValueChanged && isEngineLValueChanged && isEngineJValueChanged && isEngineKeValueChanged && isEngineKmValueChanged)
                isAllEngineValueChanged = true;
            else
                MessageBox.Show("The entered motor parameter values must be floating point numbers!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            if (isAllEngineValueChanged)
            {
                if (engineValueR >= 0 && engineValueL >= 0 && engineValueJ >= 0 && engineValueKe >= 0 && engineValueKm >= 0)
                    isCorrectlyAllEngineValueParsed = true;
                else
                    MessageBox.Show("The entered motor parameter values must be greater than zero!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (isCorrectlyAllEngineValueParsed)
            {
                electricMotor.SetParameters(engineValueR, engineValueL, engineValueJ, engineValueKe, engineValueKm);
                currentDifferentialEquation = new EquationI(electricMotor);
                speedDifferentialEquation = new EquationOmega(electricMotor);
                numberOfSuccessfulOperations++;
            }

            numberOfOperations++;
            isLoadTorqueSignalValueChanged = NumberChecker.TryDoubleParse(TextBoxLoadMomentMaxValue.Text, ref maxLoadTorqueSignalValue);
            isTickLoadTorqueValueChanged = NumberChecker.TryDoubleParse(TextBoxLoadMomentSliderTick.Text, ref tickLoadTorqueValue);

            if (isLoadTorqueSignalValueChanged && isTickLoadTorqueValueChanged)
                isAllLoadTorqueValuesChanged = true;
            else
                MessageBox.Show("The entered external load torque values must be floating point numbers!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            if(isAllLoadTorqueValuesChanged)
            {
                if (maxLoadTorqueSignalValue >= 0 && tickLoadTorqueValue >= 0)
                {
                    isTickLoadTorqueGreaterThanZero = true;

                    if (tickLoadTorqueValue < maxLoadTorqueSignalValue)
                        isTickLoadTorqueLesserThanMaxValueOfLoadTorque = true;
                    else
                        MessageBox.Show("The entered load torque tick value must be lesser than maximal value of load torque!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                else
                    MessageBox.Show("The entered external load torque values must be greater than zero!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (isTickLoadTorqueGreaterThanZero && isTickLoadTorqueLesserThanMaxValueOfLoadTorque)
                isAllLoadTorqueValuesParsed = true;

            if(isAllLoadTorqueValuesParsed)
            {
                SliderLoadTorqueValue.Maximum = maxLoadTorqueSignalValue;
                SliderLoadTorqueValue.TickFrequency = tickLoadTorqueValue;
                numberOfSuccessfulOperations++;
            }


            if (boolManualControl)
            {
                numberOfOperations++;
                isInputSignalValueManualChanged = NumberChecker.TryDoubleParse(TextBoxMaxInputValueForControl.Text, ref maxInputSignalValueManual);
                isSliderInputSingalValueChanged = NumberChecker.TryDoubleParse(TextBoxSliderTick.Text, ref sliderInputSignalValue);

                if (isInputSignalValueManualChanged && isSliderInputSingalValueChanged)
                    isAllInputSignalValueChanged = true;
                else
                    MessageBox.Show("The entered input signal values must be floating point numbers!", "BError", MessageBoxButton.OK, MessageBoxImage.Error);

                if(isAllInputSignalValueChanged)
                {
                    if (maxInputSignalValueManual >= 0 && sliderInputSignalValue >= 0)
                    {
                        isSliderTickInputSignalValueGreaterThanZero = true;

                        if (sliderInputSignalValue < maxInputSignalValueManual)
                            isSliderTickInputSignalValueLesserThanInputSignalValue = true;
                        else
                            MessageBox.Show("The entered input signal tick value must be lesser than maximal value of input signal!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                        MessageBox.Show("The entered input signal values must be greater than zero!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (isSliderTickInputSignalValueGreaterThanZero && isSliderTickInputSignalValueLesserThanInputSignalValue)
                    isAllInputSignalManualValueParsed = true;

                if(isAllInputSignalManualValueParsed)
                {
                    MaxInputSignalValue = maxInputSignalValueManual;
                    Slider.Maximum = MaxInputSignalValue;
                    SliderTick = sliderInputSignalValue;
                    Slider.TickFrequency = SliderTick;

                    LabelMaxValueDisplayedOnInputChart.Content = Math.Round(MaxInputSignalValue, 2);
                    Label34ValueDisplayedOnInputChart.Content = Math.Round(MaxInputSignalValue / 4.0 * 3.0, 2);
                    LabelHalfValueDisplayedOnInputChart.Content = Math.Round(MaxInputSignalValue / 2.0, 2);
                    Label14ValueDisplayedOnInputChart.Content = Math.Round(MaxInputSignalValue / 4.0, 2);
                    Label0ValueDisplayedOnInputChart.Content = 0;
                    scaleInputChartCoefficientY = canvasHeightValue / MaxInputSignalValue;
                    shiftInputChartCoefficientY = 0;
                    numberOfSuccessfulOperations++;
                }
            }

            else
            {
                numberOfOperations++;
                isAmplitudeValueChanged = NumberChecker.TryDoubleParse(TextBoxSineAmplitudeValue.Text, ref sineAmplitudeValue);
                isFrequencyValueChanged = NumberChecker.TryDoubleParse(TextBoxSineFrequencyValue.Text, ref sineFrequencyChanged);
                isMeanValueChanged = NumberChecker.TryDoubleParse(TextBoxSineMeanValue.Text, ref sineMeanValue);

                if (isAmplitudeValueChanged && isFrequencyValueChanged && isMeanValueChanged)
                    isAllSineValueChanged = true;
                else
                    MessageBox.Show("The entered sinusoidal input values must be floating point numbers!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                if (isAllSineValueChanged)
                {
                    if (sineAmplitudeValue >= 0 && sineFrequencyChanged >= 0)
                        isCorrectlyAllSineValueParsed = true;
                    else
                        MessageBox.Show("The entered sinusoidal input values must be greater than zero!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if(isCorrectlyAllSineValueParsed)
                {
                    this.sineMeanValue = sineMeanValue;
                    this.sineAmplitudeValue = sineAmplitudeValue;
                    sineFrequencyValue = sineFrequencyChanged;

                    MaxInputSignalValue = sineAmplitudeValue + sineMeanValue;
                    LabelMaxValueDisplayedOnInputChart.Content = MaxInputSignalValue;
                    Label34ValueDisplayedOnInputChart.Content = MaxInputSignalValue - sineAmplitudeValue / 2.0;
                    LabelHalfValueDisplayedOnInputChart.Content = MaxInputSignalValue - sineAmplitudeValue;
                    Label14ValueDisplayedOnInputChart.Content = MaxInputSignalValue - sineAmplitudeValue / 2.0 * 3.0;
                    Label0ValueDisplayedOnInputChart.Content = MaxInputSignalValue - sineAmplitudeValue / 2.0 * 4.0;

                    shiftInputChartCoefficientY = canvasHeightValue / 2.0;
                    scaleInputChartCoefficientY = (canvasHeightValue - canvasHeightValue / 2.0) / sineAmplitudeValue;
                    numberOfSuccessfulOperations++;
                }
            }

            if (boolOpenLoopControl == false)
            {
                numberOfOperations++;
                isPIDSignalValueChanged = NumberChecker.TryDoubleParse(TextBox_PID_U_Max_Value.Text, ref maxPIDSignalValue);
                
                if(isPIDSignalValueChanged == false)
                    MessageBox.Show("The entered controller parameters values must be floating point numbers!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    MaxOutputControllerValue = maxPIDSignalValue;
                    scaleControllerChartCoefficientY = canvasHeightValue / 2 / MaxOutputControllerValue;

                    LabelMaxValueDisplayedOnPIDChart.Content = MaxOutputControllerValue;
                    LabelMinValueDisplayedOnPIDChart.Content = -MaxOutputControllerValue;
                    LabelHalfValueDisplayedOnPIDChart.Content = MaxOutputControllerValue / 2.0;
                    LabelMinusHalfValueDisplayedOnPIDChart.Content = -MaxOutputControllerValue / 2.0;

                    shiftControllerChartCoefficientY = canvasHeightValue / 2.0;
                    numberOfSuccessfulOperations++;
                }
            }

            numberOfOperations++;
            isOutputSignalValueChanged = NumberChecker.TryDoubleParse(TextBoxOutputChartMaxValue.Text, ref maxOutputSignalValue);

            if(isOutputSignalValueChanged == false)
                MessageBox.Show("The entered axis value value must be floating point number!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (maxOutputSignalValue > 0)
                    isOutputSignalValueParsed = true;
                else
                   MessageBox.Show("The entered axis value value must be greater than zero!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if(isOutputSignalValueParsed)
            {
                MaxOutputSignalValue = maxOutputSignalValue;
                LabelMaxValueDisplayedOnOutputChart.Content = MaxOutputSignalValue;
                Label23ValueDisplayedOnInputChart.Content = Math.Round(MaxOutputSignalValue / 3.0 * 2.0, 2);
                Label13ValueDisplayedOnInputChart.Content = Math.Round(MaxOutputSignalValue / 3.0, 2);
                LabelMinus13ValueDisplayedOnInputChart.Content = -Math.Round(MaxOutputSignalValue / 3.0, 2);

                scaleOutputChartCoefficientY = (canvasHeightValue - canvasHeightValue / 4) / MaxOutputSignalValue;
                shiftOutputChartCoefficientY = canvasHeightValue / 4.0;
                numberOfSuccessfulOperations++;
            }

            if (numberOfOperations == numberOfSuccessfulOperations)
            {
                FillDataListBasicInformation();
                ButtonStart.IsEnabled = true;
                ButtonReset.IsEnabled = false;
                MessageBox.Show("Simulation parameters have been saved correctly!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                ButtonStart.IsEnabled = false;
        }
        private void RadioButtonManualControlEvent(object sender, RoutedEventArgs e)
        {
            Slider.IsEnabled = true;
            TextBoxMaxInputValueForControl.IsEnabled = true;
            TextBoxSliderTick.IsEnabled = true;

            TextBoxSineAmplitudeValue.IsEnabled = false;
            TextBoxSineMeanValue.IsEnabled = false;
            TextBoxSineFrequencyValue.IsEnabled = false;
            boolManualControl = true;
        }
        private void RadioButtonSinusControlCheckedEvent(object sender, RoutedEventArgs e)
        {
            Slider.IsEnabled = false;
            TextBoxMaxInputValueForControl.IsEnabled = false;
            TextBoxSliderTick.IsEnabled = false;
            TextBoxSineAmplitudeValue.IsEnabled = true;
            TextBoxSineMeanValue.IsEnabled = true;
            TextBoxSineFrequencyValue.IsEnabled = true;

            boolManualControl = false;
        }
        private void RadioButtonOpenLoopCheckedEvent(object sender, RoutedEventArgs e)
        {
            boolOpenLoopControl = true;
            SetControlsAvailabilityPIDController(false);
            DelegateSimulationMode = SimulationOpenLoop;
            GroupBoxInputSignal.Header = "Input signal [V]";
        }
        private void RadioButtonClosedLoopCheckedEvent(object sender, RoutedEventArgs e)
        {
            boolOpenLoopControl = false;
            SetControlsAvailabilityPIDController(true);
            DelegateSimulationMode = SimulationClosedLoop;
            GroupBoxInputSignal.Header = "Input signal [rad/s]";
        }
        private void SimulationOpenLoop(double inputSignalValue, double loadTorqueValue, double time, double dt, int numberOfIteration, double sineShift)
        {
            if (Math.Round(366 * time, 3) > horizontalAxisLength)
            {
                if (numberOfIteration % 2 == 0)
                {
                    Dispatcher.Invoke(delegate { PolylineInputSignal.RenderTransform = leftShift; });
                    Dispatcher.Invoke(delegate { PolylineOutputSignal.RenderTransform = leftShift; });

                    Dispatcher.Invoke(delegate { PolylineInputSignal.Points.RemoveAt(0); });
                    Dispatcher.Invoke(delegate { PolylineOutputSignal.Points.RemoveAt(0); });
                }

                if (numberOfIteration % 10 == 0)
                {
                    AxisTimeValue0 = Math.Round(time - 5, 2);
                    AxisTimeValue1 = Math.Round(time - 4, 2);
                    AxisTimeValue2 = Math.Round(time - 3, 2);
                    AxisTimeValue3 = Math.Round(time - 2, 2);
                    AxisTimeValue4 = Math.Round(time - 1, 2);
                    AxisTimeValueEnd = Math.Round(time, 2);

                    Dispatcher.Invoke(delegate { LabelInputSignalTime0.Content = AxisTimeValue0; });
                    Dispatcher.Invoke(delegate { LabelInputSignalTime1.Content = AxisTimeValue1; });
                    Dispatcher.Invoke(delegate { LabelInputSignalTime2.Content = AxisTimeValue2; });
                    Dispatcher.Invoke(delegate { LabelInputSignalTime3.Content = AxisTimeValue3; });
                    Dispatcher.Invoke(delegate { LabelInputSignalTime4.Content = AxisTimeValue4; });
                    Dispatcher.Invoke(delegate { LabelInputSignalTimeEnd.Content = AxisTimeValueEnd; });

                    Dispatcher.Invoke(delegate { LabelPIDSignalTime0.Content = AxisTimeValue0; });
                    Dispatcher.Invoke(delegate { LabelPIDSignalTime1.Content = AxisTimeValue1; });
                    Dispatcher.Invoke(delegate { LabelPIDSignalTime2.Content = AxisTimeValue2; });
                    Dispatcher.Invoke(delegate { LabelPIDSignalTime3.Content = AxisTimeValue3; });
                    Dispatcher.Invoke(delegate { LabelPIDSignalTime4.Content = AxisTimeValue4; });
                    Dispatcher.Invoke(delegate { LabelPIDSignalTimeEnd.Content = AxisTimeValueEnd; });

                    Dispatcher.Invoke(delegate { LabelOutputSignalTime0.Content = AxisTimeValue0; });
                    Dispatcher.Invoke(delegate { LabelOutputSignalTime1.Content = AxisTimeValue1; });
                    Dispatcher.Invoke(delegate { LabelOutputSignalTime2.Content = AxisTimeValue2; });
                    Dispatcher.Invoke(delegate { LabelOutputSignalTime3.Content = AxisTimeValue3; });
                    Dispatcher.Invoke(delegate { LabelOutputSignalTime4.Content = AxisTimeValue4; });
                    Dispatcher.Invoke(delegate { LabelOutputSignalTimeEnd.Content = AxisTimeValueEnd; });
                }

                chartShift = 366 * dt;
                Dispatcher.Invoke(delegate { leftShift.X -= chartShift; });
            }

            Error = inputValue - OutputSignalValue;

            if (numberOfIteration % 2 == 0)
            {
                Point pt = new Point(366 * time, scaleInputChartCoefficientY * (inputSignalValue - sineShift) + shiftInputChartCoefficientY);
                Dispatcher.Invoke(delegate { PolylineInputSignal.Points.Add(pt); });
            }

            if (numberOfIteration % 2 == 0)
            {
                Point pt3 = new Point(366 * time, scaleOutputChartCoefficientY * OutputSignalValue + shiftOutputChartCoefficientY);
                Dispatcher.Invoke(delegate { PolylineOutputSignal.Points.Add(pt3); });
            }

            DataList.Add(Math.Round(time, 3) + "," + inputSignalValue + "," + OutputSignalValue + "," + Error + "," + loadTorqueValue);

            OutputSignalValue = equationsSolver.SolveEquations(currentDifferentialEquation, speedDifferentialEquation, inputValue, loadTorqueValue, ref currentValue, ref omegaValue, dt);

            Dispatcher.Invoke(delegate { TextBlockInputSignalValue.Content = Math.Round(inputValue, 3); });
            Dispatcher.Invoke(delegate { TextBlockOutputSignalValue.Content = Math.Round(OutputSignalValue, 3); });
            Dispatcher.Invoke(delegate { TextBlockLoadMomentSignalValue.Content = Math.Round(loadTorqueValue, 3); });
        }
        private void SimulationClosedLoop(double inputSignalValue, double loadTorqueValue, double time, double dt, int numberOfIteration, double sineShift)
        {
            if (Math.Round(366 * time, 3) > horizontalAxisLength)
            {

                if (numberOfIteration % 2 == 0)
                {
                    Dispatcher.Invoke(delegate { PolylineInputSignal.RenderTransform = leftShift; });
                    Dispatcher.Invoke(delegate { PolylinePIDSignal.RenderTransform = leftShift; });
                    Dispatcher.Invoke(delegate { PolylineOutputSignal.RenderTransform = leftShift; });

                    Dispatcher.Invoke(delegate { PolylineInputSignal.Points.RemoveAt(0); });
                    Dispatcher.Invoke(delegate { PolylinePIDSignal.Points.RemoveAt(0); });
                    Dispatcher.Invoke(delegate { PolylineOutputSignal.Points.RemoveAt(0); });
                }

                if (numberOfIteration % 10 == 0)
                {
                    AxisTimeValue0 = Math.Round(time - 5, 2);
                    AxisTimeValue1 = Math.Round(time - 4, 2);
                    AxisTimeValue2 = Math.Round(time - 3, 2);
                    AxisTimeValue3 = Math.Round(time - 2, 2);
                    AxisTimeValue4 = Math.Round(time - 1, 2);
                    AxisTimeValueEnd = Math.Round(time, 2);

                    Dispatcher.Invoke(delegate { LabelInputSignalTime0.Content = AxisTimeValue0; });
                    Dispatcher.Invoke(delegate { LabelInputSignalTime1.Content = AxisTimeValue1; });
                    Dispatcher.Invoke(delegate { LabelInputSignalTime2.Content = AxisTimeValue2; });
                    Dispatcher.Invoke(delegate { LabelInputSignalTime3.Content = AxisTimeValue3; });
                    Dispatcher.Invoke(delegate { LabelInputSignalTime4.Content = AxisTimeValue4; });
                    Dispatcher.Invoke(delegate { LabelInputSignalTimeEnd.Content = AxisTimeValueEnd; });

                    Dispatcher.Invoke(delegate { LabelPIDSignalTime0.Content = AxisTimeValue0; });
                    Dispatcher.Invoke(delegate { LabelPIDSignalTime1.Content = AxisTimeValue1; });
                    Dispatcher.Invoke(delegate { LabelPIDSignalTime2.Content = AxisTimeValue2; });
                    Dispatcher.Invoke(delegate { LabelPIDSignalTime3.Content = AxisTimeValue3; });
                    Dispatcher.Invoke(delegate { LabelPIDSignalTime4.Content = AxisTimeValue4; });
                    Dispatcher.Invoke(delegate { LabelPIDSignalTimeEnd.Content = AxisTimeValueEnd; });

                    Dispatcher.Invoke(delegate { LabelOutputSignalTime0.Content = AxisTimeValue0; });
                    Dispatcher.Invoke(delegate { LabelOutputSignalTime1.Content = AxisTimeValue1; });
                    Dispatcher.Invoke(delegate { LabelOutputSignalTime2.Content = AxisTimeValue2; });
                    Dispatcher.Invoke(delegate { LabelOutputSignalTime3.Content = AxisTimeValue3; });
                    Dispatcher.Invoke(delegate { LabelOutputSignalTime4.Content = AxisTimeValue4; });
                    Dispatcher.Invoke(delegate { LabelOutputSignalTimeEnd.Content = AxisTimeValueEnd; });
                }

                chartShift = 366 * dt;
                Math.Round(chartShift, 3);
                Dispatcher.Invoke(delegate { leftShift.X -= chartShift; ; });

            }

            if (numberOfIteration % 2 == 0)
            {
                Point pt = new Point(366 * time, scaleInputChartCoefficientY * (inputSignalValue - sineShift) + shiftInputChartCoefficientY);
                Dispatcher.Invoke(delegate { PolylineInputSignal.Points.Add(pt); });

                Point pt3 = new Point(366 * time, scaleOutputChartCoefficientY * OutputSignalValue + shiftOutputChartCoefficientY);
                Dispatcher.Invoke(delegate { PolylineOutputSignal.Points.Add(pt3); });
            }

            Error = inputSignalValue - OutputSignalValue;
            ControllerSignalValue = PiD.CalculateResult(Error, dt, P, I, D, numberOfIteration, MaxOutputControllerValue, -MaxOutputControllerValue);

            DataList.Add(Math.Round(time, 3) + "," + inputSignalValue + "," + ControllerSignalValue + "," + OutputSignalValue + "," + Error + "," + LoadTorqueValue);

            if (numberOfIteration % 2 == 0)
            {
                Point pt2 = new Point(366 * time, scaleControllerChartCoefficientY * ControllerSignalValue + shiftControllerChartCoefficientY);
                Dispatcher.Invoke(delegate { PolylinePIDSignal.Points.Add(pt2); });
            }

            OutputSignalValue = equationsSolver.SolveEquations(currentDifferentialEquation, speedDifferentialEquation, ControllerSignalValue, loadTorqueValue, ref currentValue, ref omegaValue, dt);

            Dispatcher.Invoke(delegate { TextBlockInputSignalValue.Content = Math.Round(inputValue, 3); });
            Dispatcher.Invoke(delegate { TextBlockErrorSignalValue.Content = Math.Round(Error, 3); });
            Dispatcher.Invoke(delegate { TextBlockPIDSignalValue.Content = Math.Round(ControllerSignalValue, 3); });
            Dispatcher.Invoke(delegate { TextBlockOutputSignalValue.Content = Math.Round(OutputSignalValue, 3); });
            Dispatcher.Invoke(delegate { TextBlockLoadMomentSignalValue.Content = Math.Round(loadTorqueValue, 3); });
        }
        private void ButtonResetClick(object sender, RoutedEventArgs e)
        {
            ResetInitialValues();
            FillDataListBasicInformation();
            ButtonStart.IsEnabled = true;
            ButtonReset.IsEnabled = false;
            ButtonSaveSettings.IsEnabled = true;
        }
        private void ResetInitialValues()
        {
            time = 0;
            currentValue = 0;
            omegaValue = 0;
            iteration = 0;
            OutputSignalValue = 0;

            PolylineInputSignal.Points.Clear();
            PolylinePIDSignal.Points.Clear();
            PolylineOutputSignal.Points.Clear();


            PolylineInputSignal.RenderTransform = new TranslateTransform(chartShift, 0);
            PolylinePIDSignal.RenderTransform = new TranslateTransform(chartShift, 0);
            PolylineOutputSignal.RenderTransform = new TranslateTransform(chartShift, 0);

            chartShift = 0d;

            PiD.ResetSupportParameters();
            TextBlockInputSignalValue.Content = 0;
            TextBlockErrorSignalValue.Content = 0;
            TextBlockPIDSignalValue.Content = 0;
            TextBlockOutputSignalValue.Content = 0;
            TextBlockLoadMomentSignalValue.Content = 0;

            LabelInputSignalTime0.Content = "0.0";
            LabelInputSignalTime1.Content = "1.0";
            LabelInputSignalTime2.Content = "2.0";
            LabelInputSignalTime3.Content = "3.0";
            LabelInputSignalTime4.Content = "4.0";
            LabelInputSignalTimeEnd.Content = "5.0";

            LabelPIDSignalTime0.Content = "0.0";
            LabelPIDSignalTime1.Content = "1.0";
            LabelPIDSignalTime2.Content = "2.0";
            LabelPIDSignalTime3.Content = "3.0";
            LabelPIDSignalTime4.Content = "4.0";
            LabelPIDSignalTimeEnd.Content = "5.0";

            LabelOutputSignalTime0.Content = "0.0";
            LabelOutputSignalTime1.Content = "1.0";
            LabelOutputSignalTime2.Content = "2.0";
            LabelOutputSignalTime3.Content = "3.0";
            LabelOutputSignalTime4.Content = "4.0";
            LabelOutputSignalTimeEnd.Content = "5.0";
        }
        private void ButtonSaveToFileClick(object sender, RoutedEventArgs e)
        {
            if (DataList.Count != 0)
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Text file (*.txt)|*.txt";

                if (dialog.ShowDialog() == true)
                {
                    TextWriter tw = new StreamWriter(dialog.FileName);

                    foreach (string s in DataList)
                    {
                        tw.WriteLine(s);
                    }

                    tw.Close();
                    MessageBox.Show("Writing simulation data to file completed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
                MessageBox.Show("No data to save! Please start the simulation and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void FillDataListBasicInformation()
        {
            DataList.Clear();
            DataList.Add("Basic information:");
            DataList.Add("");
            DataList.Add("Motor:");
            DataList.Add("Motor resistance: " + TextBoxEngineRValue.Text);
            DataList.Add("Motor inductance: " + TextBoxEngineLValue.Text);
            DataList.Add("Moment of inertia of the engine: " + TextBoxEngineJValue.Text);
            DataList.Add("Electric motor constant: " + TextBoxEngineKeValue.Text);
            DataList.Add("Mechanical motor constant: " + TextBoxEngineKmValue.Text);
            DataList.Add("");
            if (boolOpenLoopControl)
                DataList.Add("Data format: time, input signal value, output signal value, error value, load torque value");
            else
                DataList.Add("Data format: time, input signal value, regulator signal value, output signal value, error value, load torque value");
            DataList.Add("");
        }
        private void ButtonInformationAboutAuthorClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Made by Wojciech Strzeboński 2019", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
