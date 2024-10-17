def CalculatePI():
    resultLabel.Text = "Calculating..."
    
    k = 1
    s = 0
    iterations = 10000
    
    for i in range(iterations):
        progressBar.Value = int((i * 100) / iterations) + 1
        
        if i % 2 == 0:
            s += 4/k
        else:
            s -= 4/k
        k += 2
         
    resultLabel.Text = "PI approx. value: {0}".format(s)
