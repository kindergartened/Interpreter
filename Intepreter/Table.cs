using System.Text;

namespace Intepreter;

/// <summary>
/// Класс таблицы, реализует простейшее представление таблицы.
/// </summary>
/// <typeparam name="T">Тип элементов, содержащихся в таблице</typeparam>
public class Table<T>
{
    private string[] _headers;
    private T[,] _matrix;

    public string[] Headers => _headers;
    public T[,] Matrix => _matrix;

    public Table(string[] headers, T[,] matrix)
    {
        _headers = headers;
        _matrix = matrix;
    }

    /// <summary>
    /// Переопределение метода ToString()
    /// </summary>
    /// <returns>Возвращает таблицу истинности в строковом представлении</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();
        
        foreach (var header in _headers)
        {
            sb.Append(header).Append("\t");
        }

        sb.AppendLine();


        for (int i = 0; i < _matrix.GetLength(0); i++)
        {
            for (int j = 0; j < _matrix.GetLength(1); j++)
            {
                sb.Append(_matrix[i, j]).Append("\t");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}