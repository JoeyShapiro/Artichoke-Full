from datetime import datetime
import pandas as pd
import time
import sys

# df = pd.DataFrame({'old_date': [20180501, 20181230, 20181001, 9223372036854775807]})

# df['new_date'] = pd.to_datetime(df['old_date'].astype(str), format='%Y%m%d')

# 67767976233554399
# 9223372036854775807

print(pd.to_datetime(9223372036854775807, unit='ns'))
print(pd.to_datetime(67767976233554399, unit='ns'))
print(time.time())
print(time.ctime(time.time()))
print(time.ctime(int(time.time())))
print(time.ctime(67767976233554399))
print()
newtime = 67767976233554399 # max python time
inc = 1

while True:
    newtime += inc
    print(newtime, time.ctime(newtime))
    if '-' in time.ctime(newtime):
        break

