#include <stdio.h>
#include <time.h>
#include <stdint.h>
 
int main(void)
{
    //             72057594037927935 <- segfault
    //             67553994410557439
    time_t epoch = 2147483647;
    printf("%lu\n", sizeof(epoch));
    printf("%jd seconds since the epoch began\n", (intmax_t)epoch);
    printf("%s", asctime(gmtime(&epoch)));
    // while (1) {
    //     epoch += 1;
    //     printf("%jd %s", (intmax_t)epoch,  asctime(gmtime(&epoch)));
    // }
}