using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

namespace Intepreter;

public class TruthTable : VariablesExpressionInterpreter
{
    private Table<int>? _table;
    public Table<int>? Table => _table;

    public TruthTable(Dictionary<string, double> variables) : base(variables)
    {
        _table = null;
    }

    /// <summary>
    /// Построение таблицы истинности.
    /// Состоит из вызова двух вспомогательных методов.
    ///     1. MakeVariablesHeaders. Заполнение массива заголовков таблицы
    ///     2. MakeVariables_table.Matrix. Заполнение тела таблицы (двумерного массива) 
    /// </summary>
    /// <param name="expression">Выражение</param>
    /// <returns>Таблица истинности</returns>
    public TruthTable BuildTruthTable(string expression)
    {
        var variableNames = Variables.Keys.ToArray();
        var variablesCount = variableNames.Length + 1;

        var headers = MakeTableHeaders(variableNames, variablesCount);
        var matrix = MakeTableBody(variablesCount, expression);
        _table = new Table<int>(headers, matrix);
        
        return this;
    }

    /// <summary>
    /// Вспомогательный метод заполнения заголовков таблицы истинности
    /// </summary>
    /// <param name="variableNames">Массив имен переменных</param>
    /// <param name="variablesCount">Количество переменных</param>
    /// <returns>Массив заголовков</returns>
    private string[] MakeTableHeaders(string[] variableNames, int variablesCount)
    {
        var result = new string[variablesCount];
        
        for (var i = 0; i < variablesCount - 1; i++)
            result[i] = variableNames[i];

        result[^1] = "Result";

        return result;
    }

    /// <summary>
    /// Вспомогательный метод заполнения тела таблицы истинности
    /// </summary>
    /// <param name="variablesCount">Количество переменных</param>
    /// <param name="expression">Строка выражения</param>
    /// <returns>Тело (Двумерный массив) таблицы истинности</returns>
    private int[,] MakeTableBody(int variablesCount, string expression)
    {
        var numRows = (int)Math.Pow(2, variablesCount - 1);
        var result = new int[numRows, variablesCount];
        
        for (var i = 0; i < numRows; i++)
        {
            var j = 0;
            foreach (var pair in _variables)
            {
                result[i, j] = ((i >> j) & 1) == 1 ? 1 : 0;
                _variables[pair.Key] = result[i, j];
                j++;
            }

            result[i, j] = (int)Interpret(expression);
        }

        return result;
    }
    
    /// <summary>
    /// Вывод дизъюнктивной нормальной формы логического выражения.
    /// Работает за счет сформированной таблице истинности
    /// </summary>
    /// <returns>Строковое представление выражение в ДНФ</returns>
    public string GetDnf()
    {
        var result = "";

        for (var i = 0; i < _table?.Matrix.GetLength(0); i++)
        {
            if (_table.Matrix[i, _table.Matrix.GetLength(1) - 1] == 1)
            {
                var subExpression = Dnf_GetSubExpression(i);
                subExpression = subExpression.Remove(subExpression.Length - 1);
                result += $" ({subExpression}) |";
            }
        }
        result = result.Remove(result.Length - 1);
        return result.Trim();
    }

    /// <summary>
    /// Вспомогательный метод формирования подвыражений для Dnf()
    /// </summary>
    /// <param name="i">Индекс столбца матрицы</param>
    /// <returns>Подвыражение в строковом представлении</returns>
    private string Dnf_GetSubExpression(int i)
    {
        var expression = "";
        for (var j = 0; j < _table?.Matrix.GetLength(1) - 1; j++)
        {
            if (_table?.Matrix[i, j] == 0)
            {
                expression += $" !{_table.Headers[j]} &";
            }
            else
            {
                expression += $" {_table?.Headers[j]} &";
            }
        }

        return expression;
    }

    /// <summary>
    /// Вывод конъюктивной нормальной формы логического выражения.
    /// Работает за счет сформированной таблице истинности
    /// </summary>
    /// <returns>Строковое представление выражение в КНФ</returns>
    public string GetKnf()
    {
        var result = "";

        for (var i = 0; i < _table?.Matrix.GetLength(0); i++)
        {
            if (_table.Matrix[i, _table.Matrix.GetLength(1) - 1] == 0)
            {
                var subExpression = Knf_GetSubExpression(i);
                subExpression = subExpression.Remove(subExpression.Length - 1);
                result += $" ({subExpression}) &";
            }
        }
        result = result.Remove(result.Length - 1);
        return result.Trim();
    }
    
    /// <summary>
    /// Вспомогательный метод формирования подвыражений для Knf()
    /// </summary>
    /// <param name="i">Индекс столбца матрицы</param>
    /// <returns>Подвыражение в строковом представлении</returns>
    private string Knf_GetSubExpression(int i)
    {
        var expression = "";
        for (var j = 0; j < _table?.Matrix.GetLength(1) - 1; j++)
        {
            if (_table?.Matrix[i, j] == 0)
                expression += $" {_table.Headers[j]} |";
            else
                expression += $" !{_table?.Headers[j]} |";
        }

        return expression;
    }
}