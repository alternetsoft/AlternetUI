using System;
using System.Diagnostics;
using System.Security;

namespace Alternet.UI
{
    public static class CommandHelpers
    {
        public static bool CanExecuteCommandSource(ICommandSource commandSource)
        {
            var command = commandSource.Command;
            if (command != null)
            {
                var parameter = commandSource.CommandParameter;
                return command.CanExecute(parameter);
            }

            return false;
        }

        public static void ExecuteCommandSource(ICommandSource commandSource)
        {
            var command = commandSource.Command;
            if (command != null)
            {
                var parameter = commandSource.CommandParameter;
                if (command.CanExecute(parameter))
                {
                    command.Execute(parameter);
                }
            }
        }
    }
}
