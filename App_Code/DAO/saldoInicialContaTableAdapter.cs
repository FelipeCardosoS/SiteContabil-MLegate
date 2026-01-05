using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace RelatoriosDAOTableAdapters
{
    public partial class saldoInicialContaTableAdapter
    {
        //public RelatoriosDAO.saldoInicialContaDataTable executa()
        //{
        //    string sql = "select convert(datetime,'20101215') as data, " +
        //                " cc.cod_conta + ' - ' + cc.descricao as descricao_conta, " +
        //                " sum(case when deb_cred = 'D' then isnull(lc.valor,0) else -isnull(lc.valor,0) end) as valor " +
        //                " from lanctos_contab lc right join cad_contas cc on lc.cod_conta = cc.cod_conta  " +
        //                " where lc.pendente='False' " +
        //                " and lc.data <= '20110115' " +
        //                " and lc.cod_empresa = 1 " +
        //                " group by cc.cod_conta + ' - ' + cc.descricao  ";

        //    if (this.Connection.State == System.Data.ConnectionState.Closed)
        //        this.Connection.Open();

        //    RelatoriosDAO.saldoInicialContaDataTable tb = new RelatoriosDAO.saldoInicialContaDataTable();
        //    using (SqlCommand comando = new SqlCommand(sql, this.Connection))
        //    {
        //        this.Adapter.SelectCommand = comando;
        //        this.Adapter.Fill(tb);
        //        return tb;
        //    }
        //}
    }
}
