using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace VFi.NetDevPack.Domain;

public class OutValidationResult<TValue> : FluentValidation.Results.ValidationResult
{
    private bool _valueSet = false;

    public TValue _value;

    public OutValidationResult(FluentValidation.Results.ValidationResult valid)
    {
        base.Errors = valid.Errors;
        base.RuleSetsExecuted = valid.RuleSetsExecuted;
    }
    public OutValidationResult()
    {

    }
    public TValue Value
    {
        get
        {
            if (!_valueSet)
                throw new InvalidOperationException("Value not set.");

            return _value;
        }
    }

    public void SetValue(object value)
    {
        _valueSet = true;

        _value = null == value || Convert.IsDBNull(value) ? default(TValue) : (TValue)value;
    }
}
