from Tokens import List
from Parser import parse
from Samvid import eval

def repl(prompt = "samvid> "):
    while True:
        read = eval(parse(input(prompt)))
        if read is not None: 
            print(schemestr(read))

def schemestr(exp):
    "Convert a Python object back into a Scheme-readable string."
    if isinstance(exp, List):
        return '(' + ' '.join(map(schemestr, exp)) + ')' 
    else:
        return str(exp)

repl()