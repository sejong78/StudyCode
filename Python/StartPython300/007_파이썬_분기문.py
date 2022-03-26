import requests;

if 4 < 3:
    print( "Hello World!" );
else:
    print( "Hi, There" );

str = input( "문자를 입력하세요 : " );
print( str * 2 );

str = input( "숫자를 입력하세요 : " );
val_int = int(str);
print( val_int + 10 );

str = input( "숫자를 입력하세요 : " );
val_int = int(str);
if 0 < val_int % 2:
    print( "홀수" );
else:
    print( "짝수" );

str = input( "입력값 : " );
val_int = int(str) + 20;
if 255 < val_int:
    print( f"출력값 : 255" );
else:
    print( f"출력값 : { val_int }" );

str = input( "입력값 : " );
val_int = int(str) - 20;
if 255 < val_int:
    print( f"출력값 : 255" );
elif val_int < 0:
    print( f"출력값 : 0" );
else:    
    print( f"출력값 : { val_int }" );

fruit = ["사과", "포도", "홍시"];
str = input( "좋아하는 과일은? : " );
if str in fruit:
    print( "정답입니다." );
else:
    print( "오답입니다." );

warn_investment_list = ["Microsoft", "Google", "Naver", "Kakao", "SAMSUNG", "LG"];
str = input( "투자 종목 이름은? : " );
if str in warn_investment_list:
    print( "투자 경고 종목입니다." );
else:
    print( "투자 경고 종목이 아닙니다." );

fruit_dict = {"봄" : "딸기", "여름" : "토마토", "가을" : "사과"};
fruit_tuple = ("봄", "여름", "가을");
fruit_list = ["봄", "여름", "가을"];
str = input( "제가 좋아하는 계절은 : " );
if str in fruit_dict:
    print( "딕셔너리 안에 있습니다" );
if str in fruit_tuple:
    print( "튜플 안에 있습니다" );
if str in fruit_list:
    print( "리스트 안에 있습니다" );

fruit_dict = {"봄" : "딸기", "여름" : "토마토", "가을" : "사과"};
str = input( "좋아하는 과일은 : " );
if str in fruit_dict.values():
    print( "정답입니다" );
else:
    print( "오답입니다" );


str = input( "알파벳 : " );
if str.islower():
    print( str.upper() );
else:
    print( str.lower() );

score_dic = { (81,100):"A", (61,80):"B", (41,60):"C", (21,40):"D", (0,20):"E" };
str_val = int( input( "점수는 : " ) );
for score in score_dic:
    if score[0] <= str_val and str_val <= score[1]:
        print( score_dic[score] );
        break;

환율_dec = { "달러":1167, "엔":1.096, "유로":1268, "위안":171 };
str = input( "입력 : " );
환율, 통화명 = str.split();
if 통화명 in 환율_dec:
    print( float(환율) * 환율_dec[통화명], "원" );

number_list = [];
number_list.append( int( input( "input number1 : " ) ));
number_list.append( int( input( "input number2 : " ) ));
number_list.append( int( input( "input number3 : " ) ));
print( max(number_list) );

주민번호 = input( "주민등록번호: " );
주민번호 = 주민번호.replace( "-", "" ).strip();
num_list = [ 2,3,4,5,6,7,8,9,2,3,4,5 ];
sum = 0;
for idx in range(0,12):
    sum += int(주민번호[idx]) * num_list[idx];
check = 11 - ( sum % 11 );
if int(주민번호[12]) == check:
    print( "유효한 주민등록번호입니다." );
else:
    print( "유효 하지 않은 주민등록번호입니다." );


btc = requests.get("https://api.bithumb.com/public/ticker/").json()["data"];
print( f"최근 24시간 내 시작 거래금액 = {btc['opening_price']}" );
print( f"최근 24시간 내 마지막 거래금액 = {btc['closing_price']}" );
print( f"최근 24시간 내 최저 거래금액 = {btc['min_price']}" );
print( f"최근 24시간 내 최고 거래금액 = {btc['max_price']}" );
시가   = int( btc["opening_price"] );
최고가 = int( btc["max_price"] );
변동폭 = 최고가 - int( btc["min_price"] );
if 최고가 < 시가 + 변동폭:
    print( "상승장" );
else:
    print( "하락장" );

