using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Base.Collections;

namespace Alternet.UI
{
    public partial class Grid
    {
        /// <summary>
        /// Gets or sets count of <see cref="RowDefinitions"/>.
        /// </summary>
        /// <remarks>
        /// If row count grows, <see cref="RowDefinition.CreateAuto"/> is used to add new rows.
        /// </remarks>
        public int RowCount
        {
            get
            {
                return RowDefinitions.Count;
            }

            set
            {
                if (value < 0)
                    throw new ArgumentException(nameof(RowCount));
                ListUtils.SetCount(RowDefinitions, value, RowDefinition.CreateAuto);
            }
        }

        /// <summary>
        /// Gets or sets count of <see cref="ColumnDefinitions"/>.
        /// </summary>
        /// <remarks>
        /// If column count grows, <see cref="ColumnDefinition.CreateAuto"/> is used to add new rows.
        /// </remarks>
        public int ColumnCount
        {
            get
            {
                return ColumnDefinitions.Count;
            }

            set
            {
                if (value < 0)
                    throw new ArgumentException(nameof(ColumnCount));
                ListUtils.SetCount(ColumnDefinitions, value, ColumnDefinition.CreateAuto);
            }
        }

        /// <summary>
        /// Initializes <see cref="Grid"/> rows and columns for the specified controls.
        /// </summary>
        /// <param name="grid">Grid instance.</param>
        /// <param name="controls">Grid controls.</param>
        /// <remarks>
        /// Dimensions for all rows and columns are set to <see cref="GridLength.Auto"/>.
        /// <see cref="Control.SetRowColumn"/> is called for the each control with row and column
        /// indexes equal to position in the <paramref name="controls"/> array.
        /// </remarks>
        public void Setup(Control[,] controls)
        {
            var rowCount = controls.GetLength(0);
            var colCount = controls.GetLength(1);

            this.DoInsideLayout(() =>
            {
                for (int i = 0; i < rowCount; i++)
                    this.AddAutoRow();
                for (int i = 0; i < colCount; i++)
                    this.AddAutoColumn();

                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        var control = controls[i, j];
                        control.Parent ??= this;
                        Grid.SetRowColumn(control, i, j);
                    }
                }
            });
        }

        /// <summary>
        /// Adds <see cref="RowDefinition"/> instance with
        /// <see cref="RowDefinition.Height"/> equal to
        /// <see cref="GridLength.Auto"/>.
        /// </summary>
        public RowDefinition AddAutoRow()
        {
            var result = RowDefinition.CreateAuto();
            RowDefinitions.Add(result);
            return result;
        }

        /// <summary>
        /// Adds <see cref="ColumnDefinition"/> instance
        /// with <see cref="ColumnDefinition.Width"/>
        /// equal to <see cref="GridLength.Auto"/>.
        /// </summary>
        public ColumnDefinition AddAutoColumn()
        {
            var result = ColumnDefinition.CreateAuto();
            ColumnDefinitions.Add(result);
            return result;
        }

        /// <summary>
        /// Adds <see cref="RowDefinition"/> instance with <see cref="RowDefinition.Height"/>
        /// equal to <see cref="GridLength.Star"/>.
        /// </summary>
        public RowDefinition AddStarRow()
        {
            var result = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
            RowDefinitions.Add(result);
            return result;
        }

        /// <summary>
        /// Adds <see cref="ColumnDefinition"/> instance with <see cref="ColumnDefinition.Width"/>
        /// equal to <see cref="GridLength.Star"/>.
        /// </summary>
        public ColumnDefinition AddStarColumn()
        {
            var result = new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) };
            ColumnDefinitions.Add(result);
            return result;
        }
    }
}
