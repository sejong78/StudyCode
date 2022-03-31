for val in range( 0, 100 ):
    print( val );

print( "" );
for year in range( 2002, 2051, 4 ):
    print( year );

print( "" );
for val in range( 0, 100 ):
    print( 99 - val );

print( "" );
for val in range( 0, 10 ):
    print( val / 10 );

print( "" );
for val in range( 1, 10 ):
    print( f"3 X {val} = {3*val}" );

print( "" );
for val in range( 1, 10, 2 ):
    print( f"3 X {val} = {3*val}" );

print( "" );
sum_val = 0;
for val in range( 1, 11 ):
    sum_val += val;
print( f"{sum_val}" );

print( "" );
mul_val = 1;
for val in range( 1, 11 ):
    mul_val *= val;
print( f"{mul_val}" );

print( "" );
price_list = [32100, 32150, 32000, 32500];
for val in range( 0, len(price_list) ):
    print( val, price_list[val] );

print( "" );
price_list = [32100, 32150, 32000, 32500];
for val in range( 0, len(price_list) ):
    idx = len(price_list) - val - 1;
    print( idx, price_list[idx] );

print( "" );
my_list = ["가", "나", "다", "라"];
for val in range( 0, len(my_list) - 1 ):
    print( my_list[val], my_list[val + 1] );

print( "" );
max_idx = len(my_list) - 1;
for val in range( 0, max_idx ):
    idx = max_idx - val;
    print( my_list[idx], my_list[idx - 1] );

print( "" );
my_list = [100, 200, 400, 800];
max_idx = len(my_list) - 1;
for val in range( 0, max_idx ):
    print( my_list[val + 1] - my_list[val] );

print( "" );
my_list = [100, 200, 400, 800, 1000, 1300];
max_idx = len(my_list) - 1;
for val in range( 1, max_idx ):
    print( sum( my_list[val - 1:val + 2]) / 3 );

print( "" );
low_prices  = [100, 200, 400, 800, 1000];
high_prices = [150, 300, 430, 880, 1000];    
volatility  = [];
for idx in range( 0, len(low_prices) ):
    volatility.append( high_prices[idx] - low_prices[idx] );
print( volatility );

print( "" );
apart = [ [101, 102], [201, 202], [301, 302] ]
for i in apart:
    for j in i:
        print( f"{j} 호" );

print( "" );
apart = [ [101, 102], [201, 202], [301, 302] ]
for i in apart[::-1]:
    for j in i[::-1]:
        print( f"{j} 호" );

print( "" );
data = [ [ 2000,  3050,  2050,  1980],
         [ 7500,  2050,  2050,  1980],
         [15450, 15050, 15550, 14900] ];
result = [];
for i in data:
    for j in i:
        result.append( j * 1.00014 );
print( result );

print( "" );
ohlc = [["open", "high", "low", "close"],
        [100, 110, 70, 100],
        [200, 210, 180, 190],
        [300, 310, 300, 310]];
for row in ohlc[ 1:: ]:
    close_val = row[-1];
    if row[0] <= close_val:
        print( close_val );