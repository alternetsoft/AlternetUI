using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alternet.UI
{
    /// <summary>
    /// Default implementation of the <see cref="ISizerFactory"/> interface.
    /// </summary>
    internal class SizerFactory : ISizerFactory
    {
        private readonly bool disposeHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="SizerFactory"/> class.
        /// </summary>
        /// <param name="disposeHandle">Whether to dispose created sizers automatically.</param>
        public SizerFactory(bool disposeHandle = false)
        {
            this.disposeHandle = disposeHandle;
        }

        /// <summary>
        /// Gets or sets default <see cref="ISizerFactory"/> provider.
        /// </summary>
        public static ISizerFactory Default { get; set; } = new SizerFactory(false);

        /// <summary>
        /// Gets the sizer of which this control is a member, if any, otherwise <c>null</c>.
        /// </summary>
        /// <returns></returns>
        public static ISizer? ControlGetContainingSizer(IControl control)
        {
            var sizer = ((UI.Native.Control)control.NativeControl).GetContainingSizer();

            if (sizer == IntPtr.Zero)
                return null;

            return new Sizer(sizer, false);
        }

        /// <summary>
        /// This method calls SetSizer() and then updates the initial control size to the
        /// size needed to accommodate all sizer elements and sets the size hints which,
        /// if this control is a top level one, prevent the user from resizing it to be
        /// less than this minimal size.
        /// </summary>
        /// <param name="control">Affected control.</param>
        /// <param name="sizer">The sizer to set. Pass <c>null</c> to disassociate
        /// and conditionally delete the control's sizer.</param>
        /// <param name="deleteOld">If <c>true</c> (the default), this will delete any
        /// pre-existing sizer. Pass <c>false</c> if you wish to handle deleting
        /// the old sizer yourself but remember to do it yourself in this case
        /// to avoid memory leaks.</param>
        public static void SetSizerAndFit(IControl control, ISizer? sizer, bool deleteOld = false)
        {
            var nativeControl = (UI.Native.Control)control.NativeControl;

            if (nativeControl is null)
                return;

            if (sizer is null)
                nativeControl.SetSizerAndFit(IntPtr.Zero, deleteOld);
            else
                nativeControl.SetSizerAndFit(((Sizer)sizer).Handle, deleteOld);
        }

        /// <summary>
        /// Gets the sizer associated with the control by a previous call to <see cref="ControlSetSizer"/>,
        /// or <c>null</c>.
        /// </summary>
        public static ISizer? ControlGetSizer(IControl control)
        {
            var nativeControl = (UI.Native.Control)control.NativeControl;

            if (nativeControl is null)
                return null;

            var sizer = nativeControl.GetSizer();

            if (sizer == IntPtr.Zero)
                return null;

            return new Sizer(sizer, false);
        }

        /// <summary>
        /// Sets the control to have the given layout sizer.
        /// </summary>
        /// <param name="sizer">The sizer to set. Pass <c>null</c> to disassociate
        /// and conditionally delete the control's sizer.</param>
        /// <param name="deleteOld">If <c>true</c> (the default), this will delete any
        /// pre-existing sizer. Pass <c>false</c> if you wish to handle deleting
        /// the old sizer yourself but remember to do it yourself in this case
        /// to avoid memory leaks.</param>
        /// <remarks>
        /// The control will then own the object, and will take care of its deletion.
        /// If an existing layout constraints object is already owned by the control,
        /// it will be deleted if the <paramref name="deleteOld"/> parameter is <c>true</c>.
        /// </remarks>
        /// <remarks>
        /// This function will also update layout so that the sizer will be effectively
        /// used to layout the control children whenever it is resized.
        /// </remarks>
        /// <remarks>
        /// This function enables and disables Layout automatically.
        /// </remarks>
        /// <param name="control">Affected control.</param>
        public static void ControlSetSizer(IControl control, ISizer? sizer, bool deleteOld = true)
        {
            var nativeControl = (UI.Native.Control)control.NativeControl;

            if (nativeControl is null)
                return;

            if (sizer is null)
                nativeControl.SetSizer(IntPtr.Zero, deleteOld);
            else
                nativeControl.SetSizer(((Sizer)sizer).Handle, deleteOld);
        }

        /// <inheritdoc cref="ISizerFactory.CreateSizerFlags"/>
        public virtual ISizerFlags CreateSizerFlags(int proportion = 0)
        {
            return new SizerFlags(proportion);
        }

        /// <inheritdoc cref="ISizerFactory.CreateFlexGridSizer(int, int, int)"/>
        public virtual IFlexGridSizer CreateFlexGridSizer(int cols, int vgap, int hgap)
        {
            return new FlexGridSizer(cols, vgap, hgap, disposeHandle);
        }

        /// <inheritdoc cref="ISizerFactory.CreateFlexGridSizer(int, int, int, int)"/>
        public virtual IFlexGridSizer CreateFlexGridSizer(int rows, int cols, int vgap, int hgap)
        {
            return new FlexGridSizer(rows, cols, vgap, hgap, disposeHandle);
        }

        /// <inheritdoc cref="ISizerFactory.CreateGridBagSizer"/>
        public virtual IGridBagSizer CreateGridBagSizer(int vgap = 0, int hgap = 0)
        {
            return new GridBagSizer(vgap, hgap, disposeHandle);
        }

        /// <inheritdoc cref="ISizerFactory.CreateBoxSizer"/>
        public virtual IBoxSizer CreateBoxSizer(bool isVertical)
        {
            return new BoxSizer(isVertical, disposeHandle);
        }

        /// <inheritdoc cref="ISizerFactory.CreateGridSizer(int, int, int)"/>
        public virtual IGridSizer CreateGridSizer(int cols, int vgap, int hgap)
        {
            return new GridSizer(cols, vgap, hgap, disposeHandle);
        }

        /// <inheritdoc cref="ISizerFactory.CreateGridSizer(int, int, int, int)"/>
        public virtual IGridSizer CreateGridSizer(int rows, int cols, int vgap, int hgap)
        {
            return new GridSizer(rows, cols, vgap, hgap, disposeHandle);
        }

        /// <inheritdoc cref="ISizerFactory.CreateWrapSizer"/>
        public virtual IWrapSizer CreateWrapSizer(
            bool isVertical,
            WrapSizerFlag flags = WrapSizerFlag.Default)
        {
            return new WrapSizer(isVertical, flags, disposeHandle);
        }

        /// <inheritdoc cref="ISizerFactory.CreateSizerItem(Control, int, SizerFlag, int)"/>
        public virtual ISizerItem CreateSizerItem(
            Control window,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0)
        {
            return new SizerItem(window, proportion, flag, border);
        }

        /// <inheritdoc cref="ISizerFactory.CreateSizerItem(Control, ISizerFlags)"/>
        public virtual ISizerItem CreateSizerItem(Control window, ISizerFlags sizerFlags)
        {
            return new SizerItem(window, sizerFlags);
        }

        /// <inheritdoc cref="ISizerFactory.CreateSizerItem(ISizer, int, SizerFlag, int)"/>
        public virtual ISizerItem CreateSizerItem(
            ISizer sizer,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0)
        {
            return new SizerItem(sizer, proportion, flag, border);
        }

        /// <inheritdoc cref="ISizerFactory.CreateSizerItem(ISizer, ISizerFlags)"/>
        public virtual ISizerItem CreateSizerItem(ISizer sizer, ISizerFlags sizerFlags)
        {
            return new SizerItem(sizer, sizerFlags);
        }

        /// <inheritdoc cref="ISizerFactory.CreateSizerItem(int, int, int, SizerFlag, int)"/>
        public virtual ISizerItem CreateSizerItem(
            int width,
            int height,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0)
        {
            return new SizerItem(width, height, proportion, flag, border);
        }

        /// <inheritdoc cref="ISizerFactory.CreateSizerItem(int, int, ISizerFlags)"/>
        public virtual ISizerItem CreateSizerItem(int width, int height, ISizerFlags sizerFlags)
        {
            return new SizerItem(width, height, sizerFlags);
        }

        /// <inheritdoc cref="ISizerFactory.CreateSizerItem()"/>
        public virtual ISizerItem CreateSizerItem()
        {
            return new SizerItem();
        }
    }
}