using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for itemNotaFiscalDAO
/// </summary>
public class itemNotaFiscalDAO
{
    private Conexao _conn;
    private notaFiscalDAO _notaFiscalDAO;
	public itemNotaFiscalDAO(Conexao c, notaFiscalDAO nfDAO)
	{
        _conn = c;
        _notaFiscalDAO = nfDAO;
	}

    public void insert(SItemNotaFiscal item)
    {
        string sql = "INSERT INTO ITENS_NOTA_FISCAL(ORDEM,[NUMERO_NOTA] " +
           " ,[ENTRADA_SAIDA] " +
           " ,[LOTE] " +
           " ,[PRODUTO_SERVICO] " +
           " ,[COD_PRODUTO],QTDE,VALOR_UNIT " +
           " ,[VALOR_TOTAL],VALOR_BASE_IMP,ALIQ_ICMS,VALOR_ICMS,ALIQ_PIS,VALOR_PIS,ALIQ_COFINS,VALOR_COFINS,COD_EMPRESA,VALOR_DESCONTO,";
        string sqlValores = "VALUES("+item.ordem+"," + item.numeroNota + ",'" + item.entradaSaida + "'," + item.lote + ",'" +
            item.produtoServico + "'," + item.codigoProduto + "," + item.qtde.ToString().Replace(",", ".") + "," +
            item.valorUnitario.ToString().Replace(",", ".") + "," + item.valorTotal.ToString().Replace(",", ".") + "," +
            item.valorBaseImp.ToString().Replace(",", ".") + "," + item.aliquotaIcms.ToString().Replace(",", ".") + "," + 
            item.valorIcms.ToString().Replace(",", ".") + "," + item.aliquotaPis.ToString().Replace(",", ".") + "," + 
            item.valorPis.ToString().Replace(",", ".") + "," + item.aliquotaCofins.ToString().Replace(",", ".") + "," +
            item.valorCofins.ToString().Replace(",", ".") + ", " + item.codEmpresa + ", " + item.desconto.ToString().Replace(",", ".") + ",";

        if (item.GetType() == typeof(SItemNotaFiscalProduto))
        {
            sql += "ALIQ_IPI,VALOR_IPI,CFOP,VALOR_FRETE";
            sqlValores += "" + (item as SItemNotaFiscalProduto).aliquotaIpi.ToString().Replace(",", ".") + "," +
                (item as SItemNotaFiscalProduto).valorIpi.ToString().Replace(",", ".") + ",'" + (item as SItemNotaFiscalProduto).cfop + "'," + (item as SItemNotaFiscalProduto).frete.ToString().Replace(",", ".") + "";
        }
        else
        {
            sql = sql.Substring(0, sql.Length - 1);
            sqlValores = sqlValores.Substring(0, sqlValores.Length - 1);
            //sql += "ALIQ_IR,VALOR_IR,ALIQ_CSLL,VALOR_CSLL";
            //sqlValores += "" + (item as SItemNotaFiscalServico).aliquotaIr.ToString().Replace(",", ".") + "," +
            //        (item as SItemNotaFiscalServico).valorIr.ToString().Replace(",", ".") + "," +
            //    (item as SItemNotaFiscalServico).aliquotaCsll.ToString().Replace(",", ".") + "," +
            //    (item as SItemNotaFiscalServico).valorCsll.ToString().Replace(",", ".") + "";
        }
        sql += ")";
        sqlValores += ")";

        _conn.execute(sql + sqlValores);
    }

    public void deleteOfNota(double numeroNota, string entradaSaida, double lote, string produtoServico, int codEmpresa)
    {
        string sql = "DELETE FROM ITENS_NOTA_FISCAL WHERE NUMERO_NOTA=" + numeroNota + /*" AND ENTRADA_SAIDA='" + entradaSaida +*/ " AND LOTE=" + lote + /*" AND PRODUTO_SERVICO='" + produtoServico +*/ " AND COD_EMPRESA=" + codEmpresa;
        _conn.execute(sql);
    }

    public SItemNotaFiscal load(int ordem, double numeroNota, string entradaSaida, double lote, string produtoServico, int codEmpresa)
    {
        string sql = "select * from itens_nota_fiscal where ORDEM="+ordem+" AND [NUMERO_NOTA]=" + numeroNota + " " +
           " AND [ENTRADA_SAIDA]='" + entradaSaida + "' " +
           " AND [LOTE]=" + lote + " " +
           " AND COD_EMPRESA=" + codEmpresa + " ";

        DataTable tb = _conn.dataTable(sql, "item");
        SItemNotaFiscal item = null;
        if (tb.Rows.Count > 0)
        {
            createObject(tb.Rows[0], produtoServico);
        }

        return item;
    }

    private SItemNotaFiscal createObject(DataRow row, string produtoServico)
    {
        SItemNotaFiscal item = null;
        if (produtoServico == "P")
            item = new SItemNotaFiscalProduto();
        else
            item = new SItemNotaFiscalServico();

        item.numeroNota =  Convert.ToDouble(row["NUMERO_NOTA"]);
        item.entradaSaida =  row["ENTRADA_SAIDA"].ToString();
        item.lote = Convert.ToDouble(row["LOTE"]);
        item.produtoServico = row["PRODUTO_SERVICO"].ToString();
        item.codEmpresa = Convert.ToInt32(row["COD_EMPRESA"]);
        item.ordem = Convert.ToInt32(row["ORDEM"]);
        item.codigoProduto = Convert.ToInt32(row["COD_PRODUTO"]);
        item.descProduto = row["DESC_PRODUTO"].ToString();
        item.qtde = Convert.ToDouble(row["QTDE"]);
        item.valorTotal = Convert.ToDouble(row["VALOR_TOTAL"]);
        item.valorBaseImp = Convert.ToDouble(row["VALOR_BASE_IMP"]);
        item.aliquotaCofins = Convert.ToDouble(row["ALIQ_COFINS"]);
        item.aliquotaIcms = Convert.ToDouble(row["ALIQ_ICMS"]);
        item.aliquotaPis = Convert.ToDouble(row["ALIQ_PIS"]);
        if (produtoServico == "P")
        {
            SItemNotaFiscalProduto temp = (SItemNotaFiscalProduto)item;
            temp.cfop = row["CFOP"].ToString();
            temp.aliquotaIpi = Convert.ToDouble(row["ALIQ_IPI"]);
            temp.valorIpi = Convert.ToDouble(row["VALOR_IPI"]);
            return temp;
        }
        else
        {
            SItemNotaFiscalServico temp = (SItemNotaFiscalServico)item;
            //temp.aliquotaIr = Convert.ToDouble(row["ALIQ_IR"]);
            //temp.valorIr = Convert.ToDouble(row["VALOR_IR"]);
            //temp.aliquotaCsll = Convert.ToDouble(row["ALIQ_CSLL"]);
            //temp.valorCsll = Convert.ToDouble(row["VALOR_CSLL"]);
            return temp;
        }
    }

    public List<SItemNotaFiscal> listItensProduto(double numeroNota, string entradaSaida, double lote, string produtoServico, int codEmpresa)
    {
        string sql = "select itens_nota_fiscal.*,cad_produtos.descricao as desc_produto from itens_nota_fiscal,cad_produtos where [NUMERO_NOTA]=" + numeroNota + " AND PRODUTO_SERVICO='" + produtoServico + "' " +
           " AND [ENTRADA_SAIDA]='" + entradaSaida + "' " +
           " AND [LOTE]=" + lote + " " +
           " AND itens_nota_fiscal.COD_EMPRESA=" + codEmpresa + " " +
            " and itens_nota_fiscal.cod_produto = cad_produtos.cod_produto ";

        DataTable tb = _conn.dataTable(sql, "itens");
        List<SItemNotaFiscal> list = new List<SItemNotaFiscal>();
        for (int i = 0; i < tb.Rows.Count; i++)
        {
            list.Add(createObject(tb.Rows[i], produtoServico));
        }
        return list;
    }

    public List<SItemNotaFiscal> listItensServico(double numeroNota, string entradaSaida, double lote, string produtoServico, int codEmpresa)
    {
        string sql = "select itens_nota_fiscal.*,cad_produtos.descricao as desc_produto from itens_nota_fiscal,cad_produtos where [NUMERO_NOTA]=" + numeroNota + " AND PRODUTO_SERVICO='" + produtoServico + "' " +
           " AND [ENTRADA_SAIDA]='" + entradaSaida + "' " +
           " AND [LOTE]=" + lote + " " +
           " AND itens_nota_fiscal.COD_EMPRESA=" + codEmpresa + " " +
           " and itens_nota_fiscal.cod_produto = cad_produtos.cod_produto ";

        DataTable tb = _conn.dataTable(sql, "itens");
        List<SItemNotaFiscal> list = new List<SItemNotaFiscal>();
        for (int i = 0; i < tb.Rows.Count; i++)
        {
            list.Add(createObject(tb.Rows[i], produtoServico));
        }
        return list;
    }
}