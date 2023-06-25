Store private keys in this folder. Do not add them to github.

# REBUILDING ALTERNET SOFTWARE DLLs

Before building the AlterNET Software dll's, you have to create a own strong name file, 
which can be generated with the following command: sn -k Key.snk 
You need then to copy the newly generated Key.snk file to the \Source\Key\ directory. 
The Sn.exe utility is located in your FrameworkSDK binary directory.
Please read the 'Strong-Named Assemblies' topic in MSDN for more information.
Once you complete these steps, you can distribute your compilied AlterNET Software dlls. 
Please refer to EULA for a list of all redistributable dlls. 

DISCLAIMER
These sources are provided "as is", without any guarantee. By deciding to rebuild our 
sources and replacing the original libraries shipped with our products, 
you take responsibility for any damage that may unintentionally be caused by their use.
Use them at your own risk.

For help with rebulding source code, plase feel free to write to our support at: 
support@alternetsoft.com