using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Alternet.Drawing;

namespace Alternet.UI
{
    public partial class PropertyGrid : Control
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
        public static IEnumerable<(string Title, Action Action)>? GetSimpleActions(Type t)
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
        public virtual bool ClearSelection(bool validation)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.ClearSelection(validation);
        }

        /// <summary>
        /// Resets modified status of all properties.
        /// </summary>
        public virtual void ClearModifiedStatus()
        {
            if (DisposingOrDisposed)
                return;
            Handler.ClearModifiedStatus();
        }

        /// <summary>
        /// Collapses all items that can be collapsed. This functions clears selection.
        /// </summary>
        public virtual bool CollapseAll()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.CollapseAll();
        }

        /// <summary>
        /// Returns true if all property grid data changes have been committed.
        /// </summary>
        /// <returns>Usually only returns false if value in active editor has been invalidated
        /// by a validator.</returns>
        public virtual bool EditorValidate()
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.EditorValidate();
        }

        /// <summary>
        /// Expands all items that can be expanded. This functions clears selection.
        /// </summary>
        /// <param name="expand"></param>
        /// <returns></returns>
        public virtual bool ExpandAll(bool expand)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.ExpandAll(expand);
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
        /// Coordinates are specified in pixels.
        /// </remarks>
        public virtual PointI CalcScrolledPositionI(PointI point)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.CalcScrolledPosition(point);
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
        public virtual PointD CalcScrolledPositionD(PointD point)
        {
            if (DisposingOrDisposed)
                return default;
            var pointI = PixelFromDip(point);
            var result = Handler.CalcScrolledPosition(pointI);
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
        public virtual PointI CalcUnscrolledPositionI(PointI point)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.CalcUnscrolledPosition(point);
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
        public virtual PointD CalcUnscrolledPositionD(PointD point)
        {
            if (DisposingOrDisposed)
                return default;
            var pointI = PixelFromDip(point);
            var result = Handler.CalcUnscrolledPosition(pointI);
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
        public virtual int GetHitTestColumn(PointD point)
        {
            if (DisposingOrDisposed)
                return default;
            var pointI = PixelFromDip(point);
            var result = Handler.GetHitTestColumn(pointI);
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
        public virtual IPropertyGridItem? GetHitTestProp(PointD point)
        {
            if (DisposingOrDisposed)
                return default;
            return Handler.GetHitTestProp(point);
        }

        /// <summary>
        /// Gets whether property item can be reset.
        /// </summary>
        /// <param name="item">Property item.</param>
        /// <returns></returns>
        /// <remarks>
        /// Uses <see cref="AssemblyUtils.CanResetProp"/>.
        /// </remarks>
        public virtual bool CanResetProp(IPropertyGridItem? item)
        {
            var result = AssemblyUtils.CanResetProp(item?.Instance, item?.PropInfo);
            return result;
        }

        /// <summary>
        /// Resets property item value.
        /// </summary>
        /// <param name="item">Property item.</param>
        /// <remarks>
        /// Uses <see cref="AssemblyUtils.ResetProperty"/>.
        /// </remarks>
        public virtual void ResetProp(IPropertyGridItem? item)
        {
            if(AssemblyUtils.ResetProperty(item?.Instance, item?.PropInfo))
                ReloadPropertyValue(item);
        }
    }
}