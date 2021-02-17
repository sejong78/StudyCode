"""
로또 번호 추출기 2.0.0
Python 버전
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

        self.m_default_number_list = list( range( 1, 46 ) );# 1 ~ 45 까지의 숫자 리스트

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
        # 버퍼를 섞어준다.
        self.m_rand.shuffle( self.m_default_number_list );
        # 그 중 6개를 추려서 정렬한다.
        nums = self.m_rand.sample( self.m_default_number_list, k = 6 );
        # 순서 정렬
        nums.sort();
        return nums;
    # MakeNumbers

#Lotto