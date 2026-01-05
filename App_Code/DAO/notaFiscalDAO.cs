using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for notaFiscalDAO
/// </summary>
public class notaFiscalDAO
{
    private Conexao _conn;

	public notaFiscalDAO(Conexao c)
	{
        _conn = c;
	}

    public void insert(SNotaFiscal nota)
    {
        string sql = "INSERT INTO CAD_NOTAS_FISCAIS([NUMERO_NOTA] "+
           " ,[ENTRADA_SAIDA] " +
           " ,[LOTE] " +
           " ,[PRODUTO_SERVICO] " +
           " ,[FORNECEDOR_CLIENTE] " +
           " ,[VALOR],[DESCONTOS],[ICMS],[BASE_IMPOSTOS],COD_EMPRESA,NUMERO_ELETRONICA,";
        string sqlValores = "VALUES(" + nota.numeroNota + ",'" + nota.entradaSaida + "'," + nota.lote + ",'" + 
            nota.produtoServico + "'," + nota.fornecedorCliente + "," + nota.valor.ToString().Replace(",", ".") + "," + 
            nota.descontos.ToString().Replace(",", ".") + "," + nota.icms.ToString().Replace(",", ".") + "," + 
            nota.baseImpostos.ToString().Replace(",", ".") + ","+nota.codEmpresa+",'"+nota.numeroEletronica+"', ";

        if (nota.GetType() == typeof(SNotaFiscalProduto))
        {
            sql += "[FRETE],[IPI],[VALOR_PIS],[VALOR_COFINS]";
            sqlValores += "" + (nota as SNotaFiscalProduto).frete.ToString().Replace(",", ".") + "," + 
                (nota as SNotaFiscalProduto).ipi.ToString().Replace(",", ".") + "," + 
                (nota as SNotaFiscalProduto).valorPis.ToString().Replace(",", ".") + "," +
                (nota as SNotaFiscalProduto).valorCofins.ToString().Replace(",", ".") + " ";
        }
        else
        {
            sql += "[VALOR_RETIDO_PIS] "+
               " ,[VALOR_RETIDO_COFINS] "+
               " ,[VALOR_RETIDO_CSLL] " +
               " ,[VALOR_RETIDO_IR],[VALOR_CSLL] " +
               " ,[VALOR_IR] " +
               " ,[VALOR_ISS] " +
               " ,[VALOR_ISS_RETIDO]";
            sqlValores += "" + (nota as SNotaFiscalServico).valorPisRetido.ToString().Replace(",", ".") + "," + 
                (nota as SNotaFiscalServico).valorCofinsRetido.ToString().Replace(",", ".") + "," + 
                (nota as SNotaFiscalServico).valorCsllRetido.ToString().Replace(",", ".") + "," + 
                (nota as SNotaFiscalServico).valorIrRetido.ToString().Replace(",", ".") + "," + 
                (nota as SNotaFiscalServico).valorCsll.ToString().Replace(",", ".") + "," + 
                (nota as SNotaFiscalServico).valorIR.ToString().Replace(",", ".") + "," + 
                (nota as SNotaFiscalServico).valorIss.ToString().Replace(",", ".") + "," + 
                (nota as SNotaFiscalServico).valorIssRetido.ToString().Replace(",", ".") + "";
        }
        sql += ")";
        sqlValores += ")";

        _conn.execute(sql + sqlValores);
    }

    public void delete(double numeroNota, string entradaSaida, double lote, string produtoServico, int codEmpresa)
    {
        string sql = "DELETE FROM CAD_NOTAS_FISCAIS WHERE NUMERO_NOTA=" + numeroNota + /*" AND ENTRADA_SAIDA='" + entradaSaida +*/ " AND LOTE=" + lote + /*" AND PRODUTO_SERVICO='" + produtoServico +*/ " AND COD_EMPRESA=" + codEmpresa;
        _conn.execute(sql);
    }

    public void update(SNotaFiscal nota)
    {
        string sql = "UPDATE CAD_NOTAS_FISCAIS SET " +
           " ,[FORNECEDOR_CLIENTE]=" + nota.fornecedorCliente + " " +
           " ,[VALOR]=" + nota.valor.ToString().Replace(",", ".") + ",[DESCONTOS]=" + nota.descontos.ToString().Replace(",", ".") + ",[ICMS]=" + nota.icms.ToString().Replace(",", ".") + ",[BASE_IMPOSTOS]=" + nota.baseImpostos.ToString().Replace(",", ".") + ", NUMERO_ELETRONICA='"+nota.numeroEletronica+"',";
        if (nota.GetType() == typeof(SNotaFiscalProduto))
        {
            sql += "[FRETE]=" + (nota as SNotaFiscalProduto).frete.ToString().Replace(",", ".") + ", " + 
                " [IPI]= " + (nota as SNotaFiscalProduto).ipi.ToString().Replace(",", ".") + ", " + 
                "[VALOR_PIS]= " + (nota as SNotaFiscalProduto).valorPis.ToString().Replace(",", ".") + ", " + 
                "[VALOR_COFINS] = " + (nota as SNotaFiscalProduto).valorCofins.ToString().Replace(",", ".") + " ";

        }
        else
        {
            sql += "[VALOR_RETIDO_PIS]=" + (nota as SNotaFiscalServico).valorPisRetido.ToString().Replace(",", ".") + " " +
               " ,[VALOR_RETIDO_COFINS]="+(nota as SNotaFiscalServico).valorCofinsRetido.ToString().Replace(",", ".") + " " +
               " ,[VALOR_RETIDO_CSLL]= " +  (nota as SNotaFiscalServico).valorCsllRetido.ToString().Replace(",", ".") + " " +
               " ,[VALOR_RETIDO_IR]= " +(nota as SNotaFiscalServico).valorIrRetido.ToString().Replace(",", ".") + ",[VALOR_CSLL]=" +(nota as SNotaFiscalServico).valorCsll.ToString().Replace(",", ".") + " " +
               " ,[VALOR_IR] = " +(nota as SNotaFiscalServico).valorIR.ToString().Replace(",", ".") + " " +
               " ,[VALOR_ISS] = " +(nota as SNotaFiscalServico).valorIss.ToString().Replace(",", ".") + " " +
               " ,[VALOR_ISS_RETIDO] = " +(nota as SNotaFiscalServico).valorIssRetido.ToString().Replace(",", ".") + "";
        }

        sql += " where [NUMERO_NOTA]=" + nota.numeroNota + " " +
           " AND [ENTRADA_SAIDA]='" + nota.entradaSaida + "' " +
           " AND [LOTE]=" + nota.lote + " " +
           " AND [PRODUTO_SERVICO]='" + nota.produtoServico + "' AND COD_EMPRESA="+nota.codEmpresa+"";

        _conn.execute(sql);
    }

    public SNotaFiscal load(double numeroNota, string entradaSaida, double lote, string produtoServico, int codEmpresa)
    {
        string sql = "select * from cad_notas_fiscais where [NUMERO_NOTA]=" + numeroNota + " " +
           " AND [ENTRADA_SAIDA]='" + entradaSaida + "' " +
           " AND [LOTE]=" + lote + " " +
           " AND COD_EMPRESA=" + codEmpresa + " ";

        DataTable tb = _conn.dataTable(sql, "notas");
        SNotaFiscal nota = null;
        if (tb.Rows.Count > 0)
        {
            nota = createObject(tb.Rows[0], produtoServico);
        }

        return nota;
    }

    public SNotaFiscal getOfLote(double lote, int codEmpresa)
    {
        string sql = "select * from cad_notas_fiscais where "+
           " [LOTE]=" + lote + " " +
           " AND COD_EMPRESA=" + codEmpresa + " ";

        DataTable tb = _conn.dataTable(sql, "notas");
        SNotaFiscal nota = null;
        if (tb.Rows.Count > 0)
        {
            string produtoServico = tb.Rows[0]["PRODUTO_SERVICO"].ToString();
            nota = createObject(tb.Rows[0], produtoServico);
        }

        return nota;
    }

    public List<SNotaFiscal> list(int codEmpresa, string produtoServico)
    {
        string sql = "select * from cad_notas_fiscais where " +
           " [PRODUTO_SERVICO]='" + produtoServico + "' " +
           " AND COD_EMPRESA=" + codEmpresa + " and lote in (select distinct lote from lanctos_contab) ";

        DataTable tb = _conn.dataTable(sql, "notas");
        List<SNotaFiscal> nota = new List<SNotaFiscal>();
        for (int i = 0; i < tb.Rows.Count;i++ )
        {
            nota.Add(createObject(tb.Rows[i], produtoServico));
        }

        return nota;
    }



    public List<SNotaFiscal> list(int codEmpresa, string produtoServico, DateTime inicio, DateTime termino)
    {
        string sql = "select * from cad_notas_fiscais where " +
           " [PRODUTO_SERVICO]='" + produtoServico + "' " +
           " AND (select distinct data from lanctos_contab where lote=cad_notas_fiscais.lote and pendente='False' and cod_empresa=" + codEmpresa + ") >= '" + inicio.ToString("yyyyMMdd") + "' and (select distinct data from lanctos_contab where lote=cad_notas_fiscais.lote and pendente='False' and cod_empresa=" + codEmpresa + ") <= '" + termino.ToString("yyyyMMdd") + "' " + 
           " AND COD_EMPRESA=" + codEmpresa + " and lote in (select distinct lote from lanctos_contab) ";

        DataTable tb = _conn.dataTable(sql, "notas");
        List<SNotaFiscal> nota = new List<SNotaFiscal>();
        for (int i = 0; i < tb.Rows.Count; i++)
        {
            nota.Add(createObject(tb.Rows[i], produtoServico));
        }

        return nota;
    }

    
    private SNotaFiscal createObject(DataRow row, string produtoServico)
    {
        lanctosContabDAO lanctosContabDAO = new lanctosContabDAO(_conn);

        SNotaFiscal nota = null;
        if (produtoServico == "P")
            nota = new SNotaFiscalProduto();
        else
            nota = new SNotaFiscalServico();

        nota.numeroNota = Convert.ToDouble(row["NUMERO_NOTA"]);
        nota.entradaSaida = row["ENTRADA_SAIDA"].ToString();
        nota.lote = Convert.ToDouble(row["LOTE"]);
        nota.cod_part = lanctosContabDAO.retornaCodCliente(Convert.ToInt32(row["LOTE"]));
        nota.data_emissao = lanctosContabDAO.retornaDataEmissao(Convert.ToInt32(row["LOTE"]));
        nota.produtoServico = row["PRODUTO_SERVICO"].ToString();
        nota.codEmpresa = Convert.ToInt32(row["COD_EMPRESA"]);
        nota.fornecedorCliente = Convert.ToInt32(row["FORNECEDOR_CLIENTE"]);
        nota.valor = Convert.ToDouble(row["VALOR"]);
        nota.descontos = Convert.ToDouble(row["DESCONTOS"]);
        nota.icms = Convert.ToDouble(row["ICMS"]);
        nota.baseImpostos = Convert.ToDouble(row["BASE_IMPOSTOS"]);
        nota.valorPis = Convert.ToDouble(row["VALOR_PIS"]);
        nota.valorCofins = Convert.ToDouble(row["VALOR_COFINS"]);
        nota.numeroEletronica = row["NUMERO_ELETRONICA"].ToString();
        nota.itens = loadItemNotaFiscal(Convert.ToInt32(row["LOTE"]));
        nota.frete = Convert.ToDouble(row["VALOR_COFINS"]);
        nota.ipi = Convert.ToDouble(row["IPI"]);
        nota.frete = Convert.ToDouble(row["VALOR_COFINS"]);
        nota.baseImpostos = Convert.ToDouble(row["BASE_IMPOSTOS"]);

        if (produtoServico == "P")
        {
            SNotaFiscalProduto temp = (SNotaFiscalProduto)nota;
            temp.frete = Convert.ToDouble(row["FRETE"]);
            temp.ipi = Convert.ToDouble(row["IPI"]);
            return temp;
        }
        else
        {
            SNotaFiscalServico temp = (SNotaFiscalServico)nota;
            temp.valorCofinsRetido = Convert.ToDouble(row["VALOR_RETIDO_COFINS"]);
            temp.valorPisRetido = Convert.ToDouble(row["VALOR_RETIDO_PIS"]);
            temp.valorCsllRetido = Convert.ToDouble(row["VALOR_RETIDO_CSLL"]);
            temp.valorIrRetido = Convert.ToDouble(row["VALOR_RETIDO_IR"]);
            temp.valorIssRetido = Convert.ToDouble(row["VALOR_ISS_RETIDO"]);
            temp.valorCsll = Convert.ToDouble(row["VALOR_CSLL"]);
            temp.valorIR = Convert.ToDouble(row["VALOR_IR"]);
            temp.valorIss = Convert.ToDouble(row["VALOR_ISS"]);
            return temp;
        }
    }

    public List<SItemNotaFiscal> loadItemNotaFiscal(int lote) 
    {
        List<SItemNotaFiscal> list = new List<SItemNotaFiscal>();
        
        string sql = "SELECT * FROM ITENS_NOTA_FISCAL WHERE LOTE = " + lote + " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " ";
        DataTable tb = _conn.dataTable(sql, "ITENS_NOTA_FISCAL");

        foreach(DataRow r in tb.Rows)
        {
            SItemNotaFiscal item = new SItemNotaFiscal();
            item.ordem = Convert.ToInt32(r["ORDEM"]);
            item.numeroNota = Convert.ToDouble(r["NUMERO_NOTA"]);
            item.entradaSaida = r["ENTRADA_SAIDA"].ToString();
            item.lote = Convert.ToDouble(r["LOTE"]);
            item.produtoServico = r["PRODUTO_SERVICO"].ToString();
            item.codigoProduto = Convert.ToInt32(r["COD_PRODUTO"]);
            item.qtde = Convert.ToDouble(r["QTDE"]);
            item.valorTotal = Convert.ToDouble(r["VALOR_TOTAL"]);
            item.valorBaseImp = Convert.ToDouble(r["VALOR_BASE_IMP"]);
            item.aliquotaIcms = Convert.ToDouble(r["ALIQ_ICMS"]);
            item.aliquotaCofins = Convert.ToDouble(r["ALIQ_COFINS"]);
            item.aliquotaPis = Convert.ToDouble(r["ALIQ_PIS"]);
            if (r["CFOP"] == null || r["CFOP"].ToString() == "")
            {
                item.cfop = 0;
            }
            else
            {
                item.cfop = Convert.ToInt32(r["CFOP"]);
            }
            item.aliquotaIpi = Convert.ToDouble(r["ALIQ_IPI"]);
            item.valorIpi = Convert.ToDouble(r["VALOR_IPI"]);
            list.Add(item);
        }

        return list;
    }
}