using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Sistema.Universitario.Application.Validation;

public class MatriculaAttribute : ValidationAttribute
{
    private readonly int _min;
    private readonly int _max;

    public MatriculaAttribute(int min = 4, int max = 20)
    {
        _min = min;
        _max = max;
        ErrorMessage = $"Matrícula deve ter entre {_min} e {_max} caracteres e conter apenas letras ou números.";
    }

    public override bool IsValid(object value)
    {
        var s = value as string;
        if (string.IsNullOrWhiteSpace(s)) return false;
        if (s.Length < _min || s.Length > _max) return false;
        return Regex.IsMatch(s, "^[a-zA-Z0-9]+$");
    }
}
