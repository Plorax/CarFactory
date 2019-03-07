using Core;
using ScreenCard;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarFactory
{
    class Program
    {
        static Widget_Square screenBorder = new Widget_Square();
        static Widget_ProgressBar progressBar = new Widget_ProgressBar();
        static Widget_Prompt keyPromptWidget = new Widget_Prompt();
        static Car_Widget carListWidget = new Car_Widget();
        static Widget_TextBox textGreetingWidget = new Widget_TextBox();
        static Widget_TextBox carSelectQuestion = new Widget_TextBox();
        static AM_Widget amWidget = new AM_Widget();
        static Help_Widget helpWidget = new Help_Widget();

        private static void Run(CrashCatch cc)
        {
            Console.Clear();

            cc
                .Reset()
                .Try(_onRun)
                .OnCrash(_onCrash)
                .OnResume(_onResume)
                .DumpOnCrash()
                .Execute();
        }

        private static void _onCrash(CrashCatch cc)
        {
            Console.Clear();
            Console.Write(cc.GetGeneralInformationLog());
            Console.Write(cc.GetErrorLogInformation());

            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        private static void _onResume(CrashCatch cc)
        {
            Run(cc);
        }

        #region Main Application Loop
        private static void _onRun(CrashCatch cc)
        {
            string keyInput = string.Empty;

            // Initialize Console Screen Buffer
            Display.Initialize(ConsoleScreenBuffer.Create());

            screenBorder.SetBorderColour(ConsoleColor.Green);
            screenBorder.SetDimensions(50, 20);
            screenBorder.SetLocation(0, 0);

            helpWidget.SetLocation(2, 15);
            keyPromptWidget.SetLocation(2, 10);
            carListWidget.SetLocation(5, 5);
            amWidget.SetLocation(1, 19);
            textGreetingWidget.SetLocation(2, 1);
            carSelectQuestion.SetLocation(2, 2);

            textGreetingWidget.SetColour(ConsoleColor.White);
            carSelectQuestion.SetColour(ConsoleColor.White);
            progressBar.SetColour(ConsoleColor.DarkGray);

            progressBar.ProgressText = "Building...";

            textGreetingWidget.SetText("Good {0} Sir,", (amWidget.GetAMPM() == AM_Widget.AMPM.AM ? "morning" : "evening"));
            carSelectQuestion.SetText("Which car would you like to build today ?");

            // multi-threaded Time-Widget
            amWidget.MakeLive();

            // multi-threaded Keyboard Input Handler
            keyPromptWidget.MakeLive();

            // multi-threaded Progress Bar
            //progressBar.MakeLive();

            // Specify the prompt negotiator
            keyPromptWidget.OnPrompt(_onKeyEnterPrompt);

            while (string.Compare(keyPromptWidget.TextInput, "quit", true) != 0)
            {
                // Render our 2D environment
                screenBorder.Render();
                carListWidget.Render();
                textGreetingWidget.Render();
                carSelectQuestion.Render();
                progressBar.Render();
                helpWidget.Render();

                // Always make sure we have focus on the keyboard input handler
                keyPromptWidget.SetFocus();

                Thread.Sleep(1);
            }

            Display.Release();
            cc.Quit(); /* this lets the application quit instead of resuming/restarting */
        }

        private static void _onKeyEnterPrompt(string promptText)
        {
            int option = 0;

            if (int.TryParse(promptText, out option))
            {
                keyPromptWidget.Show(false);
                helpWidget.Show(false);
                carListWidget.Show(false);
                textGreetingWidget.Show(false);
                carSelectQuestion.Show(false);

                Display.ShowCursor(false);

                progressBar.ProgressText = "Building your car ";
                progressBar.ProgressValue = 0;
                progressBar.Show(true);

                Display.UpdateLayout();

                Thread _worker = new Thread(() =>
                {
                    while (progressBar.ProgressValue < 100)
                    {
                        ++progressBar.ProgressValue;

                        Thread.Sleep(100);
                    }

                    progressBar.Show(false);

                    Display.ShowCursor(true);

                    keyPromptWidget.Show(true);
                    helpWidget.Show(true);
                    carListWidget.Show(true);
                    textGreetingWidget.Show(true);
                    carSelectQuestion.Show(true);

                    Display.UpdateLayout();
                });
                
                _worker.Start();
                _worker.Join();
            }
        }
        #endregion

        static void Main(string[] args)
        {
            Run(CrashCatch.Create());
        }
    }
}
