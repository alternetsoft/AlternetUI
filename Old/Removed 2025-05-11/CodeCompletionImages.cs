#region Copyright (c) 2016-2025 Alternet Software

/*
    AlterNET Scripter Library

    Copyright (c) 2016-2025 Alternet Software
    ALL RIGHTS RESERVED

    http://www.alternetsoft.com
    contact@alternetsoft.com
*/

#endregion Copyright (c) 2016-2025 Alternet Software

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Alternet.Common;
using Alternet.Scripter.Debugger.ExpressionEvaluation.CodeCompletion;
using Alternet.UI;

namespace Alternet.Scripter.Debugger.UI.AlternetUI.Evaluation.CodeCompletion
{
    /// <summary>
    /// Provides methods to manage a collection of System.Drawing.Image objects for code completion controller.
    /// </summary>
    //public class CodeCompletionImages
    //{
    //    private Dictionary<SymbolKind, int> imageListIndices;

    //    private ImageList scaledImageList;
    //    private AlphaImageList scaledAlphaImageList;

    //    /// <summary>
    //    /// Gets output images, with scaling applied, if required.
    //    /// </summary>
    //    /// <returns>Scaled image list.</returns>
    //    public virtual ImageList GetScaledImageList()
    //    {
    //        if (scaledImageList == null)
    //        {
    //            scaledImageList = new DisplayScaledImages(
    //                () => LoadImageList(string.Empty, 16),
    //                () => LoadImageList("_HighDpi", 32)).Images;
    //        }

    //        return scaledImageList;
    //    }

    //    /// <summary>
    //    /// Gets output images, with scaling applied, if required.
    //    /// </summary>
    //    /// <returns>Scaled alpha image list.</returns>
    //    public virtual AlphaImageList GetScaledAlphaImageList()
    //    {
    //        if (scaledAlphaImageList == null)
    //        {
    //            scaledAlphaImageList = new DisplayScaledAlphaImages(
    //                () => LoadAlphaImageList(string.Empty, 16),
    //                () => LoadAlphaImageList("_HighDpi", 32)).Images;
    //        }

    //        return scaledAlphaImageList;
    //    }

    //    /// <summary>
    //    /// Gets number of image corresponding to specified <c>SymbolKind</c>.
    //    /// </summary>
    //    /// <param name="kind">Kind of symbol to find image index.</param>
    //    /// <returns>Index inside image collection for specified <c>SymbolKind</c>.</returns>
    //    public virtual int GetImageListIndex(SymbolKind kind)
    //    {
    //        if (imageListIndices == null)
    //        {
    //            imageListIndices = new Dictionary<SymbolKind, int>();
    //            var values = Enum.GetValues(typeof(SymbolKind)).Cast<SymbolKind>().ToArray();
    //            for (int i = 0; i < values.Length; i++)
    //                imageListIndices.Add(values[i], i);
    //        }

    //        return imageListIndices[kind];
    //    }

    //    private ImageList LoadImageList(string suffix, int size)
    //    {
    //        var imageList = new ImageList();
    //        imageList.ImageSize = new Size(size, size);

    //        var assembly = typeof(CodeCompletionPopup).Assembly;

    //        foreach (var name in Enum.GetNames(typeof(SymbolKind)))
    //        {
    //            const string ResourceNameFormat = "Alternet.Scripter.Debugger.UI.Resources.CodeComletionSymbols.{0}{1}.png";
    //            var resourceName = string.Format(ResourceNameFormat, name, suffix);

    //            var image = Image.FromStream(assembly.GetManifestResourceStream(resourceName));
    //            imageList.Images.Add(image);
    //        }

    //        return imageList;
    //    }

    //    private AlphaImageList LoadAlphaImageList(string suffix, int size)
    //    {
    //        IList<Image> images = new List<Image>();

    //        var assembly = typeof(CodeCompletionPopup).Assembly;

    //        foreach (var name in Enum.GetNames(typeof(SymbolKind)))
    //        {
    //            const string ResourceNameFormat = "Alternet.Scripter.Debugger.UI.Resources.CodeComletionSymbols.{0}Alpha{1}.png";
    //            var resourceName = string.Format(ResourceNameFormat, name, suffix);

    //            var image = Image.FromStream(assembly.GetManifestResourceStream(resourceName));
    //            images.Add(image);
    //        }

    //        return new AlphaImageList(images);
    //    }
    //}
}