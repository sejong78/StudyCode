"""
PyQt5 Tutorial
"""
import sys;
from PyQt5.QtWidgets import *;
from PyQt5.QtCore import *;
from PyQt5.QtGui import *;

class MyApp( QMainWindow ):
    def __init__(self):
        super().__init__();
        self.Init_ToolTip();
        self.Init_MenuBar();
        self.Init_StatusBar();
        self.Init_ToolBar();
        self.Init_Window();
        self.Init_Button();
        self.Init_ProgressBar();
        self.show();
    #__init__

    def Init_ToolTip( self ):
        """ 툴팀 설정 """
        QToolTip.setFont( QFont( "SansSerif", 10 ) );
    #Init_ToolTip

    def Init_StatusBar( self ):
        """ 상태바 설정 """
        self.status_bar = self.statusBar();
    #Init_StatusBar

    def Init_MenuBar( self ):
        """ 메뉴바 세팅 """
        exitAction = QAction( QIcon( "exit.png" ), 'Exit', self );
        exitAction.setShortcut( "Ctrl+Q" );
        exitAction.setStatusTip( "Exit application" );
        exitAction.triggered.connect( qApp.quit );

        self.menu_bar = self.menuBar();
        self.menu_bar.setNativeMenuBar( False );
        menu_file = self.menu_bar.addMenu( "&File" );
        menu_file.addAction( exitAction );
    #Init_MenuBar

    def Init_ToolBar( self ):
        """ 메뉴바 세팅 """
        exitAction = QAction( QIcon( "exit.png" ), 'Exit', self );
        exitAction.setShortcut( "Ctrl+Q" );
        exitAction.setStatusTip( "Exit application" );
        exitAction.triggered.connect( qApp.quit );

        self.tool_bar = self.addToolBar( "Exit" );
        self.tool_bar.addAction( exitAction );
    #Init_MenuBar

    def Init_Window( self ):
        """ 윈도우 설정 """

        self.setToolTip( "This is a <b>QMainWindow</b> widget" );
        self.setWindowTitle( "My First Application" );
        self.resize( 400, 200 );

        self.__move_center( self );
    # Init_Window

    def Init_Button( self ):
        """ 버튼 설정 """
        btn = QPushButton( "Quit", self );
        btn.setToolTip( "This is a <b>QPushButton</b> widget" );
        btn.move( 50, 50 );
        btn.resize( btn.sizeHint() );
        btn.clicked.connect( qApp.quit );
    #Init_Button

    def Init_ProgressBar( self ):
        self.p_bar = QProgressBar( self );
        self.p_bar.setGeometry( 80, 90, 200, 25 );

        self.p_btn = QPushButton( "Start", self );
        self.p_btn.setGeometry( 30, 90, 40, 25 );
        self.p_btn.clicked.connect( self.__DoProgressAction );

        self.p_timer = QBasicTimer();
        self.p_step = 0;

        self.p_lcd = QLCDNumber( self );
        self.p_lcd.setGeometry( 30, 120, 200, 50 );
        self.p_lcd.setDigitCount( 8 );#표현할 자릿수 88,888,888
        self.p_lcd.display( self.p_step );

        # Min 과 Max를 0으로 설정하면 게이지가 계속 흘러가는 상태가 된다.
        # 전체 크기를 알 수 없는 경우 유용하다.
        #self.p_bar.setMinimum( 0 );
        #self.p_bar.setMaximum( 0 );
    #Init_ProgressBar

    def timerEvent( self, *args, **kwargs ):
        """ 이벤트 등록이 안되어 있어도, QBasicTimer 가 기동 되었을때 부터 설정된 Term 마다 불린다.
            때문에 sender 를 파악할 수 없다.
        """
        if self.p_step >= 100:
            self.p_timer.stop();
            self.p_step = 0;
            self.p_btn.setText( "Finished" );
            return;

        self.p_step += 1;
        self.p_bar.setValue( self.p_step );
        self.p_lcd.display( self.p_step );
    #timerEvent

    def __DoProgressAction( self ):
        if self.p_timer.isActive():
            self.p_timer.stop();
            self.p_btn.setText( "Start" );
        else:
            self.p_timer.start( 200, self );
            self.p_btn.setText( "Stop" );
    #__DoProgressAction

    def __move_center( self, obj:QWidget ):
        """QWidget 을 중앙으로 보낸다. """
        qr = obj.frameGeometry();
        cp = QDesktopWidget().availableGeometry().center();
        qr.moveCenter( cp );
        obj.move( qr.topLeft() );
    #__move_center

    def set_status( self, msg:str ):
        """ 스테터스 텍스트 세팅 """
        self.status_bar.showMessage( msg );
    #set_status

#MyApp

def add_styleVBox( app:MyApp ):

    qwg = QWidget();
    vbox = QVBoxLayout();

    lbl_red = QLabel( "RED" );
    lbl_green = QLabel( "GREEN" );
    lbl_blue = QLabel( "BLUE" );

    lbl_red.setStyleSheet( "color: red;"
                           "border-style: solid;"
                           "border-width: 2px;"
                           "border-color: #FA8072;"
                           "border-radius: 3px" );

    lbl_green.setStyleSheet( "color: green;"
                             "background-color: #7FFFD4" );

    lbl_blue.setStyleSheet( "color: blue;"
                            "background-color: #87CEFA;"
                            "border-style: dashed;"
                            "border-width: 3px;"
                            "border-color: #1E90FF" );

    vbox.addWidget( lbl_red );
    vbox.addWidget( lbl_green );
    vbox.addWidget( lbl_blue );

    qwg.setLayout( vbox );

    app.setCentralWidget( qwg );

#add_styleVBox

def run():
    app = QApplication( sys.argv );
    ex = MyApp();
    #add_styleVBox( ex );
    ex.set_status( "I'm Ready" );
    sys.exit( app.exec_() );
#run

if __name__ == "__main__":
    run();