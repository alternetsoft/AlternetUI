using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Alternet.UI.Integration.VisualStudio.Models;
using Alternet.UI.Integration.VisualStudio.Utils;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Text;
using Serilog;
using CompletionEngine = Alternet.UI.Integration.IntelliSense.CompletionEngine;

namespace Alternet.UI.Integration.VisualStudio.IntelliSense
{
    internal class XamlCompletionSource : ICompletionSource
    {
        private readonly DocumentOperations _documentOperations;
        private readonly ITextBuffer _buffer;
        private readonly IVsImageService2 _imageService;
        private readonly CompletionEngine _engine;

        public XamlCompletionSource(IServiceProvider serviceProvider, ITextBuffer textBuffer, IVsImageService2 imageService)
        {
            _documentOperations = new DocumentOperations(serviceProvider);
            _buffer = textBuffer;
            _imageService = imageService;
            _engine = new CompletionEngine();
        }

        (string CodeBehindFullText, Integration.IntelliSense.Language CodeBehindLanguage) TryGetCodeBehindText(string uixmlFileName)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            string codeBehindFullText = null;
            Integration.IntelliSense.Language codeBehindLanguage = null;
            var codeBehindFileName = Integration.IntelliSense.CodeBehindFileLocator.TryFindCodeBehindFile(uixmlFileName);
            if (codeBehindFileName != null)
            {
                codeBehindLanguage = Integration.IntelliSense.LanguageDetector.DetectLanguageFromFileName(codeBehindFileName);
                codeBehindFullText = FileContentProvider.GetFileText(_documentOperations, codeBehindFileName);
            }

            return (codeBehindFullText, codeBehindLanguage);
        }

        public void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // HACK: in some cases on VS2019 double code completion list are displayed. See https://github.com/AvaloniaUI/AvaloniaVS/issues/85.
            if (XamlCompletionCommandHandler.SkipCompletion)
                return;

            if (_buffer.Properties.TryGetProperty<XamlBufferMetadata>(typeof(XamlBufferMetadata), out var metadata) &&
                metadata.CompletionMetadata != null)
            {
                var sw = Stopwatch.StartNew();
                var pos = session.TextView.Caret.Position.BufferPosition;
                var text = pos.Snapshot.GetText();
                _buffer.Properties.TryGetProperty("AssemblyName", out string assemblyName);
                var uixmlFilePath = TextBufferHelper.GetTextBufferFilePath(_buffer);
                var codeBehindText = TryGetCodeBehindText(uixmlFilePath);

                var completions = _engine.GetCompletions(
                    metadata.CompletionMetadata,
                    text,
                    pos,
                    assemblyName,
                    codeBehindText.CodeBehindFullText,
                    codeBehindText.CodeBehindLanguage);

                if (completions?.Completions.Count > 0)
                {
                    var start = completions.StartPosition;
                    var span = new SnapshotSpan(pos.Snapshot, start, pos.Position - start);
                    var applicableTo = pos.Snapshot.CreateTrackingSpan(span, SpanTrackingMode.EdgeInclusive);

                    completionSets.Insert(0, new CompletionSet(
                        "AlternetUI",
                        "AlterNET UI",
                        applicableTo,
                        XamlCompletion.Create(completions.Completions, _imageService),
                        null));

                    string completionHint = completions.Completions.Count == 0 ?
                        "no completions found" :
                        $"{completions.Completions.Count} completions found (First:{completions.Completions.FirstOrDefault()?.DisplayText})";

                    Log.Logger.Verbose("XAML completion took {Time}, {CompletionHint}", sw.Elapsed, completionHint);
                }

                sw.Stop();
            }
        }

        public void Dispose()
        {
        }
    }
}
