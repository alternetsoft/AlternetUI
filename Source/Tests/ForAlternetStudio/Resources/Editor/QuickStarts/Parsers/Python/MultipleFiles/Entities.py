class Env(dict):
    def __init__(self , params = () , args = () , outer = None):
        self.update(zip(params,args))
        self.outer = outer
    
    def find(self,var):
        if(var in self):
            return self;
        else:
            return self.outer.find(var)