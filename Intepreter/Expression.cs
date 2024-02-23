namespace Intepreter;

struct Expression
{
    public int priority;
    public Func<double, double, double>? method;
    public Func<double, double>? unaryMethod;
    public OperationType type;
    
    public Expression(int priority, Func<double, double, double>? method, Func<double, double>? unaryMethod, OperationType type)
    {
        this.priority = priority;
        this.method = method;
        this.type = type;
        this.unaryMethod = unaryMethod;
    }
}