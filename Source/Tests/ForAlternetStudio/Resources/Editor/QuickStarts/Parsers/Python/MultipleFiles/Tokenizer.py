def tokenize(code):
    return code.replace('(',' ( ').replace(')',' ) ').split()