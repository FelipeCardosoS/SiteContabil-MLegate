using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SAliquotaImposto
/// </summary>
/// 
[Serializable]
public class SAliquotaImposto
{
    private string _tipoImposto;
    private bool _cumulativo;
    private double _aliquota;
    private double _aliquotaRetencao;
    private int _codEmpresa;

    public string tipoImposto
    {
        get { return _tipoImposto; }
        set { _tipoImposto = value; }
    }
    public bool cumulativo
    {
        get { return _cumulativo; }
        set { _cumulativo = value; }
    }
    public double aliquota
    {
        get { return _aliquota; }
        set { _aliquota = value; }
    }
    public double aliquotaRetencao
    {
        get { return _aliquotaRetencao; }
        set { _aliquotaRetencao = value; }
    }
    public int codEmpresa
    {
        get { return _codEmpresa; }
        set { _codEmpresa = value; }
    }

	public SAliquotaImposto()
	{
		//
		// TODO: Add constructor logic here
		//
	}
}