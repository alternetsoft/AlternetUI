///<reference path="clr.d.ts" />

class CustomClass {
        
   TestMethod(firstParam, secondParam) {
      System.Windows.Forms.MessageBox.Show(System.String.Format("first param is {0}, second param is {1}", firstParam, secondParam));
   }
}