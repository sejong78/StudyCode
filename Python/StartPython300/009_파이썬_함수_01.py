
def print_coin():
    for i in range( 100 ):
        print( "비트코인" );

print( "" );
print_coin();

def print_with_smile( str ):
    print( str ,":D" );

print( "" );
문자 = input( "문자 : " );
print_with_smile( 문자 );

def print_upper_price( price ):
    print( price * 1.3 );

print( "" );
price = int( input( "현재 가격 : " ) );
print_upper_price( price );

def print_sum( a, b ):
    print( a + b );

def print_arithmetic_operation( a, b ):
    print( f"{a} + {b} = {a+b}" );
    print( f"{a} - {b} = {a-b}" );
    print( f"{a} * {b} = {a*b}" );
    print( f"{a} / {b} = {a/b}" );

print( "" );
print_arithmetic_operation( 3, 4 );

def print_max( a, b, c ):
    val_max = a;
    if val_max < b:
        val_max = b;
    if val_max < c:
        val_max = c;
    print( val_max );

print( "" );
print_max( -10, -5, -45 );
