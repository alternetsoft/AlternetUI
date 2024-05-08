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
