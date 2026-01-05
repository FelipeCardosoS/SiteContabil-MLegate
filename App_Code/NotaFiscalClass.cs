using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for NotaFiscalClass
/// </summary>
public class NotaFiscalClass
{
    private notaFiscalDAO _notaFiscalDAO;
    private itemNotaFiscalDAO _itemDAO;
    private empresasDAO _empresaDAO;
    private List<string> _erros = new List<string>();

    public List<string> erros
    {
        get { return _erros; }
        set { _erros = value; }
    }
	
    public NotaFiscalClass(notaFiscalDAO nfDAO, itemNotaFiscalDAO itemDAO, empresasDAO empresaDAO)
	{
        _notaFiscalDAO = nfDAO;
        _itemDAO = itemDAO;
        _empresaDAO = empresaDAO;
	}

    public bool valida(SNotaFiscal notaFiscal)
    {
        _erros = new List<string>();
        //pelo menos um item por nota
        if (notaFiscal.itens == null)
            _erros.Add("Nenhum item encontrado para a nota fiscal");
        if (notaFiscal.itens.Count == 0)
            _erros.Add("Nenhum item encontrado para a nota fiscal");
        if (string.IsNullOrEmpty(notaFiscal.entradaSaida) || notaFiscal.entradaSaida.Equals("0"))
            _erros.Add("Informe se a Nota Fiscal é de Entrada ou Saída");

        double totalIcmsItens = 0;
        foreach (SItemNotaFiscal item in notaFiscal.itens)
        {
            totalIcmsItens += item.valorIcms;
        }
        if (totalIcmsItens != notaFiscal.icms)
            _erros.Add("Soma do icms dos itens não bate com o total de icms da nota.");

        double totalBaseImpItens = 0;
        foreach (SItemNotaFiscal item in notaFiscal.itens)
        {
            totalBaseImpItens += item.valorBaseImp;
        }

        if (totalBaseImpItens != notaFiscal.baseImpostos)
            _erros.Add("Soma da base de impostos dos itens não bate com o total de base de imposto da nota.");

        if (notaFiscal.GetType() == typeof(SNotaFiscalProduto))
        {
            SNotaFiscalProduto nfProd = (SNotaFiscalProduto)notaFiscal;
            double totalItens = 0;
            foreach (SItemNotaFiscal item in notaFiscal.itens)
            {
                totalItens += item.valorTotal;
            }

            totalItens = (totalItens - nfProd.descontos) + nfProd.frete;
            if (nfProd.valor != totalItens)
                _erros.Add("Soma do valor dos itens não bate com o valor total da nota.");

            double totalIpiItens = 0;
            foreach (SItemNotaFiscal item in nfProd.itens)
            {
                SItemNotaFiscalProduto itemProd = (SItemNotaFiscalProduto)item;
                totalIpiItens += itemProd.valorIpi;
            }

            if (totalIpiItens != nfProd.ipi)
                _erros.Add("Soma do ipi dos itens não bate com o total de ipi da nota.");

            //cfop deve ser informado e deve ser maior ou igual a 4 digitos
            int errosCfop = 0;
            foreach (SItemNotaFiscal item in nfProd.itens)
            {
                SItemNotaFiscalProduto itemProd = (SItemNotaFiscalProduto)item;
                if (string.IsNullOrEmpty(itemProd.cfop))
                {
                    errosCfop++;
                }

                if (itemProd.cfop.Length < 4)
                {
                    errosCfop++;
                }
            }

            if (errosCfop > 0)
                _erros.Add("Itens sem CFOP ou com menos de 4 caracteres.");

            string estadoFornecedorCliente = "";
            string estadoGrupo = "";
            string paisFornecedorCliente = "";
            string paisGrupo = "";

            DataTable tbFornecedorCliente = _empresaDAO.load(nfProd.fornecedorCliente);
            if (nfProd.fornecedorCliente != 0)
            {
                DataTable tbGrupo = _empresaDAO.load(nfProd.codEmpresa);
                if (tbFornecedorCliente.Rows.Count > 0)
                {
                    DataRow row = tbFornecedorCliente.Rows[0];
                    estadoFornecedorCliente = row["UF"].ToString();
                    paisFornecedorCliente = "0";
                    if (row["COD_PAIS"] != DBNull.Value)
                        paisFornecedorCliente = row["COD_PAIS"].ToString();
                }
                if (tbGrupo.Rows.Count > 0)
                {
                    DataRow row = tbGrupo.Rows[0];
                    estadoGrupo = row["UF"].ToString();
                    paisGrupo = "0";
                    if (row["COD_PAIS"] != DBNull.Value)
                        paisGrupo = row["COD_PAIS"].ToString();

                }

                //estado fornecedor/cliente = estado grupo o cfop iniciar com 1 ou 5
                if (estadoFornecedorCliente == estadoGrupo)
                {
                    foreach (SItemNotaFiscal item in nfProd.itens)
                    {
                        SItemNotaFiscalProduto itemProd = (SItemNotaFiscalProduto)item;
                        if (itemProd.cfop.Substring(0, 1) != "1" && itemProd.cfop.Substring(0, 1) != "5")
                            _erros.Add("Este CFOP não pode ser utilizado para empresas do mesmo Estado");
                    }
                }
                //estado fornecedor/cliente <> estado grupo o cfop iniciar com 2 ou 6
                else if (estadoFornecedorCliente != estadoGrupo)
                {
                    foreach (SItemNotaFiscal item in nfProd.itens)
                    {
                        SItemNotaFiscalProduto itemProd = (SItemNotaFiscalProduto)item;
                        if (itemProd.cfop.Substring(0, 1) != "2" && itemProd.cfop.Substring(0, 1) != "6")
                            _erros.Add("Este CFOP não pode ser utilizado para empresas de outro Estado.");
                    }
                }
                //país fornecedor/cliente <> país grupo o cfop iniciar 3 ou 7
                else if (paisFornecedorCliente != paisGrupo)
                {
                    foreach (SItemNotaFiscal item in nfProd.itens)
                    {
                        SItemNotaFiscalProduto itemProd = (SItemNotaFiscalProduto)item;
                        if (itemProd.cfop.Substring(0, 1) != "3" && itemProd.cfop.Substring(0, 1) != "7")
                            _erros.Add("Este CFOP não pode ser utilizado para empresas de outro Pais.");
                    }
                }
            }
        }
        else
        {
            SNotaFiscalServico nfServ = (SNotaFiscalServico)notaFiscal;
            double totalItens = 0;
            foreach (SItemNotaFiscal item in notaFiscal.itens)
            {
                totalItens += item.valorTotal;
            }
            totalItens = (totalItens - nfServ.descontos);

            if (nfServ.valor != totalItens)
                _erros.Add("Soma do valor dos itens não bate com o valor total da nota.");
        }

        if (_erros.Count == 0)
            return true;
        else
            return false;
    }

    public void deletar(double numeroNota, string entradaSaida, double lote, string produtoServico, int codEmpresa)
    {
        _itemDAO.deleteOfNota(numeroNota, entradaSaida, lote, produtoServico, codEmpresa);
        _notaFiscalDAO.delete(numeroNota, entradaSaida, lote, produtoServico, codEmpresa);
    }

    public bool salvar(SNotaFiscal notaFiscal)
    {
        if (valida(notaFiscal))
        {
            double totalItens = 0;
            foreach (SItemNotaFiscal item in notaFiscal.itens)
            {
                totalItens += item.valorTotal;
            }

            if (notaFiscal.GetType() == typeof(SNotaFiscalProduto))
            {                SNotaFiscalProduto nfProd = (SNotaFiscalProduto)notaFiscal;

                //rateio frete
                foreach (SItemNotaFiscal item in nfProd.itens)
                {
                    SItemNotaFiscalProduto itemProd = (SItemNotaFiscalProduto)item;
                    itemProd.frete = (nfProd.frete / totalItens) * itemProd.valorTotal;
                }
            }

            double total = 0;
            foreach (SItemNotaFiscal item in notaFiscal.itens)
            {
                total += item.valorTotal;
            }

            foreach (SItemNotaFiscal item in notaFiscal.itens)
            {
                item.desconto = (notaFiscal.descontos / total) * item.valorTotal;
            }

            double totalPis = 0;
            foreach (SItemNotaFiscal item in notaFiscal.itens)
            {
                totalPis += item.valorPis;
            }

            double totalCofins = 0;
            foreach (SItemNotaFiscal item in notaFiscal.itens)
            {
                totalCofins += item.valorCofins;
            }

            notaFiscal.valorPis = totalPis;
            notaFiscal.valorCofins = totalCofins;

            _notaFiscalDAO.insert(notaFiscal);
            for (int i = 0; i < notaFiscal.itens.Count; i++)
            {
                _itemDAO.insert(notaFiscal.itens[i]);
            }
            return true;
        }
        return false;
    }
}