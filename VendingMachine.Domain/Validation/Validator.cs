using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Domain.Core;

namespace VendingMachine.Domain.Validation;

public class Validator<TEntity>
{

    //private readonly TEntity entity;
    //private readonly List<Error> errors;
    private readonly List<ValidationRule<TEntity>> rules;
    public Validator()
    {
        rules = new List<ValidationRule<TEntity>>();
    }
    public Validator(List<ValidationRule<TEntity>> _rules)
    {
        rules = new List<ValidationRule<TEntity>>(_rules);
    }
    public Validator<TEntity> AddRule(Func<TEntity, bool> validationRule, Error error)
    {
        rules.Add(new(validationRule, error));
        return this;
    }

    public bool Validate(TEntity entity, out Error[] errors)
    {
        List<Error> errorsList = new List<Error>();
        foreach (var validationRule in rules)
        {
            if (validationRule.Rule(entity) == false)
                errorsList.Add(validationRule.Error);

        }
        errors = errorsList.ToArray();


        return errors.Length == 0;

    }

    internal List<ValidationRule<TEntity>> Rules() => this.rules.ToList();


    public Validator<TDistnation> CreateCopy<TDistnation>()
        where TDistnation : TEntity
    {
        var newRules = rules.Select(oldRule => new ValidationRule<TDistnation>(newEntity => oldRule.Rule(newEntity), oldRule.Error));
        return new Validator<TDistnation>(newRules.ToList());
    }



}

public class ValidationRule<TEntity>
{

    internal readonly Func<TEntity, bool> Rule;
    internal readonly Error Error;

    public ValidationRule(Func<TEntity, bool> rule, Error error)
    {
        Rule = rule;
        Error = error;
    }
}
