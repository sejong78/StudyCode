import sys
from PyQt5.QtWidgets import *
from PyQt5 import uic

#UI파일 연결
#단, UI파일은 Python 코드 파일과 같은 디렉토리에 위치해야한다.
ui_lotto_frame = uic.loadUiType("LottoFrame.ui")[0];

#화면을 띄우는데 사용되는 Class 선언
class WindowClass( QMainWindow, ui_lotto_frame ):
    def __init__( self ):
        super().__init__();
        self.setupUi( self );
        #선택 버튼 연결
        self.m_btn_create_numbers.clicked.connect( self.OnClick_CreateNumbers );
        #각 LCD를 줄단위 묶어줌
        self.m_numbers = [ [self.m_number_00, self.m_number_01, self.m_number_02, self.m_number_03, self.m_number_04, self.m_number_05 ],
         [self.m_number_10, self.m_number_11, self.m_number_12, self.m_number_13, self.m_number_14, self.m_number_15 ],
         [self.m_number_20, self.m_number_21, self.m_number_22, self.m_number_23, self.m_number_24, self.m_number_25 ],
         [self.m_number_30, self.m_number_31, self.m_number_32, self.m_number_33, self.m_number_34, self.m_number_35 ],
         [self.m_number_40, self.m_number_41, self.m_number_42, self.m_number_43, self.m_number_44, self.m_number_45 ]];
        self.m_numbers_0 = [self.m_number_00, self.m_number_01, self.m_number_02, self.m_number_03, self.m_number_04, self.m_number_05 ];
        self.m_numbers_1 = [self.m_number_10, self.m_number_11, self.m_number_12, self.m_number_13, self.m_number_14, self.m_number_15 ];
        self.m_numbers_2 = [self.m_number_20, self.m_number_21, self.m_number_22, self.m_number_23, self.m_number_24, self.m_number_25 ];
        self.m_numbers_3 = [self.m_number_30, self.m_number_31, self.m_number_32, self.m_number_33, self.m_number_34, self.m_number_35 ];
        self.m_numbers_4 = [self.m_number_40, self.m_number_41, self.m_number_42, self.m_number_43, self.m_number_44, self.m_number_45 ];

    def OnClick_CreateNumbers( self ):
        for i in range(6):
            self.m_numbers[0][i].display( 10 + i );
            self.m_numbers[1][i].display( 20 + i );
            self.m_numbers[2][i].display( 30 + i );
            self.m_numbers[3][i].display( 40 + i );
            self.m_numbers[4][i].display( 50 + i );


if __name__ == "__main__":
    #QApplication : 프로그램을 실행시켜주는 클래스
    app = QApplication( sys.argv );

    #WindowClass의 인스턴스 생성
    myWindow = WindowClass();

    #프로그램 화면을 보여주는 코드
    myWindow.show();

    #프로그램을 이벤트루프로 진입시키는(프로그램을 작동시키는) 코드
    app.exec_();