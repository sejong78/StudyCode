"""
Web Crawling 을 위한 모듈
beautifulsoup4 을 사용한다.
라이브러리 설치는 아래 윈도우중  Terminal 에서 pip install requests beautifulsoup4 --upgrade 을 실행
"""

from urllib.request import urlopen as URL;
from bs4 import BeautifulSoup as BS;


class WebCrawlerModule( object ):
    """ beautifulsoup4 를 사용한 웹 크롤링 모듈 클레스 """

    def __init__( self ):
        self.__InitializeValue();# 초기화 및 번수 선언
    #__init__

    def __InitializeValue( self ):
        """ 초기화 및 번수 선언 """
        pass;
    #__InitializeValue

    def GetHtml( self, url:str ):
        html = URL( url );
        bsObject = BS( html, "html.parser" )

        print( bsObject );

        return bsObject;
        #GetHtml

#WebCrawler