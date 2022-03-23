letter = "python";
print( letter[0], letter[2] );

license_plate = "24가 2210";
license_num = license_plate[-4:];
print( license_num );

string = "홀짝홀짝홀짝";
print( string[::2] );

string = "PYTHON";
print( string[::-1] );

phone_number = "010-1111-2222"
print( phone_number.replace( "-", " " ) );

url = "http://sharebook.kr";
domain = url.split( "." );
print( domain[-1] );

string = "abcdfe2a354a32a";
print( string.replace( "a", "A" ) );

a = "3"; b = "4";
print( a + b );

print( "Hi" * 3 );

print( "-" * 80 );

t1 = "python";
t2 = "java";
t_sum = t1 + " " + t2 + " ";
print( t_sum * 4 );

name1 = "김민수" 
age1 = 10
name2 = "이철희"
age2 = 13
print( "이름 : %s 나이 : %d" % ( name1, age1 ) );
print( "이름 : {} 나이 : {}".format( name2, age2 ) );
print( f"이름 : {name2} 나이 : {age2}" );

상장주식수 = "5,969,782,550";
주식수 = int( 상장주식수.replace( ",", "" ) );
print( 주식수, type(주식수) );

분기 = "2020/03(E) (IFRS연결)";
분기_split = 분기.split( "(" );
print( 분기_split[0] );

data = "   삼성전자    ";
data_strip = data.strip();
print( data_strip );

ticker = "btc_krw".upper();
print( ticker );
print( ticker.lower() );

a = "hello";
a = a.capitalize();
print( a );

file_name = "보고서.xlsx";
print( file_name.endswith( ( "xlsx", "xls" ) ) );

file_name = "2020_보고서.xlsx";
print( file_name.startswith( "2020" ) );

ticker = "btc_krw";
ticker_split = ticker.split( "_" );
for ts in ticker_split:
    print( ts );

date = "2020-05-01";
date_split = date.split( "-" );
for ds in date_split:
    print( ds );

