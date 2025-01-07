using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VFi.NetDevPack.Domain;

public class OutputParameter<TValue>
{
    private bool _valueSet = false;

    public TValue _value;

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
