import math
from Tokens import List, Number, Symbol
from Entities import Env
import operator as op

def standard_env():
    env = Env()
    env.update(vars(math)) # sin, cos, sqrt, pi, ...
    env.update(
        {
            '+':       op.add, 
            '-':       op.sub, 
            '*':       op.mul, 
            '/':       op.truediv, 
            '>':       op.gt, 
            '<':       op.lt, 
            '>=':      op.ge, 
            '<=':      op.le, 
            '=':       op.eq, 
            'abs':     abs,
            'append':  op.add,  
            'apply':   lambda proc, args: proc(*args),
            'begin':   lambda *x: x[-1],
            'car':     lambda x: x[0],
            'cdr':     lambda x: x[1:], 
            'cons':    lambda x,y: [x] + y,
            'eq?':     op.is_, 
            'expt':    pow,
            'equal?':  op.eq, 
            'length':  len, 
            'list':    lambda *x: List(x), 
            'list?':   lambda x: isinstance(x, List), 
            'map':     map,
            'max':     max,
            'min':     min,
            'not':     op.not_,
            'null?':   lambda x: x == [], 
            'number?': lambda x: isinstance(x, Number),  
            'print':   print,
            'procedure?': callable,
            'round':   round,
            'symbol?': lambda x: isinstance(x, Symbol),
        }
    )
    return env      


globalenv = standard_env()