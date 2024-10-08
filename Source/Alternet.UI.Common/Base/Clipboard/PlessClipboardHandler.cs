﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Implements internal <see cref="IClipboardHandler"/> which is not connected to the
    /// operating system clipboard.
    /// </summary>
    public class PlessClipboardHandler : DisposableObject, IClipboardHandler
    {
        private static IDataObject? data;

        /// <inheritdoc/>
        public bool AsyncRequired => false;

        /// <inheritdoc/>
        public bool OnlyText => false;

        /// <inheritdoc/>
        public virtual IDataObject? GetData()
        {
            return data;
        }

        /// <inheritdoc/>
        public Task<IDataObject?> GetDataAsync()
        {
            var result = GetData();
            return Task.FromResult(result);
        }

        /// <inheritdoc/>
        public void GetDataAsync(Action<IDataObject?> action)
        {
            var result = GetData();
            action(result);
        }

        /// <inheritdoc/>
        public virtual void SetData(IDataObject? value)
        {
            data = value;
        }

        /// <inheritdoc/>
        public virtual Task SetDataAsync(IDataObject? value)
        {
            SetData(value);
            return Task.CompletedTask;
        }
    }
}
