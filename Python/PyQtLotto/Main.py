import sys
from PyQt5.QtCore import *
from PyQt5.QtWidgets import *
from PyQt5 import uic
import LottoModule as LM;
import WebCrawlerModule as WM;
import TelegramModule as TM;
import re as STR_RE;

#UI파일 연결
#단, UI파일은 Python 코드 파일과 같은 디렉토리에 위치해야한다.
ui_lotto_frame = uic.loadUiType("LottoFrame.ui")[0];

class LottoViewModel:
    """ 로도 번호 전달용 뷰모델 """
    def __init__(self):
        self.row = 0;
        self.col1 = 0;
        self.col2 = 0;
        self.col3 = 0;
        self.col4 = 0;
        self.col5 = 0;
        self.col6 = 0;
    # __init__
#LottoViewModel


#화면을 띄우는데 사용되는 Class 선언
class WindowClass( QMainWindow, ui_lotto_frame ):
    """ 연결된 LCD 에 생성한 숫자를 세팅한다. """

    def __init__( self ):
        super().__init__();
        self.setupUi( self );
        self.initValues();
    # __init__

    # 내부 값들을 초기화 한다.
    def initValues( self ):
        # 선택 버튼 연결
        self.m_btn_create_numbers.clicked.connect( self.OnClick_CreateNumbers );

        # 각 LCD를 관리를 위해 배열로 묶어줌
        self.m_displays = [[self.m_number_00, self.m_number_01, self.m_number_02, self.m_number_03, self.m_number_04, self.m_number_05],
                           [self.m_number_10, self.m_number_11, self.m_number_12, self.m_number_13, self.m_number_14, self.m_number_15 ],
                           [self.m_number_20, self.m_number_21, self.m_number_22, self.m_number_23, self.m_number_24, self.m_number_25 ],
                           [self.m_number_30, self.m_number_31, self.m_number_32, self.m_number_33, self.m_number_34, self.m_number_35 ],
                           [self.m_number_40, self.m_number_41, self.m_number_42, self.m_number_43, self.m_number_44, self.m_number_45 ]];

        # 생성한 로또 숫자
        self.m_create_numbers = [[ 0, 0, 0, 0, 0, 0 ], [ 0, 0, 0, 0, 0, 0 ], [ 0, 0, 0, 0, 0, 0 ], [ 0, 0, 0, 0, 0, 0 ], [ 0, 0, 0, 0, 0, 0 ]];

        #웹 크롤링을 통한 지난 시즌 당첨번호
        webcrawler = WM.WebCrawlerModule();
        crawled_html = webcrawler.GetHtml( "https://dhlottery.co.kr/gameResult.do?method=byWin" );
        find_msgs = crawled_html.head.find( "meta", { "name": "description" } ).get( 'content' );
        print( f"{find_msgs}" );

        last_index, last_num1, last_num2, last_num3, last_num4, last_num5, last_num6 = self.parse_last_info( find_msgs ); #문장 내용중 필요내용 파싱
        print( f"last_index = {last_index}, last_num1 = {last_num1}, last_num2 = {last_num2}, last_num3 = {last_num3}, last_num4 = {last_num4}, last_num5 = {last_num5}, last_num6 = {last_num6}" );

        # 지난 회차 + 1 해서 이번회차
        self.m_index = last_index + 1;

        #텔레그램 객체 생성
        self.m_telegram = TM.TelegramModule();

        #지난주 정보 푸시
        msg = f"지난 {last_index}회차 로또 당첨번호\n {last_num1}, {last_num2}, {last_num3}, {last_num4}, {last_num5}, {last_num6}\n를 바탕으로 로또 번호 생성 합니다.";
        self.m_telegram.SendMessageToGroupLotto( msg );

        #로또 객체 생성
        self.m_lotto = LM.LottoModule();

        # 웹크롤링을 통해 지난 번호를 가져와서 세팅
        self.m_lotto.SetLastLottoInfo( last_index, last_num1, last_num2, last_num3, last_num4, last_num5, last_num6 );

        # 스레드 초기화
        self.m_th_01 = None;
        self.m_th_02 = None;
        self.m_th_03 = None;
        self.m_th_04 = None;
        self.m_th_05 = None;

        print( "준비 완료!!" );

    # initValues

    def parse_last_info( self, lastinfo: str ):
        """ex>동행복권 950회 당첨번호 3,4,15,22,28,40+10. 1등 총 8명, 1인당 당첨금액 3,281,920,500원."""

        # 문장안에서 수만( "\d" 옵션이면 숫자만 ) 뽑아서 문자열 리스트로 뽑아준다.
        numbers = STR_RE.findall( "\d+", lastinfo );

        return int( numbers[0] ), int( numbers[1] ), int( numbers[2] ), int( numbers[3] ), int( numbers[4] ), int(
                numbers[5] ), int( numbers[6] );

    # parse_last_info

    # 끝났는지 체크한다. 리스트 돌면서 0번 값이 모두 0이 아니면 앱을 끈다.
    def check_finish( self ):
        # 하나라도 0이 있으면 끝이 아니다
        if 0 == self.m_create_numbers[0][0] * self.m_create_numbers[1][0] * self.m_create_numbers[2][0] * self.m_create_numbers[3][0] * self.m_create_numbers[4][0]:
            print( f"{self.m_create_numbers[0][0]} * {self.m_create_numbers[1][0]} * {self.m_create_numbers[2][0]} * {self.m_create_numbers[3][0]} * {self.m_create_numbers[4][0]} 아직 모두 완료되지는 않았습니다" );
            return;

        # 모두 0이 아니기 때문에 생성이 완료되었다.
        msg = f"{self.m_index} 회차 로또 번호 생성이 완료 되었습니다.\n{ self.m_create_numbers[0] }\n{ self.m_create_numbers[1] }\n{ self.m_create_numbers[2] }\n{ self.m_create_numbers[3] }\n{ self.m_create_numbers[4] }\n행운을 빕니다!";
        self.m_telegram.SendMessageToGroupLotto( msg );

    # check_finish

    # 생성된 뷰모델을 받아서 디스플레이에 내부 값을 세팅한다.
    @pyqtSlot(LottoViewModel)
    def OnEvent_ShowNumber( self, vm:LottoViewModel ):
        self.m_displays[vm.row][0].display( vm.col1 );
        self.m_displays[vm.row][1].display( vm.col2 );
        self.m_displays[vm.row][2].display( vm.col3 );
        self.m_displays[vm.row][3].display( vm.col4 );
        self.m_displays[vm.row][4].display( vm.col5 );
        self.m_displays[vm.row][5].display( vm.col6 );
    # OnEvent_SetNumber

    # 생성된 뷰모델을 받아서 저장한다.
    @pyqtSlot(LottoViewModel)
    def OnEvent_SetNumber( self, vm:LottoViewModel ):
        # 우선 마지막으로 찍고
        self.OnEvent_ShowNumber( vm );

        self.m_create_numbers[ vm.row ][ 0 ] = vm.col1;
        self.m_create_numbers[ vm.row ][ 1 ] = vm.col2;
        self.m_create_numbers[ vm.row ][ 2 ] = vm.col3;
        self.m_create_numbers[ vm.row ][ 3 ] = vm.col4;
        self.m_create_numbers[ vm.row ][ 4 ] = vm.col5;
        self.m_create_numbers[ vm.row ][ 5 ] = vm.col6;

        # 종료 체크
        self.check_finish();
    # OnEvent_SetNumber

    # 번호 생성을 시작한다.
    def OnClick_CreateNumbers( self ):

        if self.m_th_01 == None:
            self.m_th_01 = ThreadWorker( row = 0, parent = self );
            self.m_th_01.start();
            # 시그널 연결
            self.m_th_01.onEvent_ShowNumber.connect( self.OnEvent_ShowNumber );
            self.m_th_01.onEvent_SetNumber.connect( self.OnEvent_SetNumber );

        if self.m_th_02 == None:
            self.m_th_02 = ThreadWorker( row = 1, parent = self );
            self.m_th_02.start();
            # 시그널 연결
            self.m_th_02.onEvent_ShowNumber.connect( self.OnEvent_ShowNumber );
            self.m_th_02.onEvent_SetNumber.connect( self.OnEvent_SetNumber );

        if self.m_th_03 == None:
            self.m_th_03 = ThreadWorker( row = 2, parent = self );
            self.m_th_03.start();
            # 시그널 연결
            self.m_th_03.onEvent_ShowNumber.connect( self.OnEvent_ShowNumber );
            self.m_th_03.onEvent_SetNumber.connect( self.OnEvent_SetNumber );

        if self.m_th_04 == None:
            self.m_th_04 = ThreadWorker( row = 3, parent = self );
            self.m_th_04.start();
            # 시그널 연결
            self.m_th_04.onEvent_ShowNumber.connect( self.OnEvent_ShowNumber );
            self.m_th_04.onEvent_SetNumber.connect( self.OnEvent_SetNumber );

        if self.m_th_05 == None:
            self.m_th_05 = ThreadWorker( row = 4, parent = self );
            self.m_th_05.start();
            # 시그널 연결
            self.m_th_05.onEvent_ShowNumber.connect( self.OnEvent_ShowNumber );
            self.m_th_05.onEvent_SetNumber.connect( self.OnEvent_SetNumber );

    # OnClick_CreateNumbers

# WindowClass

mutex = QMutex();

class ThreadWorker( QThread ):

    onEvent_ShowNumber = pyqtSignal( LottoViewModel );#생성한 로또 번호를 받아서 세팅한다.
    onEvent_SetNumber = pyqtSignal( LottoViewModel );#생성한 로또 번호를 받아서 세팅한다.

    """ 번호 생성용 스레드 """
    def __init__( self, row = 0, parent = None ):
        super().__init__();
        self.owner = parent;
        self.row = row;
        self.is_working = True;
    # __init__

    def __del__( self ):
        if self.is_working == True:
            self.wait();
        self.is_working = False;
    # __del__

    # 루프 돌면서 만족하는 결과 나올때 까지 실행
    def run( self ):
        # 값을 계속 돌려서 전주와 동일한 번호가 나올떄 까지 뽑는다.

        vm = LottoViewModel();
        nums = None;

        while self.is_working == True:

            mutex.lock();

            try:

                nums = self.owner.m_lotto.MakeLottoNumbers();

                vm.row = self.row;
                vm.col1 = nums[0];
                vm.col2 = nums[1];
                vm.col3 = nums[2];
                vm.col4 = nums[3];
                vm.col5 = nums[4];
                vm.col6 = nums[5];

                self.onEvent_ShowNumber.emit( vm );

                # 지난번 당첨 번호와 비교해서 동일 하다면, 루프를 멈춘다.
                if self.owner.m_lotto.CompareWithLastLottoNumbers( nums ):
                    self.is_working = False;

            finally:
                mutex.unlock();

        mutex.lock();
        try:
            #최종적으로 동일한 번호를 얻었기 때문에, 다음 값을 가져온다.
            nums = self.owner.m_lotto.MakeLottoNumbers();

            vm.row = self.row;
            vm.col1 = nums[0];
            vm.col2 = nums[1];
            vm.col3 = nums[2];
            vm.col4 = nums[3];
            vm.col5 = nums[4];
            vm.col6 = nums[5];

            # 값을 세팅한다.
            self.onEvent_SetNumber.emit( vm );
        finally:
            mutex.unlock();

        self.quit();
        self.wait( 5000 );  # 5000ms = 5s
    # run

# ThreadWorker

if __name__ == "__main__":
    #QApplication : 프로그램을 실행시켜주는 클래스
    app = QApplication( sys.argv );

    #WindowClass의 인스턴스 생성
    myWindow = WindowClass();

    #프로그램 화면을 보여주는 코드
    myWindow.show();

    #프로그램을 이벤트루프로 진입시키는(프로그램을 작동시키는) 코드
    app.exec_();