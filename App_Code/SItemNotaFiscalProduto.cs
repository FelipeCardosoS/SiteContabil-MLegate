using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SItemNotaFiscalProduto
/// </summary>
public class SItemNotaFiscalProduto : SItemNotaFiscal
{
    private string _cfop;
    private double _aliquotaIpi;
    private double _valorIpi;
    private double _frete;

    public string cfop
    {
        get { return _cfop; }
        set { _cfop = value; }
    }
    public double aliquotaIpi
    {
        get { return _aliquotaIpi; }
        set { _aliquotaIpi = value; }
    }
    public double valorIpi
    {
        get { return _valorIpi; }
        set { _valorIpi = value; }
    }
    public double frete
    {
        get { return _frete; }
        set { _frete = value; }
    }

	public SItemNotaFiscalProduto()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}