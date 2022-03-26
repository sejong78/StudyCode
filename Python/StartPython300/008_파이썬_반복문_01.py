리스트 = [ "김밥", "라면", "튀김" ];
for 음식 in 리스트:
    print( f"오늘의 메뉴: {음식}" );

print( "" );
리스트 = [ "SK하이닉스", "삼성전자", "LG전자" ];
for 종목 in 리스트:
    print( f"{len(종목)}" );

print( "" );
리스트 = ['dog', 'cat', 'parrot'];
for 동물 in 리스트:
    print( f"{동물} {len(동물)}" );

print( "" );
for 동물 in 리스트:
    print( f"{동물[0]}" );

print( "" );
리스트 = [1, 2, 3];
for 숫자 in 리스트:
    print( f"3 X {숫자} = {3*숫자}" );

print( "" );
리스트 = ["가", "나", "다", "라"];
for idx in range( 1, 4 ):
    print( 리스트[idx] );

print( "" );
리스트 = ["가", "나", "다", "라"];
for 문자 in 리스트[0::2]:
    print( 문자 );

print( "" );
리스트 = ["가", "나", "다", "라"];
for 문자 in 리스트[::-1]:
    print( 문자 );

print( "" );
리스트 = [3, -20, -3, 44];
for 숫자 in 리스트:
    if 숫자 < 0:
        print( 숫자 );

print( "" );
리스트 = [3, 100, 23, 44];
for 숫자 in 리스트:
    if 0 == 숫자 % 3:
        print( 숫자 );

print( "" );
리스트 = [13, 21, 12, 14, 30, 18];
for 숫자 in 리스트:
    if 0 == 숫자 % 3 and 숫자 < 20:
        print( 숫자 );

print( "" );
리스트 = ['dog', 'cat', 'parrot'];
for 동물 in 리스트:
    print( f"{동물[0].upper()}{동물[1:]}" );

print( "" );
리스트 = ['intra.h', 'intra.c', 'define.h', 'run.py'];
확장자 = "";
for 파일명 in 리스트:
    확장자 = 파일명.split(".")[-1];
    if 확장자 == "h" or 확장자 == "c":
        print( 파일명 );
