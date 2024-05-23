using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI.Localization
{
    /// <summary>
    /// Defines localizations for property categories.
    /// </summary>
    public class PropCategoryStrings
    {
        private static AdvDictionary<string, Func<string, string>>? titles;

        /// <summary>
        /// Gets <see cref="IDictionary"/> which is used to get
        /// localized title for the property category.
        /// </summary>
        public static IDictionary<string, Func<string, string>>? Titles => titles ?? CreateTitles();

        /// <summary>
        /// Current localizations for control categories.
        /// </summary>
        public static PropCategoryStrings Default { get; set; } = new();

        /// <summary>
        /// Gets or sets property category localization.
        /// </summary>
        public string Other { get; set; } = "Other";

        /// <inheritdoc cref="Other"/>
        public string Action { get; set; } = "Action";

        /// <inheritdoc cref="Other"/>
        public string Appearance { get; set; } = "Appearance";

        /// <inheritdoc cref="Other"/>
        public string Asynchronous { get; set; } = "Asynchronous";

        /// <inheritdoc cref="Other"/>
        public string Behavior { get; set; } = "Behavior";

        /// <inheritdoc cref="Other"/>
        public string Data { get; set; } = "Data";

        /// <inheritdoc cref="Other"/>
        public string Design { get; set; } = "Design";

        /// <inheritdoc cref="Other"/>
        public string DragDrop { get; set; } = "DragDrop";

        /// <inheritdoc cref="Other"/>
        public string Focus { get; set; } = "Focus";

        /// <inheritdoc cref="Other"/>
        public string Format { get; set; } = "Format";

        /// <inheritdoc cref="Other"/>
        public string Key { get; set; } = "Key";

        /// <inheritdoc cref="Other"/>
        public string Layout { get; set; } = "Layout";

        /// <inheritdoc cref="Other"/>
        public string Mouse { get; set; } = "Mouse";

        /// <inheritdoc cref="Other"/>
        public string WindowStyle { get; set; } = "WindowStyle";

        /// <summary>
        /// Gets localized title for the specified category id.
        /// </summary>
        /// <param name="id">Category id.</param>
        public virtual string GetLocalizedTitle(string id)
        {
            CreateTitles();
            var fn = titles!.GetValueOrDefault(id, (s) => s);
            return fn(id);
        }

        private static IDictionary<string, Func<string, string>>? CreateTitles()
        {
            titles ??= new()
                {
                    { nameof(Other), (s) => Default.Other },
                    { nameof(Action), (s) => Default.Action },
                    { nameof(Appearance), (s) => Default.Appearance },
                    { nameof(Asynchronous), (s) => Default.Asynchronous },
                    { nameof(Behavior), (s) => Default.Behavior },
                    { nameof(Data), (s) => Default.Data },
                    { nameof(Design), (s) => Default.Design },
                    { nameof(DragDrop), (s) => Default.DragDrop },
                    { nameof(Focus), (s) => Default.Focus },
                    { nameof(Format), (s) => Default.Format },
                    { nameof(Key), (s) => Default.Key },
                    { nameof(Layout), (s) => Default.Layout },
                    { nameof(Mouse), (s) => Default.Mouse },
                    { nameof(WindowStyle), (s) => Default.WindowStyle },
                };

            return titles;
        }
    }
}