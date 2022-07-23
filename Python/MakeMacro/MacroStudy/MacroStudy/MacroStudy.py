import pyautogui as mr;
import pygetwindow as gw;

sr = mr.size();
print( sr );

mpt = mr.position();
print( mpt );

mr.moveTo( 100, 100 );

mr.alert( "경고!!" );

vars = mr.confirm( "확인은 OK, 취소는 Cancel" );
if vars is "OK":
    print( "확인을 눌렀구만" );
else:
    print( "취소를 눌렀구만" );

mr.screenshot( "foo.png" );

chrome = gw.getWindowsWithTitle( "Chrome" )[0];

chrome.activate();

mr.click(chrome.center);