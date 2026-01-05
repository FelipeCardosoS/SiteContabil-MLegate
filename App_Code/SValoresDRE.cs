using System;
using System.Data;
using System.Configuration;
using System.Web;

public struct SValoresDRE
{
    private DateTime _periodo;
    private decimal _valor;

    public DateTime periodo
    {
        get { return _periodo; }
        set { _periodo = value; }
    }

    public decimal valor
    {
        get { return _valor; }
        set { _valor = value; }
    }

    public SValoresDRE(DateTime p, decimal v)
    {
        _periodo = p;
        _valor = v;
    }
}
