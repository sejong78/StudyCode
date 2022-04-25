import pyautogui as pag;

sr = pag.size();
print( sr );

mpt = pag.position();
print( mpt );

pag.moveTo( 100, 100 );

pag.alert( "경고!!" );

vars = pag.confirm( "확인은 OK, 취소는 Cancel" );
if vars is "OK":
    print( "확인을 눌렀구만" );
else:
    print( "취소를 눌렀구만" );

pag.screenshot( "foo.png" );

