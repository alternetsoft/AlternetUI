using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class PropertyGrid
    {
        /// <summary>
        /// Adds simple action for the specified <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type for which action is registered.</typeparam>
        /// <param name="name">Action name.</param>
        /// <param name="action">Action.</param>
        /// <returns><see cref="IPropertyGridTypeRegistry"/> of the specified
        /// <typeparamref name="T"/> type so you can chain calls and perform other actions
        /// on it.</returns>
        public static IPropertyGridTypeRegistry AddSimpleAction<T>(string name, Action action)
        {
            var registry = PropertyGrid.GetTypeRegistry(typeof(T));
            registry.AddSimpleAction(name, action);
            return registry;
        }

        /// <summary>
        /// Gets list of simple actions or <c>null</c> if there are no actions.
        /// </summary>
        /// <param name="t">Type for which actions are requested.</param>
        /// <returns></returns>
        public static IEnumerable<(string, Action)>? GetSimpleActions(Type t)
        {
            var registry = PropertyGrid.GetTypeRegistryOrNull(t);
            return registry?.GetSimpleActions();
        }

        /// <summary>
        /// Clears current selection, if any.
        /// </summary>
        /// <param name="validation">If set to false, deselecting the property will
        /// always work, even if its editor had invalid value in it.</param>
        /// <returns>Returns true if successful or if there was no selection. May fail if validation
        /// was enabled and active editor had invalid value.</returns>
        public bool ClearSelection(bool validation) => NativeControl.ClearSelection(validation);

        /// <summary>
        /// Resets modified status of all properties.
        /// </summary>
        public void ClearModifiedStatus() => NativeControl.ClearModifiedStatus();

        /// <summary>
        /// Collapses all items that can be collapsed. This functions clears selection.
        /// </summary>
        public bool CollapseAll() => NativeControl.CollapseAll();

        /// <summary>
        /// Returns true if all property grid data changes have been committed.
        /// </summary>
        /// <returns>Usually only returns false if value in active editor has been invalidated
        /// by a validator.</returns>
        public bool EditorValidate() => NativeControl.EditorValidate();

        /// <summary>
        /// Expands all items that can be expanded. This functions clears selection.
        /// </summary>
        /// <param name="expand"></param>
        /// <returns></returns>
        public bool ExpandAll(bool expand) => NativeControl.ExpandAll(expand);

        /// <summary>
        /// Translates the logical coordinates to the device ones.
        /// </summary>
        /// <param name="point">Logical coordinates.</param>
        /// <returns></returns>
        /// <remarks>
        /// For example, if a control is scrolled 10 pixels to the bottom, the device
        /// coordinates of the origin are (0, 0) (as always), but the logical coordinates
        /// are (0, 10) and so the call to CalcScrolledPosition(0, 10) will return 0 in y.
        /// </remarks>
        /// <remarks>
        /// Coordinates are specified in pixels.
        /// </remarks>
        public PointI CalcScrolledPositionI(PointI point)
        {
            return NativeControl.CalcScrolledPosition(point);
        }

        /// <summary>
        /// Translates the logical coordinates to the device ones.
        /// </summary>
        /// <param name="point">Logical coordinates.</param>
        /// <returns></returns>
        /// <remarks>
        /// For example, if a control is scrolled 10 pixels to the bottom, the device
        /// coordinates of the origin are (0, 0) (as always), but the logical coordinates
        /// are (0, 10) and so the call to CalcScrolledPosition(0, 10) will return 0 in y.
        /// </remarks>
        /// <remarks>
        /// Coordinates are specified in dips.
        /// </remarks>
        public PointD CalcScrolledPositionD(PointD point)
        {
            var pointI = PixelFromDip(point);
            var result = NativeControl.CalcScrolledPosition(pointI);
            var pointD = PixelToDip(result);
            return pointD;
        }

        /// <summary>
        /// Translates the device coordinates to the logical ones.
        /// </summary>
        /// <param name="point">Device coordinates.</param>
        /// <returns></returns>
        /// <remarks>
        /// For example, if a control is scrolled 10 pixels to the bottom, the device
        /// coordinates of the origin are (0, 0) (as always), but the logical coordinates
        /// are (0, 10) and so the call to CalcUnscrolledPosition(0, 0) will return 10 in y.
        /// </remarks>
        /// <remarks>
        /// Coordinates are specified in pixels.
        /// </remarks>
        public PointI CalcUnscrolledPositionI(PointI point)
        {
            return NativeControl.CalcUnscrolledPosition(point);
        }

        /// <summary>
        /// Translates the device coordinates to the logical ones.
        /// </summary>
        /// <param name="point">Device coordinates.</param>
        /// <returns></returns>
        /// <remarks>
        /// For example, if a control is scrolled 10 pixels to the bottom, the device
        /// coordinates of the origin are (0, 0) (as always), but the logical coordinates
        /// are (0, 10) and so the call to CalcUnscrolledPosition(0, 0) will return 10 in y.
        /// </remarks>
        /// <remarks>
        /// Coordinates are specified in dips.
        /// </remarks>
        public PointD CalcUnscrolledPositionD(PointD point)
        {
            var pointI = PixelFromDip(point);
            var result = NativeControl.CalcUnscrolledPosition(pointI);
            var pointD = PixelToDip(result);
            return pointD;
        }

        /// <summary>
        /// Returns column information about arbitrary position in the grid.
        /// </summary>
        /// <param name="point">Coordinates in the control.</param>
        /// <returns></returns>
        /// <remarks>
        /// Coordinates in the virtual grid space. You may need to use
        /// <see cref="CalcScrolledPositionD"/> for translating <see cref="PropertyGrid"/>
        /// client coordinates into something this member function can use.
        /// </remarks>
        public int GetHitTestColumn(PointD point)
        {
            var pointI = PixelFromDip(point);
            var result = NativeControl.GetHitTestColumn(pointI);
            return result;
        }

        /// <summary>
        /// Returns property information about arbitrary position in the grid.
        /// </summary>
        /// <param name="point">Coordinates in the control.</param>
        /// <returns></returns>
        /// <remarks>
        /// Coordinates in the virtual grid space. You may need to use
        /// <see cref="CalcScrolledPositionD"/> for translating <see cref="PropertyGrid"/>
        /// client coordinates into something this member function can use.
        /// </remarks>
        public IPropertyGridItem? GetHitTestProp(PointD point)
        {
            var pointI = PixelFromDip(point);
            var ptr = NativeControl.GetHitTestProp(pointI);
            var item = PtrToItem(ptr);
            return item;
        }

        /// <summary>
        /// Gets whether property item can be reset.
        /// </summary>
        /// <param name="item">Property item.</param>
        /// <returns></returns>
        /// <remarks>
        /// Property can be reset if it is nullable, has <see cref="DefaultValueAttribute"/>
        /// specified or class has 'Reset(PropertyName)' method.
        /// </remarks>
        public bool CanResetProp(IPropertyGridItem? item)
        {
            if (item is null || item.PropInfo is null || item.Instance is null)
                return false;
            var nullable = AssemblyUtils.GetNullable(item.PropInfo);
            var value = item.PropInfo.GetValue(item.Instance);
            var resetMethod = AssemblyUtils.GetResetPropMethod(item.Instance, item.PropInfo.Name);
            var hasDevaultAttr = AssemblyUtils.GetDefaultValue(item.PropInfo, out _);
            return hasDevaultAttr || resetMethod != null || (nullable && value is not null);
        }

        /// <summary>
        /// Resets property item value.
        /// </summary>
        /// <param name="item">Property item.</param>
        /// <remarks>
        /// Property can be reset if it is nullable, has <see cref="DefaultValueAttribute"/>
        /// specified or class has 'Reset(PropertyName)' method.
        /// </remarks>
        public void ResetProp(IPropertyGridItem? item)
        {
            if (item is null || item.PropInfo is null || item.Instance is null)
                return;

            var resetMethod = AssemblyUtils.GetResetPropMethod(item.Instance, item.PropInfo.Name);
            if (resetMethod is not null)
            {
                resetMethod.Invoke(item.Instance, Array.Empty<object?>());
                ReloadPropertyValue(item);
                return;
            }

            var hasDevaultAttr = AssemblyUtils.GetDefaultValue(item.PropInfo, out var defValue);
            if (hasDevaultAttr)
            {
                item.PropInfo.SetValue(item.Instance, defValue);
                ReloadPropertyValue(item);
                return;
            }

            var nullable = AssemblyUtils.GetNullable(item.PropInfo);
            var value = item.PropInfo.GetValue(item.Instance);
            if (nullable && value is not null)
            {
                item.PropInfo.SetValue(item.Instance, null);
                ReloadPropertyValue(item);
                return;
            }
        }
    }
}