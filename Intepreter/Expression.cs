﻿namespace Intepreter;

struct Expression
{
    public int Priority;
    public Func<double, double, double>? BinaryMethod;
    public Func<double, double> UnaryMethod;
    public OperationType Type;
    
    public Expression(int priority, Func<double, double, double>? binaryMethod, Func<double, double> unaryMethod, OperationType type)
    {
        Priority = priority;
        BinaryMethod = binaryMethod;
        Type = type;
        UnaryMethod = unaryMethod;
    }
}