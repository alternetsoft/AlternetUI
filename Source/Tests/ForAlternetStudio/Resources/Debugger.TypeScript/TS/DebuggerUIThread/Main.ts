///<reference path="clr.d.ts" />
function CalculatePI() {
  var iterations, k, s;
  resultLabel.Text = "Calculating...";
  System.Windows.Forms.Application.DoEvents();
  k = 1;
  s = 0;
  iterations = 10000;

  for (let i = 0; i < iterations; i++) {
    progressBar.Value = Math.floor(i / iterations * 100) + 1;
    System.Windows.Forms.Application.DoEvents();

    if (i % 2 === 0) {
      s += 4 / k;
    } else {
      s -= 4 / k;
    }

    k += 2;
  }

  resultLabel.Text = `PI approx. value: ${s}`;
}

