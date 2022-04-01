import time;
from datetime import datetime, timedelta;

now = datetime.now();
print( now, type(now) ); 

print( "" ); 
for day in range( 5, 0, -1 ):
    delta = timedelta( day );
    date = now - delta;
    print( f"{day} 일전은 ", date );

print( "" ); 
print( now.strftime( "%H:%M:%S" ) );

print( "" ); 
print( datetime.strptime( "2020-05-04", "%Y-%m-%d" ) );

print( "" ); 
for i in range(2):
    now = datetime.now();
    print( now );
    time.sleep( 1 );

import os;

print( "" ); 
print( os.getcwd() );

import numpy;

print( "" ); 
for i in numpy.arange( 0, 5, 0.1 ):
    print(i);