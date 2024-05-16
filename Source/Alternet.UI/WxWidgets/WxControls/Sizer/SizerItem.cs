using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    internal class SizerItem : DisposableObject<IntPtr>, ISizerItem
    {
        public SizerItem(
            Control window,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0)
            : this(
                  Native.SizerItem.CreateSizerItem(
                    WxPlatform.WxWidget(window),
                    proportion,
                    (int)flag,
                    border,
                    default), true)
        {
        }

        public SizerItem(Control window, ISizerFlags sizerFlags)
            : this(
                  Native.SizerItem.CreateSizerItem2(
                WxPlatform.WxWidget(window),
                ((SizerFlags)sizerFlags).Handle),
                  true)
        {
        }

        public SizerItem(
            ISizer sizer,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0)
            : this(
                  Native.SizerItem.CreateSizerItem3(
                      ((Sizer)sizer).Handle,
                      proportion,
                      (int)flag,
                      border,
                      default), true)
        {
        }

        public SizerItem(ISizer sizer, ISizerFlags sizerFlags)
            : this(Native.SizerItem.CreateSizerItem4(((Sizer)sizer).Handle, ((SizerFlags)sizerFlags).Handle), true)
        {
        }

        public SizerItem(
            int width,
            int height,
            int proportion = 0,
            SizerFlag flag = 0,
            int border = 0)
            : this(
                  Native.SizerItem.CreateSizerItem5(
                    width,
                    height,
                    proportion,
                    (int)flag,
                    border,
                    default), true)
        {
        }

        public SizerItem(int width, int height, ISizerFlags sizerFlags)
            : this(Native.SizerItem.CreateSizerItem6(width, height, ((SizerFlags)sizerFlags).Handle), true)
        {
        }

        public SizerItem()
            : this(Native.SizerItem.CreateSizerItem7(), true)
        {
        }

        public SizerItem(IntPtr handle, bool disposeHandle)
            : base(handle, disposeHandle)
        {
        }

        public bool IsShown
        {
            get
            {
                return Native.SizerItem.IsShown(Handle);
            }
        }

        public SizeI Size
        {
            get
            {
                return Native.SizerItem.GetSize(Handle);
            }
        }

        public SizeI MinSizeWithBorder
        {
            get
            {
                return Native.SizerItem.GetMinSizeWithBorder(Handle);
            }
        }

        public SizeI MaxSize
        {
            get
            {
                return Native.SizerItem.GetMaxSize(Handle);
            }
        }

        public SizeI MaxSizeWithBorder
        {
            get
            {
                return Native.SizerItem.GetMaxSizeWithBorder(Handle);
            }
        }

        public SizeI MinSize
        {
            get
            {
                return Native.SizerItem.GetMinSize(Handle);
            }

            set
            {
                Native.SizerItem.SetMinSize(Handle, value.Width, value.Height);
            }
        }

        public int Id
        {
            get
            {
                return Native.SizerItem.GetId(Handle);
            }

            set
            {
                Native.SizerItem.SetId(Handle, value);
            }
        }

        public bool IsControl
        {
            get
            {
                return Native.SizerItem.IsWindow(Handle);
            }
        }

        public bool IsSizer
        {
            get
            {
                return Native.SizerItem.IsSizer(Handle);
            }
        }

        public bool IsSpacer
        {
            get
            {
                return Native.SizerItem.IsSpacer(Handle);
            }
        }

        public int Proportion
        {
            get
            {
                return Native.SizerItem.GetProportion(Handle);
            }

            set
            {
                Native.SizerItem.SetProportion(Handle, value);
            }
        }

        public SizerFlag Flag
        {
            get
            {
                return (SizerFlag)Native.SizerItem.GetFlag(Handle);
            }

            set
            {
                Native.SizerItem.SetFlag(Handle, (int)value);
            }
        }

        public int Border
        {
            get
            {
                return Native.SizerItem.GetBorder(Handle);
            }

            set
            {
                Native.SizerItem.SetBorder(Handle, value);
            }
        }

        public float Ratio
        {
            get
            {
                return Native.SizerItem.GetRatio(Handle);
            }

            set
            {
                Native.SizerItem.SetRatio2(Handle, value);
            }
        }

        /// <summary>
        /// Gets or sets the userData item attribute.
        /// </summary>
        internal IntPtr UserData
        {
            get
            {
                return Native.SizerItem.GetUserData(Handle);
            }

            set
            {
                Native.SizerItem.SetUserData(Handle, value);
            }
        }

        public void DetachSizer()
        {
            Native.SizerItem.DetachSizer(Handle);
        }

        public void DetachControl()
        {
            Native.SizerItem.DetachWindow(Handle);
        }

        public SizeI CalcMin()
        {
            return Native.SizerItem.CalcMin(Handle);
        }

        public void SetDimension(PointI pos, SizeI size)
        {
            Native.SizerItem.SetDimension(Handle, pos, size);
        }

        public void SetInitSize(int w, int h)
        {
            Native.SizerItem.SetInitSize(Handle, w, h);
        }

        public void SetRatio(int width, int height)
        {
            Native.SizerItem.SetRatio(Handle, width, height);
        }

        public RectI GetRect()
        {
            return Native.SizerItem.GetRect(Handle);
        }

        public ISizer GetSizer()
        {
            var result = Native.SizerItem.GetSizer(Handle);
            return new Sizer(result, false);
        }

        public SizeI GetSpacer()
        {
            return Native.SizerItem.GetSpacer(Handle);
        }

        public void Show(bool show = true)
        {
            Native.SizerItem.Show(Handle, show);
        }

        public PointI GetPosition()
        {
            return Native.SizerItem.GetPosition(Handle);
        }

        public void SetControl(Control control)
        {
            Native.SizerItem.AssignWindow(Handle, WxPlatform.WxWidget(control));
        }

        public void SetSizer(ISizer sizer)
        {
            if(sizer is null)
                Native.SizerItem.AssignSizer(Handle, default);
            else
                Native.SizerItem.AssignSizer(Handle, ((Sizer)sizer).Handle);
        }

        public void SetSpacer(int w, int h)
        {
            Native.SizerItem.AssignSpacer(Handle, w, h);
        }

        /// <summary>
        /// Destroys the control or the controls in a subsizer, depending on the type of item.
        /// </summary>
        internal void DeleteControls()
        {
            Native.SizerItem.DeleteWindows(Handle);
        }

        /// <summary>
        /// If this item is tracking a control then return it; <c>null</c> otherwise.
        /// </summary>
        internal IntPtr GetControl()
        {
            return Native.SizerItem.GetWindow(Handle);
        }

        // Called once the first component of an item has been decided. This is
        // used in algorithms that depend on knowing the size in one direction
        // before the min size in the other direction can be known.
        // Returns true if it made use of the information (and min size was changed).
        internal bool InformFirstDirection(
            int direction,
            int size,
            int availableOtherDir = -1)
        {
            return Native.SizerItem.InformFirstDirection(Handle, direction, size, availableOtherDir);
        }
    }
}