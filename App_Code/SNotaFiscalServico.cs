using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SNotaFiscalServico
/// </summary>
public class SNotaFiscalServico : SNotaFiscal
{

    private double _valorCsll;
    private double _valorIR;
    private double _valorPisRetido;
    private double _valorCofinsRetido;
    private double _valorCsllRetido;
    private double _valorIrRetido;
    private double _valorIssRetido;
    private double _valorIss;

    public double valorCsll
    {
        get { return _valorCsll; }
        set { _valorCsll = value; }
    }
    public double valorIR
    {
        get { return _valorIR; }
        set { _valorIR = value; }
    }
    public double valorPisRetido
    {
        get { return _valorPisRetido; }
        set { _valorPisRetido = value; }
    }
    public double valorCofinsRetido
    {
        get { return _valorCofinsRetido; }
        set { _valorCofinsRetido = value; }
    }
    public double valorCsllRetido
    {
        get { return _valorCsllRetido; }
        set { _valorCsllRetido = value; }
    }
    public double valorIrRetido
    {
        get { return _valorIrRetido; }
        set { _valorIrRetido = value; }
    }
    public double valorIssRetido
    {
        get { return _valorIssRetido; }
        set { _valorIssRetido = value; }
    }
    public double valorIss
    {
        get { return _valorIss; }
        set { _valorIss = value; }
    }

	public SNotaFiscalServico()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public override void listItens()
    {
        Conexao c = new Conexao();
        itemNotaFiscalDAO itemDAO = new itemNotaFiscalDAO(c, new notaFiscalDAO(c));
        _itens = itemDAO.listItensServico(numeroNota, entradaSaida, lote, produtoServico, codEmpresa);
    }
}