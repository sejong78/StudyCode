import random;

class Account:
    
    count = 0;

    def __init__(self, name, balance):
        self.name = name;
        self.balance = balance;
        self.set_bank_name();
        self.set_account_number();
        Account.count += 1;

    def set_bank_name(self):
        self.bank_name = "SC은행";

    def set_account_number(self):
        self.account_number = f"{str(self.get_random_number(0, 999)).zfill( 3 )}-{str(self.get_random_number(0, 99)).zfill( 2 )}-{str(self.get_random_number(0, 999999)).zfill( 6 )}";
        
    def get_random_number( self, start, max ):
        return random.randrange( start, max + 1 );
        
    def deposit( self, amount ):
        if 0 < amount:
            self.balance += amount;

    def withdraw( self, amount ):
        if amount <= self.balance:
            self.balance -= amount;

    def show(self):
        print( f"예금주 : {self.name}" );
        print( f"은행이름 : {self.bank_name}" );
        print( f"계좌번호 : {self.account_number}" );
        print( f"잔액 : {self.balance}" );

    @classmethod
    def account_num():
        return Account.count;

a = Account( "이름", 100 );
a.show();

print( "" );
b = Account( "이름2", 200 );
b.show();

print( "" );
print( f"Account Number = {Account.account_num()}" );
