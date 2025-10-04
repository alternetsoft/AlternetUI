using System;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Alternet.UI
{
    /// <summary>
    /// SIMD support logger.
    /// It uses direct probes for Vector<T> and reflection for other runtime types.
    /// </summary>
    internal static class SimdSupportLogger
    {
        /// <summary>
        /// Returns a human-readable multi-line report that describes SIMD availability.
        /// </summary>
        public static string GetReport()
        {
            var sb = new StringBuilder();
            sb.AppendLine("SIMD / Runtime capability report");
            sb.AppendLine("--------------------------------");
            sb.AppendLine($"Timestamp: {DateTime.UtcNow:O} (UTC)");
            try
            {
                sb.AppendLine($"OS: {RuntimeInformation.OSDescription}");
            }
            catch
            {
                sb.AppendLine("OS: (unknown)");
            }

            try
            {
                sb.AppendLine($"ProcessArchitecture: {RuntimeInformation.ProcessArchitecture}");
                sb.AppendLine($"OSArchitecture: {RuntimeInformation.OSArchitecture}");
                sb.AppendLine($"Framework: {RuntimeInformation.FrameworkDescription}");
            }
            catch
            {
                // Some trimmed / weird runtimes could throw; ignore
            }

            sb.AppendLine($"Is64BitProcess: {Environment.Is64BitProcess}");
            sb.AppendLine($"ProcessorCount: {Environment.ProcessorCount}");

            // System.Numerics.Vector<T> info (safe on netstandard2.1)
            try
            {
                sb.AppendLine($"Vector.IsHardwareAccelerated: {Vector.IsHardwareAccelerated}");
                sb.AppendLine($"Vector<float>.Count: {Vector<float>.Count}");
                sb.AppendLine($"Vector<byte>.Count: {Vector<byte>.Count}");
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Vector<T> probe failed: {ex.GetType().Name}: {ex.Message}");
            }

            // Probe intrinsics (X86) using reflection so the code compiles on netstandard2.1
            sb.AppendLine("X86 intrinsics (probe via reflection):");
            ProbeIntrinsicType(sb, "System.Runtime.Intrinsics.X86.Sse");
            ProbeIntrinsicType(sb, "System.Runtime.Intrinsics.X86.Sse2");
            ProbeIntrinsicType(sb, "System.Runtime.Intrinsics.X86.Sse3");
            ProbeIntrinsicType(sb, "System.Runtime.Intrinsics.X86.Ssse3");
            ProbeIntrinsicType(sb, "System.Runtime.Intrinsics.X86.Sse41");
            ProbeIntrinsicType(sb, "System.Runtime.Intrinsics.X86.Sse42");
            ProbeIntrinsicType(sb, "System.Runtime.Intrinsics.X86.Avx");
            ProbeIntrinsicType(sb, "System.Runtime.Intrinsics.X86.Avx2");

            sb.AppendLine("ARM intrinsics (probe via reflection):");
            ProbeIntrinsicType(sb, "System.Runtime.Intrinsics.Arm.ArmBase");
            ProbeIntrinsicType(sb, "System.Runtime.Intrinsics.Arm.AdvSimd");

            // Environment hints
            sb.AppendLine("Environment variables (relevant):");
            try
            {
                string[] keys = new[]
                {
                    "COMPlus_EnableAVX",
                    "COMPlus_EnableAVX2",
                    "SKIA_DISABLE_GPU",
                    "DOTNET_TieredCompilation",
                    "DOTNET_UseSharedCompilation",
                    "DOTNET_EnablePreviewFeatures"
                };
                foreach (var k in keys)
                {
                    var v = Environment.GetEnvironmentVariable(k);
                    if (!string.IsNullOrEmpty(v))
                        sb.AppendLine($"  {k}={v}");
                }
            }
            catch
            {
                // ignore
            }

            sb.AppendLine("--------------------------------");
            return sb.ToString();
        }

        /// <summary>
        /// Writes the report to Console.WriteLine by default or uses a provided log action.
        /// </summary>
        public static void LogSimdSupport(Action<string>? logAction)
        {
            string report;
            try
            {
                report = GetReport();
            }
            catch (Exception ex)
            {
                // Last-resort safety: do not throw from logging.
                logAction?.Invoke($"SIMD probe failed: {ex.GetType().Name}: {ex.Message}");
                return;
            }

            if (logAction != null)
            {
                logAction(report);
                return;
            }
        }

        // Helper: probe presence of a type and read its public static bool IsSupported property (if present)
        private static void ProbeIntrinsicType(StringBuilder sb, string typeFullName)
        {
            try
            {
                // Try to find the type in loaded assemblies first
                Type? t = null;
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    try
                    {
                        t = asm.GetType(typeFullName, false, false);
                        if (t != null) break;
                    }
                    catch
                    {
                        // ignore assembly load/reflection issues
                    }
                }

                // Fallback to Type.GetType (may return null)
                if (t == null)
                {
                    try
                    {
                        t = Type.GetType(typeFullName, false);
                    }
                    catch
                    {
                        // ignore
                    }
                }

                if (t == null)
                {
                    sb.AppendLine($"  {typeFullName}: not present");
                    return;
                }

                // Look for public static bool IsSupported { get; }
                var prop = t.GetProperty("IsSupported", BindingFlags.Public | BindingFlags.Static);
                if (prop != null && prop.PropertyType == typeof(bool))
                {
                    bool val = false;
                    try
                    {
                        var v = prop.GetValue(null);
                        if (v is bool b) val = b;
                        sb.AppendLine($"  {typeFullName}: IsSupported = {val}");
                    }
                    catch (TargetInvocationException tie)
                    {
                        sb.AppendLine($"  {typeFullName}: IsSupported raised {tie.InnerException?.GetType().Name ?? tie.GetType().Name}");
                    }
                    catch (Exception ex)
                    {
                        sb.AppendLine($"  {typeFullName}: IsSupported read failed: {ex.GetType().Name}");
                    }
                }
                else
                {
                    sb.AppendLine($"  {typeFullName}: present (no static IsSupported property)");
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"  {typeFullName}: probe failed: {ex.GetType().Name}");
            }
        }
    }
}