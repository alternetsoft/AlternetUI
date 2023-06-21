using System;
using System.Collections.Generic;
using System.Text;

namespace Alternet.UI
{
    public class CommandLineArgs
    {
        public static CommandLineArgs Default
        {
            get
            {
                return m_instance;
            }
        }

        public string ArgAsString(string argName)
        {
            if (m_args.ContainsKey(argName))
            {
                return m_args[argName];
            }
            else return "";
        }

        public long ArgAsLong(string argName)
        {
            if (m_args.ContainsKey(argName))
            {
                return Convert.ToInt64(m_args[argName]);
            }
            else return 0;
        }

        public double ArgAsDouble(string argName)
        {
            if (m_args.ContainsKey(argName))
            {
                return Convert.ToDouble(m_args[argName]);
            }
            else return 0;
        }

        public override string ToString()
        {
            string result = string.Empty;
            foreach(var s in m_args.Keys)
            {
                string v = m_args[s];
                result += "["+ s + "] = ["+ v+"]; ";
            }
            return result;
        }

        private string ReplaceQuotes(string s)
        {
            return s.Trim().Replace("&quot;", "\"");
        }

        public void ParseArgs(string[] args)
        {
            void parseArg(string arg)
            {
                string[] words = arg.Trim().Split('=');
                words[0] = ReplaceQuotes(words[0]);
                if (words.Length > 1)
                {
                    words[1] = ReplaceQuotes(words[1]).Trim('"');
                    m_args![words[0]] = words[1];
                }
                else
                {
                    m_args![words[0]] = string.Empty;
                }
            }

            foreach (string arg in args)
            {
                parseArg(arg);
            }
        }

        public void ParseDefaults(string defaultArgs)
        {
            if (string.IsNullOrEmpty(defaultArgs)) return;
            defaultArgs = ReplaceQuotes(defaultArgs);
            string[] args = defaultArgs.Split(';');
            ParseArgs(args);
        }

        private Dictionary<string, string> m_args = new Dictionary<string, string>();
        static readonly CommandLineArgs m_instance = new CommandLineArgs();
    }
}
