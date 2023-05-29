using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace BusinnesLayer;

[AttributeUsage(AttributeTargets.Property | 
                AttributeTargets.Field, AllowMultiple = false)]
public class NameValidationAttribute: ValidationAttribute
{
    private readonly string _regex;

    public NameValidationAttribute(string regex)
    {
        _regex = regex;
    }

    public override bool IsValid(object? value)
    {
        return value != null && Regex.IsMatch(value.ToString()!, _regex);;
    }
}