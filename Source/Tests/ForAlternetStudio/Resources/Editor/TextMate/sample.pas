program BPAGEPAS;

   %include CSRBPPAS

   CONST
     m               = 250;
     n               = 250;
     p               = 250;
     kelement_size   = 4;
     a_size          = m*n*kelement_size;
     b_size          = n*p*kelement_size;
     c_size          = m*p*kelement_size;

   VAR
     a               : array (.1..m, 1..n.) of integer;
     b               : array (.1..n, 1..p.) of integer;
     c               : array (.1..m, 1..p.) of integer;
     i               : integer;
     j               : integer;
     k               : integer;
     rc              : integer;
     rsn             : integer;

   BEGIN
          csrirp (a(.1,1.), a_size, csr_forward,
                  kelement_size*m,
                  0,
                  50,
                  rc,
                  rsn);
          csrirp (b(.1,1.), b_size, csr_forward,
                  kelement_size*n,
                  0,
                  20,
                  rc,
                  rsn);
     for i:=1 to m do
       for j:=1 to n do
         a(.i,j.) := i + j;
     for i:=1 to n do
       for j:=1 to p do
         b(.i,j.) := i + j;
          csrrrp (a(.1,1.), a_size,
                  rc,
                  rsn);
          csrrrp (b(.1,1.), b_size,
                  rc,
                  rsn);
     /* Multiply the two arrays together */

          csrirp (a(.1,1.), m*n*kelement_size, csr_forward,
                  kelement_size*n,
                  0,
                  20,
                  rc,
                  rsn);
          csrirp (b(.1,1.), n*p*kelement_size, csr_forward,
                  (p-1)*kelement_size,
                  0,
                  50,
                  rc,
                  rsn);
     for i:=1 to m do
       for J:=1 to p do
         begin;
         c(.i,j.) := 0;
         for k:=1 to n do
           c(.i,j.) := c(.i,j.) + a(.i,k.) * b(.k,j.);
         end;

          csrrrp (a(.1,1.), m*n*kelement_size,
                  rc,
                  rsn);
          csrrrp (b(.1,1.), n*p*kelement_size,
                  rc,
                  rsn);
   END.