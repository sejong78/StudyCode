import Lotto as lt;

def run():
    lotto = lt.Lotto();
    lottoNums = lotto.MakeLottoNumbers();
    lotto.SetLastLottoInfo( 1, lottoNums[0], lottoNums[1], lottoNums[2], lottoNums[3], lottoNums[4], lottoNums[5] );

    print( f"{lotto.CompareWithLastLottoNumbers( lottoNums )}" );

#run

if __name__ == "__main__":
    run();

