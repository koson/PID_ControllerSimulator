# PID_ControllerSimulator
The DC motor control simulator with the PID controller

## Introduction
This is my first major project that was written using C#. In this project there is a (almost) real time simulator, that allows you to control the speed of the DC motor shaft. The simulation can run in two modes: 
* control in the open-loop
* control in the closed-loop

In the closed-loop control the PID controller is used. It allows you to obtain desired speed value of the DC motor shaft. In the open-loop control, the voltage value is directly applied to the DC motor.

## Functionalities
* The DC motor speed control in open and closed-loop
* The input signal value can be changed manually or automaticaly generated to form a sine wave
* The DC motor shaft can be loaded with the external load torque
* The PID controller parameters can be changed by the user
* The DC motor parameters can be changed by the user
* Individual signal values are displayed on the panel
* The simulation run can be saved to the external file (txt)
* Waveforms of individual signal (input signal, controller output, output signal) are drawn on charts

## What has been done?
I wanted to code everything from scratch, that's why I didn't use any external plugins. I coded the entire logic for displaying charts correctly, depending on the user parameters entered. That's why whole GUI are written in the "clear" WPF. The DC motor was modeled using differential equations. The 4th order Runge-Kutta method was appied to solve the system of differential equations. The PID controller also was coded using equation, that describe it.

## Built with
In this project I used Visual Studio 2019 and C# version 4.7.2, GUI was written using WPF framework. I also used Microsecond timer class that was written by ken.loveday ([source](https://www.codeproject.com/Articles/98346/Microsecond-and-Millisecond-NET-Timer)).

### Last more thing...
I know that the code presented is not the best. I'm learning all the time and I'm trying to write better code :)
