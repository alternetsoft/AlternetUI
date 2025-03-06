ECHO OFF

SETLOCAL EnableDelayedExpansion

set PW=pwsh -File Get-LatestNuGetVersion.ps1 -PackageName

%PW% "SkiaSharp"
%PW% "SkiaSharp.Extended"
%PW% "SkiaSharp.Extended.Iconify"
%PW% "SkiaSharp.Extended.Iconify.FontAwesome"
%PW% "Svg.Skia"
%PW% "Microsoft.Maui"
%PW% "CommunityToolkit.Maui"
