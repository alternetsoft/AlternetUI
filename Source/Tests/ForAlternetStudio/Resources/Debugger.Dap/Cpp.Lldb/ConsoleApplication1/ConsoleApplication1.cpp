#include <cstdlib>
#include <complex>
#include <iostream>

using namespace std;

int main(int argc, char** argv) {

    // User defined values

    const char brush[] = ".:-=+*#%@";

    const double xmin = -2;
    const double xmax = 1;
    const double ymin = -1;
    const double ymax = 1;

    const int iterations = 20;

    // Initialization

    const int width = argc > 1 ? atoi(argv[1]) : 30;
    const int height = static_cast<int>((width / (xmax - xmin)) * (ymax - ymin));

    const double xstep = (xmax - xmin) / width;
    const double ystep = (ymax - ymin) / height;

    const int blength = (sizeof(brush) / sizeof(*brush)) - 1;

    int escape;

    for (float y = ymin; y < ymax; y += ystep) {
        for (float x = xmin; x < xmax; x += xstep) {
            const complex<float> pt0(x, y);
            complex<float> pt(pt0);
            escape = 0;
            while (++escape < iterations) {
                pt = pt * pt + pt0;
                if (abs(pt) >= 2) break;
            }
            cout << brush[static_cast<int>((static_cast<float>(escape) / iterations * blength - 1))];
        }
        cout << endl;
    }

    return 0;
}
