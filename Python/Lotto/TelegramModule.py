"""
텔레그램 모듈
텔레그램 라이브러리 설치는 아래 윈도우중  Terminal 에서 pip install python-telegram-bot --upgrade 을 실행
"""
import telegram as TG;

class TelegramModule( object ):
    """텔레그램 모듈 제어 클레스"""

    #smbenkei_telegram_bot 의 토큰
    TELEGRAM_CHAT_BOT_TOKEN = "1673974176:AAGgGCRZkh2bDe-jpBUUrHAUBvOB02HReMg";

    #smbenki_telegram_bot 의 개인 챗 id
    TELEGRAM_CHAT_BOT_PERSONAL_ID = "55510580";

    #smbenki_telegram_bot 의 그룹 챗 ( 로또번호 알림 ) id
    TELEGRAM_CHAT_BOT_GROUP_LOTTO_ID = "-531880449";

    def __init__( self ):
        self.__InitializeValues();#변수 선언 및 초기화
        self.__CreateInstance();#인스턴스 할당
    #__init__

    def __InitializeValues( self ):
        """ 변수 선언 및 초기화 """
        self.m_instance : TG.Bot = None; #인스턴스
    #__InitializeValues

    def __CheckInstance( self ):
        """인스턴스 할당여부 체크"""
        if self.m_instance is None:
            print( "인스턴스가 할당되어 있지 않습니다." );
            return False;

        return True;
    #__CheckInstance

    def __CreateInstance( self ):
        """ 인스턴스 할당 """
        if self.m_instance is None:
            self.m_instance = TG.Bot( token = TelegramModule.TELEGRAM_CHAT_BOT_TOKEN );
    #__CreateInstance

    def SendMessageToPersonal( self, msg:str ):
        """쳇봇과의 1:1 메세지 창에 글을 남긴다."""
        if not self.__CheckInstance():
            return;

        self.m_instance.sendMessage( chat_id = TelegramModule.TELEGRAM_CHAT_BOT_PERSONAL_ID, text = msg, parse_mode = 'Markdown' );
    #SendMessageToPersonal

    def SendMessageToGroupLotto( self, msg:str ):
        """로또 번호 알림 그룹 메세지 창에 글을 남긴다."""
        if not self.__CheckInstance():
            return;

        self.m_instance.sendMessage( chat_id = TelegramModule.TELEGRAM_CHAT_BOT_GROUP_LOTTO_ID, text = msg, parse_mode = 'Markdown' );
    #SendMessageToGroupLotto

#TelegramModule
