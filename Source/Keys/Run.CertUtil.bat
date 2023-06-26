Echo Specify password as a parameter 
Echo Certificate information will be in Run.CertUtil.Output.txt

certutil -p %1 -dump ./Alternet.pfx > Run.CertUtil.Output.txt

