using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CotacaoItem
/// </summary>
public class CotacaoItem
{
    private int _codMoeda = 0;
    private decimal _valorMoeda = 0;
    private string _descrMoeda = "";
    private DateTime _data;

    public DateTime data 
    {
        get { return _data; }
        set { _data = value; }
    }

    public int codMoeda
    {
        get { return _codMoeda; }
        set { _codMoeda = value; }
    }

    public decimal valorMoeda
    {
        get { return _valorMoeda; }
        set { _valorMoeda = value; }
    }

    public string descrMoeda
    {
        get { return _descrMoeda; }
        set { _descrMoeda = value; }
    }

    public CotacaoItem()
	{

	}
}