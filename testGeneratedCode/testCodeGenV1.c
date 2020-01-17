#include <stdlib.h>
#include <stdio.h>
#include <stdbool.h>
#include "testCodeGenV1.h"

// ID functions
long intId(long x);
double doubleId(double x);
bool boolId(bool x);

// add functions
long addIntInt(long x, long y);
double addIntDouble(long x, double y);
double addDoubleDouble(double x, double y);

// sub functions
long subIntInt(long x, long y);
double subIntDouble(long x, double y);
double subDoubleDouble(double x, double y);

// mult functions
long multIntInt(long x, long y);
double multIntDouble(long x, double y);
double multDoubleDouble(double x, double y);

// div functions
long divIntInt(long x, long y);
double divIntDouble(long x, double y);
double divDoubleDouble(double x, double y);

// greater functions
bool greaterIntInt(long x, long y);
bool greaterIntDouble(long x, double y);
bool greaterDoubleDouble(double x, double y);

// less functions
bool lessIntInt(long x, long y);
bool lessIntDouble(long x, long y);
bool lessDoubleDouble(double x, double y);

// equal functions
bool equalIntInt(long x, long y);
bool equalIntDouble(long x, double y);
bool equalDoubleDouble(double x, double y);

// if functions
long withoutElse(long x);
long withElse(long x);
long withoutElseReturning(long x);
long withElseReturning(long x);

// while functions
long returningWhile(long x);
long notReturningWhile(long x);

// addAssign functions
long addAssignIntInt(long x, long y);
double addAssignIntDouble(long x, double y);
double addAssignDoubleDouble(double x, double y);

// subAssign functions
long subAssignIntInt(long x, long y);
double subAssignIntDouble(long x, double y);
double subAssignDoubleDouble(double x, double y);

// multAssign functions
long multAssignIntInt(long x, long y);
double multAssignIntDouble(long x, double y);
double multAssignDoubleDouble(double x, double y);

// divAssign functions
long divAssignIntInt(long x, long y);
double divAssignIntDouble(long x, double y);
double divAssignDoubleDouble(double x, double y);

// increment functions
long incrementPostInt(long x);
long incrementPreInt(long x);
double incrementPostDouble(double x);
double incrementPreDouble(double x);

// decrement functions
long decrementPostInt(long x);
long decrementPreInt(long x);
double decrementPostDouble(double x);
double decrementPreDouble(double x);

// not operator functions
bool notOperator(bool x);

// and operator functions
bool andOperator(bool x, bool y);

// register overflows
long overFlowOnlyInt(long x, long y, long z, long a, long b, long c, long d, long e, long f);
double overFlowOnlyDouble(double x, double y, double z, double a, double b, double c, double d, double e, double f);
double overFlowIntMixed(long x, double y, long z, double a, long b, long c, long d, long e, long f, long g);
double overFlowDoubleMixed(double x, int y, double z, int a, double b, double c, double d, double e, double f);
double overFlowBoth(double x, double y, double z, double a, double b, double c, double d, double e, long f, long g, long h, long i, long j, long k, long l, long m, long n);

// recursion functions
long factorial(long x);

// multipleFunctionCalls
long callMultipleOthers(long x);

int failedCount = 0;
int overallCount = 0;

void printInt(long expected, long returned, char* funName)
{
    overallCount++;
    if(expected != returned)
    {
        printf("%s failed: expected %ld but returned %ld\n", funName, expected, returned);
        failedCount++;
    }
    else
        printf("%s passed\n", funName);
}

void printDouble(double expected, double returned, char* funName)
{
    overallCount++;
    if(expected != returned)
    {
        printf("%s failed: expected %f but returned %f\n", funName, expected, returned);
        failedCount++;
    }
    else
        printf("%s passed\n", funName);
}

void printBool(bool expected, bool returned, char* funName)
{
    overallCount++;
    if(expected != returned)
    {
        printf("%s failed: expected %d but returned %d\n", funName, expected, returned);
        failedCount++;
    }
    else
        printf("%s passed\n", funName);
}

void startTests()
{
    long intResult = 0;
    double doubleResult = 0.0;
    bool boolResult = false;

    intResult = intId(42);
    printInt(42, intResult, "intId");
    doubleResult = doubleId(42.0);
    printDouble(42.0, doubleResult, "doubleId");
    boolResult = boolId(true);
    printBool(true, boolResult, "boolId");

    intResult = addIntInt(17, 25);
    printInt(42, intResult, "addIntInt");
    doubleResult = addIntDouble(17, 25.5);
    printDouble(42.5, doubleResult, "addIntDouble");
    doubleResult = addDoubleDouble(13.2, 28.8);
    printDouble(42.0, doubleResult, "addDoubleDouble");

    intResult = subIntInt(27, 15);
    printInt(12, intResult, "subIntInt");
    doubleResult = subIntDouble(17, 3.2);
    printDouble(13.8, doubleResult, "subIntDouble");
    doubleResult = subDoubleDouble(7.7, 0.5);
    printDouble(7.2, doubleResult, "subDoubleDouble");

    intResult = multIntInt(3, 15);
    printInt(45, intResult, "multIntInt");
    doubleResult = multIntDouble(2, 5.5);
    printDouble(11.0, doubleResult, "multIntDouble");
    doubleResult = multDoubleDouble(0.5, 2.5);
    printDouble(1.25, doubleResult, "multDoubleDouble");

    intResult = divIntInt(3, 2);
    printInt(1, intResult, "divIntInt");
    doubleResult = divIntDouble(2, 0.5);
    printDouble(4, doubleResult, "divIntDouble");
    doubleResult = divDoubleDouble(0.5, 0.5);
    printDouble(1.0, doubleResult, "divDoubleDouble");

    boolResult = greaterIntInt(5, 4);
    printBool(true, boolResult, "greaterIntInt");
    boolResult = greaterIntDouble(5, 4.2);
    printBool(true, boolResult, "greaterIntDouble");
    boolResult = greaterDoubleDouble(7.2, 7.199999999);
    printBool(true, boolResult, "greaterDoubleDouble");

    boolResult = lessIntInt(2, 3);
    printBool(true, boolResult, "lessIntInt");
    boolResult = lessIntDouble(2, 3.5);
    printBool(true, boolResult, "lessIntDouble");
    boolResult = lessDoubleDouble(2.1999999, 2.2);
    printBool(true, boolResult, "lessDoubleDouble");

    boolResult = equalIntInt(3, 3);
    printBool(true, boolResult, "equalIntInt");
    boolResult = equalIntDouble(3, 3.0);
    printBool(true, boolResult, "equalIntDouble");
    boolResult = equalDoubleDouble(3.0, 3.0);
    printBool(true, boolResult, "equalDoubleDouble");

    intResult = withoutElse(42);
    printInt(17, intResult, "withoutElse");
    intResult = withoutElse(17);
    printInt(42, intResult, "withoutElse");
    intResult = withElse(42);
    printInt(17, intResult, "withElse");
    intResult = withElse(17);
    printInt(42, intResult, "withElse");
    intResult = withoutElseReturning(42);
    printInt(17, intResult, "withoutElseReturning");
    intResult = withoutElseReturning(17);
    printInt(42, intResult, "withoutElseReturning");
    intResult = withElseReturning(42);
    printInt(17, intResult, "withElseReturning");
    intResult = withElseReturning(17);
    printInt(42, intResult, "withElseReturning");

    intResult = returningWhile(42);
    printInt(42, intResult, "returningWhile");
    intResult = returningWhile(17);
    printInt(17, intResult, "returningWhile");
    intResult = notReturningWhile(17);
    printInt(17, intResult, "notReturningWhile");

    intResult = addAssignIntInt(17, 25);
    printInt(42, intResult, "addAssignIntInt");
    doubleResult = addAssignIntDouble(17, 25.5);
    printDouble(42.5, doubleResult, "addAssignIntDouble");
    doubleResult = addAssignDoubleDouble(16.5, 25.5);
    printDouble(42.0, doubleResult, "addAssignDoubleDouble");

    intResult = subAssignIntInt(17, 15);
    printInt(2, intResult, "subAssignIntInt");
    doubleResult = subAssignIntDouble(17, 15.5);
    printDouble(1.5, doubleResult, "subAssignIntDouble");
    doubleResult = subAssignDoubleDouble(17.2, 0.2);
    printDouble(17.0, doubleResult, "subAssignDoubleDouble");

    intResult = multAssignIntInt(3, 5);
    printInt(15, intResult, "multAssignIntInt");
    doubleResult = multAssignIntDouble(3, 5.5);
    printDouble(16.5, doubleResult, "multAssignIntDouble");
    doubleResult = multAssignDoubleDouble(0.5, 1.5);
    printDouble(0.75, doubleResult, "multAssignDoubleDouble");

    intResult = divAssignIntInt(3, 2);
    printInt(1, intResult, "divAssignIntInt");
    doubleResult = divAssignIntDouble(3, 0.5);
    printDouble(6.0, doubleResult, "divAssignIntDouble");
    doubleResult = divAssignDoubleDouble(0.5, 0.5);
    printDouble(1.0, doubleResult, "divAssignDoubleDouble");

    intResult = incrementPostInt(4);
    printInt(4, intResult, "incrementPostInt");
    intResult = incrementPreInt(4);
    printInt(5, intResult, "incrementPreInt");
    doubleResult = incrementPostDouble(2.5);
    printDouble(2.5, doubleResult, "incrementPostDouble");
    doubleResult = incrementPreDouble(2.5);
    printDouble(3.5, doubleResult, "incrementPreDouble");

    intResult = decrementPostInt(4);
    printInt(4, intResult, "decrementPostInt");
    intResult = decrementPreInt(4);
    printInt(3, intResult, "decrementPreInt");
    doubleResult = decrementPostDouble(2.5);
    printDouble(2.5, doubleResult, "decrementPostDouble");
    doubleResult = decrementPreDouble(2.5);
    printDouble(1.5, doubleResult, "decrementPreDouble");

    boolResult = notOperator(true);
    printBool(false, boolResult, "notOperator1");
    boolResult = notOperator(false);
    printBool(true, boolResult, "notOperator2");

    boolResult = andOperator(true, true);
    printBool(true, boolResult, "andOperatorTrueTrue");
    boolResult = andOperator(false, true);
    printBool(false, boolResult, "andOperatorFalseTrue");
    boolResult = andOperator(true, false);
    printBool(false, boolResult, "andOperatorTrueFalse");
    boolResult = andOperator(false, false);
    printBool(false, boolResult, "andOperatorFalseFalse");

    intResult = overFlowOnlyInt(1, 1, 1, 1, 1, 1, 1, 1, 1);
    printInt(9, intResult, "overFlowOnlyInt");
    doubleResult = overFlowOnlyDouble(1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0);
    printDouble(9.0, doubleResult, "overflowOnlyDouble");
    doubleResult = overFlowIntMixed(1, 1.0, 1, 1.0, 1, 1, 1, 1, 1, 1);
    printDouble(10.0, doubleResult, "overflowIntMixed");
    doubleResult = overFlowDoubleMixed(1.0, 1, 1.0, 1, 1.0, 1.0, 1.0, 1.0, 1.0);
    printDouble(9.0, doubleResult, "overflowDoubleMixed");
    doubleResult = overFlowBoth(1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1, 1, 1, 1, 1, 1, 1, 1, 1);
    printDouble(17.0, doubleResult, "overflowBoth");

    intResult = factorial(4);
    printInt(24, intResult, "factorial");

    intResult = callMultipleOthers(7);
    printInt(19, intResult, "callMultiple");

    int successCount = overallCount - failedCount;
    printf("\n\n%d tests of %d were successfull\n", successCount, overallCount);

    if(failedCount > 0)
        exit(EXIT_FAILURE);
}