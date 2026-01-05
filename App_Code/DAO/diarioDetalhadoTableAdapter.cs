using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace RelatoriosDAOTableAdapters
{
    public partial class diarioDetalhadoTableAdapter
    {
        public RelatoriosDAO.diarioDetalhadoDataTable executa(DateTime periodoInicio, DateTime periodoTermino,
                    int inicioPagina, int totalPaginas, string detalhamento)
        {
            string sqlColuna = "";
            string sqlFrom = "";
            string sqlWhere = "";
            string sqlGroupBy = "";

            switch (detalhamento)
            {
                case "JOB":
                    sqlColuna = " cad_jobs.cod_job as cod_detalhamento, cad_empresas.nome_fantasia + ' - ' + cad_jobs.descricao ";
                    sqlFrom = " cad_jobs inner join cad_empresas on cad_jobs.cod_cliente = cad_empresas.cod_empresa ";
                    sqlWhere = " and lc.cod_job = cad_jobs.cod_job ";
                    sqlGroupBy = " cad_jobs.cod_job, cad_empresas.nome_fantasia + ' - ' + cad_jobs.descricao ";
                    break;
                case "LINHA_NEGOCIO":
                    sqlColuna = "  cad_linha_negocios.cod_linha_negocio as cod_detalhamento, cad_linha_negocios.descricao ";
                    sqlFrom = " cad_linha_negocios ";
                    sqlWhere = " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio ";
                    sqlGroupBy = " cad_linha_negocios.cod_linha_negocio, cad_linha_negocios.descricao ";
                    break;
                case "DIVISAO":
                    sqlColuna = " cad_divisoes.cod_divisao as cod_detalhamento,  cad_divisoes.descricao ";
                    sqlFrom = " cad_divisoes ";
                    sqlWhere = " and lc.cod_divisao = cad_divisoes.cod_divisao ";
                    sqlGroupBy = " cad_divisoes.cod_divisao, cad_divisoes.descricao ";
                    break;
                case "CLIENTE":
                    sqlColuna = " cad_empresas.cod_empresa as cod_detalhamento, cad_empresas.nome_razao_social ";
                    sqlFrom = " cad_empresas ";
                    sqlWhere = " and lc.cod_cliente = cad_empresas.cod_empresa ";
                    sqlGroupBy = " cad_empresas.cod_empresa, cad_empresas.nome_razao_social ";
                    break;
            }

            string sql = "select ce.nome_razao_social,ce.ie_rg,ce.cnpj_cpf, lc.data, lc.lote,lc.deb_cred,lc.cod_conta + ' - ' +cc.descricao as conta,lc.numero_documento, " +
                        " lc.historico, sum(lc.valor) as valor, "+sqlColuna+" as descricao_detalhamento " +
                        " from lanctos_contab lc, cad_contas cc, cad_empresas ce, "+sqlFrom+" " +
                        " where lc.pendente='False' " +
                        " and lc.cod_conta = cc.cod_conta " +
                        " and lc.cod_empresa = ce.cod_empresa " +
                        " and lc.data >= '" + periodoInicio.ToString("yyyyMMdd") + "' " +
                        " and lc.data <= '" + periodoTermino.ToString("yyyyMMdd") + "' " +
                        " "+sqlWhere+" " +
                        " group by ce.nome_razao_social,ce.ie_rg, ce.cnpj_cpf,lc.data,lc.lote,lc.seq_lote,lc.deb_cred, " +
                        " lc.cod_conta + ' - ' +cc.descricao, lc.numero_documento, " +
                        " lc.historico, "+sqlGroupBy+" " +
                        " order by lc.data,lc.lote,lc.cod_conta + ' - ' +cc.descricao";

            if (this.Connection.State == System.Data.ConnectionState.Closed)
                this.Connection.Open();

            RelatoriosDAO.diarioDetalhadoDataTable tb = new RelatoriosDAO.diarioDetalhadoDataTable();
            using (SqlCommand comando = new SqlCommand(sql, this.Connection))
            {
                this.Adapter.SelectCommand = comando;
                this.Adapter.Fill(tb);
                return tb;
            }
        }
    }
}
