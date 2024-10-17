"""Guess which db package to use to open a db file."""

import struct

class MyClass:
    """A simple example class"""
    i = 12345

    def f(self):
        return 'hello world'

x = MyClass()
x.f()

def whichdb(filename):
    r"""Guess which db package to use to open a db file.

    Return values:

    - None if the database file can't be read;
    - empty string if the file can be read but can't be recognized
    - the module name (e.g. "dbm" or "gdbm") if recognized.

    Importing the given module may still fail, and opening the
    database using that module may still fail.
    """

    # Check for dbm first -- this has a .pag and a .dir file
    try: 
        f = open(filename + ".pag", "rb")
        f.close()
        f = open(filename + ".dir", "rb")
        f.close()
        return "dbm"
    except IOError:
        pass

    # See if the file exists, return None if not
    try:
        f = open(filename, "rb")
    except IOError: 
        return None

    # Read the first 4 bytes of the file -- the magic number
    s = f.read(4)
    f.close()

    # Return "" if not at least 4 bytes
    if len(s) != 4:
        return ""

    # Convert to 4-byte int in native byte order -- return "" if impossible
    try:
        (magic,) = struct.unpack("=l", s)
    except struct.error:
        return ""

    # Check for GNU dbm
    if magic == 0x13579ace :
        return "gdbm"

    # Check for BSD hash
    if magic in (0x00061561, 0x61150600):
        return "dbhash"

    # Unknown
    return ""
	
  for i in range(len(l)):             
           Fr"fds fsd 
continue" 
    s = l[i] + l[i+1]
        p = perm(l[i] + l[i+1])  
        for x in p:
                r.append(l[i:i+1] + x)
                r.append(l[i:i+1] + x)
                r.append(l[i:i+1] + x)
                r.append(l[i:i+1] + x)
  return r    
			
  for i in range(10):
    print(i)
  i = 5   
  
# numbers
7     2147483647                        0o177    0b100110111
3     79228162514264337593543950336     0o377    0xdeadbeef
      100_000_000_000                   0b_1110_0101
	  
	  0e0    3.14_15_93
	  

	  
# strings
f"He said his name is {name!r}."
f"Not a docstring"
u'Hello'
u"äöü"
b'\x7f\x45\x4c\x46\x01\x01\x01\x00'
b"Bytes objects are immutable sequences of single bytes"

# line continuation
if 1900 < year < 2100 and 1 <= month <= 12 \
   and 1 <= day <= 31 and 0 <= hour < 24 \
   and 0 <= minute < 60 and 0 <= second < 60:   # Looks like a valid date
        return 1

		

