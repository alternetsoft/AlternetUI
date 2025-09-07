#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using Alternet.Drawing;

namespace Alternet.Scripter.Debugger.UI.AlternetUI.Evaluation.CodeCompletion
{
    public interface IParameterInfoToolTip
    {
        bool Visible { get; }

        SizeD GetPreferredSize();

        void ShowPopup(ParameterInfoSymbol data, PointD position);

        void Close();
    }
}