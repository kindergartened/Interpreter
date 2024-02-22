namespace Intepreter;

struct Expression
{
    public int priority;
    public Func<double, double, double> method;
    
    public Expression(int priority, Func<double, double, double> method)
    {
        this.priority = priority;
        this.method = method;
    }
}