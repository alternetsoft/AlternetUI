#Must be the first statement in your script (not counting comments)
param([String]$message="Message text") 

if ($message -eq "")
{
  $message = "Message text"
}

[reflection.assembly]::loadwithpartialname('System.Windows.Forms')
[reflection.assembly]::loadwithpartialname('System.Drawing')
$notify = new-object system.windows.forms.notifyicon
$notify.icon = [System.Drawing.SystemIcons]::Information
$notify.visible = $true
$notify.showballoontip(10,'Information',$message,[system.windows.forms.tooltipicon]::None)