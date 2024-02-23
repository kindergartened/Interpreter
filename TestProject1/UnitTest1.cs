using Intepreter;


namespace InterpreterTests
{
    [TestClass]
    public class InterpreterTest
    {
        [TestMethod]

        public void InterTest()
        {
            ExpressionInterpreter interpreter = new ExpressionInterpreter();
            string actual = "1 + 1";
            int expected = 2;
            Assert.AreEqual(expected, interpreter.Interpret(actual));
            string actual2 = "1 + 0";
            int expected2 = 1;
            Assert.AreEqual(expected2, interpreter.Interpret(actual2));
            string actual3 = "1 + 1 + 1";
            int expected3 = 3;
            Assert.AreEqual(expected3, interpreter.Interpret(actual3));
            string actual4 = "1 + 2      ";
            int expected4 = 3;
            Assert.AreEqual(expected4, interpreter.Interpret(actual4));
            string actual5 = "1 + 1000000000";
            int expected5 = 1000000001;
            Assert.AreEqual(expected5, interpreter.Interpret(actual5));

        }
        [TestMethod]
        public void PolskaKurwatest()
        {
            ExpressionInterpreter interpreter = new ExpressionInterpreter();
            string actual = "1 + 1";
            string expected = "1 1 +";
            Assert.AreEqual(expected, interpreter.ConvertToPostfix(actual));
        }
        [TestMethod]
        public void EvaluatePostfixTest()
        {
            ExpressionInterpreter interpreter = new ExpressionInterpreter();
            string actual = "1 1 +";
            int expected = 2;
            Assert.AreEqual(expected, interpreter.EvaluatePostfix(actual));
        }
    }
}