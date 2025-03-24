using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace Gaza_Support.Domains.Extensions
{
    public static class ValidationExtensions
    {
        public static Dictionary<string, List<string>> GetErrorsDictionary(this IEnumerable<ValidationFailure> validationFailures)
        {
            Dictionary<string, List<string>> errors = [];

            foreach(var item  in validationFailures)
            {
                if (errors.TryGetValue(item.PropertyName, out List<string>? value))
                {
                    value.Add(item.ErrorMessage);
                }
                else
                {
                    errors.Add(item.PropertyName, [item.ErrorMessage]);
                }
            }

            return errors;
        }

        public static Dictionary<string,List<string>> GetErrorsDictionary(this IEnumerable<IdentityError> validationFailures)
        {
            Dictionary<string,List<string>> errors = [];
            foreach (var item in validationFailures)
            {
                if (errors.TryGetValue(item.Code, out List<string>? value))
                {
                    value.Add(item.Description);
                }
                else
                {
                    errors.Add(item.Code, [item.Description]);
                }
            }

            return errors;
        }
    }
}
