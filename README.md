# IAS0410_Plant_Logger
A logger for IAS0410PlantEmulator

## Getting Started
There are three projects in this solution. 
1. __Library__:
   This is the library for the whole project. All the necessary classes reside in this project. This project is compiled into a __`.dll`__ file.

2. __ConsoleUI__:
   ConsoleUI is the console application of the solution. This provides a console user interface for the logger. The logger writes the logs into a text file, the path of which is specified in the __`appsettings.json`__ file.

3. __WinFormsUI__:
   WinFormsUI is the GUI for the logger. It provides all the features of the ConsoleUI, plus it can change the log file specified by the user from a GUI file selector dialog.


## Technologies
Technologies used to build this system:
+ .NET Core 3.1
+ C# 8
+ Visual Studio

## Build & Run
To build the application, you need .NET Core 3.1 SDK installed in your machine. The WinFormsUI can only run on Windows machines. 
`cd` into the ConsoleUI or WinFormsUI folder and run `dotnet run` command.
