import UIKit

// Declare our new class 
class Person {
// We can define class property here
var age  = 25
// Implement Class initializer. Initializers are called when a new object of this class is created
init() { 
   print(“A new instance of this class Person is created.”) 
 } 
} 
// We can now create an instance of class Person - an object - by putting parentheses after the class name
let personObj =  Person()
// Once an instance of Person class is created we can access its properties using the dot “.” syntax.
print(“This person age is \(personObj.age)”)

class ViewController: UIViewController {
    
    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
        
        let button = UIButton(type: UIButton.ButtonType.system) as UIButton
        
        let xPostion:CGFloat = 50
        let yPostion:CGFloat = 100
        let buttonWidth:CGFloat = 150
        let buttonHeight:CGFloat = 45
        
        button.frame = CGRect(x:xPostion, y:yPostion, width:buttonWidth, height:buttonHeight)
        
        button.backgroundColor = UIColor.lightGray
        button.setTitle("Tap me", for: UIControl.State.normal)
        button.tintColor = UIColor.black
        button.addTarget(self, action: #selector(self.buttonAction), for: .touchUpInside)
        
        self.view.addSubview(button)
    }
    
    @objc func buttonAction(_ sender:UIButton!)
    {
        print("Button tapped")
    }
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
}

