using System;
using System.Web;
using System.Data.SqlClient;
/// <summary>
/// Summary description for TitulosPendentesTableAdapter
/// </summary>
/// 
namespace RelatoriosDAOTableAdapters
{
    public partial class TitulosPendentesTableAdapter
    {
        public RelatoriosDAO.TitulosPendentesDataTable executar(DateTime periodo, int diasVencimento, 
            string contaDe, string contaAte, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
            Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
            Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, 
            Nullable<int> terceiroDe, Nullable<int> terceiroAte, string ordenacao)
        {
            string sqlWhere = "";

            if (contaDe != null)
                sqlWhere += " and lc.cod_conta >= '" + contaDe + "'";

            if (contaAte != null)
                sqlWhere += " and lc.cod_conta <= '" + contaAte + "'";

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

            if (terceiroDe.Value > 0)
                sqlWhere += " and lc.cod_terceiro >= " + terceiroDe.Value + " ";
            if (terceiroAte.Value > 0)
                sqlWhere += " and lc.cod_terceiro <= " + terceiroAte.Value + " ";

            DateTime periodoNovo = new DateTime();

            if (diasVencimento > 0)
            {
                periodoNovo = periodo.AddDays(-diasVencimento);
                sqlWhere += " and lcpai.data >= '" + periodoNovo.ToString("yyyyMMdd") + "' and lcpai.data <= '" + periodo.ToString("yyyyMMdd") + "' ";
            }
            else
            {
                periodoNovo = periodo;
                sqlWhere += " and lcpai.data <= '" + periodoNovo.ToString("yyyyMMdd") + "' ";
            }
            string Grupo = "";
            string GrupoOrderBy = "";

            switch (ordenacao)
            {
                case "TERCEIRO":
                    {
                        Grupo = "ce.nome_razao_social as grupo, (case when lc.pendente = 'False' then year(lc.data_anterior)*10000 + month(lc.data_anterior) * 100 + day(lc.data_anterior) else year(lc.data) * 10000 + month(lc.data) * 100 + day(lc.data) end) ";
                        GrupoOrderBy = "ce.nome_razao_social , (case when lc.pendente = 'False' then year(lc.data_anterior)*10000 + month(lc.data_anterior) * 100 + day(lc.data_anterior) else year(lc.data) * 10000 + month(lc.data) * 100 + day(lc.data) end) ";
                        break;
                    }
                case "EMITENTE":
                    {
                        Grupo = "isnull(nf.NOME_RAZAO_SOCIAL, 'Sem Origem no Faturamento') as grupo, (case when lc.pendente = 'False' then year(lc.data_anterior)*10000 + month(lc.data_anterior) * 100 + day(lc.data_anterior) else year(lc.data) * 10000 + month(lc.data) * 100 + day(lc.data) end) ";
                        GrupoOrderBy = "isnull(nf.NOME_RAZAO_SOCIAL, 'Sem Origem no Faturamento') , (case when lc.pendente = 'False' then year(lc.data_anterior)*10000 + month(lc.data_anterior) * 100 + day(lc.data_anterior) else year(lc.data) * 10000 + month(lc.data) * 100 + day(lc.data) end) ";
                        break;
                    }
                case "VENCIMENTO":
                    {
                        Grupo = "(case when lc.pendente = 'False' then year(lc.data_anterior)*10000 + month(lc.data_anterior) * 100 + day(lc.data_anterior) else year(lc.data) * 10000 + month(lc.data) * 100 + day(lc.data) end) as grupo, lc.cod_terceiro ";
                        GrupoOrderBy = "(case when lc.pendente = 'False' then year(lc.data_anterior)*10000 + month(lc.data_anterior) * 100 + day(lc.data_anterior) else year(lc.data) * 10000 + month(lc.data) * 100 + day(lc.data) end) , lc.cod_terceiro ";
                        break;
                    }
            default:
                    {
                        Grupo = "(case when lc.pendente = 'False' then year(lc.data_anterior)*10000 + month(lc.data_anterior) * 100 + day(lc.data_anterior) else year(lc.data) * 10000 + month(lc.data) * 100 + day(lc.data) end) as grupo, lc.cod_terceiro ";
                        GrupoOrderBy = "(case when lc.pendente = 'False' then year(lc.data_anterior)*10000 + month(lc.data_anterior) * 100 + day(lc.data_anterior) else year(lc.data) * 10000 + month(lc.data) * 100 + day(lc.data) end) , lc.cod_terceiro ";
                        break;
                    }
            }
          

            string sql = "select " + Grupo +
                         ", ce.nome_razao_social,lc.cod_job,lc.lote,lc.seq_baixa, lc.lote_pai, " +
                         " cej.nome_fantasia + ' - ' + cj.descricao as desc_job, (case when lc.pendente='False' then lc.data_anterior else lc.data end) as data,   " +
                         " sum((case when CGC.DESCRICAO like 'Ativo%' then (case when lc.deb_cred = 'D' then -lc.valor else lc.valor end) else (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) end))  as valor,   " +
                         " case when (case when lc.pendente='False' then lc.data_anterior else lc.data end) < '" + periodo.ToString("yyyyMMdd") + "' then 'Vencido' else 'à Vencer' end as status,  " +
                         " lcpai.data as data_lancamento, lc.cod_conta, lc.cod_conta + ' - ' + cc.descricao as desc_conta, " +
                         " '" + ordenacao + "' as desc_grupo, " +
                         " lcpai.numero_documento, lcpai.valor as valor_origem, valor_bruto, isnull(nf.NOME_RAZAO_SOCIAL, 'Sem Origem no Faturamento') as Emitente " +
                         " from lanctos_contab lc " +
                         " left join (select distinct nf.lote, ce.NOME_RAZAO_SOCIAL from FATURAMENTO_NF nf, CAD_EMPRESAS CE where nf.COD_EMITENTE = ce.COD_EMPRESA and isnull(lote,0) <> 0) NF " +
                         " on lc.LOTE_PAI = nf.LOTE " +
                         " left join cad_contas cc on lc.cod_conta = cc.cod_conta and lc.cod_empresa = cc.cod_empresa left join CAD_GRUPOS_CONTABEIS CGC on cc.cod_grupo_contabil = cgc.COD_GRUPO_CONTABIL, " +
                         " (select lote, seq_lote, data, cod_empresa, duplicata, modulo, max(isnull(numero_documento,0)) as numero_documento, sum(VALOR) as valor from lanctos_contab group by lote, seq_lote, data, cod_empresa, duplicata, modulo) lcpai ,  " +
                         " cad_empresas ce,cad_jobs cj, cad_empresas cej " +
                         " where lc.duplicata > 0    " +
                         " and lc.lote_pai > 0    " +
                         " and lc.cod_empresa = " + HttpContext.Current.Session["empresa"] + "   " +
                         " and cej.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + "   " +
                         sqlWhere +
                         " and (lc.pendente = 'True' or (lc.pendente='False' and lc.data >  '" + periodo.ToString("yyyyMMdd") + "'))   " +
                         " and lc.duplicata = lcpai.duplicata   " +
                         " and lc.lote_pai = lcpai.lote   " +
                         " and lc.cod_empresa = lcpai.cod_empresa   " +
                         " and lc.seq_lote_pai = lcpai.seq_lote   " +
                         " and lc.cod_terceiro = ce.cod_empresa    " +
                         " and lc.cod_job = cj.cod_job    " +
                         " and cj.cod_cliente = cej.cod_empresa " +
                         " group by nf.NOME_RAZAO_SOCIAL , " + GrupoOrderBy + " ,ce.nome_razao_social, " +
                         " lc.cod_job,lc.lote,lc.seq_baixa,lc.lote_pai,cej.nome_fantasia + ' - ' + cj.descricao,  " +
                         " (case when lc.pendente='False' then lc.data_anterior else lc.data end), lcpai.data, lc.cod_conta, cc.descricao, lcpai.numero_documento, lcpai.valor, valor_bruto order by grupo";

            try
            {
                if (this.Connection.State == System.Data.ConnectionState.Closed)
                    this.Connection.Open();

                RelatoriosDAO.TitulosPendentesDataTable tb = new RelatoriosDAO.TitulosPendentesDataTable();
                using (SqlCommand comando = new SqlCommand(sql, this.Connection))
                {
                    comando.CommandTimeout = 0;
                    this.Adapter.SelectCommand = comando;
                    this.Adapter.Fill(tb);
                    return tb;
                }
            }
            catch(Exception wer)
            {
                throw new Exception(wer.Message);
            }
        }
    }
}