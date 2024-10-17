#include "stdio.h"

__global__ void cuda_hello(){
    printf("Hello World from GPU!\n");
}

int main() {
  cuda_hello<<<1,1>>>(); 
  int a,b,c;
  int *dev_c;
  a=3;
  b=4;
  cudaMalloc((void**)&dev_c, sizeof(int));
  add<<<1,1>>>(a,b,dev_c);
  cudaMemcpy(&c, dev_c, sizeof(int), cudaMemcpyDeviceToHost);
  printf("%d + %d is %d\n", a, b, c);
  cudaFree(dev_c);
  return 0;
}