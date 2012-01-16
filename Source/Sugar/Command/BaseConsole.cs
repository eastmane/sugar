﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sugar.Command
{
    /// <summary>
    /// Provides core functionality for console applications
    /// </summary>
    public abstract class BaseConsole
    {
        /// <summary>
        /// Pauses if a debugger is attached.
        /// </summary>
        private static void PauseIfInDebuggerAttached()
        {
            if (!Debugger.IsAttached) return;

            Console.WriteLine("");
            Console.Write("Press any key to exit...");
            Console.ReadLine();
        }

        /// <summary>
        /// Gets or sets the command line arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public Parameters Arguments { get; private set; }

        /// <summary>
        /// Runs this console application.
        /// </summary>
        public void Run(string[] args)
        {
            // Get the command line arguments
            Arguments = new ParameterParser().Parse(args);

            // Check user input
            Main();      

            // Pause if in debug mode
            PauseIfInDebuggerAttached();
        }

        /// <summary>
        /// Sets the console arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        protected void SetArguments(string args)
        {
            Arguments = new ParameterParser().Parse(args, new List<string>());
        }

        /// <summary>
        /// Sets the console arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="switches">The command switches, e.g. a leading "-", "--" or "/".</param>
        protected void SetArguments(string args, IList<string> switches)
        {
            Arguments = new ParameterParser().Parse(args, switches);
        }

        /// <summary>
        /// Entry point for the program logic
        /// </summary>
        protected abstract void Main();

        /// <summary>
        /// Validates the user command line parameters.
        /// </summary>
        /// <returns>true is command line parameters are correct, false if not.</returns>
        public virtual bool Validate()
        {
            return true;
        }
    }
}