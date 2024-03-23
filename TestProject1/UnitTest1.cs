using Intepreter;


namespace InterpreterTests
{
    [TestClass]
    public class InterpreterTest
    {

        public ExpressionInterpreter interpreter = new ExpressionInterpreter();

        [DataTestMethod]
        [DataRow("1 +* 1")]
        [DataRow("1 +, 1")]
        [DataRow("1,,5 + 1")]
        [DataRow("1, ,5 + 1")]
        public void InterpreterExceptionTest(string actualExc)
        {
            Assert.ThrowsException<KeyNotFoundException>(() => interpreter.Interpret(actualExc));
        }

        [DataTestMethod]
        [DataRow("2**8/2", 128)]
        [DataRow("2**sin( 2 ** ( tan( 5 + 2 * 3 / 5) ) )", 1.752992741799427)]
        [DataRow("sin(90)", 0.8939966636005579)]
        [DataRow("sin(0)", 0)]
        [DataRow("cos(0)", 1)]
        [DataRow("tan(0)", 0)]
        [DataRow("sinh(0)", 0)]
        [DataRow("cosh(0)", 1)]
        [DataRow("tanh(0)", 0)]
        [DataRow("2**2.5", 5.656854249492381)]
        public void InterCalculationTest(string actual, double expected)
        {
            Assert.AreEqual(expected, interpreter.Interpret(actual));
        }

        [DataTestMethod]
        [DataRow("1**5 + 1", 2)]
        [DataRow("1 + 1", 2)]
        [DataRow("1 + 0", 1)]
        [DataRow("1 + 1 + 1", 3)]
        [DataRow("1 + 2", 3)]
        [DataRow("1 + 1000000000", 1000000001)]
        [DataRow("1 - 1", 0)]
        [DataRow("1 * 4", 4)]
        [DataRow("4 / 2", 2)]
        [DataRow("2 ** 2", 4)]
        [DataRow("2 ** 2 ** 2 ** 2", 256)]
        public void InterTest(string actual, int expected)
        {
            Assert.AreEqual(expected, interpreter.Interpret(actual));
        }

        
        [DataTestMethod]
        [DataRow("2,5<3,5", 1)]
        [DataRow("2,5<=3,5", 1)]
        [DataRow("3,5<2,5", 0)]
        [DataRow("3,5>=2,5", 1)]
        [DataRow("1&&0", 0)]
        [DataRow("1&&1", 1)]
        [DataRow("1||0", 1)]
        [DataRow("1||1", 1)]
        [DataRow("1&0", 0)]
        [DataRow("1&1", 1)]
        [DataRow("1|0", 1)]
        [DataRow("1|1", 1)]
        [DataRow("1==0", 0)]
        [DataRow("1==1", 1)]
        [DataRow("1!=0", 1)]
        [DataRow("1!=1", 0)]
        [DataRow("!1", 0)]
        [DataRow("!0", 1)]
        [DataRow("!1 && !1", 0)]
        [DataRow("! (1 && 0)", 1)]
        [DataRow("0->1", 1)]
        [DataRow("1->0", 0)]
        [DataRow("0->0", 1)]
        [DataRow("1->1", 1)]
        [DataRow("1^0", 1)]
        [DataRow("0^1", 1)]
        [DataRow("1^1", 0)]
        [DataRow("0^0", 0)]
        public void InterpretLogicalTests(string expression, int expected)
        {
            var actual = interpreter.Interpret(expression);
            Assert.AreEqual(expected, actual);
        }
    }
}