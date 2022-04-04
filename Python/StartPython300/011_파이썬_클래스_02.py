class Stock:
    def __init__( self, name, code, per, pbr, dividend ):
        self.str_name = name;
        self.str_code = code;
        self.float_per = per;
        self.float_pbr = pbr;
        self.float_dividend = dividend;

    def set_name( self, name ):
        self.str_name = name;

    def set_code( self, code ):
        self.str_code = code;

    def set_per( self, per ):
        self.float_per = per;

    def set_pbr( self, pbr ):
        self.float_pbr = pbr;

    def set_dividend( self, dividend ):
        self.float_dividend = dividend;

    def get_name( self ):
        return self.str_name;

    def get_code( self ):
        return self.get_code;

    def show( self ):
        print( self.str_name, self.str_code, self.float_per, self.float_pbr, self.float_dividend );


삼성 = Stock( "삼성전자", "005930", 15.79, 1.33, 2.83 );
삼성.show();

a = Stock( None, None, None, None, None );
a.set_name( "삼성전자" );
print( "" );
a.show();

a.set_code( "005930" );
print( "" );
a.show();

a.set_per( 12.75 );
print( "" );
a.show();

stock_list = [];
stock_list.append( Stock( "삼성전자", "005930", 15.79, 1.33, 2.83 ) );
stock_list.append( Stock( "현대차", "005380", 8.70, 0.35, 4.27 ) );
stock_list.append( Stock( "LG전자", "066570", 317.34, 0.69, 1.37 ) );

print( "" );
for st in stock_list:
    st.show();

