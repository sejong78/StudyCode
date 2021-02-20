import LottoModule as LM;
import WebCrawlerModule as WM;
import TelegramModule as TM;
import re as STR_RE;
import TimeModule as TIME;

def get_new_lotto_numbers( lotto:LM.LottoModule ):
    """ 이번주 로또 번호를 생성한다. """

    start_time : float = TIME.time();
    total_time : float = 0.0;
    elapse_time : float = 0.0;

    # 값을 계속 돌려서 전주와 동일한 번호가 나올떄 까지 뽑는다.
    while not lotto.CompareWithLastLottoNumbers( lotto.MakeLottoNumbers() ):
        elapse_time = TIME.time() - start_time;
        if 60 < elapse_time:
            total_time += elapse_time;
            start_time = TIME.time();
            print( f"번호 생성을 시작한지 {TIME.convert_seconds_to_kor_time( total_time )} 지났습니다." );

    print( f"{TIME.convert_seconds_to_time( total_time )} 만에 번호생성을 완료했습니다." );
    # 지난번꺼랑 같아 졌으니 이번주 번호를 리턴
    return lotto.MakeLottoNumbers();
#get_new_lotto_numbers

def parse_last_info( lastinfo:str ):
    """ex>동행복권 950회 당첨번호 3,4,15,22,28,40+10. 1등 총 8명, 1인당 당첨금액 3,281,920,500원."""
    numbers = STR_RE.findall( "\d+", lastinfo );

    return int(numbers[0]), int(numbers[1]), int(numbers[2]), int(numbers[3]), int(numbers[4]), int(numbers[5]), int(numbers[6]);
#parse_last_info

def run():

    #웹 크롤링을 통한 지난 시즌 당첨번호
    webcrawler = WM.WebCrawlerModule();
    crawled_html = webcrawler.GetHtml( "https://dhlottery.co.kr/gameResult.do?method=byWin" );
    find_msgs = crawled_html.head.find( "meta", { "name": "description" } ).get( 'content' );
    print( f"{find_msgs}" );

    last_index, last_num1, last_num2, last_num3, last_num4, last_num5, last_num6 = parse_last_info( find_msgs ); #문장 내용중 필요내용 파싱
    print( f"last_index = {last_index}, last_num1 = {last_num1}, last_num2 = {last_num2}, last_num3 = {last_num3}, last_num4 = {last_num4}, last_num5 = {last_num5}, last_num6 = {last_num6}" );

    #텔레그램 객체 생성
    telegram = TM.TelegramModule();

    #지난주 정보 푸시
    msg = f"지난 {last_index}회차 로또 당첨번호\n {last_num1}, {last_num2}, {last_num3}, {last_num4}, {last_num5}, {last_num6}\n를 바탕으로 로또 번호 생성 합니다.";
    telegram.SendMessageToGroupLotto( msg );

    #로또 객체 생성
    lotto = LM.LottoModule();

    # 웹크롤링을 통해 지난 번호를 가져와서 세팅
    lotto.SetLastLottoInfo( last_index, last_num1, last_num2, last_num3, last_num4, last_num5, last_num6 );

    # 이번주 번호 로직 동작
    print( f"첫번째 번호생성을 시작합니다." );
    lottoNums_1 = get_new_lotto_numbers( lotto );
    print( f"두번째 번호생성을 시작합니다." );
    lottoNums_2 = get_new_lotto_numbers( lotto );
    print( f"세번째 번호생성을 시작합니다." );
    lottoNums_3 = get_new_lotto_numbers( lotto );
    print( f"네번째 번호생성을 시작합니다." );
    lottoNums_4 = get_new_lotto_numbers( lotto );
    print( f"다섯번째 번호생성을 시작합니다." );
    lottoNums_5 = get_new_lotto_numbers( lotto );

    msg = f"로또 번호 생성이 완료 되었습니다.\n{ lottoNums_1 }\n{ lottoNums_2 }\n{ lottoNums_3 }\n{ lottoNums_4 }\n{ lottoNums_5 }\n행운을 빕니다!";
    telegram.SendMessageToGroupLotto( msg );

#run

if __name__ == "__main__":
    run();

