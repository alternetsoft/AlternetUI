///<reference path="clr.d.ts" />


var initialized = false;
var arenaBackgroundBrush: any;
var dotBrush: any;

var currentAngle: number;
var radius: number;
var dotRadius: number;
var center: any;

function DegreesToRadians(degrees: any)
{
    return (System.Math.PI / 180) * degrees;
}


function InitializeIfNeeded(bounds: any)
{
    if (initialized)
        return;
    
    arenaBackgroundBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkBlue);
    dotBrush = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);

    var maxSide = System.Math.Max(bounds.Width, bounds.Height);
    radius = (maxSide - (maxSide / 3)) / 2;
    dotRadius = maxSide / 20;

    center = new System.Windows.Point(
        Math.floor(bounds.Left + bounds.Width / 2),
        Math.floor(bounds.Top + bounds.Height / 2));

    currentAngle = 0;

    initialized = true;
}

function OnRender(dc: System.Windows.Media.DrawingContext, bounds: any)
{
    InitializeIfNeeded(bounds);

            var arenaBounds = bounds;
            arenaBounds.Inflate(-2, -2);
            let center: System.Windows.Point = new System.Windows.Point((arenaBounds.Left + arenaBounds.Right) / 2, (arenaBounds.Top + arenaBounds.Bottom) / 2);
            dc.DrawEllipse(arenaBackgroundBrush, null, center, Math.floor(arenaBounds.Width / 2), Math.floor(arenaBounds.Height / 2));

    var radians = DegreesToRadians(currentAngle);

    var dotCenter = new System.Windows.Point(
        center.X + Math.floor(System.Math.Cos(radians) * radius),
        center.Y + Math.floor(System.Math.Sin(radians) * radius));

    dc.DrawEllipse(
        dotBrush,
        null,
        dotCenter,
        Math.floor(dotRadius * 2),
        Math.floor(dotRadius * 2));
}

function ConstrainAngle(x: number): number
{
    x %= 360;
    if (x < 0)
        x += 360;

    return x;
}

function OnUpdate(deltaTimeMs: number)
{
    currentAngle += deltaTimeMs * 0.1;
    currentAngle = ConstrainAngle(currentAngle);
}
