﻿namespace Sugar.Command
{
    /// <summary>
    /// Represent a command bound to parameters (e.g. program.exe -parameter value)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BoundCommand<T> : ICommand where T : class, new()
    {
        protected T OptionsBound;

        /// <summary>
        /// Returns the success exit code cast as an int (0).
        /// </summary>
        /// <returns>0</returns>
        public static int Success()
        {
            return (int)ExitCode.Success;
        }

        /// <summary>
        /// Returns the default failure return code (-1).
        /// </summary>
        /// <returns>-1</returns>
        public static int Fail()
        {
            return (int)ExitCode.GeneralError;
        }

        /// <summary>
        /// Determines whether this instance can execute with the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        ///   <c>true</c> if this instance can execute the specified parameters; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool CanExecute(Parameters parameters)
        {
            OptionsBound = new ParameterBinder().Bind<T>(parameters);

            return OptionsBound != null;
        }

        /// <summary>
        /// Executes this instance with the specified parameters.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public int Execute(Parameters parameters)
        {
            return Execute(OptionsBound);
        }

        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public abstract int Execute(T options);
    }
}
