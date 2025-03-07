using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// Allows to parse command line arguments and get argument values as different types.
    /// Example of the supported command line arguments:
    /// -r=download Url="http://localhost/wxWidgets.7z" Path="e:/file.7z"
    /// </summary>
    /// <remarks>
    /// Argument value can be omitted. Html double quote special code
    /// can be used in the command line.
    /// </remarks>
    public class CommandLineArgs
    {
        private static CommandLineArgs? defaultInstance;

        private readonly Dictionary<string, string> args = new();

        /// <summary>
        /// Occurs when exception is raised.
        /// </summary>
        public event EventHandler<ThrowExceptionEventArgs>? Error;

        /// <summary>
        /// Gets or sets default <see cref="CommandLineArgs"/> instance.
        /// </summary>
        public static CommandLineArgs Default
        {
            get
            {
                return defaultInstance ??= new();
            }

            set
            {
                defaultInstance = value;
            }
        }

        /// <summary>
        /// Parses command line args of the application and gets "-IsDark"
        /// argument value.
        /// </summary>
        /// <returns></returns>
        public static bool ParseAndGetIsDark()
        {
            return ParseAndGetBool("-IsDark");
        }

        /// <summary>
        /// Parses command line args of the application and checks if the specified argument exists.
        /// </summary>
        /// <param name="prmName">Command line argument name. Example: -IsDark.</param>
        /// <returns>True if the argument exists, otherwise false.</returns>
        public static bool ParseAndHasArgument(string prmName)
        {
            Default.Parse();
            var result = Default.HasArgument(prmName);
            return result;
        }

        /// <summary>
        /// Parses command line args of the application and gets
        /// value of the argument specified by the name.
        /// </summary>
        /// <returns></returns>
        /// <param name="prmName">Command line argument name. Example: -IsDark.</param>
        public static bool ParseAndGetBool(string prmName)
        {
            Default.Parse();
            var result = Default.AsBool(prmName);
            return result;
        }

        /// <summary>
        /// Gets whether argument with the specified name exists in the command line.
        /// </summary>
        /// <param name="argName">Argument name.</param>
        /// <returns></returns>
        public virtual bool HasArgument(string argName)
        {
            if (args.TryGetValue(argName, out _))
                return true;
            return false;
        }

        /// <summary>
        /// Gets command line argument as string and splits it to the array of strings.
        /// Semicolon is used as the separator of the items.
        /// </summary>
        /// <param name="argName">Argument name.</param>
        /// <returns></returns>
        public virtual string[] AsSemicolonArray(string argName)
        {
            var value = AsString(argName);
            var result = value.Split(';');
            return result;
        }

        /// <summary>
        /// Gets command line argument as boolean.
        /// </summary>
        /// <param name="argName">Argument name.</param>
        /// <param name="defaultValue">Default value.
        /// Used if argument is not specified in the command line.</param>
        /// <returns></returns>
        public virtual bool AsBool(string argName, bool defaultValue = false)
        {
            try
            {
                if (args.TryGetValue(argName, out string? value))
                    return value.ToLower().Trim() == "true";
                return defaultValue;
            }
            catch (Exception e)
            {
                OnError(e);
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets command line argument as string.
        /// </summary>
        /// <param name="argName">Argument name.</param>
        /// <param name="defaultValue">Default value.
        /// Used if argument is not specified in the command line.</param>
        /// <returns></returns>
        public virtual string AsString(string argName, string? defaultValue = null)
        {
            defaultValue ??= string.Empty;

            try
            {
                if (args.TryGetValue(argName, out string? value))
                    return value;
                return defaultValue;
            }
            catch (Exception e)
            {
                OnError(e);
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets command line argument as <see cref="long"/>.
        /// </summary>
        /// <param name="argName">Argument name.</param>
        /// <param name="defaultValue">Default value.
        /// Used if argument is not specified in the command line.</param>
        /// <returns></returns>
        public virtual long AsLong(string argName, long defaultValue = default)
        {
            try
            {
                if (args.TryGetValue(argName, out string? value))
                    return Convert.ToInt64(value);
                return defaultValue;
            }
            catch (Exception e)
            {
                OnError(e);
                return defaultValue;
            }
        }

        /// <summary>
        /// Gets command line argument as <see cref="double"/>.
        /// </summary>
        /// <param name="argName">Argument name.</param>
        /// <param name="defaultValue">Default value.
        /// Used if argument is not specified in the command line.</param>
        /// <returns></returns>
        public virtual double AsDouble(string argName, double defaultValue = default)
        {
            try
            {
                if (args.TryGetValue(argName, out string? value))
                    return Convert.ToDouble(value);
                return defaultValue;
            }
            catch (Exception e)
            {
                OnError(e);
                return defaultValue;
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            string result = string.Empty;
            foreach (var s in args.Keys)
            {
                string v = args[s];
                result += "[" + s + "] = [" + v + "]; ";
            }

            return result;
        }

        /// <summary>
        /// Parses command line arguments from <see cref="Environment.GetCommandLineArgs"/>.
        /// </summary>
        public virtual void Parse(bool reset = true)
        {
            if (reset)
                Reset();
            var args = Environment.GetCommandLineArgs();
            Parse(args);
        }

        /// <summary>
        /// Resets parsed command line arguments as if no arguments were specified.
        /// </summary>
        public virtual void Reset()
        {
            args.Clear();
        }

        /// <summary>
        /// Parses command line arguments.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public virtual void Parse(string[] args)
        {
            void ParseArg(string arg)
            {
                string[] words = arg.Trim().Split('=');
                words[0] = ReplaceHtmlChars(words[0]);
                if (words.Length > 1)
                {
                    words[1] = ReplaceHtmlChars(words[1]).Trim('"');
                    this.args![words[0]] = words[1];
                }
                else
                {
                    this.args![words[0]] = string.Empty;
                }
            }

            foreach (string arg in args)
            {
                try
                {
                    ParseArg(arg);
                }
                catch (Exception e)
                {
                    OnError(e);
                }
            }
        }

        /// <summary>
        /// Parses command line defaults.
        /// </summary>
        /// <param name="defaultArgs">Command line default separated by semicolon.</param>
        public virtual void ParseDefaults(string defaultArgs)
        {
            try
            {
                if (string.IsNullOrEmpty(defaultArgs))
                    return;
                defaultArgs = ReplaceHtmlChars(defaultArgs);
                string[] args = defaultArgs.Split(';');
                Parse(args);
            }
            catch (Exception e)
            {
                OnError(e);
            }
        }

        /// <summary>
        /// Raised on error. Calls <see cref="Error"/> event.
        /// </summary>
        /// <param name="e">Exception.</param>
        protected virtual void OnError(Exception e)
        {
            if (Error is null)
                return;
            ThrowExceptionEventArgs args = new(e);
            Error(this, args);
            if (args.ThrowIt)
                throw e;
        }

        /// <summary>
        /// Replaces special html characters. By default only double quotes are replaced.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        protected virtual string ReplaceHtmlChars(string s)
        {
            return s.Trim().Replace("&quot;", "\"");
        }
    }
}
