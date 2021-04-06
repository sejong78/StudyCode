from PyQt5.QtCore import *;
from  PyQt5.QtWidgets import  *;
import sys;

class MyDialogForm( QDialog ):
    """ GUI 다이얼로그 Form """
    def __init__( self, parent = None ):
        super().__init__( parent );

        self.qtxt   = QTextEdit( self );
        self.qbtn1  = QPushButton( "Start", self );
        self.qbtn2  = QPushButton( "Stop", self );
        self.qbtn3  = QPushButton( "Add 100", self );
        self.qbtn4  = QPushButton( "Send instance", self );

        vbox = QVBoxLayout();
        vbox.addWidget( self.qtxt );
        vbox.addWidget( self.qbtn1 );
        vbox.addWidget( self.qbtn2 );
        vbox.addWidget( self.qbtn3 );
        vbox.addWidget( self.qbtn4 );

        self.setLayout( vbox );

        self.setGeometry( 100, 50, 300, 300 );
    # __init__

# MyDialog

class TestModel:
    """ 스레드 통신에 사용할 정보 모델 """
    def __init__( self ):
        self.msg = "";
    # __init__
# Test

class Worker( QThread ):
    """ 학습용 QThread 모듈 """
    qt_signal_second_changed = pyqtSignal( str );

    def __init__( self, sec = 0, parent = None ):
        super().__init__();
        self.main = parent;
        self.is_working = True;
        self.sec = sec;
    # __init__

    def __del__( self ):
        print( "... end thread ..." );
        if self.is_working == True:
            self.wait();
    # __del__

    def run( self ):
        """ 스레드 중요 업데이트 함수 """
        while self.is_working:
            self.qt_signal_second_changed.emit( f"time(secs) : {self.sec}" );
            self.sleep(1);
            self.sec += 1;
    # run

    @pyqtSlot()
    def add_sec( self ):
        print( "add_sec ..." );
        self.sec += 100;
    # add_sec

    @pyqtSlot( "PyQt_PyObject" )
    def recive_instance_signal( self, inst ):
        print( inst.msg );
    # recive_instance_signal

# Worker

class MyMain( MyDialogForm ):
    """ MyDialog 상속받은 제어클레스 """
    qt_signal_add_seconds = pyqtSignal();
    qt_signal_send_instance = pyqtSignal( "PyQt_PyObject" );

    def __init__( self, parent = None ):
        super().__init__( parent );

        self.qbtn1.clicked.connect( self.time_start );
        self.qbtn2.clicked.connect( self.time_stop );
        self.qbtn3.clicked.connect( self.add_sec );
        self.qbtn4.clicked.connect( self.send_instance );

        # Thread 할당
        self.myThread = Worker( parent = self );

        # Thread 에서 제어 클레스로 통신 - Thread signal 에 제어 클레스의 함수 연결
        self.myThread.qt_signal_second_changed.connect( self.time_update );

        # 제어클레스에서 Thread 로 통신 - 제어 클레스의 signal 에 Thread 함수 연결
        self.qt_signal_add_seconds.connect( self.myThread.add_sec );
        self.qt_signal_send_instance.connect( self.myThread.recive_instance_signal );

        # 다이얼로그 출력
        self.show();

    # __init__

    @pyqtSlot()
    def time_start( self ):
        self.myThread.start();
        self.myThread.is_working = True;
    # time_start

    @pyqtSlot()
    def time_stop( self ):
        self.myThread.is_working = False;
    # time_stop

    @pyqtSlot()
    def add_sec( self ):
        print( ".... add signal emit ...." );
        self.qt_signal_add_seconds.emit();
    # add_sec

    @pyqtSlot( str )
    def time_update( self, msg ):
        self.qtxt.append( msg );
    # time_update

    @pyqtSlot()
    def send_instance( self ):
        t1 = TestModel();
        t1.msg = "Super Power!!!";

        self.qt_signal_send_instance.emit( t1 );
    # send_instance

# MyMain

if __name__ == "__main__":
    app = QApplication( sys.argv );
    form = MyMain();
    app.exec_();
