///<reference path="clr.d.ts" />

function reverseString(str)
{
    var newString = "";
    for (var i = str.length - 1; i >= 0; i--)
        newString += str[i];
    return newString;
}

System.Windows.MessageBox.Show(reverseString("Hello World"));
