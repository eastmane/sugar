using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sugar.Command
{
    /// <summary>
    /// Class to represent the applications command line parameters.
    /// </summary>
    public class Parameters : List<string>, ICloneable
    {
        private static Parameters current;
        private static string filename;
        private static string directory;

        /// <summary>
        /// Initializes the static members.
        /// </summary>
        private static void InitializeStaticMembers()
        {
            // Only perform once
            if (current != null) return;

            // Get the current command line
            var args = Environment.CommandLine;
            current = new ParameterParser().Parse(args);

            // Set current filename
            filename = current[0];

            // Remove filename from parameters
            current.RemoveAt(0);

            var codebase = Assembly.GetExecutingAssembly().GetName().CodeBase;

            // Get assembly directory
            if (!string.IsNullOrEmpty(codebase))
            {
                directory = Path.GetDirectoryName(codebase) ?? string.Empty;
            }
            else
            {
                directory = System.IO.Directory.GetCurrentDirectory();
            }
            directory = directory.Replace("file:\\", "");
        }

        /// <summary>
        /// Sets the current command line to the given value.
        /// </summary>
        /// <param name="commandLine">The command line.</param>
        public static void SetCurrent(string commandLine)
        {
            InitializeStaticMembers();

            current = new ParameterParser().Parse(commandLine);
        }

        /// <summary>
        /// Gets the current command line arguments.
        /// </summary>
        public static Parameters Current
        {
            get
            {
                InitializeStaticMembers();

                return current;
            }
        }
        
        /// <summary>
        /// Gets the current filename of the executable.
        /// </summary>
        public static string Filename
        {
            get
            {
                InitializeStaticMembers();

                return filename;
            }
        }

        /// <summary>
        /// Gets the current directory of the executable.
        /// </summary>
        public static string Directory
        {
            get
            {
                InitializeStaticMembers();

                return directory;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameters"/> class.
        /// </summary>
        public Parameters()
        {
            Switches = new List<string> { "-", "--", "/" };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameters"/> class.
        /// </summary>
        /// <param name="args">The args.</param>
        public Parameters(string args) : this()
        {
            if (string.IsNullOrEmpty(args)) return;

            AddRange(new ParameterParser().Parse(args));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameters"/> class.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <param name="switches">The switches.</param>
        public Parameters(string args, IEnumerable<string> switches)
        {
            if (string.IsNullOrEmpty(args)) return;

            AddRange(new ParameterParser().Parse(args, switches));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameters"/> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public Parameters(IEnumerable<string> parameters) : this()
        {
            foreach (var parameter in parameters)
            {
                Add(parameter);
            }
        }

        /// <summary>
        /// Gets or sets the command switches.
        /// </summary>
        /// <value>
        /// By default, values starting with "-", "--" or "/"
        /// </value>
        public IList<string> Switches { get; private set; }

        /// <summary>
        /// Returns a parameter as a string value
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string AsString(string name)
        {
            return AsString(name, string.Empty);
        }

        /// <summary>
        /// Returns a parameter as a string value
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="default">The @default value.</param>
        /// <returns></returns>
        public string AsString(string name, string @default)
        {
            return AsStrings(name, new[] { @default }).First();
        }

        /// <summary>
        /// Returns a parameter as a string value
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="defaults">The default values.</param>
        /// <returns></returns>
        public IList<string> AsStrings(string name, params string[] defaults)
        {
            var result = new List<string>();

            var index = IndexOf(name);

            while (index > -1 && Count > index + 1)
            {
                if (IsFlag(this[index + 1]) && Switches.Count > 0)
                {
                    break;
                }

                result.Add(this[index + 1]);

                index++;
            }

            if (result.Count == 0) result.AddRange(defaults);

            return result;
        }

        /// <summary>
        /// Returns a parameter as an integer value
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public int AsInteger(string name)
        {
            return AsInteger(name, 0);
        }

        /// <summary>
        /// Returns a parameter as an integer value
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="default">The @default value.</param>
        /// <returns></returns>
        public int AsInteger(string name, int @default)
        {
            int result;

            var resultAsString = AsString(name);

            if (!int.TryParse(resultAsString, out result))
            {
                result = @default;
            }

            return result;
        }

        /// <summary>
        /// Returns a parameter as an integer value
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Today if not found
        /// </returns>
        public DateTime AsDateTime(string name)
        {
            return AsDateTime(name, DateTime.Today);
        }

        /// <summary>
        /// Returns a parameter as an integer value
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="default">The @default value.</param>
        /// <returns></returns>
        public DateTime AsDateTime(string name, DateTime @default)
        {
            DateTime result;

            var resultAsString = AsString(name);

            if (!DateTime.TryParse(resultAsString, out result))
            {
                result = @default;
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified name contains argument.
        /// </summary>
        /// <param name="names">The names.</param>
        /// <returns>
        ///   <c>true</c> if the specified name contains argument; otherwise, <c>false</c>.
        /// </returns>
        public new bool Contains(string names)
        {
            return IndexOf(names) > -1;
        }

        public bool ContainsAny(params string[] names)
        {
            return ContainsAny(names as IEnumerable<string>);
        }

        /// <summary>
        /// Determines whether the specified name contains argument.
        /// </summary>
        /// <param name="names">The names.</param>
        /// <returns>
        ///   <c>true</c> if the specified name contains argument; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsAny(IEnumerable<string> names)
        {
            return names.All(name => IndexOf(name) != -1);
        }

        /// <summary>
        /// Gets the index of the paramter with the given name
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public new int IndexOf(string name)
        {
            var result = -1;

            var switches = new List<string>(Switches);

            if (switches.Count == 0) switches.Add(string.Empty);

            foreach (var @switch in switches)
            {
                result = base.IndexOf(string.Concat(@switch, name));

                if (result > -1) break;
            }

            return result;
        }

        /// <summary>
        /// Gets the boolean value
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool AsBool(string name)
        {
            return AsBool(name, false);
        }

        /// <summary>
        /// Gets the boolean value
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="default">if set to <c>true</c> [@default].</param>
        /// <returns></returns>
        public bool AsBool(string name, bool @default)
        {
            bool result;

            var resultAsString = AsString(name, @default.ToString());

            if (!bool.TryParse(resultAsString, out result))
            {
                result = @default;
            }

            return result;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Parameters(this);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var parameter in this)
            {
                if (sb.Length > 0) sb.Append(" ");

                if (parameter.Contains(" "))
                {
                    sb.Append(@"""");
                }

                sb.Append(parameter);

                if (parameter.Contains(" "))
                {
                    sb.Append(@"""");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Removes the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        public new void Remove(string name)
        {
            var index = IndexOf(name);

            if (index <= -1) return;

            var length = AsStrings(name).Count + 1;

            RemoveRange(index, length);
        }

        /// <summary>
        /// Replaces the value of the parameter with the specified key.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="values">The values.</param>
        public void Replace(string name, params string[] values)
        {
            var index = IndexOf(name);

            if (index <= -1) return;

            if (Switches.Count > 0)
            {
                var length = AsStrings(name).Count;

                if (index + 1 + length <= Count) RemoveRange(index + 1, length);

                InsertRange(index + 1, values);
            }
            else
            {
                RemoveAt(index);

                InsertRange(index, values);
            }
        }

        /// <summary>
        /// Determines whether the specified name is a command flag.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if the specified name is flag; otherwise, <c>false</c>.
        /// </returns>
        public bool IsFlag(string name)
        {
            return Switches.Count == 0 || Switches.Any(name.StartsWith);
        }

        public T AsCustomType<T>(string name)
        {
            return (T)AsCustomType(name, typeof(T));
        }

        public object AsCustomType(string name, Type type)
        {
            object result = null;

            if (Contains(name))
            {
                var value = AsString(name);

                if (Nullable.GetUnderlyingType(type) != null)
                {
                    DateTime date;
                    if (DateTime.TryParse(value, out date))
                    {
                        result = new DateTime?(date);
                    }
                }
                else
                {
                    result = Convert.ChangeType(value, type);
                }
            }

            return result;
        }

        public object AsCustomType(int index, Type type)
        {
            object result = null;

            if (index < Count)
            {
                result = Convert.ChangeType(this[index], type);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified name has a value.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if the specified name has value; otherwise, <c>false</c>.
        /// </returns>
        public bool HasValue(string name)
        {
            return AsStrings(name).Count > 0;
        }
    }
}