using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SItemNotaFiscal
/// </summary>
public class SItemNotaFiscal
{
    private int _ordem;
    private double _numeroNota;
    private string _entradaSaida;
    private double _lote;
    private string _produtoServico;
    private int _codEmpresa;
    private int _codigoProduto;
    private string _descProduto;
    private double _qtde;
    private double _valorUnitario = 0;
    private double _valorTotal;
    private double _valorBaseImp;
    private double _aliquotaIcms;
    private double _valorIcms;
    private double _aliquotaPis;
    private double _valorPis;
    private double _aliquotaCofins;
    private double _valorCofins;
    private double _desconto;
    private int _cfop;
    private double _aliquotaIpi;
    private double _valorIpi;

    public double valorIpi
    {
        get { return _valorIpi; }
        set { _valorIpi = value; }
    }

    public double aliquotaIpi
    {
        get { return _aliquotaIpi; }
        set { _aliquotaIpi = value; }
    }

    public int cfop
    {
        get { return _cfop; }
        set { _cfop = value; }
    }

    public int ordem
    {
        get { return _ordem; }
        set { _ordem = value; }
    }
    public double numeroNota
    {
        get { return _numeroNota; }
        set { _numeroNota = value; }
    }
    public string entradaSaida
    {
        get { return _entradaSaida; }
        set { _entradaSaida = value; }
    }
    public double lote
    {
        get { return _lote; }
        set { _lote = value; }
    }
    public string produtoServico
    {
        get { return _produtoServico; }
        set { _produtoServico = value; }
    }
    public int codEmpresa
    {
        get { return _codEmpresa; }
        set { _codEmpresa = value; }
    }
    public int codigoProduto
    {
        get { return _codigoProduto; }
        set { _codigoProduto = value; }
    }
    public string descProduto
    {
        get { return _descProduto; }
        set { _descProduto = value; }
    }
    public double qtde
    {
        get { return _qtde; }
        set { _qtde = value; }
    }
    public double valorUnitario
    {
        get { return _valorTotal / _qtde; }
    }
    public double valorTotal
    {
        get { return _valorTotal; }
        set { _valorTotal = value; }
    }
    public double valorBaseImp
    {
        get { return _valorBaseImp; }
        set { _valorBaseImp = value; }
    }
    public double aliquotaIcms
    {
        get { return _aliquotaIcms; }
        set { _aliquotaIcms = value; }
    }
    public double valorIcms
    {
        get { return _valorTotal * (_aliquotaIcms/100); }
    }
    public double aliquotaPis
    {
        get { return _aliquotaPis; }
        set { _aliquotaPis = value; }
    }
    public double valorPis
    {
        get { return _valorBaseImp * (_aliquotaPis / 100); }

    }
    public double aliquotaCofins
    {
        get { return _aliquotaCofins; }
        set { _aliquotaCofins = value; }
    }
    public double valorCofins
    {
        get { return _valorBaseImp * (_aliquotaCofins / 100); }
    }
    public double desconto
    {
        get { return _desconto; }
        set { _desconto = value; }
    }
	public SItemNotaFiscal()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    
}