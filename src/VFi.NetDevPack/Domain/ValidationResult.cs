using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace VFi.NetDevPack.Domain;
public class ValidationResult : FluentValidation.Results.ValidationResult
{
    public ValidationResult() : base()
    {

    }
    public ValidationResult(IEnumerable<ValidationFailure> failures) : base(failures)
    {

    }
    public ValidationResult(Object data) : base()
    {
        this.Data = data;
    }
    public ValidationResult(IEnumerable<ValidationFailure> failures, Object data) : base(failures)
    {
        this.Data = data;
    }
    public Object Data { get; set; }

}

public class ValidationResult<T> : FluentValidation.Results.ValidationResult
{
    public ValidationResult() : base()
    {

    }
    public ValidationResult(IEnumerable<ValidationFailure> failures) : base(failures)
    {

    }
    public ValidationResult(T data) : base()
    {
        this.Data = data;
    }
    public ValidationResult(IEnumerable<ValidationFailure> failures, T data) : base(failures)
    {
        this.Data = data;
    }
    public T Data { get; set; }
}
