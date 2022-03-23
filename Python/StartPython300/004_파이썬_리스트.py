movie_rank = [ "닥터 스트레인지", "스플릿", "럭키" ];
print( movie_rank );

movie_rank.append( "배트맨" );
print( movie_rank );

movie_rank.insert( 1, "슈퍼맨" );
print( movie_rank );

movie_rank.remove( "럭키" );
print( movie_rank );

del movie_rank[2];
del movie_rank[2];
print( movie_rank );

lang1 = ["C", "C++", "JAVA"];
lang2 = ["Python", "Go", "C#"];
langs = lang1 + lang2;
print( langs );

nums = [1, 2, 3, 4, 5, 6, 7];
print( f"min : {min( nums )}" );
print( f"max : {max( nums )}" );

print( f"sum : {sum( nums )}" );

cook = ["피자", "김밥", "만두", "양념치킨", "족발", "피자", "김치만두", "쫄면", "소시지", "라면", "팥빙수", "김치전"];
print( f"데이터 개수 : {len(cook)}" );

nums_총합 = sum( nums );
nums_평균 = nums_총합 / len( nums );
print( f"평균 : {nums_평균}" );

price = ['20180728', 100, 130, 140, 150, 160, 170];
print( price[1:] );

nums = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
print( nums[::2] );
print( nums[1::2] );
print( nums[::-1] );

interest = ['삼성전자', 'LG전자', 'Naver', 'SK하이닉스', '미래에셋대우'];
print( " ".join( interest[::2] )  );
print( "/".join( interest[::-1] )  );
print( "\n".join( interest )  );

string = "삼성전자/LG전자/Naver";
interest = string.split( "/" );
print( " ".join( interest )  );

data = [2, 4, 3, 1, 5, 10, 9];
print( sorted( data ) );
print( sorted( data, reverse = True ) );
print( data );
data.sort();
print( data );
data.sort( reverse = True );
print( data );

Tuple_Datas = [ (1, 2), (0, 1), (5, 1), (5, 2), (3, 0) ];
print( sorted( Tuple_Datas, key=lambda x:x[0] ) );
print( sorted( Tuple_Datas, key=lambda x:(x[0], -x[1]) ) );