///<reference path="clr.d.ts" />

var initialized = false;
var arenaBackgroundBrush;
var dotBrush;

var currentAngle = 0;
var radius = 0;
var dotRadius = 0;
var center;
var Int32T = host.type("System.Int32");

function DegreesToRadians(degrees)
{
    return (System.Math.PI / 180) * degrees;
}

function ToInt(value)
{
   return host.cast(Int32T, value);
}

function InitializeIfNeeded(bounds)
{
	if (initialized)
		return;
	
	arenaBackgroundBrush = new System.Drawing.SolidBrush(System.Drawing.Color.DarkBlue);
	dotBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);

	var maxSide = System.Math.Max(bounds.Width, bounds.Height);
	radius = (maxSide - (maxSide / 3)) / 2;
	dotRadius = maxSide / 20;

	center = new System.Drawing.Point(
		ToInt(bounds.Left + bounds.Width / 2),
		ToInt(bounds.Top + bounds.Height / 2));

    currentAngle = 0;

	initialized = true;
}

function OnPaint(g, bounds)
{
	InitializeIfNeeded(bounds);

	g.SmoothingMode =
		System.Drawing.Drawing2D.SmoothingMode.HighQuality;

	g.Clear(System.Drawing.SystemColors.Control);

	var arenaBounds = bounds;
	arenaBounds.Inflate(-2, -2);
	g.FillEllipse(arenaBackgroundBrush, arenaBounds);

	var radians = DegreesToRadians(currentAngle);

	

	var dotCenter = new System.Drawing.Point(
        center.X + ToInt(System.Math.Cos(radians) * radius),
        center.Y + ToInt(System.Math.Sin(radians) * radius));

	g.FillEllipse(
		dotBrush,
		ToInt(dotCenter.X - dotRadius),
		ToInt(dotCenter.Y - dotRadius),
		ToInt(dotRadius * 2),
		ToInt(dotRadius * 2));
}

function ConstrainAngle(x)
{
    x %= 360;
    if (x < 0)
        x += 360;

    return x;
}

function OnUpdate(deltaTimeMs)
{
	currentAngle += deltaTimeMs * 0.1;
	currentAngle = ConstrainAngle(currentAngle);
}
