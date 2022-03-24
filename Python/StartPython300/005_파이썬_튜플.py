my_variable = ();
print( type( my_variable ) );

movie_rank = ( "닥터 스트레인지", "스플릿", "럭키" );
print( movie_rank );

t = ('a', 'b', 'c');
t = ('A', 'b', 'c');

interest = ['삼성전자', 'LG전자', 'SK Hynix'];
interest_tuple = tuple( interest );
print( interest_tuple, type( interest_tuple ) );

temp = ('apple', 'banana', 'cake');
a, b, c = temp;
print( a, b, c );

range_tuple = tuple( range( 2, 99, 2 ) );
print( range_tuple );

scores = [8.8, 8.9, 8.7, 9.2, 9.3, 9.7, 9.9, 9.5, 7.8, 9.4];
#valid_score = scores[:8];
*valid_score, _, _ = scores;
print( valid_score );

scores = [8.8, 8.9, 8.7, 9.2, 9.3, 9.7, 9.9, 9.5, 7.8, 9.4];
_, *valid_score, _ = scores;
print( valid_score );
