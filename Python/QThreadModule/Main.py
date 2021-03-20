from PyQt5.QtCore import *;
from  PyQt5.QtWidgets import  *;
import sys;

class MyDialog( QDialog ):
    """ GUI 다이얼로그  """
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

class Test:
    def __init__( self ):
        name = "";
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
        self.wait();
    # __del__

    def run( self ):
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
        print( inst.name );
    # recive_instance_signal

# Worker

class MyMain( MyDialog ):
    """ MyDialog 상속받은 제어클레스 """
    qt_signal_add_seconds = pyqtSignal();
    qt_signal_send_instance = pyqtSignal( "PyQt_PyObject" );

    def __init__( self, parent = None ):
        super().__init__( parent );

        self.qbtn1.clicked.connect( self.time_start );
        self.qbtn2.clicked.connect( self.time_stop );
        self.qbtn3.clicked.connect( self.add_sec );
        self.qbtn4.clicked.connect( self.send_instance );

        self.thread = Worker( parent = self );
        self.thread.qt_signal_second_changed.connect( self.time_update );

        self.qt_signal_add_seconds.connect( self.thread.add_sec );
        self.qt_signal_send_instance.connect( self.thread.recive_instance_signal );

        self.show();

    # __init__

    @pyqtSlot()
    def time_start( self ):
        self.thread.start();
        self.thread.is_working = True;
    # time_start

    @pyqtSlot()
    def time_stop( self ):
        self.thread.is_working = False;
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
        t1 = Test();
        t1.name = "Super Power!!!";

        self.qt_signal_send_instance.emit( t1 );
    # send_instance

# MyMain

if __name__ == "__main__":
    app = QApplication( sys.argv );
    form = MyMain();
    app.exec_();
