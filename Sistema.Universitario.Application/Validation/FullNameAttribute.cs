using System.ComponentModel.DataAnnotations;

namespace Sistema.Universitario.Application.Validation;

public class FullNameAttribute : ValidationAttribute
{
    private readonly int _minWords;

    public FullNameAttribute(int minWords = 2)
    {
        _minWords = minWords;
        ErrorMessage = $"O nome deve conter pelo menos {_minWords} palavras.";
    }

    public override bool IsValid(object value)
    {
        var s = value as string;
        if (string.IsNullOrWhiteSpace(s)) return false;
        var parts = s.Trim().Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= _minWords;
    }
}
