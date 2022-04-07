#파일 쓰기
file = open( "매수종목1.txt", mode = "wt", encoding = "utf-8" );
file.write( "005930\n" );
file.write( "005380\n" );
file.write( "035420\n" );
file.close();

file = open( "매수종목2.txt", mode = "wt", encoding = "utf-8" );
file.write( "005930 삼성전자\n" );
file.write( "005380 현대차\n" );
file.write( "035420 NAVER\n" );
file.close();

#CSV 파일 쓰기
import csv;

file = open( "매수종목.csv", mode = "wt", encoding = "cp949", newline="" );

writer = csv.writer(file);
writer.writerow( ["종목명", "종목코드", "PER"] );

writer.writerow( ["삼성전자", "005930", "15.79"] );
writer.writerow( ["NAVER", "035420", "55.82"] );
file.close();


file = open( "매수종목1.txt", mode = "rt", encoding = "utf-8" );
lines = file.readlines();

codes = [];
for line in lines:
    code = line.strip();
    codes.append( code );

file.close();

print( codes );

file = open( "매수종목2.txt", mode = "rt", encoding = "utf-8" );
lines = file.readlines();

codes = {};
for line in lines:
    splits = line.strip().split( " " );
    codes[splits[0]] = splits[1];
    

file.close();

print( codes );

#예외처리
per = ["10.31", "", "8.00"];

for i in per:
    try:
        print(float(i))
    except:
        print( 0 );
