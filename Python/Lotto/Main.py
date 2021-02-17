import Lotto as lt;


def get_new_lotto_numbers( lotto:lt.Lotto ):
    """ 이번주 로또 번호를 생성한다. """

    # 값을 계속 돌려서 전주와 동일한 번호가 나올떄 까지 뽑는다.
    while not lotto.CompareWithLastLottoNumbers( lotto.MakeLottoNumbers() ):
        pass;

    # 지난번꺼랑 같아 졌으니 이번주 번호를 리턴
    return lotto.MakeLottoNumbers();
#get_new_lotto_numbers

def run():
    #로또 객체 생성
    lotto = lt.Lotto();

    # todo: 이후 웹크롤링을 통해 지난 번호를 가져와야 한다.
    lottoNums = lotto.MakeLottoNumbers();
    lotto.SetLastLottoInfo( 1, lottoNums[0], lottoNums[1], lottoNums[2], lottoNums[3], lottoNums[4], lottoNums[5] );

    lottoNums_1 = get_new_lotto_numbers( lotto );
    lottoNums_2 = get_new_lotto_numbers( lotto );
    lottoNums_3 = get_new_lotto_numbers( lotto );
    lottoNums_4 = get_new_lotto_numbers( lotto );
    lottoNums_5 = get_new_lotto_numbers( lotto );

    # todo: 메세지를 텔레그램 푸시
    print( f"{ lottoNums_1 }\n{ lottoNums_2 }\n{ lottoNums_3 }\n{ lottoNums_4 }\n{ lottoNums_5 }" );

#run

if __name__ == "__main__":
    run();

