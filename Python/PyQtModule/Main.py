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
        self.setToolTip( "This is a <b>QWidget</b> widget" );
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

def run():
    app = QApplication( sys.argv );
    ex = MyApp();
    ex.set_status( "I'm Ready" );
    sys.exit( app.exec_() );
#run

if __name__ == "__main__":
    run();