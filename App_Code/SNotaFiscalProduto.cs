using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SNotaFiscalProduto
/// </summary>
public class SNotaFiscalProduto : SNotaFiscal
{
    
    private double _frete;
    private double _ipi;
    

    public double frete
    {
        get { return _frete; }
        set { _frete = value; }
    }

    
    public double ipi
    {
        get { return _ipi; }
        set { _ipi = value; }
    }

	public SNotaFiscalProduto()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public override void listItens()
    {
        Conexao c = new Conexao();
        itemNotaFiscalDAO itemDAO = new itemNotaFiscalDAO(c, new notaFiscalDAO(c));
        _itens = itemDAO.listItensProduto(numeroNota, entradaSaida, lote, produtoServico, codEmpresa);
    }
}