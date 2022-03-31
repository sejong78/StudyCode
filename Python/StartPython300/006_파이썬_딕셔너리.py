
temp = {};
print( type(temp) );

ice_cream = { "메로나": 1000, "폴라포": 1200, "빵빠레": 1800 };
print( ice_cream, type(ice_cream) );

ice_cream[ "죠스바" ] = 1200;
ice_cream[ "월드콘" ] = 1500;
print( ice_cream, type(ice_cream) );

print( f"메로나 가격 = {ice_cream['메로나']}" );

ice_cream["메로나"] = 1300;
print( f"메로나 가격 = {ice_cream['메로나']}" );

del ice_cream["메로나"];
print( ice_cream, type(ice_cream) );

inventory = { "메로나": [300,20], "비비빅": [400, 3], "죠스바": [250,100] };
print( f"{inventory['메로나'][0]}원 {inventory['메로나'][1]}개" );

inventory[ "월드콘" ] = [500, 7];
print( inventory, type(inventory) );

key_List = inventory.keys();
print( key_List );

ice_cream.update( inventory );
print( ice_cream );

keys = ("apple", "pear", "peach");
vals = [300, 250, 400];
result = dict( zip( keys, vals ) );
print( result );

stock = { "시가": [100, 200, 300], "종가": [80, 210, 330] };
print( stock );

