using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for fluxoFinanceiroTableAdapter
/// </summary>
/// 
namespace RelatoriosDAOTableAdapters
{
    public partial class fluxoFinanceiroTableAdapter
    {
        //public RelatoriosDAO.fluxoFinanceiroDataTable executa()
        //{
        //    string sql = " select lc.lote,lc.lote_pai, lc.seq_lote_pai,lc.data,lc.cod_conta + ' - ' + cc.descricao as descricao_conta, " +
        //                " lc.cod_terceiro,(case when lc.cod_terceiro = 0 then 'Nenhum' else ce.nome_razao_social end) as descricao_terceiro,lc.cod_job,  " +
        //                " cj.descricao + '+' + ce1.nome_fantasia as desc_job, " +
        //                " lc.historico,lc.deb_cred, sum(lc.valor) valor " +
        //                " from lanctos_contab lc left join cad_empresas ce on ce.cod_empresa_pai = 1 and lc.cod_terceiro = ce.cod_empresa, cad_contas cc, " +
        //                " cad_jobs cj, cad_empresas ce1 " +
        //                " where lc.pendente='False' " +
        //                " and lc.data >= '20110115' and lc.data <= '20110215'   " +
        //        //" and lc.cod_conta >= '1111' and lc.cod_conta <= '11531' " +
        //                " and lc.cod_empresa = 1 " +
        //                " and lc.cod_conta = cc.cod_conta " +
        //                " and lc.cod_job = cj.cod_job " +
        //                " and cj.cod_cliente = ce1.cod_empresa " +
        //                " group by lc.lote,lc.lote_pai, lc.seq_lote_pai,lc.data, lc.cod_conta + ' - ' + cc.descricao,lc.cod_terceiro, " +
        //                " (case when lc.cod_terceiro = 0 then 'Nenhum' else ce.nome_razao_social end), " +
        //                " lc.cod_job, cj.descricao + '+' + ce1.nome_fantasia,lc.historico,lc.deb_cred ";

        //    if (this.Connection.State == System.Data.ConnectionState.Closed)
        //        this.Connection.Open();

        //    RelatoriosDAO.fluxoFinanceiroDataTable tb = new RelatoriosDAO.fluxoFinanceiroDataTable();
        //    using (SqlCommand comando = new SqlCommand(sql, this.Connection))
        //    {
        //        this.Adapter.SelectCommand = comando;
        //        this.Adapter.Fill(tb);
        //        return tb;
        //    }
        //}
    }
}
