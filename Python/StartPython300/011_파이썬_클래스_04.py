class 차:
    def __init__(self, 바퀴, 가격):
        self.바퀴 = 바퀴;
        self.가격 = 가격;

    def 정보(self):
        print( f"바퀴 = {self.바퀴}, 가격 = {self.가격}" );

car = 차(2, 1000);
car.정보();

print("");

class 자전차(차):
    def __init__(self, 바퀴, 가격, 구동계):
        super().__init__(바퀴, 가격);
        self.구동계 = 구동계;

    def 정보(self):
        print( f"바퀴 = {self.바퀴}, 가격 = {self.가격}, 구동계 = {self.구동계}" );

bicycle = 자전차(2, 100, "시마노");
bicycle.정보();

print("");

