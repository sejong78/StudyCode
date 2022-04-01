def print_reverse( 문자열 ):
    print( 문자열[::-1] );

print_reverse( "python" );

def print_score( val_list ):
    print( val_list, "=", sum( val_list ) / len( val_list ) );

print( "" );
print_score( [ 1, 2, 3 ] );

def print_even( val_list ):
    for val in val_list:
        if val % 2 == 0:
            print( val );

print( "" );
print_even( [1, 3, 2, 10, 12, 11, 15] );

def print_keys( val_dic ):
    for key in val_dic.keys():
        print( key );

print( "" );
print_keys( {"이름":"김말똥", "나이":30, "성별":0} );

my_dict = {"10/26" : [100, 130, 100, 100],
           "10/27" : [10, 12, 10, 11]};

def print_value_by_key( dic, key ):
    if key in dic:
        print( dic[key] );
    else:
        print( key, "는 딕셔너리에 없습니다." );

print( "" );
print_value_by_key( my_dict, "10/26" );

def print_5xn(string):
    str_len = int( ( len(string) + 4 ) / 5 );
    for idx in range( str_len ):
        print( string[ int( idx * 5 ):int( ( idx + 1 ) * 5 ) ] ); 

print( "" );
print_5xn("아이엠어보이유알어걸");

def print_mxn(string,cnt):
    str_len = int( ( len(string) + cnt - 1 ) / cnt );
    for idx in range( str_len ):
        print( string[ int( idx * cnt ):int( ( idx + 1 ) * cnt ) ] ); 

print( "" );
print_mxn("아이엠어보이유알어걸", 3)

def make_url( url ):
    return f"www.{url}.com";

url = make_url( "naver" );
print( "" );
print( url );

def make_list( string ):
    _list = [];
    for ch in string:
        _list.append( ch );
    return _list;

str_list = make_list( "abcdef" );
print( "" );
print( str_list );

def pickup_even( val_list ):
    _list = [];
    for val in val_list:
        if val % 2 == 0:
            _list.append(val);
    return _list;

print( "" );
print( pickup_even([3, 4, 5, 6, 7, 8]) );

def convert_int( str ):
    return int( str.replace( ",", "" ) );

print( "" );
print( convert_int("1,234,567") );
