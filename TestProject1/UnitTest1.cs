using Intepreter;


namespace InterpreterTests
{
    [TestClass]
    public class InterpreterTest
    {
        
        public ExpressionInterpreter interpreter = new ExpressionInterpreter();
        public string actualExc;
        public string actual;
        public int expected;
        public double expectedDouble;
        [TestMethod]
        public void InterTest1()
        {
            actualExc = "1 +* 1";
            Assert.ThrowsException<KeyNotFoundException>(() => interpreter.Interpret(actualExc));
        }
        [TestMethod]
        public void InterTest2()
        {
            actualExc = "1 +, 1";
            Assert.ThrowsException<KeyNotFoundException>(() => interpreter.Interpret(actualExc));
        }
        [TestMethod]
        public void InterTest3()
        {
            actualExc = "1,,5 + 1";
            Assert.ThrowsException<KeyNotFoundException>(() => interpreter.Interpret(actualExc));
        }
        [TestMethod]
        public void InterTest4()
        {
            actualExc = "1, ,5 + 1";
            Assert.ThrowsException<KeyNotFoundException>(() => interpreter.Interpret(actualExc));
        }
        [TestMethod]
        public void InterTest()
        {
            string actual11 = "2**8/2";
            int expected11 = 128;
            Assert.AreEqual(expected11, interpreter.Interpret(actual11));
            string actual12 = "2**sin(2**(tan(5+2*3/5)))";
            double expected12 = 1.75299;
            Assert.AreEqual(expected12, interpreter.Interpret(actual12));
            string actual13 = "sin(90)";
            double expected13 = 0.893997;
            Assert.AreEqual(expected13, interpreter.Interpret(actual13));
            string actual14 = "sin(0)";
            double expected14 = 0;
            Assert.AreEqual(expected14, interpreter.Interpret(actual14));
            string actual15 = "cos(0)";
            double expected15 = 1;
            Assert.AreEqual(expected15, interpreter.Interpret(actual15));
            string actual16 = "tan(0)";
            double expected16 = 0;
            Assert.AreEqual(expected16, interpreter.Interpret(actual16));
            string actual17 = "cot(0)";
            double expected17 = 1;
            Assert.AreEqual(expected17, interpreter.Interpret(actual17));
            string actual18 = "sinh(0)";
            double expected18 = 0;
            Assert.AreEqual(expected18, interpreter.Interpret(actual18));
            string actual19 = "cosh(0)";
            double expected19 = 1;
            Assert.AreEqual(expected19, interpreter.Interpret(actual19));
            string actual20 = "tanh(0)";
            double expected20 = 0;
            Assert.AreEqual(expected20, interpreter.Interpret(actual20));
            string actual21 = "2**2.5";
            double expected21 = 5.65685;
            Assert.AreEqual(expected21, interpreter.Interpret(actual21));
        }
        public void InterTest5()
        {
            actual = "1* *5 + 1";
            expectedDouble = 2;
            Assert.AreEqual(expectedDouble, interpreter.Interpret(actual));
        }
        [TestMethod]
        public void InterTest6()
        {
            actual = "1 +       1";
            expected = 2;
            Assert.AreEqual(expected, interpreter.Interpret(actual));
        }
        [TestMethod]
        public void InterTest7()
        {
            actual = "1     + 0";
            expected = 1;
            Assert.AreEqual(expected, interpreter.Interpret(actual));
        }
        [TestMethod]
        public void InterTest8()
        {
            actual = "     1     +     1     +     1     ";
            expected = 3;
            Assert.AreEqual(expected, interpreter.Interpret(actual));
        }
        [TestMethod]
        public void InterTest9()
        {
            actual = "1+2      ";
            expected = 3;
            Assert.AreEqual(expected, interpreter.Interpret(actual));
        }
        [TestMethod]
        public void InterTest10()
        {
            actual = "1 + 1000000000";
            expected = 1000000001;
            Assert.AreEqual(expected, interpreter.Interpret(actual));
        }
        [TestMethod]
        public void InterTest11()
        {
            actual = "1 - 1";
            expected = 0;
            Assert.AreEqual(expected, interpreter.Interpret(actual));
        }
        [TestMethod]
        public void InterTest12()
        {
            actual = "1 * 4";
            expected = 4;
            Assert.AreEqual(expected, interpreter.Interpret(actual));
        }
        [TestMethod]
        public void InterTest13()
        {
            actual = "4/2";
            expected = 2;
            Assert.AreEqual(expected, interpreter.Interpret(actual));
        }
        [TestMethod]
        public void InterTest14()
        {
            actual = "2**2";
            expected = 4;
            Assert.AreEqual(expected, interpreter.Interpret(actual));
        }
        [TestMethod]
        public void InterTest15()
        {
            actual = "2**2**2**2";
            expected = 256;
            Assert.AreEqual(expected, interpreter.Interpret(actual));
        }
        
    }
}