using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Summary description for DiarioTableAdapter
/// </summary>
/// 
namespace RelatoriosDAOTableAdapters
{
    public partial class diarioTableAdapter
    {
        public RelatoriosDAO.diarioDataTable executa(DateTime periodoInicio, DateTime periodoTermino,
            int inicioPagina, int totalPaginas)
        {
            string sql = "select ce.nome_razao_social,ce.ie_rg,ce.cnpj_cpf, lc.data, lc.lote,lc.deb_cred, " +
                        " lc.cod_conta + ' - ' +cc.descricao as conta,lc.numero_documento, " +
                        " lc.historico, sum(lc.valor) as valor " +
                        " from lanctos_contab lc, cad_contas cc, cad_empresas ce " +
                        " where lc.pendente='False' and ce.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " and lc.cod_empresa = cc.cod_empresa " +
                        " and lc.cod_conta = cc.cod_conta " +
                        " and lc.cod_empresa = ce.cod_empresa " +
                        " and lc.data >= '"+periodoInicio.ToString("yyyyMMdd")+"' " +
                        " and lc.data <= '" + periodoTermino.ToString("yyyyMMdd") + "' " +
                        " group by ce.nome_razao_social,ce.ie_rg, ce.cnpj_cpf,lc.data,lc.lote,lc.seq_lote,lc.deb_cred, " +
                        " lc.cod_conta + ' - ' +cc.descricao, lc.numero_documento, " +
                        " lc.historico " +
                        " order by lc.data,lc.lote,lc.cod_conta + ' - ' +cc.descricao";

            try
            {
                if (this.Connection.State == System.Data.ConnectionState.Closed)
                {
                    this.Connection = new SqlConnection((string)ConfigurationManager.ConnectionStrings["strConexao"].ConnectionString);
                    this.Connection.Open();
                }
            }
            catch(Exception err)
            {
                throw new Exception(err.Message);
            }

            RelatoriosDAO.diarioDataTable tb = new RelatoriosDAO.diarioDataTable();
            using (SqlCommand comando = new SqlCommand(sql, this.Connection))
            {
                this.Adapter.SelectCommand = comando;
                this.Adapter.Fill(tb);
                return tb;
            }
        }
    }
}