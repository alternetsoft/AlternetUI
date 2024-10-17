///<reference path="clr.d.ts" />

class CustomClass {
        
    function TestMethod(int firstParam, boolean secondParam) {
        System.Windows.Forms.MessageBox.Show(System.String.Format("first param is {0}, second param is {1}", firstParam, secondParam));
    }
}