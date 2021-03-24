"""
시간 관련 모듈 정리
"""
import time as TM;
import datetime as DTM;

def time():
    """ 1970년 1월 1일 0시 0분 0초 이후 경과한 시간을 초단위로 반환합니다. 시간대는 UTC(Universal Time Coordinated, 협정 세계시)를 사용합니다. """
    return TM.time();
#time()

def convert_seconds_to_kor_time( in_seconds ):
    """초를 입력받아 읽기쉬운 한국 시간으로 변환"""
    t1 = DTM.timedelta( seconds = in_seconds );
    days = t1.days;
    total_seconds = t1.seconds;
    (hours, minutes, seconds) = str( DTM.timedelta( seconds = total_seconds ) ).split( ':' );
    hours = int( hours );
    minutes = int( minutes );
    seconds = int( seconds );

    result = [];
    if days >= 1:
        result.append( str( days ) + "일" );
    if hours >= 1:
        result.append( str( hours ) + "시간" );
    if minutes >= 1:
        result.append( str( minutes ) + "분" );
    if seconds >= 1:
        result.append( str( seconds ) + "초" );
    return ' '.join( result );
#convert_seconds_to_kor_time

def convert_seconds_to_time( in_seconds ):
    """초를 입력받아 n days, nn:nn:nn으로 변환"""
    return str( DTM.timedelta( seconds = in_seconds ) );
#convert_seconds_to_time