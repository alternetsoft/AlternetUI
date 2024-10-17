
//----- Include files ---------------------------------------------------------
#include <stdio.h>              // Needed for printf()
#include <stdlib.h>             // Needed for rand() and RAND_MAX
#include <math.h>               // Needed for log()

//----- Constants -------------------------------------------------------------
#define NUM_BINS        25      // Number of measurement "bins"
#define NUM_SAMPLES  50000      // Number of samples to simulate
#define MAX_BAR        250      // Maximum bar size for histogram print-out

//----- Function prototypes ---------------------------------------------------
double expon(double x);         // Returns an exponential random variable

//===== Main program ==========================================================
void main(void)
{
	double mean_rv;               // Mean of random variable
	double exp_rv;                // Exponential random variable
	int    count[NUM_BINS];       // Counter for each bin
	int    bar_size;              // Bar size for print-out of histogram
	int    i, j;                  // Loop counters

	// Clear the count vector
	for (i = 0; i < NUM_BINS; i++)
		count[i] = 0;

	// Set mean value for this run
	mean_rv = 8.0;

	// Main loop to generate samples and update count
	for (i = 0; i < NUM_SAMPLES; i++)
	{
		exp_rv = expon(mean_rv);

		for (j = 0; j < (NUM_BINS - 1); j++)
			if (((double) j < exp_rv) && (exp_rv <= ((double) (j + 1))))
				count[j]++;
	}

	// Output a histogram of the count vector
	for (i = 0; i < NUM_BINS; i++)
	{
		printf("%2d < x <= %2d --> ", i, i + 1);
		bar_size = ((double) count[i] / NUM_SAMPLES) * (double) MAX_BAR;
		for (j = 0; j < bar_size; j++)
			printf("*");
		printf("\n");
	}
}

//=============================================================================
//=  Function to generate exponentially distributed RVs                       =
//=    - Input:  x (mean value of distribution)                               =
//=    - Output: Returns with exponential RV                                  =
//=============================================================================
double expon(double lambda)
{
	double z;     // Uniform random number from 0 to 1

	// Pull a uniform RV (0 < z < 1)
	do
	{
		z = ((double) rand() / RAND_MAX);
	}
	while ((z == 0) || (z == 1));

	return(-*log(z));
}

