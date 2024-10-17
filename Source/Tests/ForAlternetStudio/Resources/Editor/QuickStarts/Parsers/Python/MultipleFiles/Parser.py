from Tokenizer import tokenize
from Tokens import Symbol

def parse(program):
    return read_from_tokens(tokenize(program))

def read_from_tokens(tokens):
    if(len(tokens) == 0):
        raise SyntaxError("Unexpected EOF")
    token = tokens.pop(0)
    if(token == "("):
        L = []
        while(tokens[0] != ")"):
            L.append(read_from_tokens(tokens))
        tokens.pop(0)
        return L
    elif(token == ")"):
        raise SyntaxError("Unexpected )")
    else:
        return atom(token)

def atom(token):
    try: 
        return int(token)
    except ValueError: 
        try:
            return float(token)
        except ValueError:
            return Symbol(token)