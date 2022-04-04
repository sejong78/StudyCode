
class Human:
    def __init__(self, name, age, sex):
        self.SetInfo(name, age, sex);

    def Who(self):
        print( f"이름: {self.name}, 나이: {self.age}, 성별: {self.sex}" );

    def SetInfo(self, name, age, sex):
        self.name   = name;
        self.age    = age;
        self.sex    = sex;

    def __del__(self):
        print( "나의 죽음을 알리지 마라" );

areum = Human( "조아름", 25, "여자" ); 
print( areum.name );

print( "" );
areum.Who();

areum.SetInfo( "조아름", 16, "여자" );
print( "" );
areum.Who();

print( "" );
del areum;

class OMG:
    def print():
        print( "Oh! My God!!" );

print( "" );
mystock = OMG();
OMG.print();
