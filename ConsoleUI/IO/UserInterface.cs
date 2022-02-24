﻿using ConsoleUI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI.IO
{
    public static class UserInterface
    {
        static Dictionary<string, Action> executableOptions = new Dictionary<string, Action>()
        {
            { "hire", ConsoleHelpers.HireEmployee },
            { "fire", ConsoleHelpers.FireEmployee },
            { "find", ConsoleHelpers.FindEmployeeById },
            { "promote", ConsoleHelpers.PromoteEmployee },
            { "demote", ConsoleHelpers.DemoteEmployee },
            { "list", ConsoleHelpers.GetAllEmployees},
            { "current", ConsoleHelpers.GetCurrentEmployees },
            { "former", ConsoleHelpers.GetFormerEmployees },
            { "help", DisplayMainMenuOptions},
            { "quit", ExitApplication }
        };

        public static bool IsAppRunning { get; private set; } = true;
        public static void DisplayTitle()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Welcome to Employee Record System App.");
            Console.WriteLine();
            Console.ResetColor();
        }

        public static void DisplayMainMenuOptions()
        {
            Dictionary<string, string> helpContents = new Dictionary<string, string>
            {
                { "hire", "Hire an employee." },
                { "fire", "Fire an employee." },
                { "find", "Find a single employee." },
                { "promote", "Promote an employee." },
                { "demote", "Demote an employee." },
                { "list", "Show a list of current and former employees." },
                { "current", "Show a list of current employees." },
                { "former", "Show a list of former employees." },
                { "help", "Show this menu." },
                { "quit", "Exit the application." },
            };

            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (KeyValuePair<string, string> menuItem in helpContents)
            {
                Console.WriteLine($"{menuItem.Key}: {menuItem.Value}");
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        public static void MainMenuInput()
        {
            while (UserInterface.IsAppRunning)
            {
                Console.Write("Enter a command: ");
                string? userInput = Console.ReadLine();

                while (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.Write("Enter a command: ");
                    userInput = Console.ReadLine();
                }

                if (executableOptions.ContainsKey(userInput.ToLower()))
                {
                    Console.Clear();
                    executableOptions[userInput]();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Command not found");
                }
            }
        }

        private static void ExitApplication()
        {
            IsAppRunning = false;
        }

        public static void DisplayListHeader(ConsoleColor foregroundColor, string employeeType)
        {
            Console.ForegroundColor = foregroundColor;
            Console.WriteLine($"----{employeeType} EMPLOYEES----");
            Console.ResetColor();
        }
    }
}
