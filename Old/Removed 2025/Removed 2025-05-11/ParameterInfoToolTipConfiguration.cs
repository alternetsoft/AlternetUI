#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using Alternet.UI;
//using Alternet.Scripter.Debugger.ExpressionEvaluation.CodeCompletion;

namespace Alternet.Scripter.Debugger.UI.AlternetUI.Evaluation.CodeCompletion
{
    public class ParameterInfoToolTipConfiguration
    {
        public ParameterInfoToolTipConfiguration(
            Control ownerControl,
            IToolTipProvider provider,
            Action<int> changeCurrentSymbolAction)
        {
            OwnerControl = ownerControl;
            ChangeCurrentSymbolAction = changeCurrentSymbolAction;
            Provider = provider;
        }


        public IToolTipProvider Provider { get; private set; }

        public Control OwnerControl { get; private set; }

        public Action<int> ChangeCurrentSymbolAction { get; private set; }
    }
}