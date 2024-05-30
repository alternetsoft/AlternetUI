#pragma warning disable
#nullable disable
using System;

namespace Alternet.UI.Port
{
    /// <summary>
    /// Used to mark internal metadata
    /// that is allowed to be accessed from friend assemblies.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class |
        AttributeTargets.Property |
        AttributeTargets.Field |
        AttributeTargets.Method |
        AttributeTargets.Struct |
        AttributeTargets.Enum |
        AttributeTargets.Interface |
        AttributeTargets.Delegate |
        AttributeTargets.Constructor,
        AllowMultiple = false,
        Inherited = true)
    ]
    public sealed class FriendAccessAllowedAttribute : Attribute
    {
    }
}