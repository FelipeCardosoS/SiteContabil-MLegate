using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace RelatoriosDAOTableAdapters
{
    public partial class baseCalculoImpostoTableAdapter
	{
        public RelatoriosDAO.baseCalculoImpostoDataTable executar(DateTime periodoInicio, DateTime periodoTermino)
        {
            Conexao con = new Conexao();
            parametrosDAO parametros = new parametrosDAO(con);
            List<ListParametros> ListParams = parametros.carregaValores();
            
            string sql =    " select x.LOTE, x.DATA, SUM(x.Valor_Bruto) as Valor_Bruto,SUM(x.IR_Retido) as IR_Retido, SUM(x.CSL) as CSL, SUM(x.PIS) as PIS, " +
                            " SUM(x.COFINS) as COFINS, SUM(x.ISS) as ISS, SUM(x.VALOR_LIQUIDO) as VALOR_LIQUIDO from " +
                            "  (select distinct Lote from LANCTOS_CONTAB where LANCTOS_CONTAB.cod_empresa = '" + HttpContext.Current.Session["empresa"] + "' and lanctos_contab.cod_conta in (" + ListParams[0].COD_CONTAS + ") and lanctos_contab.data >= '" + periodoInicio.ToString("yyyyMMdd") + "' and lanctos_contab.data <= '" + periodoTermino.ToString("yyyyMMdd") + "') Lote_ValorBruto," +
                            "  (select LC.LOTE, LC.DATA, (case when lc.deb_cred = 'D' then LC.VALOR else -LC.VALOR end) as Valor_Bruto, " +
                            "  0 as IR_Retido, 0 as CSL,0 as PIS,0 as COFINS,0 as ISS,0 as VALOR_LIQUIDO from LANCTOS_CONTAB LC" +
                            "  WHERE LC.COD_EMPRESA = '" + HttpContext.Current.Session["empresa"] + "' AND LC.COD_CONTA IN (" + ListParams[0].COD_CONTAS + ")" +
                            "  union all" +
                            "  select LC1.LOTE, LC1.DATA, 0 as Valor_Bruto, LC1.VALOR as IR_Retido, 0 as CSL,0 as PIS,0 as COFINS,0 as ISS,0 as VALOR_LIQUIDO from LANCTOS_CONTAB LC1" +
                            "  WHERE LC1.COD_EMPRESA = '" + HttpContext.Current.Session["empresa"] + "' AND LC1.COD_CONTA IN ('" + ListParams[0].IR_NA_FONTE + "')" +
                            "  union all" +
                            "  select LC1.LOTE, LC1.DATA, 0 as Valor_Bruto, 0 as IR_Retido, LC1.VALOR as CSL,0 as PIS,0 as COFINS,0 as ISS,0 as VALOR_LIQUIDO from LANCTOS_CONTAB LC1" +
                            "  WHERE LC1.COD_EMPRESA = '" + HttpContext.Current.Session["empresa"] + "' AND LC1.COD_CONTA IN ('" + ListParams[0].CSL+ "')" +
                            "  union all" +
                            "  select LC1.LOTE, LC1.DATA, 0 as Valor_Bruto, 0 as IR_Retido, 0 as CSL,LC1.VALOR as PIS,0 as COFINS,0 as ISS,0 as VALOR_LIQUIDO from LANCTOS_CONTAB LC1" +
                            "  WHERE LC1.COD_EMPRESA = '" + HttpContext.Current.Session["empresa"] + "' AND LC1.COD_CONTA IN ('" + ListParams[0].PIS+ "')" +
                            "  union all" +
                            "  select LC1.LOTE, LC1.DATA, 0 as Valor_Bruto, 0 as IR_Retido, 0 as CSL,0 as PIS,LC1.VALOR as COFINS,0 as ISS,0 as VALOR_LIQUIDO from LANCTOS_CONTAB LC1" +
                            "  WHERE LC1.COD_EMPRESA = '" + HttpContext.Current.Session["empresa"] + "' AND LC1.COD_CONTA IN ('" + ListParams[0].COFINS + "')" +
                            "  union all" +
                            "  select LC1.LOTE, LC1.DATA, 0 as Valor_Bruto, 0 as IR_Retido, 0 as CSL,0 as PIS,0 as COFINS,LC1.VALOR as ISS,0 as VALOR_LIQUIDO from LANCTOS_CONTAB LC1" +
                            "  WHERE LC1.COD_EMPRESA = '" + HttpContext.Current.Session["empresa"] + "' AND LC1.COD_CONTA IN ('" + ListParams[0].ISS + "')" +
                            "  union all" +
                            "  select LC1.LOTE, LC1.DATA, 0 as Valor_Bruto, 0 as IR_Retido, 0 as CSL,0 as PIS,0 as COFINS,0 as ISS,LC1.VALOR as VALOR_LIQUIDO from LANCTOS_CONTAB LC1" +
                            "  WHERE LC1.COD_EMPRESA = '" + HttpContext.Current.Session["empresa"] + "' AND LC1.COD_CONTA IN ('" + ListParams[0].VALOR_LIQUIDO+ "')) x where " +
                            "  x.LOTE in (Lote_ValorBruto.Lote) group by x.LOTE, x.DATA";

            
            if (this.Connection.State == System.Data.ConnectionState.Closed)
                this.Connection.Open();

            RelatoriosDAO.baseCalculoImpostoDataTable tb = new RelatoriosDAO.baseCalculoImpostoDataTable();
            using (SqlCommand comando = new SqlCommand(sql, this.Connection))
            {
                this.Adapter.SelectCommand = comando;
                this.Adapter.Fill(tb);
                return tb;
            }
        }
	}
}