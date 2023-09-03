import sys
from unittest import result
from PyQt5.QtWidgets import *
from PyQt5 import uic
import base64

#UI파일 연결
form_class = uic.loadUiType("encryption.ui")[0]

#화면을 띄우는데 사용되는 Class 선언
class WindowClass(QMainWindow, form_class) :
    def __init__(self) :
        super().__init__()
        self.setupUi(self)

        self.encryptionButton.clicked.connect( self.encryption )
        self.decryptionButton.clicked.connect( self.decryption )


    def encryption(self):
        # UI 에서 사용할 문자열들 가져오기
        key_text = self.inputKey.toPlainText()
        src_text = self.inputText.toPlainText()

        if len( key_text ) < 1 :
            return

        if len( src_text ) < 1 :
            return

        # 각 문자열 Byte 배열화
        key_64 = base64.b64encode( key_text.encode( "utf-8" ) )
        src_64 = base64.b64encode( src_text.encode( "utf-8" ) )

        key_array = bytearray( key_64 )
        src_array = bytearray( src_64 )


        # 각 배열 크기
        key_len = len( key_array )
        src_len = len( src_array )

        # 암호화 할 문장 배열 크기만큼 바이트배열 생성
        dest_array = bytearray( src_len )

        key_index = 0

        key     = 0
        dest    = 0

        # 카운터(CTR)모드를 기본으로 하여 키배열을 사용한다.
        for src_index in range( 0, src_len ):

            # 키 인덱스 설정
            key_index = src_index % key_len

            # 암호피드백(CFB)모드의 이전단계의 암호문을 다음단계의 암호화 키에 적용하는 방식을 추가하였다.
            key = key_array[ key_index ] ^ dest
            # 배타적논리합(XOR)명령
            dest_array[ src_index ] = src_array[ src_index ] ^ key

            # 암호피드백(CFB)모드의 암호화시에는 암호화된 문장인 dest 가 다음 문장의 복호화 키에 적용된다.
            dest = dest_array[ src_index ]

        # 암호화된 문장 출력
        result = dest_array.decode( "utf-8" )
        #result = dest_array.decode( "utf-8" )# base64.b64decode( dest_array )
        self.outputText.setText( result )

    def decryption(self):
        # UI 에서 사용할 문자열들 가져오기
        key_text = self.inputKey.toPlainText()
        src_text = self.inputText.toPlainText()

        if len( key_text ) < 1 :
            return

        if len( src_text ) < 1 :
            return

        # 각 문자열 Byte 배열화
        key_64 = base64.b64encode( key_text.encode( "utf-8" ) )
        #src_64 = base64.b64encode( src_text.encode( "utf-8" ) )

        key_array = bytearray( key_64 )
        src_array = bytearray( src_text.encode( "utf-8" ) )

        # 각 배열 크기
        key_len = len( key_array )
        src_len = len( src_array )

        # 복호화 할 문장 배열 크기만큼 바이트배열 생성
        dest_array = bytearray( src_len )

        key_index = 0

        key     = 0
        dest    = 0

        # 카운터(CTR)모드를 기본으로 하여 키배열을 사용한다.
        for src_index in range( 0, src_len ):

            # 키 인덱스 설정
            key_index = src_index % key_len

            # 암호피드백(CFB)모드의 이전단계의 암호문을 다음단계의 암호화 키에 적용하는 방식을 추가하였다.
            key = key_array[ key_index ] ^ dest

            # 배타적논리합(XOR)명령
            dest_array[ src_index ] = src_array[ src_index ] ^ key

            # 암호피드백(CFB)모드의 복호화시에는 암호화된 문장인 src 가 다음 문장의 복호화 키에 적용된다.
            dest = src_array[ src_index ]

        # 복호화된 문장 출력
        result = base64.b64decode( dest_array )
        self.outputText.setText( result.decode( "utf-8", "ignore" ) )

if __name__ == "__main__" :
    #QApplication : 프로그램을 실행시켜주는 클래스
    app = QApplication( sys.argv ) 

    #WindowClass의 인스턴스 생성
    myWindow = WindowClass() 

    #프로그램 화면을 보여주는 코드
    myWindow.show()

    #프로그램을 이벤트루프로 진입시키는(프로그램을 작동시키는) 코드
    app.exec_()