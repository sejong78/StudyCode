"""
로또 번호 추출기 2.1.0
Python 버전
shuffle 방식에서 Pick 방식으로 바꿈
"""
import random as rand;
import time as t;

class LottoModule( object ):
    """ 로또 번호 생성기 """

    def __init__( self ):
        self.__InitializeValue();# 초기화 및 번수 선언
    #__init__

    def __InitializeValue( self ):
        """ 초기화 및 번수 선언 """
        self.m_rand = rand.Random(); # rand 초기화
        self.m_rand.seed( t.time() ); # seed 할당

        #self.m_default_number_list = list( range( 1, 46 ) );# 1 ~ 45 까지의 숫자 리스트
        self.m_picked_number_list = [0, 0, 0, 0, 0, 0];# 6개의 숫자버퍼

        self.m_last_index = 0;# 지난 로또 회차
        self.m_last_numbers = None;
    #__InitializeValue

    def SetLastLottoInfo( self, idx:int, val1:int, val2:int, val3:int, val4:int, val5:int, val6:int ):
        # 지난 회차 정보 세팅
        self.m_last_index = idx;
        self.m_last_numbers = [ val1, val2, val3, val4, val5, val6 ];
    #SetLastLottoInfo

    def CompareWithLastLottoNumbers( self, num_list:list ):
        """ 지난 로또번호와 비교한다"""
        return self.m_last_numbers == num_list;
    #CompareWithLastLottoNumbers

    def MakeLottoNumbers( self ):
        """ 로또 번호를 생성한다. todo: make return (num1,num2,num3...num6)"""

        for idx in range( 0, 6 ): # 0 ~ 5
            self.m_picked_number_list[ idx] = 0;

        for idx in range( 0, 6 ): # 0 ~ 5
            while self.m_picked_number_list[ idx] == 0:
                pickNum = self.m_rand.randint( 1, 45 );
                if False == self.IsContainNumber( pickNum ):
                    self.m_picked_number_list[idx] = pickNum;

        # 순서 정렬
        self.m_picked_number_list.sort();

        return self.m_picked_number_list;
    # MakeNumbers

    def IsContainNumber( self, num:int ):
        """m_default_number_list 안에 동일한 숫자가 있는가?"""
        for i in self.m_picked_number_list:
            if i == num:
                return True;
        return False;
    #IsContainNumber
#Lotto