from Tokens import Symbol, List
from Entities import Env
from Enviroment import globalenv

class Procedure:
    def __init__(self , params , body , env):
        self.params = params
        self.body = body
        self.env = env

    def __call__(self, *args):
        return eval( self.body , Env( self.params , args , self.env ) )


def eval(exp,env = globalenv):
    if(exp is None):
        return "null"
    if(isinstance(exp,Symbol)):
        return env.find(exp)[exp]
    elif(not isinstance(exp, List)):
        return exp
    elif exp[0] == 'quote':   
        _ , *args = exp       
        return " ".join(args)
    elif(exp[0] == "if"):
        ( _ , cond , conseq , alt ) = exp
        if(eval(cond,env)):
            return eval(conseq,env)
        else:
            return eval(alt,env)
    elif(exp[0] == "define"):
        ( _ , symbol , exp ) = exp
        env[symbol] = eval(exp,env)
    elif(exp[0] == "set!"):
        ( _ , symbol , exp ) = exp
        env.find(symbol)[symbol] = eval(exp,env)
    elif(exp[0] == "lambda"):
        ( _ , params , *body ) = exp
        return Procedure(params,*body,env)
    elif(exp[0] == "begin"):
        for e in exp[1:-1]:
            eval(e,env)
        return eval(exp[-1],env)
    else:
        proc = eval(exp[0],env)
        args = [ eval(arg,env) for arg in exp[1:] ]
        return proc(*args)

# def expand(exp):
#     ( _ , v, body ) = exp[0], exp[1], exp[2:]
#     if(_ == "define" and isinstance(v, list) and v): 
#         f, args = v[0], v[1:]
#         return [_ , f, ["lambda", args]+body]