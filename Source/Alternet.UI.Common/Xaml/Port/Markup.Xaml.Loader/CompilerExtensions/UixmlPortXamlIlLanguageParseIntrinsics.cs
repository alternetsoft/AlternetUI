#pragma warning disable
#nullable disable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;

using Alternet.Drawing;
using Alternet.UI;
using Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.AstNodes;
using Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions.Transformers;
using XamlX.Ast;
using XamlX.Transform;
using XamlX.TypeSystem;

namespace Alternet.UI.Markup.Xaml.XamlIl.CompilerExtensions
{
    class UixmlPortXamlIlLanguageParseIntrinsic
    {
        public static bool TryConvert(AstTransformationContext context, IXamlAstValueNode node, string text, IXamlType type, UixmlPortXamlIlWellKnownTypes types, out IXamlAstValueNode result)
        {
            if (type.FullName == "System.TimeSpan")
            {
                var tsText = text.Trim();

                if (!TimeSpan.TryParse(tsText, CultureInfo.InvariantCulture, out var timeSpan))
                {
                    // // shorthand seconds format (ie. "0.25")
                    if (!tsText.Contains(":") && double.TryParse(tsText,
                        NumberStyles.Float | NumberStyles.AllowThousands,
                        CultureInfo.InvariantCulture, out var seconds))
                        timeSpan = TimeSpan.FromSeconds(seconds);
                    else
                        throw new XamlX.XamlLoadException($"Unable to parse {text} as a time span", node);
                }

                result = new XamlStaticOrTargetedReturnMethodCallNode(node,
                    type.FindMethod("FromTicks", type, false, types.Long),
                    new[] { new XamlConstantNode(node, types.Long, timeSpan.Ticks) });
                return true;
            }

            if (type.Equals(types.FontFamily))
            {
                result = new UixmlPortXamlIlFontFamilyAstNode(types, text, node);
                return true;
            }

            if (type.Equals(types.Thickness))
            {
                try
                {
                    var thickness = Thickness.Parse(text);
            
                    result = new UixmlPortXamlIlVectorLikeConstantAstNode(node, types, types.Thickness, types.ThicknessFullConstructor,
                        new[] { thickness.Left, thickness.Top, thickness.Right, thickness.Bottom });
                
                    return true;
                }
                catch
                {
                    throw new XamlX.XamlLoadException($"Unable to parse \"{text}\" as a thickness", node);
                }
            }

            if (type.Equals(types.Point))
            {
                try
                {
                    var point = PointD.Parse(text);
            
                    result = new UixmlPortXamlIlVectorLikeConstantAstNode(node, types, types.Point, types.PointFullConstructor,
                        new[] { point.X, point.Y });
                
                    return true;
                }
                catch
                {
                    throw new XamlX.XamlLoadException($"Unable to parse \"{text}\" as a point", node);
                }
            }
            
            /*if (type.Equals(types.Vector))
            {
                try
                {
                    var vector = Vector.Parse(text);
            
                    result = new UixmlPortXamlIlVectorLikeConstantAstNode(node, types, types.Vector, types.VectorFullConstructor,
                        new[] { vector.X, vector.Y });
                
                    return true;
                }
                catch
                {
                    throw new XamlX.XamlLoadException($"Unable to parse \"{text}\" as a vector", node);
                }
            }*/
            
            if (type.Equals(types.Size))
            {
                try
                {
                    var size = SizeD.Parse(text);
                
                    result = new UixmlPortXamlIlVectorLikeConstantAstNode(node, types, types.Size, types.SizeFullConstructor,
                        new[] { size.Width, size.Height });
                
                    return true;
                }
                catch
                {
                    throw new XamlX.XamlLoadException($"Unable to parse \"{text}\" as a size", node);
                }
            }

            /* 
            if (type.Equals(types.Matrix))
            {
                try
                {
                    var matrix = Matrix.Parse(text);
                    
                    result = new UixmlPortXamlIlVectorLikeConstantAstNode(node, types, types.Matrix, types.MatrixFullConstructor,
                        new[] { matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.M31, matrix.M32 });
                
                    return true;
                }
                catch
                {
                    throw new XamlX.XamlLoadException($"Unable to parse \"{text}\" as a matrix", node);
                }
            }
            
            if (type.Equals(types.CornerRadius))
            {
                try
                {
                    var cornerRadius = CornerRadius.Parse(text);
            
                    result = new UixmlPortXamlIlVectorLikeConstantAstNode(node, types, types.CornerRadius, types.CornerRadiusFullConstructor,
                        new[] { cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomRight, cornerRadius.BottomLeft });
                
                    return true;
                }
                catch
                {
                    throw new XamlX.XamlLoadException($"Unable to parse \"{text}\" as a corner radius", node);
                }
            }

            if (type.Equals(types.Color))
            {
                if (!Color.TryParse(text, out Color color))
                {
                    throw new XamlX.XamlLoadException($"Unable to parse \"{text}\" as a color", node);
                }

                result = new XamlStaticOrTargetedReturnMethodCallNode(node,
                    type.GetMethod(
                        new FindMethodMethodSignature("FromUInt32", type, types.UInt) { IsStatic = true }),
                    new[] { new XamlConstantNode(node, types.UInt, color.ToUint32()) });

                return true;
            }

            if (type.Equals(types.GridLength))
            {
                try
                {
                    var gridLength = GridLength.Parse(text);
                
                    result = new UixmlPortXamlIlGridLengthAstNode(node, types, gridLength);
                    
                    return true;
                }
                catch
                {
                    throw new XamlX.XamlLoadException($"Unable to parse \"{text}\" as a grid length", node);
                }
            }
            */

            if (type.Equals(types.Cursor))
            {
                if (TypeSystemHelpers.TryGetEnumValueNode(types.StandardCursorType, text, node, out var enumConstantNode))
                {
                    var cursorTypeRef = new XamlAstClrTypeReference(node, types.Cursor, false);

                    result = new XamlAstNewClrObjectNode(node, cursorTypeRef, types.CursorTypeConstructor, new List<IXamlAstValueNode> { enumConstantNode });
                    
                    return true;
                }
            }

            if (type.Equals(types.ColumnDefinitions))
            {
                return ConvertDefinitionList(node, text, types, types.ColumnDefinitions, types.ColumnDefinition, "column definitions", out result);
            }

            if (type.Equals(types.RowDefinitions))
            {
                return ConvertDefinitionList(node, text, types, types.RowDefinitions, types.RowDefinition, "row definitions", out result);
            }

            if (type.Equals(types.Classes))
            {
                var classes = text.Split(' ');
                var classNodes = classes.Select(c => new XamlAstTextNode(node, c, types.XamlIlTypes.String)).ToArray();

                result = new UixmlPortXamlIlUixmlPortListConstantAstNode(node, types, types.Classes, types.XamlIlTypes.String, classNodes);
                return true;
            }

            /* 
            if (types.IBrush.IsAssignableFrom(type))
            {
                if (Color.TryParse(text, out Color color))
                {
                    var brushTypeRef = new XamlAstClrTypeReference(node, types.ImmutableSolidColorBrush, false);

                    result = new XamlAstNewClrObjectNode(node, brushTypeRef,
                        types.ImmutableSolidColorBrushConstructorColor,
                        new List<IXamlAstValueNode> { new XamlConstantNode(node, types.UInt, color.ToUint32()) });

                    return true;
                }
            }
            */

            result = null;
            return false;
        }

        private static bool ConvertDefinitionList(
            IXamlAstValueNode node, 
            string text,
            UixmlPortXamlIlWellKnownTypes types,
            IXamlType listType,
            IXamlType elementType,
            string errorDisplayName,
            out IXamlAstValueNode result)
        {
            throw new Exception();
            //try
            //{
            //    var lengths = GridLength.ParseLengths(text);

            //    var definitionTypeRef = new XamlAstClrTypeReference(node, elementType, false);

            //    var definitionConstructorGridLength = elementType.GetConstructor(new List<IXamlType> {types.GridLength});

            //    IXamlAstValueNode CreateDefinitionNode(GridLength length)
            //    {
            //        var lengthNode = new UixmlPortXamlIlGridLengthAstNode(node, types, length);

            //        return new XamlAstNewClrObjectNode(node, definitionTypeRef,
            //            definitionConstructorGridLength, new List<IXamlAstValueNode> {lengthNode});
            //    }

            //    var definitionNodes =
            //        new List<IXamlAstValueNode>(lengths.Select(CreateDefinitionNode));

            //    result = new UixmlPortXamlIlUixmlPortListConstantAstNode(node, types, listType, elementType, definitionNodes);

            //    return true;
            //}
            //catch
            //{
            //    throw new XamlX.XamlLoadException($"Unable to parse \"{text}\" as a {errorDisplayName}", node);
            //}
        }
    }
}
