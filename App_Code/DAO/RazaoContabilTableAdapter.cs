using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
/// <summary>
/// Summary description for RazaoContabilTableAdapter
/// </summary>
/// 
namespace RelatoriosDAOTableAdapters
{
    public partial class RazaoContabilTableAdapter
    {
        public RelatoriosDAO.RazaoContabilDataTable executar(string contaDe, string contaAte, DateTime periodoAnterior, 
            DateTime periodoDe,DateTime periodoAte, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
            Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
            Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte)
        {
            string sqlWhere = "";

            if (divisaoDe.Value > 0)
                sqlWhere += " and lc.cod_divisao >= " + divisaoDe.Value + " ";
            if (divisaoAte.Value > 0)
                sqlWhere += " and lc.cod_divisao <= " + divisaoAte.Value + " ";

            if (linhaNegocioDe.Value > 0)
                sqlWhere += " and lc.cod_linha_negocio >= " + linhaNegocioDe.Value + " ";
            if (linhaNegocioAte.Value > 0)
                sqlWhere += " and lc.cod_linha_negocio <= " + linhaNegocioAte.Value + " ";

            if (clienteDe.Value > 0)
                sqlWhere += " and lc.cod_cliente >= " + clienteDe.Value + " ";
            if (clienteAte.Value > 0)
                sqlWhere += " and lc.cod_cliente <= " + clienteAte.Value + " ";

            if (jobDe.Value > 0)
                sqlWhere += " and lc.cod_job >= " + jobDe.Value + " ";
            if (jobAte.Value > 0)
                sqlWhere += " and lc.cod_job <= " + jobAte.Value + " ";

            string sql = "select * from ( "+
                        " select lc.cod_conta, 0 as lote_baixa, '' as numero_documento, 'D' as deb_cred,  "+
                        " cc.descricao, convert(datetime,'" + periodoAnterior.ToString("yyyyMMdd") + "') as data,'' as historico, " +
                        " 'Saldo Inicial' as nome_razao_social,'' as divisao,  '' as job,  "+
                        " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor, "+
                        " '' as terceiro  from lanctos_contab lc,cad_contas cc   "+
                        " where   lc.cod_empresa=1   "+
                        " "+sqlWhere+"    "+
                        " and lc.data <= '20100430'  and lc.pendente = 'False'    "+
                        " and lc.cod_conta = cc.cod_conta   "+
                        " group by lc.cod_conta, cc.descricao   "+
                        " union   "+
                        " select lc.cod_conta,  "+
                        " (case when lc.seq_baixa = 0 then lc.lote else lc.seq_baixa end) as lote_baixa,  "+
                        " lc.numero_documento,lc.deb_cred, "+
                        " cc.descricao,lc.data,lc.historico,ce.nome_razao_social,cd.descricao as divisao,  "+
                        " cj.descricao as job,    "+
                        " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor,  "+
                        " isnull(ce2.nome_razao_social,'Nenhum') as terceiro  "+ 
                        " from lanctos_contab lc left join cad_empresas ce2  "+
                        " on lc.cod_terceiro = ce2.cod_empresa and ce2.cod_empresa_pai=1,cad_contas cc, "+
                        " cad_empresas ce,cad_divisoes cd,cad_jobs cj,cad_empresas ce1   "+
                        " where   lc.cod_empresa=1   "+
                        " "+sqlWhere+"   "+
                        " and lc.data >= '" + periodoDe.ToString("yyyyMMdd") + "'  and lc.data <= '" + periodoAte.ToString("yyyyMMdd") + "'  and lc.pendente = 'False'   " +
                        " and lc.cod_conta = cc.cod_conta  and lc.cod_cliente = ce.cod_empresa   "+
                        " and lc.cod_divisao = cd.cod_divisao  and lc.cod_job = cj.cod_job   "+
                        " and cj.cod_cliente = ce1.cod_empresa   "+
                        " group by lc.cod_conta,  "+
                        " (case when lc.seq_baixa = 0 then lc.lote else lc.seq_baixa end), "+
                        " lc.numero_documento,lc.deb_cred, "+
                        " cc.descricao,lc.data,lc.historico,ce.nome_razao_social,cd.descricao, cj.descricao, "+
                        " isnull(ce2.nome_razao_social,'Nenhum') " +
                        " ) x   order by x.cod_conta, x.lote_baixa,x.data";

            if (this.Connection.State == System.Data.ConnectionState.Closed)
                this.Connection.Open();

            RelatoriosDAO.RazaoContabilDataTable tb = new RelatoriosDAO.RazaoContabilDataTable();
            using (SqlCommand comando = new SqlCommand(sql, this.Connection))
            {
                this.Adapter.SelectCommand = comando;
                this.Adapter.Fill(tb);
                return tb;
            }

        }
    }
}