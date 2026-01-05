using System;
using System.Data;
using System.Configuration;
using System.Web;

public class SVencimento
{
    private Nullable<DateTime> _data;
    private Nullable<decimal> _valor;

    public Nullable<DateTime> data
    {
        get { return _data; }
        set { _data = value; }
    }

    public Nullable<decimal> valor
    {
        get { return _valor; }
        set { _valor = value; }
    }

    public SVencimento(Nullable<DateTime> data, Nullable<decimal> valor)
	{
        _data = data;
        _valor = valor;
	}

    public SVencimento()
    {    }
}
