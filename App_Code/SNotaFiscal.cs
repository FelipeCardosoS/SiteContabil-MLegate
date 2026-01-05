using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SNotaFiscal
/// </summary>
public abstract class SNotaFiscal
{
    private double _numeroNota;
    private string _entradaSaida;
    private double _lote;
    private string _produtoServico;
    private int _fornecedorCliente;
    private double _valor;
    private double _descontos;
    private double _icms;
    private double _baseImpostos;
    private double _valorPis;
    private double _valorCofins;
    private int _codEmpresa;
    private string _numeroEletronica;
    private int _cod_part;
    private DateTime _data_emissao;
    private double _frete;
    private double _desconto;
    private double _base_imposto;
    private double _ipi;

    public double frete
    {
        get { return _frete; }
        set { _frete = value; }
    }

    public double desconto
    {
        get { return _desconto; }
        set { _desconto = value; }
    }

    public double base_imposto
    {
        get { return _base_imposto; }
        set { _base_imposto = value; }
    }

    public double ipi
    {
        get { return _ipi; }
        set { _ipi = value; }
    }

    protected List<SItemNotaFiscal> _itens = new List<SItemNotaFiscal>();

    public List<SItemNotaFiscal> itens
    {
        set { _itens = value; }
        get
        {
            return _itens;
        }
    }

    public DateTime data_emissao
    {
        get { return _data_emissao; }
        set { _data_emissao = value; }
    }

    public int cod_part
    {
        get { return _cod_part; }
        set { _cod_part = value; }
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
    public int fornecedorCliente
    {
        get { return _fornecedorCliente; }
        set { _fornecedorCliente = value; }
    }
    public double valor
    {
        get { return _valor; }
        set { _valor = value; }
    }
    public double descontos
    {
        get { return _descontos; }
        set { _descontos = value; }
    }
    public double icms
    {
        get { return _icms; }
        set { _icms = value; }
    }
    public double baseImpostos
    {
        get { return _baseImpostos; }
        set { _baseImpostos = value; }
    }
    public double valorPis
    {
        get { return _valorPis; }
        set { _valorPis = value; }
    }
    public double valorCofins
    {
        get { return _valorCofins; }
        set { _valorCofins = value; }
    }

    public int codEmpresa
    {
        get { return _codEmpresa; }
        set { _codEmpresa = value; }
    }

    public string numeroEletronica
    {
        get { return _numeroEletronica; }
        set { _numeroEletronica = value; }
    }

	public SNotaFiscal()
	{
		
	}

    public abstract void listItens();
}