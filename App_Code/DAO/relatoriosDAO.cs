using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;

public class relatoriosDAO
{
    private Conexao _conn;

    public relatoriosDAO(Conexao c)
    {
        _conn = c;
    }

    public void titulosPendentes(DateTime periodo, int diasVencimento, string tipo, string contaDe, string contaAte, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, string ordenacao, ref DataTable tb)
    {
        string sqlWhere = "";
        string sqlOrder = "";
        string sql = "";

        if (tipo != null)
            sqlWhere += " and lc.modulo = '" + tipo + "'";

        if (contaDe != null)
            sqlWhere += " and lc.cod_conta >= '" + contaDe + "'";
        if (contaAte != null)
            sqlWhere += " and lc.cod_conta <= '" + contaAte + "'";

        if (divisaoDe != null)
            sqlWhere += " and lc.cod_divisao >= " + divisaoDe.Value + "";
        if (divisaoAte != null)
            sqlWhere += " and lc.cod_divisao <= " + divisaoAte.Value + "";

        if (linhaNegocioDe != null)
            sqlWhere += " and lc.cod_linha_negocio >= " + linhaNegocioDe.Value + "";
        if (linhaNegocioAte != null)
            sqlWhere += " and lc.cod_linha_negocio <= " + linhaNegocioAte.Value + "";

        if (clienteDe != null)
            sqlWhere += " and lc.cod_cliente >= " + clienteDe.Value + "";
        if (clienteAte != null)
            sqlWhere += " and lc.cod_cliente <= " + clienteAte.Value + "";

        if (jobDe != null)
            sqlWhere += " and lc.cod_job >= " + jobDe.Value + "";
        if (jobAte != null)
            sqlWhere += " and lc.cod_job <= " + jobAte.Value + "";

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

        if (ordenacao == null)
        {
            sqlOrder = "";
        }
        else
        {
            sqlOrder = "x." + ordenacao;
        }

        sql = "select * from ( " +
            " select (case when lc.modulo = 'CAP_INCLUSAO_TITULO' then 'Contas à Pagar' else 'Contas à Receber' end) as modulo, lcpai.modulo as modulo_original,lc.cod_terceiro,ce.nome_razao_social,lc.cod_job,lc.lote,lc.seq_baixa, lc.lote_pai,cej.nome_fantasia + ' - ' + cj.descricao as desc_job, lc.data, " +
            " sum(case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) as valor, " +
            " case when lc.data < '" + periodo.ToString("yyyyMMdd") + "' then 'Vencido' else 'à Vencer' end as status, lcpai.data as data_lancamento, lc.cod_conta, lc.desc_conta " +
            " from lanctos_contab lc, (select distinct lote, seq_lote, data, cod_empresa, duplicata, modulo from lanctos_contab) lcpai , cad_empresas ce,cad_jobs cj, cad_empresas cej " +
            " where lc.duplicata > 0  " +
            " and lc.lote_pai > 0  " +
            " and (lc.pendente = 'True' or (lc.pendente='False' and lc.data >  '" + periodo.ToString("yyyyMMdd") + "')) " +
            " and lc.duplicata = lcpai.duplicata " +
            " and lc.lote_pai = lcpai.lote " +
            " and lc.cod_empresa = lcpai.cod_empresa " +
            " and lc.seq_lote = lcpai.seq_lote " +
            " and lc.cod_terceiro = ce.cod_empresa  " +
            " and lc.cod_job = cj.cod_job  " +
            " and cj.cod_cliente = cej.cod_empresa " +
            " and lc.cod_empresa = " + HttpContext.Current.Session["empresa"] + " " +
            " and cej.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + " " + sqlWhere + " " +
            " group by (case when lc.modulo = 'CAP_INCLUSAO_TITULO' then 'Contas à Pagar' else 'Contas à Receber' end), lcpai.modulo, lc.cod_terceiro,ce.nome_razao_social,lc.cod_job,lc.lote,lc.seq_baixa,lc.lote_pai,cej.nome_fantasia + ' - ' + cj.descricao, lc.data, lcpai.data, lc.cod_conta, lc.desc_conta) x " +
            " order by modulo" + (sqlOrder == "" ? "" : ",") + sqlOrder + " ";

        _conn.fill(sql, ref tb);
    }

    public void dre(Nullable<int> tipoMoeda, DateTime periodoDe, DateTime periodoAte, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, string detalhamento1,
        string detalhamento2, string detalhamento3, string detalhamento4, string detalhamento5, ref DataTable tb, bool encerramento, string empresas)
    {
        string sqlWhere = "";
        string sqlInicioColunas = "";
        string sql = "";
        string tabela = " LANCTOS_CONTAB ";
        string moedaWhere = "";

        if (tipoMoeda != 0 && tipoMoeda != null)
        {
            tabela = " LANCTOS_CONTAB_MOEDA ";
            moedaWhere = " AND lc.COD_MOEDA = " + tipoMoeda + " ";
        }

        if (divisaoDe != null && divisaoAte != null)
            sqlWhere += @" and lc.cod_divisao in (select cod_divisao from cad_divisoes where cod_empresa in (" + empresas + @") and descricao between 
                                    (select min(descricao) from cad_divisoes where cod_empresa in (" + empresas + @") and  COD_DIVISAO = " + divisaoDe.Value + @") and 
                                    (select max(descricao) from cad_divisoes where cod_empresa in (" + empresas + @") and  COD_DIVISAO = " + divisaoAte.Value + ")) ";

        //if (divisaoAte != null)
        //    sqlWhere += " and lc.cod_divisao <= " + divisaoAte.Value + "";

        if (linhaNegocioDe != null && linhaNegocioAte != null)
            sqlWhere += @" and lc.COD_LINHA_NEGOCIO in (select COD_LINHA_NEGOCIO from CAD_LINHA_NEGOCIOS where cod_empresa in (" + empresas + @") and descricao between 
                                    (select min(descricao) from CAD_LINHA_NEGOCIOS where cod_empresa in (" + empresas + @") and  COD_LINHA_NEGOCIO = " + linhaNegocioDe.Value + @") and 
                                    (select max(descricao) from CAD_LINHA_NEGOCIOS where cod_empresa in (" + empresas + @") and  COD_LINHA_NEGOCIO = " + linhaNegocioAte.Value + ")) ";

        //if (linhaNegocioAte != null)
        //    sqlWhere += " and lc.cod_linha_negocio <= " + linhaNegocioAte.Value + "";

        if (clienteDe != null && clienteAte != null)
            sqlWhere += @" and lc.cod_cliente in (select COD_EMPRESA from CAD_EMPRESAS where NOME_RAZAO_SOCIAL between 
                                    (select min(NOME_RAZAO_SOCIAL) from CAD_EMPRESAS where cod_empresa_pai in (" + empresas + @") and  COD_EMPRESA = " + clienteDe.Value + @") and 
                                    (select max(NOME_RAZAO_SOCIAL) from CAD_EMPRESAS where cod_empresa_pai in (" + empresas + @") and  COD_EMPRESA = " + clienteAte.Value + ")) ";

        //if (clienteAte != null)
        //    sqlWhere += " and lc.cod_cliente <= " + clienteAte.Value + "";

        if (jobDe != null && jobAte != null)
            sqlWhere += @" and lc.cod_job in (select cod_job from CAD_JOBS cj, cad_empresas ce 
               where cj.status = 'A' and ce.cod_empresa = cj.cod_cliente and cj.cod_empresa in (" + empresas + @") 
									and ce.Nome_fantasia + cj.descricao between 
                                    (select max(ce.Nome_fantasia) + min(cj.descricao) from CAD_JOBS cj, cad_empresas ce
                                    where ce.cod_empresa = cj.cod_cliente and  cj.cod_job = " + jobDe + @"
                                    and cj.cod_empresa in (" + empresas + @")) and 
                                    (select max(ce.Nome_fantasia) + max(cj.descricao) from CAD_JOBS cj, cad_empresas ce
                                    where ce.cod_empresa = cj.cod_cliente and  cj.cod_job = " + jobAte + @"
                                    and cj.cod_empresa in (" + empresas + @")))
                                     ";


        //if (jobAte != null)
        //    sqlWhere += " and lc.cod_job <= " + jobAte.Value + "";

        if(encerramento == false)
            sqlWhere += " and lc.tipo_lancto = 'N' "; 

        if (detalhamento1 != null)
            sqlInicioColunas += ",0 as codDetalhe1,'' as descDetalhe1";
        if (detalhamento2 != null)
            sqlInicioColunas += ",0 as codDetalhe2,'' as descDetalhe2";
        if (detalhamento3 != null)
            sqlInicioColunas += ",0 as codDetalhe3,'' as descDetalhe3";
        if (detalhamento4 != null)
            sqlInicioColunas += ",0 as codDetalhe4,'' as descDetalhe4";
        if (detalhamento5 != null)
            sqlInicioColunas += ",0 as codDetalhe5,'' as descDetalhe5";



        sql = "select * from (select cc1.cod_conta, replicate('  ',len(cc1.cod_conta)-1)+cc1.descricao as descricao, year(lc.data)*100 + month(lc.data) as ano_mes , " +
            " sum(case when lc.deb_cred = 'C' then lc.valor else -lc.valor end) as valor,cc1.analitica " + sqlInicioColunas + " " +
            " from "+tabela+" lc, cad_contas cc1,cad_contas cc2 " +
            " where lc.pendente = 'False' and lc.cod_empresa in (" + empresas + ") and cc1.cod_empresa in (" + empresas + ") and cc2.cod_empresa in (" + empresas + ") and " +
            " lc.cod_empresa = cc1.COD_EMPRESA  and lc.cod_empresa = cc2.COD_EMPRESA and " +
            " year(lc.data)*100 + month(lc.data) >= " + periodoDe.ToString("yyyyMM") + " and  " +
            " year(lc.data)*100 + month(lc.data) <= " + periodoAte.ToString("yyyyMM") + " and  " +
            " lc.cod_conta in " +
            " (select cad_contas.cod_Conta from cad_contas, cad_grupos_contabeis where cad_contas.cod_grupo_contabil = cad_grupos_contabeis.cod_grupo_contabil and regra_exibicao='R'  and cad_contas.cod_empresa in (" + empresas + ")) " +
            " and cc2.cod_conta = lc.cod_Conta " +
            " and cc1.cod_conta = substring(cc2.cod_conta,1,len(cc1.cod_conta)) " + sqlWhere + "" +
            " group by cc1.cod_conta, cc1.descricao,year(lc.data)*100 + month(lc.data),cc1.analitica";

        string fromPrincipal = "";
        bool existeTerceiro = false;
        if (detalhamento1 == "TERCEIRO")
            existeTerceiro = true;
        else if (detalhamento2 == "TERCEIRO")
            existeTerceiro = true;
        else if (detalhamento3 == "TERCEIRO")
            existeTerceiro = true;
        else if (detalhamento4 == "TERCEIRO")
            existeTerceiro = true;
        else if (detalhamento5 == "TERCEIRO")
            existeTerceiro = true;

        if (existeTerceiro)
        {
            //fromPrincipal = " " + tabela + " lc left join cad_empresas ce on lc.cod_terceiro = ce.cod_empresa";
            fromPrincipal = " " + tabela + " lc left join " + tabela + " lct on lc.lote = lct.lote and lct.seq_lote = 1 and lct.det_lote = 1 left join cad_empresas ce on lc.cod_terceiro = ce.cod_empresa left join cad_empresas cet on lct.cod_terceiro = cet.cod_empresa ";
        }
        else
        {
            fromPrincipal = " " + tabela + " lc ";
        }

        if (detalhamento1 != null)
        {
            string sqlColunas1 = "";
            string sqlColunasAdicionais1 = "";
            string sqlGroup1 = "";
            string sqlFrom1 = "";
            string sqlRel1 = "";

            if (detalhamento2 != null)
                sqlColunasAdicionais1 += ",0 as codDetalhe2,'' as descDetalhe2";
            if (detalhamento3 != null)
                sqlColunasAdicionais1 += ",0 as codDetalhe3,'' as descDetalhe3";
            if (detalhamento4 != null)
                sqlColunasAdicionais1 += ",0 as codDetalhe4,'' as descDetalhe4";
            if (detalhamento5 != null)
                sqlColunasAdicionais1 += ",0 as codDetalhe5,'' as descDetalhe5";

            switch (detalhamento1)
            {
                case "DIVISAO":
                    sqlColunas1 = ",lc.cod_divisao as codDetalhe1,replicate('  ',len(cc1.cod_conta)+1) + cad_divisoes.descricao as descDetalhe1";
                    sqlGroup1 = "lc.cod_divisao,replicate('  ',len(cc1.cod_conta)+1) + cad_divisoes.descricao";


                    sqlFrom1 = ", cad_divisoes";
                    sqlRel1 = " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa in (" + empresas + ") ";

                    break;
                case "LINHA_NEGOCIO":
                    sqlColunas1 = ",lc.cod_linha_negocio as codDetalhe1,replicate('  ',len(cc1.cod_conta)+1) + cad_linha_negocios.descricao as descDetalhe1";
                    sqlGroup1 = "lc.cod_linha_negocio,replicate('  ',len(cc1.cod_conta)+1) + cad_linha_negocios.descricao";


                    sqlFrom1 = ", cad_linha_negocios";
                    sqlRel1 = " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa in (" + empresas + ") ";
                    break;
                case "CLIENTE":
                    sqlColunas1 = ",lc.cod_cliente as codDetalhe1,replicate('  ',len(cc1.cod_conta)+1) + cad_empresas.nome_razao_social as descDetalhe1";
                    sqlGroup1 = "lc.cod_cliente,replicate('  ',len(cc1.cod_conta)+1) + cad_empresas.nome_razao_social";


                    sqlFrom1 = ", cad_empresas";
                    sqlRel1 = " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai in (" + empresas + ") ";
                    break;
                case "JOB":
                    sqlColunas1 = ",lc.cod_job as codDetalhe1,replicate('  ',len(cc1.cod_conta)+1) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe1";
                    sqlGroup1 = "lc.cod_job,replicate('  ',len(cc1.cod_conta)+1) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                    sqlFrom1 = ", cad_jobs  left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                    sqlRel1 = " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa in (" + empresas + ") ";
                    break;
                case "TERCEIRO":
                    sqlColunas1 = ", (case when lc.cod_terceiro = 0 then (case when isnull(lct.cod_terceiro,0) = 0 then 99999999 else lct.cod_terceiro end) else lc.cod_terceiro end) as codDetalhe1,(case when lc.cod_terceiro = 0 then replicate('  ',len(cc1.cod_conta)+1) + isnull(cet.nome_razao_social,'Não Definido') else  replicate('  ',len(cc1.cod_conta)+1) + isnull(ce.nome_razao_social,'Não Definido') end) as descDetalhe1 ";
                    sqlGroup1 = " (case when lc.cod_terceiro = 0 then (case when isnull(lct.cod_terceiro,0) = 0 then 99999999 else lct.cod_terceiro end) else lc.cod_terceiro end), (case when lc.cod_terceiro = 0 then replicate('  ',len(cc1.cod_conta)+1) + isnull(cet.nome_razao_social,'Não Definido') else  replicate('  ',len(cc1.cod_conta)+1) + isnull(ce.nome_razao_social,'Não Definido') end) ";


                    sqlFrom1 = " ";
                    sqlRel1 = " ";
                    break;
            }

            sql += " union ";
            sql += "select cc1.cod_conta, replicate('  ',len(cc1.cod_conta)-1)+cc1.descricao as descricao, year(lc.data)*100 + month(lc.data) as ano_mes , " +
                " sum(case when lc.deb_cred = 'C' then lc.valor else -lc.valor end) as valor,cc1.analitica " + sqlColunas1 + sqlColunasAdicionais1 + " " +
                " from " + fromPrincipal + ", cad_contas cc1,cad_contas cc2 " + sqlFrom1 + "" +
                " where lc.pendente = 'False' and lc.cod_empresa in (" + empresas + ") and cc1.cod_empresa in (" + empresas + ") and cc2.cod_empresa in (" + empresas + ") and " +
                " lc.cod_empresa = cc1.COD_EMPRESA  and lc.cod_empresa = cc2.COD_EMPRESA and " +
                " year(lc.data)*100 + month(lc.data) >= " + periodoDe.ToString("yyyyMM") + " and  " +
                " year(lc.data)*100 + month(lc.data) <= " + periodoAte.ToString("yyyyMM") + " and  " +
                " lc.cod_conta in " +
                " (select cad_contas.cod_Conta from cad_contas, cad_grupos_contabeis where cad_contas.cod_grupo_contabil = cad_grupos_contabeis.cod_grupo_contabil and regra_exibicao='R' and cad_contas.cod_empresa in (" + empresas + ")) " +
                " and cc2.cod_conta = lc.cod_Conta " +
                " and cc1.analitica = 1 " +
                " and cc1.cod_conta = substring(cc2.cod_conta,1,len(cc1.cod_conta)) " + sqlWhere + sqlRel1 + "" + moedaWhere +
                " group by cc1.cod_conta, cc1.descricao,year(lc.data)*100 + month(lc.data),cc1.analitica, " + sqlGroup1 + "";

            if (detalhamento2 != null)
            {
                string sqlColunas2 = "";
                string sqlColunasAdicionais2 = "";
                string sqlGroup2 = "";
                string sqlFrom2 = "";
                string sqlRel2 = "";

                if (detalhamento3 != null)
                    sqlColunasAdicionais2 += ",0 as codDetalhe3,'' as descDetalhe3";
                if (detalhamento4 != null)
                    sqlColunasAdicionais2 += ",0 as codDetalhe4,'' as descDetalhe4";
                if (detalhamento5 != null)
                    sqlColunasAdicionais2 += ",0 as codDetalhe5,'' as descDetalhe5";

                switch (detalhamento2)
                {
                    case "DIVISAO":
                        sqlColunas2 = sqlColunas1 + ",lc.cod_divisao as codDetalhe2,replicate('  ',len(cc1.cod_conta)+2) + cad_divisoes.descricao as descDetalhe2";
                        sqlGroup2 = sqlGroup1 + ",lc.cod_divisao,replicate('  ',len(cc1.cod_conta)+2) + cad_divisoes.descricao";


                        sqlFrom2 = sqlFrom1 + ", cad_divisoes";
                        sqlRel2 = sqlRel1 + " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa in (" + empresas + ") ";

                        break;
                    case "LINHA_NEGOCIO":
                        sqlColunas2 = sqlColunas1 + ",lc.cod_linha_negocio as codDetalhe2,replicate('  ',len(cc1.cod_conta)+2) + cad_linha_negocios.descricao as descDetalhe2";
                        sqlGroup2 = sqlGroup1 + ",lc.cod_linha_negocio,replicate('  ',len(cc1.cod_conta)+2) + cad_linha_negocios.descricao";


                        sqlFrom2 = sqlFrom1 + ", cad_linha_negocios";
                        sqlRel2 = sqlRel1 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa in (" + empresas + ") ";
                        break;
                    case "CLIENTE":
                        sqlColunas2 = sqlColunas1 + ",lc.cod_cliente as codDetalhe2,replicate('  ',len(cc1.cod_conta)+2) + cad_empresas.nome_razao_social as descDetalhe2";
                        sqlGroup2 = sqlGroup1 + ",lc.cod_cliente,replicate('  ',len(cc1.cod_conta)+2) + cad_empresas.nome_razao_social";


                        sqlFrom2 = sqlFrom1 + ", cad_empresas";
                        sqlRel2 = sqlRel1 + " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai in (" + empresas + ") ";
                        break;
                    case "JOB":
                        sqlColunas2 = sqlColunas1 + ",lc.cod_job as codDetalhe2,replicate('  ',len(cc1.cod_conta)+2) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe2";
                        sqlGroup2 = sqlGroup1 + ",lc.cod_job,replicate('  ',len(cc1.cod_conta)+2) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                        sqlFrom2 = sqlFrom1 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                        sqlRel2 = sqlRel1 + " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa in (" + empresas + ") ";
                        break;
                    case "TERCEIRO":
                        sqlColunas2 = sqlColunas1 + ", (case when lc.cod_terceiro = 0 then (case when isnull(lct.cod_terceiro,0) = 0 then 99999999 else lct.cod_terceiro end) else lc.cod_terceiro end) as codDetalhe1,(case when lc.cod_terceiro = 0 then replicate('  ',len(cc1.cod_conta)+2) + isnull(cet.nome_razao_social,'Não Definido') else  replicate('  ',len(cc1.cod_conta)+2) + isnull(ce.nome_razao_social,'Não Definido') end) as descDetalhe2 ";
                        sqlGroup2 = sqlGroup1 + ", (case when lc.cod_terceiro = 0 then (case when isnull(lct.cod_terceiro,0) = 0 then 99999999 else lct.cod_terceiro end) else lc.cod_terceiro end), (case when lc.cod_terceiro = 0 then replicate('  ',len(cc1.cod_conta)+2) + isnull(cet.nome_razao_social,'Não Definido') else  replicate('  ',len(cc1.cod_conta)+2) + isnull(ce.nome_razao_social,'Não Definido') end) ";

                        sqlFrom2 = sqlFrom1 + " ";
                        sqlRel2 = sqlRel1 + " ";
                        break;
                }

                sql += " union ";
                sql += "select cc1.cod_conta, replicate('  ',len(cc1.cod_conta)-1)+cc1.descricao as descricao, year(lc.data)*100 + month(lc.data) as ano_mes , " +
                    " sum(case when lc.deb_cred = 'C' then lc.valor else -lc.valor end) as valor,cc1.analitica " + sqlColunas2 + sqlColunasAdicionais2 + " " +
                    " from " + fromPrincipal + ", cad_contas cc1,cad_contas cc2 " + sqlFrom2 + "" +
                    " where lc.pendente = 'False' and lc.cod_empresa in (" + empresas + ") and cc1.cod_empresa in (" + empresas + ") and cc2.cod_empresa in (" + empresas + ") and " +
                    " lc.cod_empresa = cc1.COD_EMPRESA  and lc.cod_empresa = cc2.COD_EMPRESA and " +
                    " year(lc.data)*100 + month(lc.data) >= " + periodoDe.ToString("yyyyMM") + " and  " +
                    " year(lc.data)*100 + month(lc.data) <= " + periodoAte.ToString("yyyyMM") + " and  " +
                    " lc.cod_conta in " +
                    " (select cad_contas.cod_Conta from cad_contas, cad_grupos_contabeis where cad_contas.cod_grupo_contabil = cad_grupos_contabeis.cod_grupo_contabil and regra_exibicao='R' and cad_contas.cod_empresa in (" + empresas + ")) " +
                    " and cc2.cod_conta = lc.cod_Conta " +
                    " and cc1.analitica = 1 " +
                    " and cc1.cod_conta = substring(cc2.cod_conta,1,len(cc1.cod_conta)) " + sqlWhere + sqlRel2 + "" + moedaWhere +
                    " group by cc1.cod_conta, cc1.descricao,year(lc.data)*100 + month(lc.data),cc1.analitica, " + sqlGroup2 + "";

                if (detalhamento3 != null)
                {
                    string sqlColunas3 = "";
                    string sqlColunasAdicionais3 = "";
                    string sqlGroup3 = "";
                    string sqlFrom3 = "";
                    string sqlRel3 = "";

                    if (detalhamento4 != null)
                        sqlColunasAdicionais3 += ",0 as codDetalhe4,'' as descDetalhe4";
                    if (detalhamento5 != null)
                        sqlColunasAdicionais3 += ",0 as codDetalhe5,'' as descDetalhe5";

                    switch (detalhamento3)
                    {
                        case "DIVISAO":
                            sqlColunas3 = sqlColunas2 + ",lc.cod_divisao as codDetalhe3,replicate('  ',len(cc1.cod_conta)+3) + cad_divisoes.descricao as descDetalhe3";
                            sqlGroup3 = sqlGroup2 + ",lc.cod_divisao,replicate('  ',len(cc1.cod_conta)+3) + cad_divisoes.descricao";


                            sqlFrom3 = sqlFrom2 + ", cad_divisoes";
                            sqlRel3 = sqlRel2 + " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa in (" + empresas + ") ";

                            break;
                        case "LINHA_NEGOCIO":
                            sqlColunas3 = sqlColunas2 + ",lc.cod_linha_negocio as codDetalhe3,replicate('  ',len(cc1.cod_conta)+3) + cad_linha_negocios.descricao as descDetalhe3";
                            sqlGroup3 = sqlGroup2 + ",lc.cod_linha_negocio,replicate('  ',len(cc1.cod_conta)+3) + cad_linha_negocios.descricao";


                            sqlFrom3 = sqlFrom2 + ", cad_linha_negocios";
                            sqlRel3 = sqlRel2 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa in (" + empresas + ") ";
                            break;
                        case "CLIENTE":
                            sqlColunas3 = sqlColunas2 + ",lc.cod_cliente as codDetalhe3,replicate('  ',len(cc1.cod_conta)+3) + cad_empresas.nome_razao_social as descDetalhe3";
                            sqlGroup3 = sqlGroup2 + ",lc.cod_cliente,replicate('  ',len(cc1.cod_conta)+3) + cad_empresas.nome_razao_social";


                            sqlFrom3 = sqlFrom2 + ", cad_empresas";
                            sqlRel3 = sqlRel2 + " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai in (" + empresas + ") ";
                            break;
                        case "JOB":
                            sqlColunas3 = sqlColunas2 + ",lc.cod_job as codDetalhe3,replicate('  ',len(cc1.cod_conta)+3) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe3";
                            sqlGroup3 = sqlGroup2 + ",lc.cod_job,replicate('  ',len(cc1.cod_conta)+3) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                            sqlFrom3 = sqlFrom2 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                            sqlRel3 = sqlRel2 + " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa in (" + empresas + ") ";
                            break;
                        case "TERCEIRO":
                            sqlColunas3 = sqlColunas2 + ", (case when lc.cod_terceiro = 0 then (case when isnull(lct.cod_terceiro,0) = 0 then 99999999 else lct.cod_terceiro end) else lc.cod_terceiro end) as codDetalhe1,(case when lc.cod_terceiro = 0 then replicate('  ',len(cc1.cod_conta)+3) + isnull(cet.nome_razao_social,'Não Definido') else  replicate('  ',len(cc1.cod_conta)+3) + isnull(ce.nome_razao_social,'Não Definido') end) as descDetalhe3 ";
                            sqlGroup3 = sqlGroup2 + ", (case when lc.cod_terceiro = 0 then (case when isnull(lct.cod_terceiro,0) = 0 then 99999999 else lct.cod_terceiro end) else lc.cod_terceiro end), (case when lc.cod_terceiro = 0 then replicate('  ',len(cc1.cod_conta)+3) + isnull(cet.nome_razao_social,'Não Definido') else  replicate('  ',len(cc1.cod_conta)+3) + isnull(ce.nome_razao_social,'Não Definido') end) ";


                            sqlFrom3 = sqlFrom2 + " ";
                            sqlRel3 = sqlRel2 + " ";
                            break;
                    }

                    sql += " union ";
                    sql += "select cc1.cod_conta, replicate('  ',len(cc1.cod_conta)-1)+cc1.descricao as descricao, year(lc.data)*100 + month(lc.data) as ano_mes , " +
                        " sum(case when lc.deb_cred = 'C' then lc.valor else -lc.valor end) as valor,cc1.analitica " + sqlColunas3 + sqlColunasAdicionais3 + " " +
                        " from " + fromPrincipal + ", cad_contas cc1,cad_contas cc2 " + sqlFrom3 + "" +
                        " where lc.pendente = 'False' and lc.cod_empresa in (" + empresas + ") and cc1.cod_empresa in (" + empresas + ") and cc2.cod_empresa in (" + empresas + ") and " +
                        " lc.cod_empresa = cc1.COD_EMPRESA  and lc.cod_empresa = cc2.COD_EMPRESA and " +
                        " year(lc.data)*100 + month(lc.data) >= " + periodoDe.ToString("yyyyMM") + " and  " +
                        " year(lc.data)*100 + month(lc.data) <= " + periodoAte.ToString("yyyyMM") + " and  " +
                        " lc.cod_conta in " +
                        " (select cad_contas.cod_Conta from cad_contas, cad_grupos_contabeis where cad_contas.cod_grupo_contabil = cad_grupos_contabeis.cod_grupo_contabil and regra_exibicao='R' and cad_contas.cod_empresa in (" + empresas + ")) " +
                        " and cc2.cod_conta = lc.cod_Conta " +
                        " and cc1.analitica = 1 " +
                        " and cc1.cod_conta = substring(cc2.cod_conta,1,len(cc1.cod_conta)) " + sqlWhere + sqlRel3 + "" + moedaWhere +
                        " group by cc1.cod_conta, cc1.descricao,year(lc.data)*100 + month(lc.data),cc1.analitica, " + sqlGroup3 + "";

                    if (detalhamento4 != null)
                    {
                        string sqlColunas4 = "";
                        string sqlColunasAdicionais4 = "";
                        string sqlGroup4 = "";
                        string sqlFrom4 = "";
                        string sqlRel4 = "";

                        if (detalhamento5 != null)
                            sqlColunasAdicionais4 += ",0 as codDetalhe5,'' as descDetalhe5";

                        switch (detalhamento4)
                        {
                            case "DIVISAO":
                                sqlColunas4 = sqlColunas3 + ",lc.cod_divisao as codDetalhe4,replicate('  ',len(cc1.cod_conta)+4) + cad_divisoes.descricao as descDetalhe4";
                                sqlGroup4 = sqlGroup3 + ",lc.cod_divisao,replicate('  ',len(cc1.cod_conta)+4) + cad_divisoes.descricao";


                                sqlFrom4 = sqlFrom3 + ", cad_divisoes";
                                sqlRel4 = sqlRel3 + " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa in (" + empresas + ") ";

                                break;
                            case "LINHA_NEGOCIO":
                                sqlColunas4 = sqlColunas3 + ",lc.cod_linha_negocio as codDetalhe4,replicate('  ',len(cc1.cod_conta)+4) + cad_linha_negocios.descricao as descDetalhe4";
                                sqlGroup4 = sqlGroup3 + ",lc.cod_linha_negocio,replicate('  ',len(cc1.cod_conta)+4) + cad_linha_negocios.descricao";


                                sqlFrom4 = sqlFrom3 + ", cad_linha_negocios";
                                sqlRel4 = sqlRel3 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa in (" + empresas + ") ";
                                break;
                            case "CLIENTE":
                                sqlColunas4 = sqlColunas3 + ",lc.cod_cliente as codDetalhe4,replicate('  ',len(cc1.cod_conta)+4) + cad_empresas.nome_razao_social as descDetalhe4";
                                sqlGroup4 = sqlGroup3 + ",lc.cod_cliente,replicate('  ',len(cc1.cod_conta)+4) + cad_empresas.nome_razao_social";


                                sqlFrom4 = sqlFrom3 + ", cad_empresas";
                                sqlRel4 = sqlRel3 + " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai in (" + empresas + ") ";
                                break;
                            case "JOB":
                                sqlColunas4 = sqlColunas3 + ",lc.cod_job as codDetalhe4,replicate('  ',len(cc1.cod_conta)+4) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe4";
                                sqlGroup4 = sqlGroup3 + ",lc.cod_job,replicate('  ',len(cc1.cod_conta)+4) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                                sqlFrom4 = sqlFrom3 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                                sqlRel4 = sqlRel3 + " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa in (" + empresas + ") ";
                                break;
                            case "TERCEIRO":
                                sqlColunas4 = sqlColunas3 + ", (case when lc.cod_terceiro = 0 then (case when isnull(lct.cod_terceiro,0) = 0 then 99999999 else lct.cod_terceiro end) else lc.cod_terceiro end) as codDetalhe1,(case when lc.cod_terceiro = 0 then replicate('  ',len(cc1.cod_conta)+4) + isnull(cet.nome_razao_social,'Não Definido') else  replicate('  ',len(cc1.cod_conta)+4) + isnull(ce.nome_razao_social,'Não Definido') end) as descDetalhe4 ";
                                sqlGroup4 = sqlGroup3 + ", (case when lc.cod_terceiro = 0 then (case when isnull(lct.cod_terceiro,0) = 0 then 99999999 else lct.cod_terceiro end) else lc.cod_terceiro end), (case when lc.cod_terceiro = 0 then replicate('  ',len(cc1.cod_conta)+4) + isnull(cet.nome_razao_social,'Não Definido') else  replicate('  ',len(cc1.cod_conta)+4) + isnull(ce.nome_razao_social,'Não Definido') end) ";

                                sqlFrom4 = sqlFrom3 + " ";
                                sqlRel4 = sqlRel3 + " ";
                                break;
                        }

                        sql += " union ";
                        sql += "select cc1.cod_conta, replicate('  ',len(cc1.cod_conta)-1)+cc1.descricao as descricao, year(lc.data)*100 + month(lc.data) as ano_mes , " +
                            " sum(case when lc.deb_cred = 'C' then lc.valor else -lc.valor end) as valor,cc1.analitica " + sqlColunas4 + sqlColunasAdicionais4 + " " +
                            " from " + fromPrincipal + ", cad_contas cc1,cad_contas cc2 " + sqlFrom4 + "" +
                            " where lc.pendente = 'False' and lc.cod_empresa in (" + empresas + ") and cc1.cod_empresa in (" + empresas + ") and cc2.cod_empresa in (" + empresas + ") and " +
                            " lc.cod_empresa = cc1.COD_EMPRESA  and lc.cod_empresa = cc2.COD_EMPRESA and " +
                            " year(lc.data)*100 + month(lc.data) >= " + periodoDe.ToString("yyyyMM") + " and  " +
                            " year(lc.data)*100 + month(lc.data) <= " + periodoAte.ToString("yyyyMM") + " and  " +
                            " lc.cod_conta in " +
                            " (select cad_contas.cod_Conta from cad_contas, cad_grupos_contabeis where cad_contas.cod_grupo_contabil = cad_grupos_contabeis.cod_grupo_contabil and regra_exibicao='R' and cad_contas.cod_empresa in (" + empresas + ") " +
                            " and cad_contas.cod_empresa  in (" + empresas + ") and cad_grupos_contabeis.COD_EMPRESA = cad_contas.cod_empresa ) " +
                            " and cc2.cod_conta = lc.cod_Conta " +
                            " and cc1.analitica = 1 " +
                            " and cc1.cod_conta = substring(cc2.cod_conta,1,len(cc1.cod_conta)) " + sqlWhere + sqlRel4 + "" + moedaWhere +
                            " group by cc1.cod_conta, cc1.descricao,year(lc.data)*100 + month(lc.data),cc1.analitica, " + sqlGroup4 + "";

                        if (detalhamento5 != null)
                        {
                            string sqlColunas5 = "";
                            string sqlColunasAdicionais5 = "";
                            string sqlGroup5 = "";
                            string sqlFrom5 = "";
                            string sqlRel5 = "";

                            switch (detalhamento5)
                            {
                                case "DIVISAO":
                                    sqlColunas5 = sqlColunas4 + ",lc.cod_divisao as codDetalhe5,replicate('  ',len(cc1.cod_conta)+4) + cad_divisoes.descricao as descDetalhe5";
                                    sqlGroup5 = sqlGroup4 + ",lc.cod_divisao,replicate('  ',len(cc1.cod_conta)+4) + cad_divisoes.descricao";


                                    sqlFrom5 = sqlFrom4 + ", cad_divisoes";
                                    sqlRel5 = sqlRel4 + " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa in (" + empresas + ") ";

                                    break;
                                case "LINHA_NEGOCIO":
                                    sqlColunas5 = sqlColunas4 + ",lc.cod_linha_negocio as codDetalhe5,replicate('  ',len(cc1.cod_conta)+4) + cad_linha_negocios.descricao as descDetalhe5";
                                    sqlGroup5 = sqlGroup4 + ",lc.cod_linha_negocio,replicate('  ',len(cc1.cod_conta)+4) + cad_linha_negocios.descricao";


                                    sqlFrom5 = sqlFrom4 + ", cad_linha_negocios";
                                    sqlRel5 = sqlRel4 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa in (" + empresas + ") ";
                                    break;
                                case "CLIENTE":
                                    sqlColunas5 = sqlColunas4 + ",lc.cod_cliente as codDetalhe5,replicate('  ',len(cc1.cod_conta)+4) + cad_empresas.nome_razao_social as descDetalhe5";
                                    sqlGroup5 = sqlGroup4 + ",lc.cod_cliente,replicate('  ',len(cc1.cod_conta)+4) + cad_empresas.nome_razao_social";


                                    sqlFrom5 = sqlFrom4 + ", cad_empresas";
                                    sqlRel5 = sqlRel4 + " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai in (" + empresas + ") ";
                                    break;
                                case "JOB":
                                    sqlColunas5 = sqlColunas4 + ",lc.cod_job as codDetalhe5,replicate('  ',len(cc1.cod_conta)+4) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe5";
                                    sqlGroup5 = sqlGroup4 + ",lc.cod_job,replicate('  ',len(cc1.cod_conta)+4) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                                    sqlFrom5 = sqlFrom4 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                                    sqlRel5 = sqlRel4 + " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa in (" + empresas + ") ";
                                    break;
                                case "TERCEIRO":
                                    sqlColunas5 = sqlColunas4 + ", (case when lc.cod_terceiro = 0 then (case when isnull(lct.cod_terceiro,0)= 0 then 99999999 else lct.cod_terceiro end) else lc.cod_terceiro end) as codDetalhe1,(case when lc.cod_terceiro = 0 then replicate('  ',len(cc1.cod_conta)+5) + isnull(cet.nome_razao_social,'Não Definido') else  replicate('  ',len(cc1.cod_conta)+5) + isnull(ce.nome_razao_social,'Não Definido') end) as descDetalhe5 ";
                                    sqlGroup5 = sqlGroup4 + ", (case when lc.cod_terceiro = 0 then (case when isnull(lct.cod_terceiro,0) = 0 then 99999999 else lct.cod_terceiro end) else lc.cod_terceiro end), (case when lc.cod_terceiro = 0 then replicate('  ',len(cc1.cod_conta)+5) + isnull(cet.nome_razao_social,'Não Definido') else  replicate('  ',len(cc1.cod_conta)+5) + isnull(ce.nome_razao_social,'Não Definido') end) ";

                                    sqlFrom5 = sqlFrom4 + " ";
                                    sqlRel5 = sqlRel4 + " ";
                                    break;
                            }

                            sql += " union ";
                            sql += "select cc1.cod_conta, replicate('  ',len(cc1.cod_conta)-1)+cc1.descricao as descricao, year(lc.data)*100 + month(lc.data) as ano_mes , " +
                                " sum(case when lc.deb_cred = 'C' then lc.valor else -lc.valor end) as valor,cc1.analitica " + sqlColunas5 + " " +
                                " from " + fromPrincipal + ", cad_contas cc1,cad_contas cc2 " + sqlFrom5 + "" +
                                " where lc.pendente = 'False' and lc.cod_empresa in (" + empresas + ") and cc1.cod_empresa in (" + empresas + ") and cc2.cod_empresa in (" + empresas + ") and " +
                                " lc.cod_empresa = cc1.COD_EMPRESA  and lc.cod_empresa = cc2.COD_EMPRESA and " +
                                " year(lc.data)*100 + month(lc.data) >= " + periodoDe.ToString("yyyyMM") + " and  " +
                                " year(lc.data)*100 + month(lc.data) <= " + periodoAte.ToString("yyyyMM") + " and  " +
                                " lc.cod_conta in " +
                                " (select cad_contas.cod_Conta from cad_contas, cad_grupos_contabeis where cad_contas.cod_grupo_contabil = cad_grupos_contabeis.cod_grupo_contabil and regra_exibicao='R' and cad_contas.cod_empresa in (" + empresas + ")) " +
                                " and cc2.cod_conta = lc.cod_Conta " +
                                " and cc1.analitica = 1 " +
                                " and cc1.cod_conta = substring(cc2.cod_conta,1,len(cc1.cod_conta)) " + sqlWhere + sqlRel5 + "" + moedaWhere +
                                " group by cc1.cod_conta, cc1.descricao,year(lc.data)*100 + month(lc.data),cc1.analitica, " + sqlGroup5 + "";
                        }
                    }
                }
            }
        }

        sql += ") vw";
        sql += " order by vw.cod_conta,vw.descricao";
        if (detalhamento1 != null)
            sql += ",vw.codDetalhe1 ";
        if (detalhamento2 != null)
            sql += ",vw.codDetalhe2 ";
        if (detalhamento3 != null)
            sql += ",vw.codDetalhe3 ";
        if (detalhamento4 != null)
            sql += ",vw.codDetalhe4 ";
        if (detalhamento5 != null)
            sql += ",vw.codDetalhe5 ";

        _conn.fill(sql, ref tb);
    }

    public void balancete(Nullable<int> tipoMoeda, DateTime periodo, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, string detalhamento1,
        string detalhamento2, string detalhamento3, string detalhamento4, string detalhamento5,
        ref DataTable tb, bool contasEncerramento, string empresas, string textAteEncerramento, bool ContaZeradas)
    {
        string sqlWhere = "";
        string sqlInicioColunas = "";
        string sql = "";
        string tabela = " LANCTOS_CONTAB ";
        string moedaWhere = "";

        if (tipoMoeda != 0 && tipoMoeda != null)
        {
            tabela = " LANCTOS_CONTAB_MOEDA ";
            moedaWhere = " AND lc.COD_MOEDA = " + tipoMoeda + " ";
        }

        if (divisaoDe != null && divisaoAte != null)
            sqlWhere += @" and lc.cod_divisao in (" + GeraListaDivisao(empresas, divisaoDe.ToString(), divisaoAte.ToString()) + ") ";

        //if (divisaoAte != null)
        //    sqlWhere += " and lc.cod_divisao <= " + divisaoAte.Value + "";

        if (linhaNegocioDe != null && linhaNegocioAte != null)
            sqlWhere += @" and lc.COD_LINHA_NEGOCIO in (" + GeraListaLinhaNegocio(empresas, linhaNegocioDe.ToString(), linhaNegocioAte.ToString()) + ") ";
        

        //if (linhaNegocioAte != null)
        //    sqlWhere += " and lc.cod_linha_negocio <= " + linhaNegocioAte.Value + "";

        if (clienteDe != null && clienteAte != null)
            sqlWhere += @" and lc.COD_EMPRESA in ("+GeraListaEmpresa(empresas, clienteDe.ToString(), clienteAte.ToString())+") ";

        //if (clienteAte != null)
        //    sqlWhere += " and lc.cod_cliente <= " + clienteAte.Value + "";

        if (jobDe != null && jobAte != null)
            sqlWhere += @" and lc.cod_job in (" + GeraListaJob(empresas, jobDe.ToString(), jobAte.ToString()) + ")";

        //if (jobAte != null)
        //    sqlWhere += " and lc.cod_job <= " + jobAte.Value + "";

        if (detalhamento1 != null)
            sqlInicioColunas += "0 as codDetalhe1,'' as descDetalhe1,";
        if (detalhamento2 != null)
            sqlInicioColunas += "0 as codDetalhe2,'' as descDetalhe2,";
        if (detalhamento3 != null)
            sqlInicioColunas += "0 as codDetalhe3,'' as descDetalhe3,";
        if (detalhamento4 != null)
            sqlInicioColunas += "0 as codDetalhe4,'' as descDetalhe4,";
        if (detalhamento5 != null)
            sqlInicioColunas += "0 as codDetalhe5,'' as descDetalhe5,";

        sql = "select * from (select CGC.resultado, cc.cod_conta, max(replicate('  ',len(cc.cod_conta)-1)+cc.descricao) as descricao, " + sqlInicioColunas + " " +
            " sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then " + (contasEncerramento ? " (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " : "(case when year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " and lc.tipo_lancto = 'E' then 0 else (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) end) else 0 end) as saldo_ini, ") +
                    " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end)") + " else 0 end) else 0 end) as debito, " +
                    " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end)") + " else 0 end) else 0 end) as credito, " +
                    " sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor else -lc.valor end) " : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end) else " + (contasEncerramento ? "-lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " then 0 else -lc.valor end) end)")) + "  else 0 end) as saldo_fim, max(cc.analitica) as analitica " +
                    " from " + tabela + " lc, cad_contas cc, CAD_GRUPOS_CONTABEIS CGC " +
                    " where cc.cod_conta = substring(lc.cod_conta,1,len(cc.cod_conta)) " +
                    " and cgc.COD_EMPRESA = cc.cod_Empresa and cc.COD_GRUPO_CONTABIL = CGC.COD_GRUPO_CONTABIL " +
                    " and lc.pendente = 'False' and lc.cod_empresa in (" + empresas + ") and cc.cod_empresa in (" + empresas + ") " +
                    " and lc.cod_empresa = cc.cod_Empresa " + moedaWhere +
                    " " + sqlWhere + " " +
                    " group by cc.cod_conta, CGC.resultado ";


        string fromPrincipal = "";
        bool existeTerceiro = false;
        if (detalhamento1 == "TERCEIRO")
            existeTerceiro = true;
        else if (detalhamento2 == "TERCEIRO")
            existeTerceiro = true;
        else if (detalhamento3 == "TERCEIRO")
            existeTerceiro = true;
        else if (detalhamento4 == "TERCEIRO")
            existeTerceiro = true;
        else if (detalhamento5 == "TERCEIRO")
            existeTerceiro = true;

        if (existeTerceiro)
        {
            fromPrincipal = " " + tabela + " lc left join cad_empresas ce on lc.cod_terceiro = ce.cod_empresa";
        }
        else
        {
            fromPrincipal = " " + tabela + " lc ";
        }


        if (detalhamento1 != null)
        {
            string sqlColunas1 = "";
            string sqlColunasAdicionais1 = "";
            string sqlGroup1 = "";
            string sqlFrom1 = "";
            string sqlRel1 = "";

            if (detalhamento2 != null)
                sqlColunasAdicionais1 += "0 as codDetalhe2,'' as descDetalhe2,";
            if (detalhamento3 != null)
                sqlColunasAdicionais1 += "0 as codDetalhe3,'' as descDetalhe3,";
            if (detalhamento4 != null)
                sqlColunasAdicionais1 += "0 as codDetalhe4,'' as descDetalhe4,";
            if (detalhamento5 != null)
                sqlColunasAdicionais1 += "0 as codDetalhe5,'' as descDetalhe5,";


            switch (detalhamento1)
            {
                case "DIVISAO":
                    sqlColunas1 = "lc.cod_divisao as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + cad_divisoes.descricao as descDetalhe1,";
                    sqlGroup1 = "lc.cod_divisao,replicate('  ',len(cc.cod_conta)+1) + cad_divisoes.descricao";


                    sqlFrom1 = ", cad_divisoes";
                    sqlRel1 = " and lc.cod_divisao = cad_divisoes.cod_divisao ";

                    break;
                case "LINHA_NEGOCIO":
                    sqlColunas1 = "lc.cod_linha_negocio as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + cad_linha_negocios.descricao as descDetalhe1,";
                    sqlGroup1 = "lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+1) + cad_linha_negocios.descricao";


                    sqlFrom1 = ", cad_linha_negocios";
                    sqlRel1 = " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio ";
                    break;
                case "CLIENTE":
                    sqlColunas1 = "lc.cod_cliente as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + cad_empresas.nome_razao_social as descDetalhe1,";
                    sqlGroup1 = "lc.cod_cliente,replicate('  ',len(cc.cod_conta)+1) + cad_empresas.nome_razao_social";


                    sqlFrom1 = ", cad_empresas";
                    sqlRel1 = " and lc.cod_cliente = cad_empresas.cod_empresa ";
                    break;
                case "JOB":
                    sqlColunas1 = "lc.cod_job as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe1,";
                    sqlGroup1 = "lc.cod_job,replicate('  ',len(cc.cod_conta)+1) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                    sqlFrom1 = ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                    sqlRel1 = " and lc.cod_job = cad_jobs.cod_job ";
                    break;
                case "TERCEIRO":
                    //sqlColunas1 = " lc.cod_terceiro as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + isnull(ce.nome_razao_social,'Não Definido') as descDetalhe1, ";
                    sqlColunas1 = " (case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end) as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + isnull(ce.nome_razao_social,'Não Definido') as descDetalhe1, ";
                    sqlGroup1 = " (case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end) ,replicate('  ',len(cc.cod_conta)+1) + isnull(ce.nome_razao_social,'Não Definido') ";


                    sqlFrom1 = " ";
                    sqlRel1 = " ";
                    break;
            }

            sql += " union ";
            sql += "select CGC.resultado, cc.cod_conta, max(replicate('  ',len(cc.cod_conta)-1)+cc.descricao) as descricao, " + sqlColunas1 + sqlColunasAdicionais1 + " " +
                " sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then " + (contasEncerramento ? " (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " : "(case when year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " and lc.tipo_lancto = 'E' then 0 else (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) end) else 0 end) as saldo_ini, ") +
                //" sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " +
                " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end)") + " else 0 end) else 0 end) as debito, " +
                " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end)") + " else 0 end) else 0 end) as credito, " +
                //" sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as debito, " +
                //" sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as credito, " +
                //" sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2)) + " then 0 else lc.valor end) else " + (contasEncerramento ? "-lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2)) + " then 0 else lc.valor end) end) else 0 end) as saldo_fim, max(cc.analitica) as analitica " +
                " sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor else -lc.valor end) " : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end) else " + (contasEncerramento ? "-lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " then 0 else -lc.valor end) end)")) + "  else 0 end) as saldo_fim, max(cc.analitica) as analitica " +
                //" sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else " + (contasEncerramento ? "-lc.valor" : "(case when lc.tipo_lancto = 'N' then -lc.valor else 0 end)") + " end) else 0 end) as saldo_fim,max(cc.analitica) as analitica " +
                " from " + fromPrincipal + ", cad_contas cc, CAD_GRUPOS_CONTABEIS CGC  " + sqlFrom1 + "" +
                " where cc.cod_conta = substring(lc.cod_conta,1,len(cc.cod_conta)) " +
                " and cgc.COD_EMPRESA = cc.cod_Empresa and cc.COD_GRUPO_CONTABIL = CGC.COD_GRUPO_CONTABIL " +
                " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_empresa in (" + empresas + ")" + " and cc.cod_empresa in (" + empresas + ") " +
                " and lc.cod_empresa = cc.cod_empresa " + moedaWhere +
                " " + sqlWhere + sqlRel1 + "  " +
                " group by cc.cod_conta, CGC.resultado, " + sqlGroup1 + "";

            if (detalhamento2 != null)
            {
                string sqlColunas2 = "";
                string sqlColunasAdicionais2 = "";
                string sqlGroup2 = "";
                string sqlFrom2 = "";
                string sqlRel2 = "";

                if (detalhamento3 != null)
                    sqlColunasAdicionais2 += "0 as codDetalhe3,'' as descDetalhe3,";
                if (detalhamento4 != null)
                    sqlColunasAdicionais2 += "0 as codDetalhe4,'' as descDetalhe4,";
                if (detalhamento5 != null)
                    sqlColunasAdicionais2 += "0 as codDetalhe5,'' as descDetalhe5,";

                switch (detalhamento2)
                {
                    case "DIVISAO":
                        sqlColunas2 = sqlColunas1 + "lc.cod_divisao as codDetalhe2,replicate('  ',len(cc.cod_conta)+2) + cad_divisoes.descricao as descDetalhe2,";
                        sqlGroup2 = sqlGroup1 + ",lc.cod_divisao,replicate('  ',len(cc.cod_conta)+2) + cad_divisoes.descricao";


                        sqlFrom2 = sqlFrom1 + ", cad_divisoes";
                        sqlRel2 = sqlRel1 + " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa in (" + empresas + ") ";

                        break;
                    case "LINHA_NEGOCIO":
                        sqlColunas2 = sqlColunas1 + "lc.cod_linha_negocio as codDetalhe2,replicate('  ',len(cc.cod_conta)+2) + cad_linha_negocios.descricao as descDetalhe2,";
                        sqlGroup2 = sqlGroup1 + ",lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+2) + cad_linha_negocios.descricao";


                        sqlFrom2 = sqlFrom1 + ", cad_linha_negocios";
                        sqlRel2 = sqlRel1 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa in (" + empresas + ") ";
                        break;
                    case "CLIENTE":
                        sqlColunas2 = sqlColunas1 + "lc.cod_cliente as codDetalhe2,replicate('  ',len(cc.cod_conta)+2) + cad_empresas.nome_razao_social as descDetalhe2,";
                        sqlGroup2 = sqlGroup1 + ",lc.cod_cliente,replicate('  ',len(cc.cod_conta)+2) + cad_empresas.nome_razao_social";


                        sqlFrom2 = sqlFrom1 + ", cad_empresas";
                        sqlRel2 = sqlRel1 + " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai in (" + empresas + ") ";
                        break;
                    case "JOB":
                        sqlColunas2 = sqlColunas1 + "lc.cod_job as codDetalhe2 ,replicate('  ',len(cc.cod_conta)+2) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe2,";
                        sqlGroup2 = sqlGroup1 + ",lc.cod_job,replicate('  ',len(cc.cod_conta)+2) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                        sqlFrom2 = sqlFrom1 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                        sqlRel2 = sqlRel1 + " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa  in (" + empresas + ") ";
                        break;
                    case "TERCEIRO":
                        sqlColunas2 = sqlColunas1 + " (case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end) as codDetalhe2,replicate('  ',len(cc.cod_conta)+2) + isnull(ce.nome_razao_social,'Não Definido') as descDetalhe2, ";
                        sqlGroup2 = sqlGroup1 + " ,(case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end),replicate('  ',len(cc.cod_conta)+2) + isnull(ce.nome_razao_social,'Não Definido') ";


                        sqlFrom2 = sqlFrom1 + "  ";
                        sqlRel2 = sqlRel1 + " ";
                        break;
                }

                sql += " union ";
                sql += "select CGC.resultado, cc.cod_conta, max(replicate('  ',len(cc.cod_conta)-1)+cc.descricao) as descricao, " + sqlColunas2 + sqlColunasAdicionais2 + " " +
                    " sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then " + (contasEncerramento ? " (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " : "(case when year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " and lc.tipo_lancto = 'E' then 0 else (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) end) else 0 end) as saldo_ini, ") +
                    //" sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " +
                    " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end)") + " else 0 end) else 0 end) as debito, " +
                    " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end)") + " else 0 end) else 0 end) as credito, " +
                    //" sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as debito, " +
                    //" sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as credito, " +
                    " sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor else -lc.valor end) " : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end) else " + (contasEncerramento ? "-lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " then 0 else -lc.valor end) end)")) + "  else 0 end) as saldo_fim, max(cc.analitica) as analitica " +
                    //" sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2)) + " then 0 else lc.valor end) else " + (contasEncerramento ? "-lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2)) + " then 0 else lc.valor end) end) else 0 end) as saldo_fim, max(cc.analitica) as analitica " +
                    //" sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else " + (contasEncerramento ? "-lc.valor" : "(case when lc.tipo_lancto = 'N' then -lc.valor else 0 end)") + " end) else 0 end) as saldo_fim,max(cc.analitica) as analitica " +
                    " from " + fromPrincipal + ", cad_contas cc , CAD_GRUPOS_CONTABEIS CGC " + sqlFrom2 + "" +
                    " where cc.cod_conta = substring(lc.cod_conta,1,len(cc.cod_conta)) " +
                    " and cgc.COD_EMPRESA = cc.cod_Empresa and cc.COD_GRUPO_CONTABIL = CGC.COD_GRUPO_CONTABIL " +
                    " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_empresa  in (" + empresas + ")" + " and cc.cod_empresa  in (" + empresas + ") " +
                    " and lc.cod_empresa = cc.cod_empresa " + moedaWhere +
                    " " + sqlWhere + sqlRel2 + "  " +
                    " group by cc.cod_conta, CGC.resultado," + sqlGroup2 + "";


                if (detalhamento3 != null)
                {
                    string sqlColunas3 = "";
                    string sqlColunasAdicionais3 = "";
                    string sqlGroup3 = "";
                    string sqlFrom3 = "";
                    string sqlRel3 = "";


                    if (detalhamento4 != null)
                        sqlColunasAdicionais3 += "0 as codDetalhe4,'' as descDetalhe4,";
                    if (detalhamento5 != null)
                        sqlColunasAdicionais3 += "0 as codDetalhe5,'' as descDetalhe5,";

                    switch (detalhamento3)
                    {
                        case "DIVISAO":
                            sqlColunas3 = sqlColunas2 + "lc.cod_divisao as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + cad_divisoes.descricao as descDetalhe3,";
                            sqlGroup3 = sqlGroup2 + ",lc.cod_divisao,replicate('  ',len(cc.cod_conta)+3) + cad_divisoes.descricao";


                            sqlFrom3 = sqlFrom2 + ", cad_divisoes";
                            sqlRel3 = sqlRel2 + " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa  in (" + empresas + ") ";

                            break;
                        case "LINHA_NEGOCIO":
                            sqlColunas3 = sqlColunas2 + "lc.cod_linha_negocio as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + cad_linha_negocios.descricao as descDetalhe3,";
                            sqlGroup3 = sqlGroup2 + ",lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+3) + cad_linha_negocios.descricao";


                            sqlFrom3 = sqlFrom2 + ", cad_linha_negocios";
                            sqlRel3 = sqlRel2 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa in (" + empresas + ") ";
                            break;
                        case "CLIENTE":
                            sqlColunas3 = sqlColunas2 + "lc.cod_cliente as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + cad_empresas.nome_razao_social as descDetalhe3,";
                            sqlGroup3 = sqlGroup2 + ",lc.cod_cliente,replicate('  ',len(cc.cod_conta)+3) + cad_empresas.nome_razao_social";


                            sqlFrom3 = sqlFrom2 + ", cad_empresas";
                            sqlRel3 = sqlRel2 + " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai in (" + empresas + ") ";
                            break;
                        case "JOB":
                            sqlColunas3 = sqlColunas2 + "lc.cod_job as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe3,";
                            sqlGroup3 = sqlGroup2 + ",lc.cod_job,replicate('  ',len(cc.cod_conta)+3) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                            sqlFrom3 = sqlFrom2 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                            sqlRel3 = sqlRel2 + " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa in (" + empresas + ") ";
                            break;
                        case "TERCEIRO":
                            sqlColunas3 = sqlColunas2 + " (case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end) as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + isnull(ce.nome_razao_social,'Não Definido') as descDetalhe3, ";
                            sqlGroup3 = sqlGroup2 + " ,(case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end),replicate('  ',len(cc.cod_conta)+3) + isnull(ce.nome_razao_social,'Não Definido') ";


                            sqlFrom3 = sqlFrom2 + "  ";
                            sqlRel3 = sqlRel2 + " ";
                            break;

                    }

                    sql += " union ";
                    sql += "select CGC.resultado, cc.cod_conta, max(replicate('  ',len(cc.cod_conta)-1)+cc.descricao) as descricao, " + sqlColunas3 + sqlColunasAdicionais3 + " " +
                        " sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then " + (contasEncerramento ? " (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " : "(case when year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " and lc.tipo_lancto = 'E' then 0 else (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) end) else 0 end) as saldo_ini, ") +
                        //" sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " +
                        " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end)") + " else 0 end) else 0 end) as debito, " +
                        " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end)") + " else 0 end) else 0 end) as credito, " +
                        //" sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as debito, " +
                        //" sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as credito, " +
                        " sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor else -lc.valor end) " : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end) else " + (contasEncerramento ? "-lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " then 0 else -lc.valor end) end)")) + "  else 0 end) as saldo_fim, max(cc.analitica) as analitica " +
                        //" sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) >= " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2)) + " then 0 else lc.valor end) else " + (contasEncerramento ? "-lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) >= " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2)) + " then 0 else lc.valor end) end) else 0 end) as saldo_fim, max(cc.analitica) as analitica " +
                        //" sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else " + (contasEncerramento ? "-lc.valor" : "(case when lc.tipo_lancto = 'N' then -lc.valor else 0 end)") + " end) else 0 end) as saldo_fim,max(cc.analitica) as analitica " +
                        " from " + fromPrincipal + ", cad_contas cc , CAD_GRUPOS_CONTABEIS CGC " + sqlFrom3 + "" +
                        " where cc.cod_conta = substring(lc.cod_conta,1,len(cc.cod_conta)) " +
                        " and cgc.COD_EMPRESA = cc.cod_Empresa and cc.COD_GRUPO_CONTABIL = CGC.COD_GRUPO_CONTABIL " +
                        " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_empresa in (" + empresas + ") and cc.cod_empresa  in (" + empresas + ") " +
                        " and lc.cod_empresa = cc.cod_empresa " +
                        " " + sqlWhere + sqlRel3 + "  " + moedaWhere +
                        " group by cc.cod_conta, CGC.resultado, " + sqlGroup3 + "";

                    if (detalhamento4 != null)
                    {
                        string sqlColunas4 = "";
                        string sqlColunasAdicionais4 = "";
                        string sqlGroup4 = "";
                        string sqlFrom4 = "";
                        string sqlRel4 = "";

                        if (detalhamento5 != null)
                            sqlColunasAdicionais4 += "0 as codDetalhe5,'' as descDetalhe5,";

                        switch (detalhamento4)
                        {
                            case "DIVISAO":
                                sqlColunas4 = sqlColunas3 + "lc.cod_divisao as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_divisoes.descricao as descDetalhe4,";
                                sqlGroup4 = sqlGroup3 + ",lc.cod_divisao,replicate('  ',len(cc.cod_conta)+4) + cad_divisoes.descricao";


                                sqlFrom4 = sqlFrom3 + ", cad_divisoes";
                                sqlRel4 = sqlRel3 + " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa  in (" + empresas + ") ";

                                break;
                            case "LINHA_NEGOCIO":
                                sqlColunas4 = sqlColunas3 + "lc.cod_linha_negocio as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_linha_negocios.descricao as descDetalhe4,";
                                sqlGroup4 = sqlGroup3 + ",lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+4) + cad_linha_negocios.descricao";


                                sqlFrom4 = sqlFrom3 + ", cad_linha_negocios";
                                sqlRel4 = sqlRel3 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa in (" + empresas + ") ";
                                break;
                            case "CLIENTE":
                                sqlColunas4 = sqlColunas3 + "lc.cod_cliente as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_empresas.nome_razao_social as descDetalhe4,";
                                sqlGroup4 = sqlGroup3 + ",lc.cod_cliente,replicate('  ',len(cc.cod_conta)+4) + cad_empresas.nome_razao_social";


                                sqlFrom4 = sqlFrom3 + ", cad_empresas";
                                sqlRel4 = sqlRel3 + " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai in (" + empresas + ")";
                                break;
                            case "JOB":
                                sqlColunas4 = sqlColunas3 + "lc.cod_job as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe4,";
                                sqlGroup4 = sqlGroup3 + ",lc.cod_job,replicate('  ',len(cc.cod_conta)+4) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                                sqlFrom4 = sqlFrom3 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                                sqlRel4 = sqlRel3 + " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa in (" + empresas + ") ";
                                break;
                            case "TERCEIRO":
                                sqlColunas4 = sqlColunas3 + " (case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end) as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + isnull(ce.nome_razao_social,'Não Definido') as descDetalhe4, ";
                                sqlGroup4 = sqlGroup3 + " ,(case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end),replicate('  ',len(cc.cod_conta)+4) + isnull(ce.nome_razao_social,'Não Definido') ";


                                sqlFrom4 = sqlFrom3 + "  ";
                                sqlRel4 = sqlRel3 + " ";
                                break;
                        }

                        sql += " union ";
                        sql += "select CGC.resultado, cc.cod_conta, max(replicate('  ',len(cc.cod_conta)-1)+cc.descricao) as descricao, " + sqlColunas4 + sqlColunasAdicionais4 + " " +
                            " sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then " + (contasEncerramento ? " (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " : "(case when year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " and lc.tipo_lancto = 'E' then 0 else (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) end) else 0 end) as saldo_ini, ") +
                            //" sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " +
                            " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end)") + " else 0 end) else 0 end) as debito, " +
                            " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end)") + " else 0 end) else 0 end) as credito, " +
                            //" sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as debito, " +
                            //" sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as credito, " +
                            " sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor else -lc.valor end) " : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end) else " + (contasEncerramento ? "-lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " then 0 else -lc.valor end) end)")) + "  else 0 end) as saldo_fim, max(cc.analitica) as analitica " +
                            //" sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) >= " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2)) + " then 0 else lc.valor end) else " + (contasEncerramento ? "-lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) >= " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2)) + " then 0 else lc.valor end) end) else 0 end) as saldo_fim, max(cc.analitica) as analitica " +
                            //" sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else " + (contasEncerramento ? "-lc.valor" : "(case when lc.tipo_lancto = 'N' then -lc.valor else 0 end)") + " end) else 0 end) as saldo_fim,max(cc.analitica) as analitica " +
                            " from " + fromPrincipal + ", cad_contas cc, CAD_GRUPOS_CONTABEIS CGC " + sqlFrom4 + "" +
                            " where cc.cod_conta = substring(lc.cod_conta,1,len(cc.cod_conta)) " +
                            " and cgc.COD_EMPRESA = cc.cod_Empresa and cc.COD_GRUPO_CONTABIL = CGC.COD_GRUPO_CONTABIL " +
                            " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_empresa in (" + empresas + ") and cc.cod_empresa in (" + empresas + ") " +
                            " and lc.cod_empresa = cc.cod_empresa " + moedaWhere +
                            " " + sqlWhere + sqlRel4 + "  " +
                            " group by cc.cod_conta, CGC.resultado, " + sqlGroup4 + "";

                        if (detalhamento5 != null)
                        {
                            string sqlColunas5 = "";
                            string sqlGroup5 = "";
                            string sqlFrom5 = "";
                            string sqlRel5 = "";

                            switch (detalhamento5)
                            {
                                case "DIVISAO":
                                    sqlColunas5 = sqlColunas4 + "lc.cod_divisao as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_divisoes.descricao as descDetalhe4,";
                                    sqlGroup5 = sqlGroup4 + ",lc.cod_divisao,replicate('  ',len(cc.cod_conta)+4) + cad_divisoes.descricao";


                                    sqlFrom4 = sqlFrom3 + ", cad_divisoes";
                                    sqlRel4 = sqlRel3 + " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa in (" + empresas + ") ";

                                    break;
                                case "LINHA_NEGOCIO":
                                    sqlColunas5 = sqlColunas4 + "lc.cod_linha_negocio as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_linha_negocios.descricao as descDetalhe4,";
                                    sqlGroup5 = sqlGroup4 + ",lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+4) + cad_linha_negocios.descricao";


                                    sqlFrom5 = sqlFrom4 + ", cad_linha_negocios";
                                    sqlRel5 = sqlRel4 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa in (" + empresas + ") ";
                                    break;
                                case "CLIENTE":
                                    sqlColunas5 = sqlColunas4 + "lc.cod_cliente as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_empresas.nome_razao_social as descDetalhe4,";
                                    sqlGroup5 = sqlGroup4 + ",lc.cod_cliente,replicate('  ',len(cc.cod_conta)+4) + cad_empresas.nome_razao_social";


                                    sqlFrom5 = sqlFrom4 + ", cad_empresas";
                                    sqlRel5 = sqlRel4 + " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai in (" + empresas + ") ";
                                    break;
                                case "JOB":
                                    sqlColunas5 = sqlColunas4 + "lc.cod_job as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe4,";
                                    sqlGroup5 = sqlGroup4 + ",lc.cod_job,replicate('  ',len(cc.cod_conta)+4) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                                    sqlFrom5 = sqlFrom4 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                                    sqlRel5 = sqlRel4 + " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa in (" + empresas + ")";
                                    break;
                                case "TERCEIRO":
                                    sqlColunas5 = sqlColunas4 + " (case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end) as codDetalhe5,replicate('  ',len(cc.cod_conta)+5) + isnull(ce.nome_razao_social,'Não Definido') as descDetalhe5, ";
                                    sqlGroup5 = sqlGroup4 + " ,(case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end),replicate('  ',len(cc.cod_conta)+5) + isnull(ce.nome_razao_social,'Não Definido') ";


                                    sqlFrom5 = sqlFrom4 + "  ";
                                    sqlRel5 = sqlRel4 + " ";
                                    break;
                            }

                            sql += " union ";
                            sql += "select CGC.resultado, cc.cod_conta, max(replicate('  ',len(cc.cod_conta)-1)+cc.descricao) as descricao," + sqlColunas5 + " " +
                                " sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then " + (contasEncerramento ? " (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " : "(case when year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " and lc.tipo_lancto = 'E' then 0 else (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) end) else 0 end) as saldo_ini, ") +
                                //" sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " +
                                " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end)") + " else 0 end) else 0 end) as debito, " +
                                " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end)") + " else 0 end) else 0 end) as credito, " +
                                //" sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as debito, " +
                                //" sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as credito, " +
                                " sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor else -lc.valor end) " : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " then 0 else lc.valor end) else " + (contasEncerramento ? "-lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) > " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2) + " then 0 else -lc.valor end) end)")) + "  else 0 end) as saldo_fim, max(cc.analitica) as analitica " +
                                //" sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) >= " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2)) + " then 0 else lc.valor end) else " + (contasEncerramento ? "-lc.valor" : "(case when lc.tipo_lancto = 'E' and year(lc.data)*100+month(lc.data) >= " + textAteEncerramento.Substring(3, 4) + "" + textAteEncerramento.Substring(0, 2)) + " then 0 else lc.valor end) end) else 0 end) as saldo_fim, max(cc.analitica) as analitica " +
                                //" sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else " + (contasEncerramento ? "-lc.valor" : "(case when lc.tipo_lancto = 'N' then -lc.valor else 0 end)") + " end) else 0 end) as saldo_fim, max(cc.analitica) as analitica " +
                                " from " + fromPrincipal + ", cad_contas cc, CAD_GRUPOS_CONTABEIS CGC  " + sqlFrom5 + 
                                " where cc.cod_conta = substring(lc.cod_conta,1,len(cc.cod_conta)) " +
                                " and cgc.COD_EMPRESA = cc.cod_Empresa and cc.COD_GRUPO_CONTABIL = CGC.COD_GRUPO_CONTABIL " + 
                                " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_empresa in (" + empresas + ")  and cc.cod_empresa in (" + empresas + ") " +
                                " and lc.cod_empresa = cc.cod_empresa " + moedaWhere +
                                " " + sqlWhere + sqlRel5 + "  " +
                                " group by cc.cod_conta, CGC.resultado, " + sqlGroup5;
                        }
                    }
                }
            }
        }

        sql += ") x";

        if (!ContaZeradas)
            sql += " where saldo_ini <> 0 or debito <> 0 or credito <> 0 or saldo_fim <> 0";

        sql += " order by x.cod_conta,";

        if (detalhamento1 != null)
            sql += " x.codDetalhe1,";
        if (detalhamento2 != null)
            sql += " x.codDetalhe2,";
        if (detalhamento3 != null)
            sql += " x.codDetalhe3,";
        if (detalhamento4 != null)
            sql += " x.codDetalhe4,";
        if (detalhamento5 != null)
            sql += " x.codDetalhe5,";
        
        sql += "x.descricao";

        
        _conn.fill(sql, ref tb);
    }

    public string GeraListaDivisao(string empresas, string divisaode, string divisaoate)
    {
        DataTable dtGera = new DataTable();
        string ListaDivisao = "";
        string StrSql = "select cod_divisao from cad_divisoes where cod_empresa in (" + empresas + @") and descricao between 
                                        (select min(descricao) from cad_divisoes where cod_empresa in (" + empresas + @") and  COD_DIVISAO = " + divisaode + @") and 
                                        (select max(descricao) from cad_divisoes where cod_empresa in (" + empresas + @") and  COD_DIVISAO = " + divisaoate + ")";

        dtGera = _conn.dataTable(StrSql, "dtGera");
        for (int i = 0; i < dtGera.Rows.Count; i++)
        {
            if (i == 0)
            {
                ListaDivisao = dtGera.Rows[i]["cod_divisao"].ToString();
            }
            else
            {
                ListaDivisao = ListaDivisao + "," + dtGera.Rows[i]["cod_divisao"].ToString();
            }
        }
        return ListaDivisao;
    }

    public string GeraListaJob(string empresas, string jobde, string jobate)
    {
        DataTable dtGera = new DataTable();
        string ListaJob = "";
        string StrSql = "select cod_job from CAD_JOBS cj, cad_empresas ce where cj.status = 'A' and ce.cod_empresa = cj.cod_cliente "+
            "and cj.cod_empresa in (" + empresas + @") and ce.Nome_fantasia + cj.descricao between (select max(ce.Nome_fantasia) + min(cj.descricao) "+
            "from CAD_JOBS cj, cad_empresas ce where ce.cod_empresa = cj.cod_cliente and  cj.cod_job = " + jobde + @" "+
            "and cj.cod_empresa in (" + empresas + @")) and (select max(ce.Nome_fantasia) + max(cj.descricao) "+
            "from CAD_JOBS cj, cad_empresas ce where ce.cod_empresa = cj.cod_cliente and  cj.cod_job = " + jobate + @" "+
            "and cj.cod_empresa in (" + empresas + @")) ";

        dtGera = _conn.dataTable(StrSql, "dtGera");
        for (int i = 0; i < dtGera.Rows.Count; i++)
        {
            if (i == 0)
            {
                ListaJob = dtGera.Rows[i]["cod_job"].ToString();
            }
            else
            {
                ListaJob = ListaJob + "," + dtGera.Rows[i]["cod_job"].ToString();
            }
        }
        return ListaJob;
    }

    public string GeraListaLinhaNegocio(string empresas, string linhanegociode, string linhanegocioate)
    {
        DataTable dtGera = new DataTable();
        string ListaLinhaNegocio = "";
        string StrSql = "select COD_LINHA_NEGOCIO from CAD_LINHA_NEGOCIOS where cod_empresa in (" + empresas + @") and descricao between 
                                    (select min(descricao) from CAD_LINHA_NEGOCIOS where cod_empresa in (" + empresas + @") and  COD_LINHA_NEGOCIO = " + linhanegociode + @") and 
                                    (select max(descricao) from CAD_LINHA_NEGOCIOS where cod_empresa in (" + empresas + @") and  COD_LINHA_NEGOCIO = " + linhanegocioate + ")";

        dtGera = _conn.dataTable(StrSql, "dtGera");
        for (int i = 0; i < dtGera.Rows.Count; i++)
        {
            if (i == 0)
            {
                ListaLinhaNegocio = dtGera.Rows[i]["COD_LINHA_NEGOCIO"].ToString();
            }
            else
            {
                ListaLinhaNegocio = ListaLinhaNegocio + "," + dtGera.Rows[i]["COD_LINHA_NEGOCIO"].ToString();
            }
        }
        return ListaLinhaNegocio;
    }

    public string GeraListaEmpresa(string empresas, string clientede, string clienteate)
    {
        DataTable dtGera = new DataTable();
        string ListaEmpresa = "";
        string StrSql = "select COD_EMPRESA from CAD_EMPRESAS where NOME_RAZAO_SOCIAL between "+
                                    "(select min(NOME_RAZAO_SOCIAL) from CAD_EMPRESAS where cod_empresa_pai in (" + empresas + @") and  COD_EMPRESA = " + clientede + @") and "+
                                    "(select max(NOME_RAZAO_SOCIAL) from CAD_EMPRESAS where cod_empresa_pai in (" + empresas + @") and  COD_EMPRESA = " + clienteate +")";

        dtGera = _conn.dataTable(StrSql, "dtGera");
        for (int i = 0; i < dtGera.Rows.Count; i++)
        {
            if (i == 0)
            {
                ListaEmpresa = dtGera.Rows[i]["COD_EMPRESA"].ToString();
            }
            else
            {
                ListaEmpresa = ListaEmpresa + "," + dtGera.Rows[i]["COD_EMPRESA"].ToString();
            }
        }
        return ListaEmpresa;
    }

    public void balanceteResultado(DateTime periodo, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
            Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
            Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, string detalhamento1,
            string detalhamento2, string detalhamento3, string detalhamento4, string detalhamento5,
            ref DataTable tb, bool contasEncerramento)
    {
        string sqlWhere = "";
        string sqlInicioColunas = "";
        string sql = "";

        if (divisaoDe != null)
            sqlWhere += " and lc.cod_divisao >= " + divisaoDe.Value + "";
        if (divisaoAte != null)
            sqlWhere += " and lc.cod_divisao <= " + divisaoAte.Value + "";

        if (linhaNegocioDe != null)
            sqlWhere += " and lc.cod_linha_negocio >= " + linhaNegocioDe.Value + "";
        if (linhaNegocioAte != null)
            sqlWhere += " and lc.cod_linha_negocio <= " + linhaNegocioAte.Value + "";

        if (clienteDe != null)
            sqlWhere += " and lc.cod_cliente >= " + clienteDe.Value + "";
        if (clienteAte != null)
            sqlWhere += " and lc.cod_cliente <= " + clienteAte.Value + "";

        if (jobDe != null)
            sqlWhere += " and lc.cod_job >= " + jobDe.Value + "";
        if (jobAte != null)
            sqlWhere += " and lc.cod_job <= " + jobAte.Value + "";

        if (detalhamento1 != null)
            sqlInicioColunas += "0 as codDetalhe1,'' as descDetalhe1,";
        if (detalhamento2 != null)
            sqlInicioColunas += "0 as codDetalhe2,'' as descDetalhe2,";
        if (detalhamento3 != null)
            sqlInicioColunas += "0 as codDetalhe3,'' as descDetalhe3,";
        if (detalhamento4 != null)
            sqlInicioColunas += "0 as codDetalhe4,'' as descDetalhe4,";
        if (detalhamento5 != null)
            sqlInicioColunas += "0 as codDetalhe5,'' as descDetalhe5,";

        sql = "select * from (select cad_grupos_contabeis.resultado, cc.cod_conta, max(replicate('  ',len(cc.cod_conta)-1)+cc.descricao) as descricao, " + sqlInicioColunas + " " +
                    " sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " +
                    " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as debito, " +
                    " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as credito, " +
                    " sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when (lc.tipo_lancto = 'N' or year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + ") then lc.valor else 0 end)") + " else " + (contasEncerramento ? "-lc.valor" : "(case when (lc.tipo_lancto = 'N' or year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + ") then -lc.valor else 0 end)") + " end) else 0 end) as saldo_fim,max(cc.analitica) as analitica " +
                    " from lanctos_contab lc, cad_contas cc, cad_grupos_contabeis " +
                    " where cc.cod_conta = substring(lc.cod_conta,1,len(cc.cod_conta)) and cc.cod_grupo_contabil = cad_grupos_contabeis.cod_grupo_contabil and cad_grupos_contabeis.resultado=1 " +
                    " and lc.pendente = 'False' and lc.cod_empresa='" + HttpContext.Current.Session["empresa"] + "' and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                    " and lc.cod_empresa = cc.cod_empresa " + 
                    " " + sqlWhere + " " +
                    " group by cad_grupos_contabeis.resultado, cc.cod_conta ";


        string fromPrincipal = "";
        bool existeTerceiro = false;
        if (detalhamento1 == "TERCEIRO")
            existeTerceiro = true;
        else if (detalhamento2 == "TERCEIRO")
            existeTerceiro = true;
        else if (detalhamento3 == "TERCEIRO")
            existeTerceiro = true;
        else if (detalhamento4 == "TERCEIRO")
            existeTerceiro = true;
        else if (detalhamento5 == "TERCEIRO")
            existeTerceiro = true;

        if (existeTerceiro)
        {
            fromPrincipal = " lanctos_contab lc left join cad_empresas ce on lc.cod_terceiro = ce.cod_empresa";
        }
        else
        {
            fromPrincipal = " lanctos_contab lc ";
        }


        if (detalhamento1 != null)
        {
            string sqlColunas1 = "";
            string sqlColunasAdicionais1 = "";
            string sqlGroup1 = "";
            string sqlFrom1 = "";
            string sqlRel1 = "";

            if (detalhamento2 != null)
                sqlColunasAdicionais1 += "0 as codDetalhe2,'' as descDetalhe2,";
            if (detalhamento3 != null)
                sqlColunasAdicionais1 += "0 as codDetalhe3,'' as descDetalhe3,";
            if (detalhamento4 != null)
                sqlColunasAdicionais1 += "0 as codDetalhe4,'' as descDetalhe4,";
            if (detalhamento5 != null)
                sqlColunasAdicionais1 += "0 as codDetalhe5,'' as descDetalhe5,";


            switch (detalhamento1)
            {
                case "DIVISAO":
                    sqlColunas1 = "lc.cod_divisao as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + cad_divisoes.descricao as descDetalhe1,";
                    sqlGroup1 = "lc.cod_divisao,replicate('  ',len(cc.cod_conta)+1) + cad_divisoes.descricao";


                    sqlFrom1 = ", cad_divisoes";
                    sqlRel1 = " and lc.cod_divisao = cad_divisoes.cod_divisao ";

                    break;
                case "LINHA_NEGOCIO":
                    sqlColunas1 = "lc.cod_linha_negocio as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + cad_linha_negocios.descricao as descDetalhe1,";
                    sqlGroup1 = "lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+1) + cad_linha_negocios.descricao";


                    sqlFrom1 = ", cad_linha_negocios";
                    sqlRel1 = " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio ";
                    break;
                case "CLIENTE":
                    sqlColunas1 = "lc.cod_cliente as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + cad_empresas.nome_razao_social as descDetalhe1,";
                    sqlGroup1 = "lc.cod_cliente,replicate('  ',len(cc.cod_conta)+1) + cad_empresas.nome_razao_social";


                    sqlFrom1 = ", cad_empresas";
                    sqlRel1 = " and lc.cod_cliente = cad_empresas.cod_empresa ";
                    break;
                case "JOB":
                    sqlColunas1 = "lc.cod_job as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe1,";
                    sqlGroup1 = "lc.cod_job,replicate('  ',len(cc.cod_conta)+1) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                    sqlFrom1 = ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                    sqlRel1 = " and lc.cod_job = cad_jobs.cod_job ";
                    break;
                case "TERCEIRO":
                    sqlColunas1 = " (case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end) as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + isnull(ce.nome_razao_social,'Não Definido') as descDetalhe1, ";
                    sqlGroup1 = " (case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end),replicate('  ',len(cc.cod_conta)+1) + isnull(ce.nome_razao_social,'Não Definido') ";


                    sqlFrom1 = " ";
                    sqlRel1 = " ";
                    break;
            }

            sql += " union ";
            sql += "select cad_grupos_contabeis.resultado, cc.cod_conta, max(replicate('  ',len(cc.cod_conta)-1)+cc.descricao) as descricao, " + sqlColunas1 + sqlColunasAdicionais1 + " " +
                " sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " +
                " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as debito, " +
                " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as credito, " +
                " sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when (lc.tipo_lancto = 'N' or year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + ") then lc.valor else 0 end)") + " else " + (contasEncerramento ? "-lc.valor" : "(case when (lc.tipo_lancto = 'N' or year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + ") then -lc.valor else 0 end)") + " end) else 0 end) as saldo_fim,max(cc.analitica) as analitica " +
                " from " + fromPrincipal + ", cad_contas cc, cad_grupos_contabeis " + sqlFrom1 + "" +
                " where cc.cod_conta = substring(lc.cod_conta,1,len(cc.cod_conta))  and cc.cod_grupo_contabil = cad_grupos_contabeis.cod_grupo_contabil and cad_grupos_contabeis.resultado=1 " +
                " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                " and lc.cod_empresa = cc.cod_empresa " + 
                " " + sqlWhere + sqlRel1 + "  " +
                " group by cad_grupos_contabeis.resultado, cc.cod_conta, " + sqlGroup1 + "";

            if (detalhamento2 != null)
            {
                string sqlColunas2 = "";
                string sqlColunasAdicionais2 = "";
                string sqlGroup2 = "";
                string sqlFrom2 = "";
                string sqlRel2 = "";

                if (detalhamento3 != null)
                    sqlColunasAdicionais2 += "0 as codDetalhe3,'' as descDetalhe3,";
                if (detalhamento4 != null)
                    sqlColunasAdicionais2 += "0 as codDetalhe4,'' as descDetalhe4,";
                if (detalhamento5 != null)
                    sqlColunasAdicionais2 += "0 as codDetalhe5,'' as descDetalhe5,";

                switch (detalhamento2)
                {
                    case "DIVISAO":
                        sqlColunas2 = sqlColunas1 + "lc.cod_divisao as codDetalhe2,replicate('  ',len(cc.cod_conta)+2) + cad_divisoes.descricao as descDetalhe2,";
                        sqlGroup2 = sqlGroup1 + ",lc.cod_divisao,replicate('  ',len(cc.cod_conta)+2) + cad_divisoes.descricao";


                        sqlFrom2 = sqlFrom1 + ", cad_divisoes";
                        sqlRel2 = sqlRel1 + " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";

                        break;
                    case "LINHA_NEGOCIO":
                        sqlColunas2 = sqlColunas1 + "lc.cod_linha_negocio as codDetalhe2,replicate('  ',len(cc.cod_conta)+2) + cad_linha_negocios.descricao as descDetalhe2,";
                        sqlGroup2 = sqlGroup1 + ",lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+2) + cad_linha_negocios.descricao";


                        sqlFrom2 = sqlFrom1 + ", cad_linha_negocios";
                        sqlRel2 = sqlRel1 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
                        break;
                    case "CLIENTE":
                        sqlColunas2 = sqlColunas1 + "lc.cod_cliente as codDetalhe2,replicate('  ',len(cc.cod_conta)+2) + cad_empresas.nome_razao_social as descDetalhe2,";
                        sqlGroup2 = sqlGroup1 + ",lc.cod_cliente,replicate('  ',len(cc.cod_conta)+2) + cad_empresas.nome_razao_social";


                        sqlFrom2 = sqlFrom1 + ", cad_empresas";
                        sqlRel2 = sqlRel1 + " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + " ";
                        break;
                    case "JOB":
                        sqlColunas2 = sqlColunas1 + "lc.cod_job as codDetalhe2 ,replicate('  ',len(cc.cod_conta)+2) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe2,";
                        sqlGroup2 = sqlGroup1 + ",lc.cod_job,replicate('  ',len(cc.cod_conta)+2) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                        sqlFrom2 = sqlFrom1 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                        sqlRel2 = sqlRel1 + " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
                        break;
                    case "TERCEIRO":
                        sqlColunas2 = sqlColunas1 + " (case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end) as codDetalhe2,replicate('  ',len(cc.cod_conta)+2) + isnull(ce.nome_razao_social,'Não Definido') as descDetalhe2, ";
                        sqlGroup2 = sqlGroup1 + " ,(case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end),replicate('  ',len(cc.cod_conta)+2) + isnull(ce.nome_razao_social,'Não Definido') ";


                        sqlFrom2 = sqlFrom1 + "  ";
                        sqlRel2 = sqlRel1 + " ";
                        break;
                }

                sql += " union ";
                sql += "select cad_grupos_contabeis.resultado, cc.cod_conta, max(replicate('  ',len(cc.cod_conta)-1)+cc.descricao) as descricao, " + sqlColunas2 + sqlColunasAdicionais2 + " " +
                    " sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " +
                    " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as debito, " +
                    " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as credito, " +
                    " sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when (lc.tipo_lancto = 'N' or year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + ") then lc.valor else 0 end)") + " else " + (contasEncerramento ? "-lc.valor" : "(case when (lc.tipo_lancto = 'N' or year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + ") then -lc.valor else 0 end)") + " end) else 0 end) as saldo_fim,max(cc.analitica) as analitica " +
                    " from " + fromPrincipal + ", cad_contas cc, cad_grupos_contabeis " + sqlFrom2 + "" +
                    " where cc.cod_conta = substring(lc.cod_conta,1,len(cc.cod_conta))  and cc.cod_grupo_contabil = cad_grupos_contabeis.cod_grupo_contabil and cad_grupos_contabeis.resultado=1 " +
                    " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                    " and lc.cod_empresa = cc.cod_empresa " + 
                    " " + sqlWhere + sqlRel2 + "  " +
                    " group by cad_grupos_contabeis.resultado, cc.cod_conta, " + sqlGroup2 + "";


                if (detalhamento3 != null)
                {
                    string sqlColunas3 = "";
                    string sqlColunasAdicionais3 = "";
                    string sqlGroup3 = "";
                    string sqlFrom3 = "";
                    string sqlRel3 = "";


                    if (detalhamento4 != null)
                        sqlColunasAdicionais3 += "0 as codDetalhe4,'' as descDetalhe4,";
                    if (detalhamento5 != null)
                        sqlColunasAdicionais3 += "0 as codDetalhe5,'' as descDetalhe5,";

                    switch (detalhamento3)
                    {
                        case "DIVISAO":
                            sqlColunas3 = sqlColunas2 + "lc.cod_divisao as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + cad_divisoes.descricao as descDetalhe3,";
                            sqlGroup3 = sqlGroup2 + ",lc.cod_divisao,replicate('  ',len(cc.cod_conta)+3) + cad_divisoes.descricao";


                            sqlFrom3 = sqlFrom2 + ", cad_divisoes";
                            sqlRel3 = sqlRel2 + " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";

                            break;
                        case "LINHA_NEGOCIO":
                            sqlColunas3 = sqlColunas2 + "lc.cod_linha_negocio as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + cad_linha_negocios.descricao as descDetalhe3,";
                            sqlGroup3 = sqlGroup2 + ",lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+3) + cad_linha_negocios.descricao";


                            sqlFrom3 = sqlFrom2 + ", cad_linha_negocios";
                            sqlRel3 = sqlRel2 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
                            break;
                        case "CLIENTE":
                            sqlColunas3 = sqlColunas2 + "lc.cod_cliente as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + cad_empresas.nome_razao_social as descDetalhe3,";
                            sqlGroup3 = sqlGroup2 + ",lc.cod_cliente,replicate('  ',len(cc.cod_conta)+3) + cad_empresas.nome_razao_social";


                            sqlFrom3 = sqlFrom2 + ", cad_empresas";
                            sqlRel3 = sqlRel2 + " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + " ";
                            break;
                        case "JOB":
                            sqlColunas3 = sqlColunas2 + "lc.cod_job as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe3,";
                            sqlGroup3 = sqlGroup2 + ",lc.cod_job,replicate('  ',len(cc.cod_conta)+3) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                            sqlFrom3 = sqlFrom2 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                            sqlRel3 = sqlRel2 + " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
                            break;
                        case "TERCEIRO":
                            sqlColunas3 = sqlColunas2 + " (case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end) as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + isnull(ce.nome_razao_social,'Não Definido') as descDetalhe3, ";
                            sqlGroup3 = sqlGroup2 + " ,(case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end),replicate('  ',len(cc.cod_conta)+3) + isnull(ce.nome_razao_social,'Não Definido') ";


                            sqlFrom3 = sqlFrom2 + "  ";
                            sqlRel3 = sqlRel2 + " ";
                            break;

                    }

                    sql += " union ";
                    sql += "select cad_grupos_contabeis.resultado, cc.cod_conta, max(replicate('  ',len(cc.cod_conta)-1)+cc.descricao) as descricao, " + sqlColunas3 + sqlColunasAdicionais3 + " " +
                        " sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " +
                        " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as debito, " +
                        " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as credito, " +
                        " sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when (lc.tipo_lancto = 'N' or year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + ") then lc.valor else 0 end)") + " else " + (contasEncerramento ? "-lc.valor" : "(case when (lc.tipo_lancto = 'N' or year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + ") then -lc.valor else 0 end)") + " end) else 0 end) as saldo_fim,max(cc.analitica) as analitica " +
                        " from " + fromPrincipal + ", cad_contas cc, cad_grupos_contabeis " + sqlFrom3 + "" +
                        " where cc.cod_conta = substring(lc.cod_conta,1,len(cc.cod_conta))  and cc.cod_grupo_contabil = cad_grupos_contabeis.cod_grupo_contabil and cad_grupos_contabeis.resultado=1 " +
                        " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                        " and lc.cod_empresa = cc.cod_empresa " + 
                        " " + sqlWhere + sqlRel3 + "  " +
                        " group by cad_grupos_contabeis.resultado, cc.cod_conta, " + sqlGroup3 + "";

                    if (detalhamento4 != null)
                    {
                        string sqlColunas4 = "";
                        string sqlColunasAdicionais4 = "";
                        string sqlGroup4 = "";
                        string sqlFrom4 = "";
                        string sqlRel4 = "";

                        if (detalhamento5 != null)
                            sqlColunasAdicionais4 += "0 as codDetalhe5,'' as descDetalhe5,";

                        switch (detalhamento4)
                        {
                            case "DIVISAO":
                                sqlColunas4 = sqlColunas3 + "lc.cod_divisao as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_divisoes.descricao as descDetalhe4,";
                                sqlGroup4 = sqlGroup3 + ",lc.cod_divisao,replicate('  ',len(cc.cod_conta)+4) + cad_divisoes.descricao";


                                sqlFrom4 = sqlFrom3 + ", cad_divisoes";
                                sqlRel4 = sqlRel3 + " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";

                                break;
                            case "LINHA_NEGOCIO":
                                sqlColunas4 = sqlColunas3 + "lc.cod_linha_negocio as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_linha_negocios.descricao as descDetalhe4,";
                                sqlGroup4 = sqlGroup3 + ",lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+4) + cad_linha_negocios.descricao";


                                sqlFrom4 = sqlFrom3 + ", cad_linha_negocios";
                                sqlRel4 = sqlRel3 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
                                break;
                            case "CLIENTE":
                                sqlColunas4 = sqlColunas3 + "lc.cod_cliente as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_empresas.nome_razao_social as descDetalhe4,";
                                sqlGroup4 = sqlGroup3 + ",lc.cod_cliente,replicate('  ',len(cc.cod_conta)+4) + cad_empresas.nome_razao_social";


                                sqlFrom4 = sqlFrom3 + ", cad_empresas";
                                sqlRel4 = sqlRel3 + " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + " ";
                                break;
                            case "JOB":
                                sqlColunas4 = sqlColunas3 + "lc.cod_job as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe4,";
                                sqlGroup4 = sqlGroup3 + ",lc.cod_job,replicate('  ',len(cc.cod_conta)+4) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                                sqlFrom4 = sqlFrom3 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                                sqlRel4 = sqlRel3 + " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
                                break;
                            case "TERCEIRO":
                                sqlColunas4 = sqlColunas3 + " (case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end) as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + isnull(ce.nome_razao_social,'Não Definido') as descDetalhe4, ";
                                sqlGroup4 = sqlGroup3 + " ,(case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end),replicate('  ',len(cc.cod_conta)+4) + isnull(ce.nome_razao_social,'Não Definido') ";


                                sqlFrom4 = sqlFrom3 + "  ";
                                sqlRel4 = sqlRel3 + " ";
                                break;
                        }

                        sql += " union ";
                        sql += "select cad_grupos_contabeis.resultado, cc.cod_conta, max(replicate('  ',len(cc.cod_conta)-1)+cc.descricao) as descricao, " + sqlColunas4 + sqlColunasAdicionais4 + " " +
                            " sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " +
                            " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as debito, " +
                            " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as credito, " +
                            " sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when (lc.tipo_lancto = 'N' or year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + ") then lc.valor else 0 end)") + " else " + (contasEncerramento ? "-lc.valor" : "(case when (lc.tipo_lancto = 'N' or year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + ") then -lc.valor else 0 end)") + " end) else 0 end) as saldo_fim,max(cc.analitica) as analitica " +
                            " from " + fromPrincipal + ", cad_contas cc, cad_grupos_contabeis " + sqlFrom4 + "" +
                            " where cc.cod_conta = substring(lc.cod_conta,1,len(cc.cod_conta))  and cc.cod_grupo_contabil = cad_grupos_contabeis.cod_grupo_contabil and cad_grupos_contabeis.resultado=1 " +
                            " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                            " and lc.cod_empresa = cc.cod_empresa " + 
                            " " + sqlWhere + sqlRel4 + "  " +
                            " group by cad_grupos_contabeis.resultado, cc.cod_conta, " + sqlGroup4 + "";

                        if (detalhamento5 != null)
                        {
                            string sqlColunas5 = "";
                            string sqlGroup5 = "";
                            string sqlFrom5 = "";
                            string sqlRel5 = "";

                            switch (detalhamento5)
                            {
                                case "DIVISAO":
                                    sqlColunas5 = sqlColunas4 + "lc.cod_divisao as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_divisoes.descricao as descDetalhe4,";
                                    sqlGroup5 = sqlGroup4 + ",lc.cod_divisao,replicate('  ',len(cc.cod_conta)+4) + cad_divisoes.descricao";


                                    sqlFrom4 = sqlFrom3 + ", cad_divisoes";
                                    sqlRel4 = sqlRel3 + " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";

                                    break;
                                case "LINHA_NEGOCIO":
                                    sqlColunas5 = sqlColunas4 + "lc.cod_linha_negocio as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_linha_negocios.descricao as descDetalhe4,";
                                    sqlGroup5 = sqlGroup4 + ",lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+4) + cad_linha_negocios.descricao";


                                    sqlFrom5 = sqlFrom4 + ", cad_linha_negocios";
                                    sqlRel5 = sqlRel4 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
                                    break;
                                case "CLIENTE":
                                    sqlColunas5 = sqlColunas4 + "lc.cod_cliente as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_empresas.nome_razao_social as descDetalhe4,";
                                    sqlGroup5 = sqlGroup4 + ",lc.cod_cliente,replicate('  ',len(cc.cod_conta)+4) + cad_empresas.nome_razao_social";


                                    sqlFrom5 = sqlFrom4 + ", cad_empresas";
                                    sqlRel5 = sqlRel4 + " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + " ";
                                    break;
                                case "JOB":
                                    sqlColunas5 = sqlColunas4 + "lc.cod_job as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe4,";
                                    sqlGroup5 = sqlGroup4 + ",lc.cod_job,replicate('  ',len(cc.cod_conta)+4) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                                    sqlFrom5 = sqlFrom4 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                                    sqlRel5 = sqlRel4 + " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
                                    break;
                                case "TERCEIRO":
                                    sqlColunas5 = sqlColunas4 + " (case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end) as codDetalhe5,replicate('  ',len(cc.cod_conta)+5) + isnull(ce.nome_razao_social,'Não Definido') as descDetalhe5, ";
                                    sqlGroup5 = sqlGroup4 + " ,(case when isnull(ce.nome_razao_social,'@') = '@' then '99999' else lc.cod_terceiro end),replicate('  ',len(cc.cod_conta)+5) + isnull(ce.nome_razao_social,'Não Definido') ";


                                    sqlFrom5 = sqlFrom4 + "  ";
                                    sqlRel5 = sqlRel4 + " ";
                                    break;
                            }

                            sql += " union ";
                            sql += "select cad_grupos_contabeis.resultado, cc.cod_conta, max(replicate('  ',len(cc.cod_conta)-1)+cc.descricao) as descricao, " + sqlColunas5 + " " +
                                " sum(case when year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then lc.valor else -lc.valor end) else 0 end) as saldo_ini, " +
                                " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as debito, " +
                                " sum(case when year(lc.data)*100+month(lc.data) = " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'C' then " + (contasEncerramento ? "lc.valor" : "(case when lc.tipo_lancto = 'N' then lc.valor else 0 end)") + " else 0 end) else 0 end) as credito, " +
                                " sum(case when year(lc.data)*100+month(lc.data) <= " + periodo.ToString("yyyyMM") + " then (case when lc.deb_cred = 'D' then " + (contasEncerramento ? "lc.valor" : "(case when (lc.tipo_lancto = 'N'  or year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + ") then lc.valor else 0 end)") + " else " + (contasEncerramento ? "-lc.valor" : "(case when (lc.tipo_lancto = 'N'   or year(lc.data)*100+month(lc.data) < " + periodo.ToString("yyyyMM") + ") then -lc.valor else 0 end)") + " end) else 0 end) as saldo_fim,max(cc.analitica) as analitica " +
                                " from " + fromPrincipal + ", cad_contas cc, cad_grupos_contabeis " + sqlFrom5 + "" +
                                " where cc.cod_conta = substring(lc.cod_conta,1,len(cc.cod_conta))  and cc.cod_grupo_contabil = cad_grupos_contabeis.cod_grupo_contabil and cad_grupos_contabeis.resultado=1 " +
                                " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                                " and lc.cod_empresa = cc.cod_empresa " + 
                                " " + sqlWhere + sqlRel5 + "  " +
                                " group by cad_grupos_contabeis.resultado, cc.cod_conta, " + sqlGroup5 + "";
                        }
                    }
                }
            }
        }

        sql += ") x order by x.cod_conta, x.descricao";

        _conn.fill(sql, ref tb);
    }

	public void movimentacao_financeira_beta(DateTime periodoDe, DateTime periodoAte, DateTime periodoAnterior, string contaDe, string contaAte, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
		Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
		Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, string detalhamento1,
		string detalhamento2, string detalhamento3, string detalhamento4, ref DataTable tb)
	{
		bool job = false;
		string sqlWhere = "";
		string sqlInicioColunas = "";
		string sql = "";
		string sqlOrder = "";

		if (contaDe != null)
			sqlWhere += " and lc.cod_conta >= '" + contaDe + "'";
		if (contaAte != null)
			sqlWhere += " and lc.cod_conta <= '" + contaAte + "'";

		if (divisaoDe != null)
			sqlWhere += " and lc.cod_divisao >= " + divisaoDe.Value + "";
		if (divisaoAte != null)
			sqlWhere += " and lc.cod_divisao <= " + divisaoAte.Value + "";

		if (linhaNegocioDe != null)
			sqlWhere += " and lc.cod_linha_negocio >= " + linhaNegocioDe.Value + "";
		if (linhaNegocioAte != null)
			sqlWhere += " and lc.cod_linha_negocio <= " + linhaNegocioAte.Value + "";

		if (clienteDe != null)
			sqlWhere += " and lc.cod_cliente >= " + clienteDe.Value + "";
		if (clienteAte != null)
			sqlWhere += " and lc.cod_cliente <= " + clienteAte.Value + "";

		if (jobDe != null)
			sqlWhere += " and lc.cod_job >= " + jobDe.Value + "";
		if (jobAte != null)
			sqlWhere += " and lc.cod_job <= " + jobAte.Value + "";

		if (detalhamento1 != null)
		{
			if (detalhamento1.Equals("JOB"))
				job = true;
			sqlInicioColunas += "0 as codDetalhe1,'' as descDetalhe1,";
		}
		else
		{
			detalhamento1 = "JOB";
			sqlInicioColunas += "0 as codDetalhe1,'' as descDetalhe1,";
			job = true;
		}
		if (detalhamento2 != null)
		{
			if (detalhamento2.Equals("JOB"))
				job = true;
			sqlInicioColunas += "0 as codDetalhe2,'' as descDetalhe2,";
		}
		else if (!job)
		{
			detalhamento2 = "JOB";
			sqlInicioColunas += "0 as codDetalhe2,'' as descDetalhe2,";
			job = true;
		}
		if (detalhamento3 != null)
		{
			if (detalhamento3.Equals("JOB"))
				job = true;
			sqlInicioColunas += "0 as codDetalhe3,'' as descDetalhe3,";
		}
		else if (!job)
		{
			detalhamento3 = "JOB";
			sqlInicioColunas += "0 as codDetalhe3,'' as descDetalhe3,";
			job = true;
		}
		if (detalhamento4 != null)
		{
			if (detalhamento4.Equals("JOB"))
				job = true;
			sqlInicioColunas += "0 as codDetalhe4,'' as descDetalhe4,";
			//sqlInicioColunas += "0 as codDetalhe5,'' as descDetalhe5,";
		}
		else if (!job)
		{
			detalhamento4 = "JOB";
			sqlInicioColunas += "0 as codDetalhe4,'' as descDetalhe4,";
			job = true;
		}


		sql = "select *,(case when lote_pai = 0 then (case when lote = 0 then 'Saldo' " +
			  "  else (select top 1 historico from lanctos_contab lc where lc.lote = x.lote) end)  " +
			" else (select top 1 historico from lanctos_contab lc where lc.lote = x.lote_pai and lc.seq_lote = x.seq_lote_pai) end) as historico, isnull((select distinct modulo from lanctos_contab where lanctos_contab.lote=x.lote_pai),'') as modulo_pai from (select 0 as lote, cc.cod_conta, cc.descricao, convert(datetime,'" + periodoAnterior.ToString("yyyyMMdd") + "') as data, 0 as cod_job, 'Saldo Inicial' as descr,'' as nome_razao_social, 0 as lote_pai, 0 as seq_lote_pai, " + sqlInicioColunas + " " +
			" sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor " +
			" from lanctos_contab lc, cad_contas cc, cad_jobs cj, cad_empresas ce " +
			" where lc.pendente = 'false' and lc.cod_conta = cc.cod_conta and cj.cod_job = lc.cod_job and cj.cod_cliente = ce.cod_empresa and lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cj.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and ce.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + " and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
			" and lc.data < '" + periodoDe.ToString("yyyyMMdd") + "' " + sqlWhere + " " +
			" group by cc.cod_conta, cc.descricao " +
			" union " +
			" select lc.lote, cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao as descr, isnull(ce1.nome_razao_social,'Nenhum') as nome_razao_social , lc.lote_pai, lc.seq_lote_pai, " + sqlInicioColunas + " " +
			" sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor " +
			" from lanctos_contab lc left join cad_empresas ce1 on lc.cod_terceiro = ce1.cod_empresa and ce1.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + ", cad_contas cc, cad_jobs cj, cad_empresas ce " +
			" where " +
			" lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cj.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
			" and lc.pendente = 'false' and lc.cod_conta = cc.cod_conta and cj.cod_job = lc.cod_job and cj.cod_cliente = ce.cod_empresa " +
			" and lc.data >= '" + periodoDe.ToString("yyyyMMdd") + "' and lc.data <= '" + periodoAte.ToString("yyyyMMdd") + "' " + sqlWhere + " " +
			" group by lc.lote, cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao, isnull(ce1.nome_razao_social,'Nenhum'), lc.lote_pai,lc.seq_lote_pai ";


		if (detalhamento1 != null)
		{
			string sqlColunas1 = "";
			string sqlColunasAdicionais1 = "";
			string sqlGroup1 = "";
			string sqlFrom1 = "";
			string sqlRel1 = "";
			sqlOrder += "codDetalhe1,";

			if (detalhamento2 != null)
			{
				sqlColunasAdicionais1 += "0 as codDetalhe2,'' as descDetalhe2,";
				sqlOrder += "codDetalhe2,";
			}

			if (detalhamento3 != null)
			{
				sqlColunasAdicionais1 += "0 as codDetalhe3,'' as descDetalhe3,";
				sqlOrder += "codDetalhe3,";
			}

			//--Gabriel
			if (detalhamento4 != null)
			{
				sqlColunasAdicionais1 += "0 as codDetalhe4,'' as descDetalhe4,";
				//sqlColunasAdicionais1 += "0 as codDetalhe5,'' as descDetalhe5,";
				sqlOrder += "codDetalhe4,";
			}


			switch (detalhamento1)
			{
				case "DIVISAO":
					sqlColunas1 = "lc.cod_divisao as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + cad_divisoes.descricao as descDetalhe1,";
					sqlGroup1 = "lc.cod_divisao,replicate('  ',len(cc.cod_conta)+1) + cad_divisoes.descricao";


					sqlFrom1 = ", cad_divisoes";
					sqlRel1 = " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";

					break;
				case "LINHA_NEGOCIO":
					sqlColunas1 = "lc.cod_linha_negocio as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + cad_linha_negocios.descricao as descDetalhe1,";
					sqlGroup1 = "lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+1) + cad_linha_negocios.descricao";


					sqlFrom1 = ", cad_linha_negocios";
					sqlRel1 = " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
					break;
				case "CLIENTE":
					sqlColunas1 = "lc.cod_cliente as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + cad_empresas.nome_razao_social as descDetalhe1,";
					sqlGroup1 = "lc.cod_cliente,replicate('  ',len(cc.cod_conta)+1) + cad_empresas.nome_razao_social";


					sqlFrom1 = ", cad_empresas";
					sqlRel1 = " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + " ";
					break;
				case "JOB":
					sqlColunas1 = "lc.cod_job as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe1,";
					sqlGroup1 = "lc.cod_job,replicate('  ',len(cc.cod_conta)+1) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


					sqlFrom1 = ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
					sqlRel1 = " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
					break;
			}

			sql += " union ";
			sql += "select lc.lote,cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao as descr, isnull(ce1.nome_razao_social,'Nenhum') as nome_razao_social,lc.lote_pai,lc.seq_lote_pai ," + sqlColunas1 + sqlColunasAdicionais1 + " " +
				   " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor " +
				   " from lanctos_contab lc left join cad_empresas ce1 on lc.cod_terceiro = ce1.cod_empresa and ce1.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + ", cad_contas cc, cad_jobs cj, cad_empresas ce " + sqlFrom1 + " " +
				   " where " +
				   " lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
				   " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_conta = cc.cod_conta and cj.cod_job = lc.cod_job and cj.cod_cliente = ce.cod_empresa and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cj.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
				   " and lc.data >= '" + periodoDe.ToString("yyyyMMdd") + "' and lc.data <= '" + periodoAte.ToString("yyyyMMdd") + "' " + sqlWhere + sqlRel1 + " " +
				   " group by lc.lote, cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao, isnull(ce1.nome_razao_social,'Nenhum'),lc.lote_pai,lc.seq_lote_pai , " + sqlGroup1;

			if (detalhamento2 != null)
			{
				string sqlColunas2 = "";
				string sqlColunasAdicionais2 = "";
				string sqlGroup2 = "";
				string sqlFrom2 = "";
				string sqlRel2 = "";

				if (detalhamento3 != null)
					sqlColunasAdicionais2 += "0 as codDetalhe3,'' as descDetalhe3,";
				if (detalhamento4 != null)
				{
					sqlColunasAdicionais2 += "0 as codDetalhe4,'' as descDetalhe4,";
					//sqlColunasAdicionais2 += "0 as codDetalhe5,'' as descDetalhe5,";
				}


				switch (detalhamento2)
				{
					case "DIVISAO":
						sqlColunas2 = sqlColunas1 + "lc.cod_divisao as codDetalhe2,replicate('  ',len(cc.cod_conta)+2) + cad_divisoes.descricao as descDetalhe2,";
						sqlGroup2 = sqlGroup1 + ",lc.cod_divisao,replicate('  ',len(cc.cod_conta)+2) + cad_divisoes.descricao";


						sqlFrom2 = sqlFrom1 + ", cad_divisoes";
						sqlRel2 = sqlRel1 + " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";

						break;
					case "LINHA_NEGOCIO":
						sqlColunas2 = sqlColunas1 + "lc.cod_linha_negocio as codDetalhe2,replicate('  ',len(cc.cod_conta)+2) + cad_linha_negocios.descricao as descDetalhe2,";
						sqlGroup2 = sqlGroup1 + ",lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+2) + cad_linha_negocios.descricao";


						sqlFrom2 = sqlFrom1 + ", cad_linha_negocios";
						sqlRel2 = sqlRel1 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
						break;
					case "CLIENTE":
						sqlColunas2 = sqlColunas1 + "lc.cod_cliente as codDetalhe2,replicate('  ',len(cc.cod_conta)+2) + cad_empresas.nome_razao_social as descDetalhe2,";
						sqlGroup2 = sqlGroup1 + ",lc.cod_cliente,replicate('  ',len(cc.cod_conta)+2) + cad_empresas.nome_razao_social";


						sqlFrom2 = sqlFrom1 + ", cad_empresas";
						sqlRel2 = sqlRel1 + " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + " ";
						break;
					case "JOB":
						sqlColunas2 = sqlColunas1 + "lc.cod_job as codDetalhe2 ,replicate('  ',len(cc.cod_conta)+2) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe2,";
						sqlGroup2 = sqlGroup1 + ",lc.cod_job,replicate('  ',len(cc.cod_conta)+2)  + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


						sqlFrom2 = sqlFrom1 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
						sqlRel2 = sqlRel1 + " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
						break;
				}

				sql += " union ";
				sql += "select lc.lote,cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao as descr, isnull(ce1.nome_razao_social,'Nenhum') as nome_razao_social,lc.lote_pai,lc.seq_lote_pai , " + sqlColunas2 + sqlColunasAdicionais2 + " " +
					   " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor " +
					   " from lanctos_contab lc left join cad_empresas ce1 on lc.cod_terceiro = ce1.cod_empresa and ce1.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + ", cad_contas cc, cad_jobs cj, cad_empresas ce " + sqlFrom2 + " " +
					   " where " +
					   " lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
					   " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_conta = cc.cod_conta and cj.cod_job = lc.cod_job and cj.cod_cliente = ce.cod_empresa and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cj.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
					   " and lc.data >= '" + periodoDe.ToString("yyyyMMdd") + "' and lc.data <= '" + periodoAte.ToString("yyyyMMdd") + "' " + sqlWhere + sqlRel2 + " " +
					   " group by lc.lote, cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao, isnull(ce1.nome_razao_social,'Nenhum'),lc.lote_pai,lc.seq_lote_pai , " + sqlGroup2;


				if (detalhamento3 != null)
				{
					string sqlColunas3 = "";
					string sqlColunasAdicionais3 = "";
					string sqlGroup3 = "";
					string sqlFrom3 = "";
					string sqlRel3 = "";


					if (detalhamento4 != null)
					{
						sqlColunasAdicionais3 += "0 as codDetalhe4,'' as descDetalhe4,";
						//sqlColunasAdicionais3 += "0 as codDetalhe5,'' as descDetalhe5,";
					}


					switch (detalhamento3)
					{
						case "DIVISAO":
							sqlColunas3 = sqlColunas2 + "lc.cod_divisao as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + cad_divisoes.descricao as descDetalhe3,";
							sqlGroup3 = sqlGroup2 + ",lc.cod_divisao,replicate('  ',len(cc.cod_conta)+3) + cad_divisoes.descricao";


							sqlFrom3 = sqlFrom2 + ", cad_divisoes";
							sqlRel3 = sqlRel2 + " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";

							break;
						case "LINHA_NEGOCIO":
							sqlColunas3 = sqlColunas2 + "lc.cod_linha_negocio as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + cad_linha_negocios.descricao as descDetalhe3,";
							sqlGroup3 = sqlGroup2 + ",lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+3) + cad_linha_negocios.descricao";


							sqlFrom3 = sqlFrom2 + ", cad_linha_negocios";
							sqlRel3 = sqlRel2 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
							break;
						case "CLIENTE":
							sqlColunas3 = sqlColunas2 + "lc.cod_cliente as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + cad_empresas.nome_razao_social as descDetalhe3,";
							sqlGroup3 = sqlGroup2 + ",lc.cod_cliente,replicate('  ',len(cc.cod_conta)+3) + cad_empresas.nome_razao_social";


							sqlFrom3 = sqlFrom2 + ", cad_empresas";
							sqlRel3 = sqlRel2 + " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + " ";
							break;
						case "JOB":
							sqlColunas3 = sqlColunas2 + "lc.cod_job as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe3,";
							sqlGroup3 = sqlGroup2 + ",lc.cod_job,replicate('  ',len(cc.cod_conta)+3) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


							sqlFrom3 = sqlFrom2 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
							sqlRel3 = sqlRel2 + " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
							break;
					}

					sql += " union ";
					sql += "select lc.lote,cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao as descr, isnull(ce1.nome_razao_social,'Nenhum') as nome_razao_social,lc.lote_pai,lc.seq_lote_pai , " + sqlColunas3 + sqlColunasAdicionais3 + " " +
						   " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor " +
						   " from lanctos_contab lc left join cad_empresas ce1 on lc.cod_terceiro = ce1.cod_empresa and ce1.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + ", cad_contas cc, cad_jobs cj, cad_empresas ce " + sqlFrom3 + " " +
						   " where " +
						   " lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
						   " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_conta = cc.cod_conta and cj.cod_job = lc.cod_job and cj.cod_cliente = ce.cod_empresa and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cj.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
						   " and lc.data >= '" + periodoDe.ToString("yyyyMMdd") + "' and lc.data <= '" + periodoAte.ToString("yyyyMMdd") + "' " + sqlWhere + sqlRel3 + " " +
						   " group by lc.lote, cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao, isnull(ce1.nome_razao_social,'Nenhum'),lc.lote_pai,lc.seq_lote_pai , " + sqlGroup3;


					if (detalhamento4 != null)
					{
						string sqlColunas4 = "";
						string sqlGroup4 = "";
						string sqlFrom4 = "";
						string sqlRel4 = "";

						switch (detalhamento4)
						{
							case "DIVISAO":
								sqlColunas4 = sqlColunas3 + "lc.cod_divisao as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_divisoes.descricao as descDetalhe4,";
								sqlGroup4 = sqlGroup3 + ",lc.cod_divisao,replicate('  ',len(cc.cod_conta)+4) + cad_divisoes.descricao";


								sqlFrom4 = sqlFrom3 + ", cad_divisoes";
								sqlRel4 = sqlRel3 + " and lc.cod_divisao = cad_divisoes.cod_divisao ";

								break;
							case "LINHA_NEGOCIO":
								sqlColunas4 = sqlColunas3 + "lc.cod_linha_negocio as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_linha_negocios.descricao as descDetalhe4,";
								sqlGroup4 = sqlGroup3 + ",lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+4) + cad_linha_negocios.descricao";


								sqlFrom4 = sqlFrom3 + ", cad_linha_negocios";
								sqlRel4 = sqlRel3 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio ";
								break;
							case "CLIENTE":
								sqlColunas4 = sqlColunas3 + "lc.cod_cliente as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_empresas.nome_razao_social as descDetalhe4,";
								sqlGroup4 = sqlGroup3 + ",lc.cod_cliente,replicate('  ',len(cc.cod_conta)+4) + cad_empresas.nome_razao_social";


								sqlFrom4 = sqlFrom3 + ", cad_empresas";
								sqlRel4 = sqlRel3 + " and lc.cod_cliente = cad_empresas.cod_empresa ";
								break;
							case "JOB":
								sqlColunas4 = sqlColunas3 + "lc.cod_job as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe4,";
								sqlGroup4 = sqlGroup3 + ",lc.cod_job,replicate('  ',len(cc.cod_conta)+4) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


								sqlFrom4 = sqlFrom3 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
								sqlRel4 = sqlRel3 + " and lc.cod_job = cad_jobs.cod_job ";
								break;
						}

						//sqlColunas4 += "0 as codDetalhe5,'' as descDetalhe5,";

						sql += " union ";
						sql += "select lc.lote,cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao as descr,isnull(ce1.nome_razao_social,'Nenhum') as nome_razao_social,lc.lote_pai,lc.seq_lote_pai , " + sqlColunas4 + " " +
							   " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor " +
							   " from lanctos_contab lc left join cad_empresas ce1 on lc.cod_terceiro = ce1.cod_empresa and ce1.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + ", cad_contas cc, cad_jobs cj, cad_empresas ce " + sqlFrom4 + " " +
							   " where " +
							   " lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
							   " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_conta = cc.cod_conta and cj.cod_job = lc.cod_job and cj.cod_cliente = ce.cod_empresa and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cj.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
							   " and lc.data >= '" + periodoDe.ToString("yyyyMMdd") + "' and lc.data <= '" + periodoAte.ToString("yyyyMMdd") + "' " + sqlWhere + sqlRel4 + " " +
							   " group by lc.lote, ce.NOME_RAZAO_SOCIAL, cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao, isnull(ce1.nome_razao_social,'Nenhum'),lc.lote_pai,lc.seq_lote_pai , " + sqlGroup4;
					}
				}
			}
		}

		sql += ") x order by cod_conta,data,nome_razao_social," + sqlOrder + "descr";

		_conn.fill(sql, ref tb);
	}

	public void movimentacao_financeira(DateTime periodoDe, DateTime periodoAte, DateTime periodoAnterior, string contaDe, string contaAte, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, string detalhamento1,
        string detalhamento2, string detalhamento3, string detalhamento4, ref DataTable tb, int tipoConta = 1)
    {
        string sqlWhere = "";
        string sqlInicioColunas = "";
        string sql = "";

        if (contaDe != null)
            sqlWhere += " and lc.cod_conta >= '" + contaDe + "'";
        if (contaAte != null)
            sqlWhere += " and lc.cod_conta <= '" + contaAte + "'";

        if (divisaoDe != null)
            sqlWhere += " and lc.cod_divisao >= " + divisaoDe.Value + "";
        if (divisaoAte != null)
            sqlWhere += " and lc.cod_divisao <= " + divisaoAte.Value + "";

        if (linhaNegocioDe != null)
            sqlWhere += " and lc.cod_linha_negocio >= " + linhaNegocioDe.Value + "";
        if (linhaNegocioAte != null)
            sqlWhere += " and lc.cod_linha_negocio <= " + linhaNegocioAte.Value + "";

        if (clienteDe != null)
            sqlWhere += " and lc.cod_cliente >= " + clienteDe.Value + "";
        if (clienteAte != null)
            sqlWhere += " and lc.cod_cliente <= " + clienteAte.Value + "";

        if (jobDe != null)
            sqlWhere += " and lc.cod_job >= " + jobDe.Value + "";
        if (jobAte != null)
            sqlWhere += " and lc.cod_job <= " + jobAte.Value + "";

        if (detalhamento1 != null)
            sqlInicioColunas += "0 as codDetalhe1,'' as descDetalhe1,";
        if (detalhamento2 != null)
            sqlInicioColunas += "0 as codDetalhe2,'' as descDetalhe2,";
        if (detalhamento3 != null)
            sqlInicioColunas += "0 as codDetalhe3,'' as descDetalhe3,";
        if (detalhamento4 != null)
            sqlInicioColunas += "0 as codDetalhe4,'' as descDetalhe4,";

        sql = "select *,(case when lote_pai = 0 then (case when lote = 0 then 'Saldo' " +
              "  else (select top 1 historico from lanctos_contab lc where lc.lote = x.lote) end)  " +
            " else (select top 1 historico from lanctos_contab lc where lc.lote = x.lote_pai and lc.seq_lote = x.seq_lote_pai) end) as historico, isnull((select distinct modulo from lanctos_contab where lanctos_contab.lote=x.lote_pai),'') as modulo_pai from (select 0 as lote, '0' as tipo_conta, cc.cod_conta, cc.descricao, convert(datetime,'" + periodoDe.ToString("yyyyMMdd") + "') as data, 0 as cod_job, 'Saldo Inicial' as descr,'' as nome_razao_social, 0 as lote_pai, 0 as seq_lote_pai, " + sqlInicioColunas + " " +
            " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor " +
            " from lanctos_contab lc, cad_contas cc, cad_jobs cj, cad_empresas ce " +
            " where lc.pendente = 'false' and lc.cod_conta = cc.cod_conta and cj.cod_job = lc.cod_job and cj.cod_cliente = ce.cod_empresa and lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cj.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and ce.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + " and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
            " and lc.data < '" + periodoDe.ToString("yyyyMMdd") + "' " + sqlWhere + " " +
            " group by cc.cod_conta, cc.descricao " +
            " union " +
            " select lc.lote, isnull(case when lc.historico like '%Transfer%' and DEB_CRED = 'C' then 'Contas a Pagar' else ce1.tipo_conta_entrada end,'Débito em Conta') as Tipo_conta, cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao as descr, isnull(ce1.nome_razao_social,'Nenhum') as nome_razao_social , lc.lote_pai, lc.seq_lote_pai, " + sqlInicioColunas + " " +
            " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor " +
            " from lanctos_contab lc left join cad_empresas ce1 on lc.cod_terceiro = ce1.cod_empresa and ce1.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + ", cad_contas cc, cad_jobs cj, cad_empresas ce " +
            " where " +
            " lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cj.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
            " and lc.pendente = 'false' and lc.cod_conta = cc.cod_conta and cj.cod_job = lc.cod_job and cj.cod_cliente = ce.cod_empresa " +
            " and lc.data >= '" + periodoDe.ToString("yyyyMMdd") + "' and lc.data <= '" + periodoAte.ToString("yyyyMMdd") + "' " + sqlWhere + " " +
            " group by lc.lote, isnull(case when lc.historico like '%Transfer%' and DEB_CRED = 'C' then 'Contas a Pagar' else ce1.tipo_conta_entrada end,'Débito em Conta'), cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao, isnull(ce1.nome_razao_social,'Nenhum'), lc.lote_pai,lc.seq_lote_pai ";


        if (detalhamento1 != null)
        {
            string sqlColunas1 = "";
            string sqlColunasAdicionais1 = "";
            string sqlGroup1 = "";
            string sqlFrom1 = "";
            string sqlRel1 = "";

            if (detalhamento2 != null)
                sqlColunasAdicionais1 += "0 as codDetalhe2,'' as descDetalhe2,";
            if (detalhamento3 != null)
                sqlColunasAdicionais1 += "0 as codDetalhe3,'' as descDetalhe3,";
            if (detalhamento4 != null)
                sqlColunasAdicionais1 += "0 as codDetalhe4,'' as descDetalhe4,";

            switch (detalhamento1)
            {
                case "DIVISAO":
                    sqlColunas1 = "lc.cod_divisao as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + cad_divisoes.descricao as descDetalhe1,";
                    sqlGroup1 = "lc.cod_divisao,replicate('  ',len(cc.cod_conta)+1) + cad_divisoes.descricao";


                    sqlFrom1 = ", cad_divisoes";
                    sqlRel1 = " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";

                    break;
                case "LINHA_NEGOCIO":
                    sqlColunas1 = "lc.cod_linha_negocio as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + cad_linha_negocios.descricao as descDetalhe1,";
                    sqlGroup1 = "lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+1) + cad_linha_negocios.descricao";


                    sqlFrom1 = ", cad_linha_negocios";
                    sqlRel1 = " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
                    break;
                case "CLIENTE":
                    sqlColunas1 = "lc.cod_cliente as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + cad_empresas.nome_razao_social as descDetalhe1,";
                    sqlGroup1 = "lc.cod_cliente,replicate('  ',len(cc.cod_conta)+1) + cad_empresas.nome_razao_social";


                    sqlFrom1 = ", cad_empresas";
                    sqlRel1 = " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + " ";
                    break;
                case "JOB":
                    sqlColunas1 = "lc.cod_job as codDetalhe1,replicate('  ',len(cc.cod_conta)+1) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe1,";
                    sqlGroup1 = "lc.cod_job,replicate('  ',len(cc.cod_conta)+1) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                    sqlFrom1 = ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                    sqlRel1 = " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
                    break;
            }

            sql += " union ";
            sql += "select lc.lote, isnull(case when lc.historico like '%Transfer%' and DEB_CRED = 'C' then 'Contas a Pagar' else ce1.tipo_conta_entrada end,'Débito em Conta') as Tipo_conta, cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao as descr, isnull(ce1.nome_razao_social,'Nenhum') as nome_razao_social,lc.lote_pai,lc.seq_lote_pai ," + sqlColunas1 + sqlColunasAdicionais1 + " " +
                   " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor " +
                   " from lanctos_contab lc left join cad_empresas ce1 on lc.cod_terceiro = ce1.cod_empresa and ce1.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + ", cad_contas cc, cad_jobs cj, cad_empresas ce " + sqlFrom1 + " " +
                   " where " +
                   " lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                   " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_conta = cc.cod_conta and cj.cod_job = lc.cod_job and cj.cod_cliente = ce.cod_empresa and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cj.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                   " and lc.data >= '" + periodoDe.ToString("yyyyMMdd") + "' and lc.data <= '" + periodoAte.ToString("yyyyMMdd") + "' " + sqlWhere + sqlRel1 + " " +
                   " group by lc.lote, isnull(case when lc.historico like '%Transfer%' and DEB_CRED = 'C' then 'Contas a Pagar' else ce1.tipo_conta_entrada end,'Débito em Conta'), cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao, isnull(ce1.nome_razao_social,'Nenhum'),lc.lote_pai,lc.seq_lote_pai , " + sqlGroup1;

            if (detalhamento2 != null)
            {
                string sqlColunas2 = "";
                string sqlColunasAdicionais2 = "";
                string sqlGroup2 = "";
                string sqlFrom2 = "";
                string sqlRel2 = "";

                if (detalhamento3 != null)
                    sqlColunasAdicionais2 += "0 as codDetalhe3,'' as descDetalhe3,";
                if (detalhamento4 != null)
                    sqlColunasAdicionais2 += "0 as codDetalhe4,'' as descDetalhe4,";

                switch (detalhamento2)
                {
                    case "DIVISAO":
                        sqlColunas2 = sqlColunas1 + "lc.cod_divisao as codDetalhe2,replicate('  ',len(cc.cod_conta)+2) + cad_divisoes.descricao as descDetalhe2,";
                        sqlGroup2 = sqlGroup1 + ",lc.cod_divisao,replicate('  ',len(cc.cod_conta)+2) + cad_divisoes.descricao";


                        sqlFrom2 = sqlFrom1 + ", cad_divisoes";
                        sqlRel2 = sqlRel1 + " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";

                        break;
                    case "LINHA_NEGOCIO":
                        sqlColunas2 = sqlColunas1 + "lc.cod_linha_negocio as codDetalhe2,replicate('  ',len(cc.cod_conta)+2) + cad_linha_negocios.descricao as descDetalhe2,";
                        sqlGroup2 = sqlGroup1 + ",lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+2) + cad_linha_negocios.descricao";


                        sqlFrom2 = sqlFrom1 + ", cad_linha_negocios";
                        sqlRel2 = sqlRel1 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
                        break;
                    case "CLIENTE":
                        sqlColunas2 = sqlColunas1 + "lc.cod_cliente as codDetalhe2,replicate('  ',len(cc.cod_conta)+2) + cad_empresas.nome_razao_social as descDetalhe2,";
                        sqlGroup2 = sqlGroup1 + ",lc.cod_cliente,replicate('  ',len(cc.cod_conta)+2) + cad_empresas.nome_razao_social";


                        sqlFrom2 = sqlFrom1 + ", cad_empresas";
                        sqlRel2 = sqlRel1 + " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + " ";
                        break;
                    case "JOB":
                        sqlColunas2 = sqlColunas1 + "lc.cod_job as codDetalhe2 ,replicate('  ',len(cc.cod_conta)+2) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe2,";
                        sqlGroup2 = sqlGroup1 + ",lc.cod_job,replicate('  ',len(cc.cod_conta)+2)  + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                        sqlFrom2 = sqlFrom1 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                        sqlRel2 = sqlRel1 + " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
                        break;
                }

                sql += " union ";
                sql += "select lc.lote, isnull(case when lc.historico like '%Transfer%' and DEB_CRED = 'C' then 'Contas a Pagar' else ce1.tipo_conta_entrada end,'Débito em Conta') as Tipo_conta, cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao as descr, isnull(ce1.nome_razao_social,'Nenhum') as nome_razao_social,lc.lote_pai,lc.seq_lote_pai , " + sqlColunas2 + sqlColunasAdicionais2 + " " +
                       " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor " +
                       " from lanctos_contab lc left join cad_empresas ce1 on lc.cod_terceiro = ce1.cod_empresa and ce1.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + ", cad_contas cc, cad_jobs cj, cad_empresas ce " + sqlFrom2 + " " +
                       " where " +
                       " lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                       " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_conta = cc.cod_conta and cj.cod_job = lc.cod_job and cj.cod_cliente = ce.cod_empresa and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cj.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                       " and lc.data >= '" + periodoDe.ToString("yyyyMMdd") + "' and lc.data <= '" + periodoAte.ToString("yyyyMMdd") + "' " + sqlWhere + sqlRel2 + " " +
                       " group by lc.lote, isnull(case when lc.historico like '%Transfer%' and DEB_CRED = 'C' then 'Contas a Pagar' else ce1.tipo_conta_entrada end,'Débito em Conta'), cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao, isnull(ce1.nome_razao_social,'Nenhum'),lc.lote_pai,lc.seq_lote_pai , " + sqlGroup2;


                if (detalhamento3 != null)
                {
                    string sqlColunas3 = "";
                    string sqlColunasAdicionais3 = "";
                    string sqlGroup3 = "";
                    string sqlFrom3 = "";
                    string sqlRel3 = "";


                    if (detalhamento4 != null)
                        sqlColunasAdicionais3 += "0 as codDetalhe4,'' as descDetalhe4,";

                    switch (detalhamento3)
                    {
                        case "DIVISAO":
                            sqlColunas3 = sqlColunas2 + "lc.cod_divisao as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + cad_divisoes.descricao as descDetalhe3,";
                            sqlGroup3 = sqlGroup2 + ",lc.cod_divisao,replicate('  ',len(cc.cod_conta)+3) + cad_divisoes.descricao";


                            sqlFrom3 = sqlFrom2 + ", cad_divisoes";
                            sqlRel3 = sqlRel2 + " and lc.cod_divisao = cad_divisoes.cod_divisao and cad_divisoes.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";

                            break;
                        case "LINHA_NEGOCIO":
                            sqlColunas3 = sqlColunas2 + "lc.cod_linha_negocio as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + cad_linha_negocios.descricao as descDetalhe3,";
                            sqlGroup3 = sqlGroup2 + ",lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+3) + cad_linha_negocios.descricao";


                            sqlFrom3 = sqlFrom2 + ", cad_linha_negocios";
                            sqlRel3 = sqlRel2 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio and cad_linha_negocios.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
                            break;
                        case "CLIENTE":
                            sqlColunas3 = sqlColunas2 + "lc.cod_cliente as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + cad_empresas.nome_razao_social as descDetalhe3,";
                            sqlGroup3 = sqlGroup2 + ",lc.cod_cliente,replicate('  ',len(cc.cod_conta)+3) + cad_empresas.nome_razao_social";


                            sqlFrom3 = sqlFrom2 + ", cad_empresas";
                            sqlRel3 = sqlRel2 + " and lc.cod_cliente = cad_empresas.cod_empresa and cad_empresas.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + " ";
                            break;
                        case "JOB":
                            sqlColunas3 = sqlColunas2 + "lc.cod_job as codDetalhe3,replicate('  ',len(cc.cod_conta)+3) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe3,";
                            sqlGroup3 = sqlGroup2 + ",lc.cod_job,replicate('  ',len(cc.cod_conta)+3) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                            sqlFrom3 = sqlFrom2 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                            sqlRel3 = sqlRel2 + " and lc.cod_job = cad_jobs.cod_job and cad_jobs.cod_empresa=" + HttpContext.Current.Session["empresa"] + " ";
                            break;
                    }

                    sql += " union ";
                    sql += "select lc.lote, isnull(case when lc.historico like '%Transfer%' and DEB_CRED = 'C' then 'Contas a Pagar' else ce1.tipo_conta_entrada end,'Débito em Conta') as Tipo_conta, cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao as descr, isnull(ce1.nome_razao_social,'Nenhum') as nome_razao_social,lc.lote_pai,lc.seq_lote_pai , " + sqlColunas3 + sqlColunasAdicionais3 + " " +
                           " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor " +
                           " from lanctos_contab lc left join cad_empresas ce1 on lc.cod_terceiro = ce1.cod_empresa and ce1.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + ", cad_contas cc, cad_jobs cj, cad_empresas ce " + sqlFrom3 + " " +
                           " where " +
                           " lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                           " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_conta = cc.cod_conta and cj.cod_job = lc.cod_job and cj.cod_cliente = ce.cod_empresa and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cj.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                           " and lc.data >= '" + periodoDe.ToString("yyyyMMdd") + "' and lc.data <= '" + periodoAte.ToString("yyyyMMdd") + "' " + sqlWhere + sqlRel3 + " " +
                           " group by lc.lote, isnull(case when lc.historico like '%Transfer%' and DEB_CRED = 'C' then 'Contas a Pagar' else ce1.tipo_conta_entrada end,'Débito em Conta'), cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao, isnull(ce1.nome_razao_social,'Nenhum'),lc.lote_pai,lc.seq_lote_pai , " + sqlGroup3;


                    if (detalhamento4 != null)
                    {
                        string sqlColunas4 = "";
                        string sqlGroup4 = "";
                        string sqlFrom4 = "";
                        string sqlRel4 = "";

                        switch (detalhamento4)
                        {
                            case "DIVISAO":
                                sqlColunas4 = sqlColunas3 + "lc.cod_divisao as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_divisoes.descricao as descDetalhe4,";
                                sqlGroup4 = sqlGroup3 + ",lc.cod_divisao,replicate('  ',len(cc.cod_conta)+4) + cad_divisoes.descricao";


                                sqlFrom4 = sqlFrom3 + ", cad_divisoes";
                                sqlRel4 = sqlRel3 + " and lc.cod_divisao = cad_divisoes.cod_divisao ";

                                break;
                            case "LINHA_NEGOCIO":
                                sqlColunas4 = sqlColunas3 + "lc.cod_linha_negocio as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_linha_negocios.descricao as descDetalhe4,";
                                sqlGroup4 = sqlGroup3 + ",lc.cod_linha_negocio,replicate('  ',len(cc.cod_conta)+4) + cad_linha_negocios.descricao";


                                sqlFrom4 = sqlFrom3 + ", cad_linha_negocios";
                                sqlRel4 = sqlRel3 + " and lc.cod_linha_negocio = cad_linha_negocios.cod_linha_negocio ";
                                break;
                            case "CLIENTE":
                                sqlColunas4 = sqlColunas3 + "lc.cod_cliente as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + cad_empresas.nome_razao_social as descDetalhe4,";
                                sqlGroup4 = sqlGroup3 + ",lc.cod_cliente,replicate('  ',len(cc.cod_conta)+4) + cad_empresas.nome_razao_social";


                                sqlFrom4 = sqlFrom3 + ", cad_empresas";
                                sqlRel4 = sqlRel3 + " and lc.cod_cliente = cad_empresas.cod_empresa ";
                                break;
                            case "JOB":
                                sqlColunas4 = sqlColunas3 + "lc.cod_job as codDetalhe4,replicate('  ',len(cc.cod_conta)+4) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao as descDetalhe4,";
                                sqlGroup4 = sqlGroup3 + ",lc.cod_job,replicate('  ',len(cc.cod_conta)+4) + ceJob.nome_fantasia + ' - ' + cad_jobs.descricao";


                                sqlFrom4 = sqlFrom3 + ", cad_jobs left join cad_empresas ceJob on cad_jobs.cod_cliente = ceJob.cod_empresa";
                                sqlRel4 = sqlRel3 + " and lc.cod_job = cad_jobs.cod_job ";
                                break;
                        }

                        sql += " union ";
                        sql += "select lc.lote, isnull(case when lc.historico like '%Transfer%' and DEB_CRED = 'C' then 'Contas a Pagar' else ce1.tipo_conta_entrada end,'Débito em Conta') as Tipo_conta, cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao as descr,isnull(ce1.nome_razao_social,'Nenhum') as nome_razao_social,lc.lote_pai,lc.seq_lote_pai , " + sqlColunas4 + " " +
                               " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor " +
                               " from lanctos_contab lc left join cad_empresas ce1 on lc.cod_terceiro = ce1.cod_empresa and ce1.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + ", cad_contas cc, cad_jobs cj, cad_empresas ce " + sqlFrom4 + " " +
                               " where " +
                               " lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                               " and lc.pendente = 'False' and cc.analitica = 1 and lc.cod_conta = cc.cod_conta and cj.cod_job = lc.cod_job and cj.cod_cliente = ce.cod_empresa and cc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " and cj.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                               " and lc.data >= '" + periodoDe.ToString("yyyyMMdd") + "' and lc.data <= '" + periodoAte.ToString("yyyyMMdd") + "' " + sqlWhere + sqlRel4 + " " +
                               " group by lc.lote, isnull(case when lc.historico like '%Transfer%' and DEB_CRED = 'C' then 'Contas a Pagar' else ce1.tipo_conta_entrada end,'Débito em Conta'), ce.NOME_RAZAO_SOCIAL, cc.cod_conta, cc.descricao, lc.data, cj.cod_job, ce.nome_fantasia + ' - ' + cj.descricao, isnull(ce1.nome_razao_social,'Nenhum'),lc.lote_pai,lc.seq_lote_pai , " + sqlGroup4;
                    }
                }
            }
        }

        sql += ") x order by cod_conta, data,"  + (tipoConta != 1 ? "tipo_conta," : string.Empty) + "nome_razao_social,descr";

        _conn.fill(sql, ref tb);
    }

    public void movimentacao_bancaria(DateTime periodoDe, DateTime periodoAte, string contaDe, string contaAte, string grupo, ref DataTable tb)
	{
        string sqlWhere = "";
        string filtroConta = "";
        string sql = "";
        if(contaDe == null && contaAte == null)
            filtroConta += " and cc.cod_tipo_conta = 'BC'";
		else
		{
            if (contaDe != null)
			{
                filtroConta += " and cc.cod_conta >= '" + contaDe + "'";
            }
            if (contaAte != null)
			{
                filtroConta += " and cc.cod_conta <= '" + contaAte + "'";
            }
        }

        sql += "select y.*, " + grupo + " as Grupo from	" +
               "(select x.* , " +
               "case when x.entradas > 0 then " +
               "(case when x.historico like '%JRS%' or x.historico like '%Juros%' then 'Receitas Financeiras' " +
               "when x.historico like '%JRS%' or x.historico like '%Juros%' then 'Receitas Financeiras' " +
               "Else 'Entradas' end) " +
               "else " +
               "(case when x.historico like '%pró-labore%' or x.historico like '%pro-labore%'  or x.historico like '%prolabore%' then 'Conta Corrente' " +
               "when x.historico like '%Salario%' or x.historico like '%Salário%'   then 'Salario' " +
               "when x.historico like '%taxa%' or x.historico like '%IOF%' or x.historico like '%tarifa%'  then 'Despesas Bancarias' " +
               "else 'Conta Corrente' end) " +
               "end as Classificacao " +
               "from " +
               "(select 0 as Tipo, cc.cod_conta, cc.descricao, '" + periodoDe.ToString("yyyyMMdd") + "' as Data, 0 as lote, 0 as Seq_baixa, 0 as det_baixa, '' as NUMERO_DOCUMENTO, " +
               "case when sum(case when DEB_CRED = 'D' then VALOR else -valor end) < 0 then 0 else sum(case when DEB_CRED = 'D' then VALOR else -valor end) end as Entradas, " +
               "case when sum(case when DEB_CRED = 'D' then VALOR else -valor end) < 0 then sum(case when DEB_CRED = 'D' then -VALOR else valor end) else 0 end as Saidas, " +
               "'Saldo Inicial' as historico, '' as terceiro " +
               "from LANCTOS_CONTAB LC, Cad_contas cc " +
               "where data < '" + periodoDe.ToString("yyyyMMdd") + "' " + filtroConta + sqlWhere + " " +
               "and lc.PENDENTE = 'False' " +
               "and cc.COD_CONTA = lc.COD_CONTA " +
               "group by cc.COD_CONTA, cc.DESCRICAO " +
               "union all " +
               "select 1 as Tipo, cc.COD_CONTA, cc.DESCRICAO, lc.DATA, (case when lc.SEQ_BAIXA = 0 then lc.LOTE else 0 end) as Lote, lc.SEQ_BAIXA, lc.DET_BAIXA, max(lc.NUMERO_DOCUMENTO) as NUMERO_DOCUMENTO, " +
               "(case when sum(case when lc.DEB_CRED = 'D' then lc.VALOR else -lc.VALOR end) < 0 then 0 else sum(case when lc.DEB_CRED = 'D' then lc.VALOR else -lc.VALOR end) end) as Entradas, " +
               "(case when sum(case when lc.DEB_CRED = 'D' then -lc.VALOR else lc.VALOR end) < 0 then 0 else sum(case when lc.DEB_CRED = 'D' then - lc.VALOR else lc.VALOR end) end) as Saidas, " +
               "lc.historico, " +
               "isnull(ct.NOME_RAZAO_SOCIAL, '') as Terceiro " +
               "from " +
               "CAD_EMPRESAS CT , cad_contas cc, lanctos_Contab LC " +
               "where lc.data between '" + periodoDe.ToString("yyyyMMdd") + "' and '" + periodoAte.ToString("yyyyMMdd") + "' " + filtroConta + sqlWhere + " " +
               "and cc.COD_CONTA = lc.COD_CONTA " +
               "and lc.PENDENTE = 'False' " +
               "and lc.COD_TERCEIRO = ct.COD_EMPRESA " +
               "group by cc.COD_CONTA, cc.DESCRICAO,  lc.DATA, (case when lc.SEQ_BAIXA = 0 then lc.LOTE else 0 end), lc.SEQ_BAIXA, lc.historico, " +
               "isnull(ct.NOME_RAZAO_SOCIAL, ''), lc.DET_BAIXA) x) y " +
               "order by COD_CONTA, DATA, Tipo, Grupo, terceiro, lote, Seq_baixa ";

        _conn.fill(sql, ref tb);
    }

    public void razao(string contaDe, string contaAte, DateTime periodoAnterior, DateTime periodoDe,
        DateTime periodoAte, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, ref DataTable tb,
        string detalhamento1, string detalhamento1Cod,
        string detalhamento2, string detalhamento2Cod, string detalhamento3, string detalhamento3Cod,
        string detalhamento4, string detalhamento4Cod, string detalhamento5, string detalhamento5Cod,
        int cod_moeda
        )
    {
        string sqlWhere = "";

        if (divisaoDe != null)
            sqlWhere += " and lc.cod_divisao >= " + divisaoDe.Value + " ";
        if (divisaoAte != null)
            sqlWhere += " and lc.cod_divisao <= " + divisaoAte.Value + " ";

        if (linhaNegocioDe != null)
            sqlWhere += " and lc.cod_linha_negocio >= " + linhaNegocioDe.Value + " ";
        if (linhaNegocioAte != null)
            sqlWhere += " and lc.cod_linha_negocio <= " + linhaNegocioAte.Value + " ";

        if (clienteDe != null)
            sqlWhere += " and lc.cod_cliente >= " + clienteDe.Value + " ";
        if (clienteAte != null)
            sqlWhere += " and lc.cod_cliente <= " + clienteAte.Value + " ";

        if (jobDe != null)
            sqlWhere += " and lc.cod_job >= " + jobDe.Value + " ";
        if (jobAte != null)
            sqlWhere += " and lc.cod_job <= " + jobAte.Value + " ";

        if (detalhamento1Cod != null)
        {
            switch (detalhamento1Cod)
            {
                case "DIVISAO":
                    sqlWhere += " and lc.cod_divisao = " + detalhamento1 + " ";

                    break;
                case "LINHA_NEGOCIO":
                    sqlWhere += " and lc.cod_linha_negocio = " + detalhamento1 + " ";
                    break;
                case "CLIENTE":
                    sqlWhere += " and lc.cod_cliente = " + detalhamento1 + " ";
                    break;
                case "JOB":
                    sqlWhere += " and lc.cod_job = " + detalhamento1 + " ";
                    break;
                case "TERCEIRO":
                    sqlWhere += " and lc.cod_terceiro = " + detalhamento1 + " ";
                    break;
            }

            if (detalhamento2Cod != null)
            {
                switch (detalhamento2Cod)
                {
                    case "DIVISAO":
                        sqlWhere += " and lc.cod_divisao = " + detalhamento2 + " ";

                        break;
                    case "LINHA_NEGOCIO":
                        sqlWhere += " and lc.cod_linha_negocio = " + detalhamento2 + " ";
                        break;
                    case "CLIENTE":
                        sqlWhere += " and lc.cod_cliente = " + detalhamento2 + " ";
                        break;
                    case "JOB":
                        sqlWhere += " and lc.cod_job = " + detalhamento2 + " ";
                        break;
                    case "TERCEIRO":
                        sqlWhere += " and lc.cod_terceiro = " + detalhamento2 + " ";
                        break;
                }

                if (detalhamento3Cod != null)
                {
                    switch (detalhamento3Cod)
                    {
                        case "DIVISAO":
                            sqlWhere += " and lc.cod_divisao = " + detalhamento3 + " ";

                            break;
                        case "LINHA_NEGOCIO":
                            sqlWhere += " and lc.cod_linha_negocio = " + detalhamento3 + " ";
                            break;
                        case "CLIENTE":
                            sqlWhere += " and lc.cod_cliente = " + detalhamento3 + " ";
                            break;
                        case "JOB":
                            sqlWhere += " and lc.cod_job = " + detalhamento3 + " ";
                            break;
                        case "TERCEIRO":
                            sqlWhere += " and lc.cod_terceiro = " + detalhamento3 + " ";
                            break;
                    }

                    if (detalhamento4Cod != null)
                    {
                        switch (detalhamento4Cod)
                        {
                            case "DIVISAO":
                                sqlWhere += " and lc.cod_divisao = " + detalhamento4 + " ";

                                break;
                            case "LINHA_NEGOCIO":
                                sqlWhere += " and lc.cod_linha_negocio = " + detalhamento4 + " ";
                                break;
                            case "CLIENTE":
                                sqlWhere += " and lc.cod_cliente = " + detalhamento4 + " ";
                                break;
                            case "JOB":
                                sqlWhere += " and lc.cod_job = " + detalhamento4 + " ";
                                break;
                            case "TERCEIRO":
                                sqlWhere += " and lc.cod_terceiro = " + detalhamento4 + " ";
                                break;
                        }

                        if (detalhamento5Cod != null)
                        {
                            switch (detalhamento5Cod)
                            {
                                case "DIVISAO":
                                    sqlWhere += " and lc.cod_divisao = " + detalhamento5 + " ";

                                    break;
                                case "LINHA_NEGOCIO":
                                    sqlWhere += " and lc.cod_linha_negocio = " + detalhamento5 + " ";
                                    break;
                                case "CLIENTE":
                                    sqlWhere += " and lc.cod_cliente = " + detalhamento5 + " ";
                                    break;
                                case "JOB":
                                    sqlWhere += " and lc.cod_job = " + detalhamento5 + " ";
                                    break;
                                case "TERCEIRO":
                                    sqlWhere += " and lc.cod_terceiro = " + detalhamento5 + " ";
                                    break;
                            }
                        }
                    }
                }
            }
        }

        string sql = "select * from (select 0 as lote, 0 as lote_pai, 0 as seq_baixa,'' as modulo,'D' as deb_cred,lc.cod_conta, cc.descricao, convert(datetime,'" + periodoAnterior.ToString("yyyyMMdd") + "') as data,'' as historico,'Saldo Inicial' as nome_razao_social,'' as divisao, " +
                    " '' as job, sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor,'' as terceiro " +
                    " from " + (cod_moeda == 0 ? "lanctos_contab" : "lanctos_contab_moeda") + " lc,cad_contas cc " +
                    " where  " +
                    " " + (cod_moeda != 0 ? "lc.COD_MOEDA = " + cod_moeda + " and " : "") + " " +
                    " lc.COD_EMPRESA = cc.COD_EMPRESA " +
                    " and lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                    " and lc.cod_conta >= '" + contaDe + "' and lc.cod_conta <= '" + contaAte + "' " +
                    " and lc.data <= '" + periodoAnterior.ToString("yyyyMMdd") + "' " +
                    " and lc.pendente = 'False'  " +
                    " and lc.cod_conta = cc.cod_conta " + sqlWhere + "" +
                    " group by lc.cod_conta, cc.descricao " +
                    " union " +
                    " select lc.lote,lc.lote_pai,lc.seq_baixa, lc.modulo,lc.deb_cred,lc.cod_conta,cc.descricao,lc.data,lc.historico,ce.nome_razao_social,cd.descricao as divisao, cj.descricao as job,  " +
                    " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor, isnull(ce2.nome_razao_social,'Nenhum') as terceiro " +
                    " from " + (cod_moeda == 0 ? "lanctos_contab" : "lanctos_contab_moeda") + " lc left join cad_empresas ce2 on lc.cod_terceiro = ce2.cod_empresa and ce2.cod_empresa_pai=" + HttpContext.Current.Session["empresa"] + ",cad_contas cc,cad_empresas ce,cad_divisoes cd,cad_jobs cj,cad_empresas ce1 " +
                    " where  " +
                    " " + (cod_moeda != 0 ? "lc.COD_MOEDA = " + cod_moeda + " and " : "") + " " +
                    " lc.COD_EMPRESA = cc.COD_EMPRESA " +
                    " and lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                    " and lc.cod_conta >= '" + contaDe + "' and lc.cod_conta <= '" + contaAte + "' " +
                    " and lc.data >= '" + periodoDe.ToString("yyyyMMdd") + "' " +
                    " and lc.data <= '" + periodoAte.ToString("yyyyMMdd") + "' " +
                    " and lc.pendente = 'False' " +
                    " and lc.cod_conta = cc.cod_conta " +
                    " and lc.cod_cliente = ce.cod_empresa " +
                    " and lc.cod_divisao = cd.cod_divisao " +
                    " and lc.cod_job = cj.cod_job " +
                    " and cj.cod_cliente = ce1.cod_empresa " + sqlWhere + " group by lc.lote,lc.lote_pai,lc.seq_baixa,lc.modulo,lc.deb_cred,lc.cod_conta,cc.descricao,lc.data,lc.historico,ce.nome_razao_social,cd.descricao, cj.descricao,isnull(ce2.nome_razao_social,'Nenhum')) x  " +
                    " order by x.cod_conta, x.descricao,x.data";

        _conn.fill(sql, ref tb);
    }

    public void razaoSemDrill(string contaDe, string contaAte, DateTime periodoAnterior, DateTime periodoDe,
    DateTime periodoAte, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, ref DataTable tb, string detalhamento1,
    string detalhamento2, string detalhamento3,
    string detalhamento4, string detalhamento5)
    {
        string sqlWhere = "";

        if (divisaoDe != null)
            sqlWhere += " and lc.cod_divisao >= " + divisaoDe.Value + " ";
        if (divisaoAte != null)
            sqlWhere += " and lc.cod_divisao <= " + divisaoAte.Value + " ";

        if (linhaNegocioDe != null)
            sqlWhere += " and lc.cod_linha_negocio >= " + linhaNegocioDe.Value + " ";
        if (linhaNegocioAte != null)
            sqlWhere += " and lc.cod_linha_negocio <= " + linhaNegocioAte.Value + " ";

        if (clienteDe != null)
            sqlWhere += " and lc.cod_cliente >= " + clienteDe.Value + " ";
        if (clienteAte != null)
            sqlWhere += " and lc.cod_cliente <= " + clienteAte.Value + " ";

        if (jobDe != null)
            sqlWhere += " and lc.cod_job >= " + jobDe.Value + " ";
        if (jobAte != null)
            sqlWhere += " and lc.cod_job <= " + jobAte.Value + " ";

        if (detalhamento1 != null)
        {
            switch (detalhamento1)
            {
                case "DIVISAO":
                    sqlWhere += " and lc.cod_divisao = " + detalhamento1 + " ";

                    break;
                case "LINHA_NEGOCIO":
                    sqlWhere += " and lc.cod_linha_negocio = " + detalhamento1 + " ";
                    break;
                case "CLIENTE":
                    sqlWhere += " and lc.cod_cliente = " + detalhamento1 + " ";
                    break;
                case "JOB":
                    sqlWhere += " and lc.cod_job = " + detalhamento1 + " ";
                    break;
                case "TERCEIRO":
                    sqlWhere += " and lc.cod_terceiro = " + detalhamento1 + " ";
                    break;
            }

            if (detalhamento2 != null)
            {
                switch (detalhamento2)
                {
                    case "DIVISAO":
                        sqlWhere += " and lc.cod_divisao = " + detalhamento2 + " ";

                        break;
                    case "LINHA_NEGOCIO":
                        sqlWhere += " and lc.cod_linha_negocio = " + detalhamento2 + " ";
                        break;
                    case "CLIENTE":
                        sqlWhere += " and lc.cod_cliente = " + detalhamento2 + " ";
                        break;
                    case "JOB":
                        sqlWhere += " and lc.cod_job = " + detalhamento2 + " ";
                        break;
                    case "TERCEIRO":
                        sqlWhere += " and lc.cod_terceiro = " + detalhamento2 + " ";
                        break;
                }

                if (detalhamento3 != null)
                {
                    switch (detalhamento3)
                    {
                        case "DIVISAO":
                            sqlWhere += " and lc.cod_divisao = " + detalhamento3 + " ";

                            break;
                        case "LINHA_NEGOCIO":
                            sqlWhere += " and lc.cod_linha_negocio = " + detalhamento3 + " ";
                            break;
                        case "CLIENTE":
                            sqlWhere += " and lc.cod_cliente = " + detalhamento3 + " ";
                            break;
                        case "JOB":
                            sqlWhere += " and lc.cod_job = " + detalhamento3 + " ";
                            break;
                        case "TERCEIRO":
                            sqlWhere += " and lc.cod_terceiro = " + detalhamento3 + " ";
                            break;
                    }

                    if (detalhamento4 != null)
                    {
                        switch (detalhamento4)
                        {
                            case "DIVISAO":
                                sqlWhere += " and lc.cod_divisao = " + detalhamento4 + " ";

                                break;
                            case "LINHA_NEGOCIO":
                                sqlWhere += " and lc.cod_linha_negocio = " + detalhamento4 + " ";
                                break;
                            case "CLIENTE":
                                sqlWhere += " and lc.cod_cliente = " + detalhamento4 + " ";
                                break;
                            case "JOB":
                                sqlWhere += " and lc.cod_job = " + detalhamento4 + " ";
                                break;
                            case "TERCEIRO":
                                sqlWhere += " and lc.cod_terceiro = " + detalhamento4 + " ";
                                break;
                        }

                        if (detalhamento5 != null)
                        {
                            switch (detalhamento5)
                            {
                                case "DIVISAO":
                                    sqlWhere += " and lc.cod_divisao = " + detalhamento5 + " ";

                                    break;
                                case "LINHA_NEGOCIO":
                                    sqlWhere += " and lc.cod_linha_negocio = " + detalhamento5 + " ";
                                    break;
                                case "CLIENTE":
                                    sqlWhere += " and lc.cod_cliente = " + detalhamento5 + " ";
                                    break;
                                case "JOB":
                                    sqlWhere += " and lc.cod_job = " + detalhamento5 + " ";
                                    break;
                                case "TERCEIRO":
                                    sqlWhere += " and lc.cod_terceiro = " + detalhamento5 + " ";
                                    break;
                            }
                        }
                    }
                }
            }
        }

        string sql = "select * from (select lc.cod_conta, cc.descricao, convert(datetime,'" + periodoAnterior.ToString("yyyyMMdd") + "') as data,'' as historico,'Saldo Inicial' as nome_razao_social, " +
                    " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor " +
                    " from lanctos_contab lc,cad_contas cc " +
                    " where  " +
                    " lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                    " and lc.cod_conta >= '" + contaDe + "' and lc.cod_conta <= '" + contaAte + "' " +
                    " and lc.data <= '" + periodoAnterior.ToString("yyyyMMdd") + "' " +
                    " and lc.pendente = 'False'  " +
                    " and lc.cod_conta = cc.cod_conta " + sqlWhere + "" +
                    " group by lc.cod_conta, cc.descricao " +
                    " union " +
                    " select lc.cod_conta,cc.descricao,lc.data,lc.historico,'' as nome_razao_social, " +
                    " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor " +
                    " from lanctos_contab lc, cad_contas cc " +
                    " where  " +
                    " lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                    " and lc.cod_conta >= '" + contaDe + "' and lc.cod_conta <= '" + contaAte + "' " +
                    " and lc.data >= '" + periodoDe.ToString("yyyyMMdd") + "' " +
                    " and lc.data <= '" + periodoAte.ToString("yyyyMMdd") + "' " +
                    " and lc.pendente = 'False' " +
                    " and lc.cod_conta = cc.cod_conta " +
                    " " + sqlWhere + " group by lc.cod_conta,cc.descricao,lc.data,lc.historico) x  " +
                    " order by x.cod_conta, x.descricao,x.data";

        _conn.fill(sql, ref tb);
    }

    public void razaoSemDrillFiltrado(string contaDe, string contaAte, DateTime periodoAnterior, DateTime periodoDe,
        DateTime periodoAte, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, ref DataTable tb, string detalhamento1,
        string detalhamento1Cod,
        string detalhamento2, string detalhamento2Cod, string detalhamento3, string detalhamento3Cod,
        string detalhamento4, string detalhamento4Cod, string detalhamento5, string detalhamento5Cod,
        int cod_moeda
        )
    {
        string sqlWhere = "";

        if (divisaoDe != null)
            sqlWhere += " and lc.cod_divisao >= " + divisaoDe.Value + " ";
        if (divisaoAte != null)
            sqlWhere += " and lc.cod_divisao <= " + divisaoAte.Value + " ";

        if (linhaNegocioDe != null)
            sqlWhere += " and lc.cod_linha_negocio >= " + linhaNegocioDe.Value + " ";
        if (linhaNegocioAte != null)
            sqlWhere += " and lc.cod_linha_negocio <= " + linhaNegocioAte.Value + " ";

        if (clienteDe != null)
            sqlWhere += " and lc.cod_cliente >= " + clienteDe.Value + " ";
        if (clienteAte != null)
            sqlWhere += " and lc.cod_cliente <= " + clienteAte.Value + " ";

        if (jobDe != null)
            sqlWhere += " and lc.cod_job >= " + jobDe.Value + " ";
        if (jobAte != null)
            sqlWhere += " and lc.cod_job <= " + jobAte.Value + " ";

        if (detalhamento1 != null)
        {
            switch (detalhamento1Cod)
            {
                case "DIVISAO":
                    sqlWhere += " and lc.cod_divisao = " + detalhamento1 + " ";

                    break;
                case "LINHA_NEGOCIO":
                    sqlWhere += " and lc.cod_linha_negocio = " + detalhamento1 + " ";
                    break;
                case "CLIENTE":
                    sqlWhere += " and lc.cod_cliente = " + detalhamento1 + " ";
                    break;
                case "JOB":
                    sqlWhere += " and lc.cod_job = " + detalhamento1 + " ";
                    break;
                case "TERCEIRO":
                    sqlWhere += " and lc.cod_terceiro = " + detalhamento1 + " ";
                    break;
            }

            if (detalhamento2 != null)
            {
                switch (detalhamento2Cod)
                {
                    case "DIVISAO":
                        sqlWhere += " and lc.cod_divisao = " + detalhamento2 + " ";

                        break;
                    case "LINHA_NEGOCIO":
                        sqlWhere += " and lc.cod_linha_negocio = " + detalhamento2 + " ";
                        break;
                    case "CLIENTE":
                        sqlWhere += " and lc.cod_cliente = " + detalhamento2 + " ";
                        break;
                    case "JOB":
                        sqlWhere += " and lc.cod_job = " + detalhamento2 + " ";
                        break;
                    case "TERCEIRO":
                        sqlWhere += " and lc.cod_terceiro = " + detalhamento2 + " ";
                        break;
                }

                if (detalhamento3 != null)
                {
                    switch (detalhamento3Cod)
                    {
                        case "DIVISAO":
                            sqlWhere += " and lc.cod_divisao = " + detalhamento3 + " ";

                            break;
                        case "LINHA_NEGOCIO":
                            sqlWhere += " and lc.cod_linha_negocio = " + detalhamento3 + " ";
                            break;
                        case "CLIENTE":
                            sqlWhere += " and lc.cod_cliente = " + detalhamento3 + " ";
                            break;
                        case "JOB":
                            sqlWhere += " and lc.cod_job = " + detalhamento3 + " ";
                            break;
                        case "TERCEIRO":
                            sqlWhere += " and lc.cod_terceiro = " + detalhamento3 + " ";
                            break;
                    }

                    if (detalhamento4 != null)
                    {
                        switch (detalhamento4Cod)
                        {
                            case "DIVISAO":
                                sqlWhere += " and lc.cod_divisao = " + detalhamento4 + " ";

                                break;
                            case "LINHA_NEGOCIO":
                                sqlWhere += " and lc.cod_linha_negocio = " + detalhamento4 + " ";
                                break;
                            case "CLIENTE":
                                sqlWhere += " and lc.cod_cliente = " + detalhamento4 + " ";
                                break;
                            case "JOB":
                                sqlWhere += " and lc.cod_job = " + detalhamento4 + " ";
                                break;
                            case "TERCEIRO":
                                sqlWhere += " and lc.cod_terceiro = " + detalhamento4 + " ";
                                break;
                        }

                        if (detalhamento5 != null)
                        {
                            switch (detalhamento5Cod)
                            {
                                case "DIVISAO":
                                    sqlWhere += " and lc.cod_divisao = " + detalhamento5 + " ";
                                    break;
                                case "LINHA_NEGOCIO":
                                    sqlWhere += " and lc.cod_linha_negocio = " + detalhamento5 + " ";
                                    break;
                                case "CLIENTE":
                                    sqlWhere += " and lc.cod_cliente = " + detalhamento5 + " ";
                                    break;
                                case "JOB":
                                    sqlWhere += " and lc.cod_job = " + detalhamento5 + " ";
                                    break;
                                case "TERCEIRO":
                                    sqlWhere += " and lc.cod_terceiro = " + detalhamento5 + " ";
                                    break;
                            }
                        }
                    }
                }
            }
        }

        string sql = "select * from (select lc.cod_conta,0 as lote, cc.descricao, convert(datetime,'" + periodoAnterior.ToString("yyyyMMdd") + "') as data,'' as historico,'Saldo Inicial' as nome_razao_social, " +
                    " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor, '' as divisao, '' as job, '' as terceiro " +
                    " from " + (cod_moeda == 0 ? "lanctos_contab" : "lanctos_contab_moeda") + " lc,cad_contas cc " +
                    " where " +
                    " " + (cod_moeda != 0 ? "lc.COD_MOEDA = " + cod_moeda + " and ": "") + " " +
                    " lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                    " and lc.cod_empresa = cc.cod_empresa " +
                    " and lc.cod_conta >= '" + contaDe + "' and lc.cod_conta <= '" + contaAte + "' " +
                    " and lc.data <= '" + periodoAnterior.ToString("yyyyMMdd") + "' " +
                    " and lc.pendente = 'False'  " +
                    " and lc.cod_conta = cc.cod_conta " + sqlWhere + "" +
                    " group by lc.cod_conta, cc.descricao " +
                    " union " +
                    " select lc.cod_conta,lc.lote as lote,cc.descricao,lc.data,lc.historico,'' as nome_razao_social, " +
                    " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor, '' as divisao, '' as job, '' as terceiro " +
                    " from " + (cod_moeda == 0 ? "lanctos_contab" : "lanctos_contab_moeda") + " lc, cad_contas cc " +
                    " where  " +
                    " " + (cod_moeda != 0 ? "lc.COD_MOEDA = " + cod_moeda + " and " : "") + " " +
                    " lc.cod_empresa=" + HttpContext.Current.Session["empresa"] + " " +
                    " and lc.cod_empresa = cc.cod_empresa " +
                    " and lc.cod_conta >= '" + contaDe + "' and lc.cod_conta <= '" + contaAte + "' " +
                    " and lc.data >= '" + periodoDe.ToString("yyyyMMdd") + "' " +
                    " and lc.data <= '" + periodoAte.ToString("yyyyMMdd") + "' " +
                    " and lc.pendente = 'False' " +
                    " and lc.cod_conta = cc.cod_conta " +
                    " " + sqlWhere + " group by lc.cod_conta,lc.lote,cc.descricao,lc.data,lc.historico) x  " +
                    " order by x.cod_conta, x.descricao,x.data";

        _conn.fill(sql, ref tb);
    }

    public void razaoContabil(int tipoMoeda, string contaDe, string contaAte, DateTime periodoAnterior, DateTime periodoDe,
        DateTime periodoAte, Nullable<int> divisaoDe, Nullable<int> divisaoAte,
        Nullable<int> linhaNegocioDe, Nullable<int> linhaNegocioAte, Nullable<int> clienteDe,
        Nullable<int> clienteAte, Nullable<int> jobDe, Nullable<int> jobAte, Nullable<int> terDe, Nullable<int> terAte, ref DataTable tb, string empresas)
    {
        string sqlWhere = "";
        string tabela = " LANCTOS_CONTAB ";
        string moedaWhere = "";

        if (tipoMoeda != 0 && tipoMoeda != null)
        {
            tabela = " LANCTOS_CONTAB_MOEDA ";
            moedaWhere = " AND lc.COD_MOEDA = " + tipoMoeda + " ";
        }

        if (divisaoDe != null && divisaoAte != null)
        {
            if (divisaoDe.Value > 0)
                //sqlWhere += " and lc.cod_divisao >= " + divisaoDe.Value + " ";
                sqlWhere += @" and lc.cod_divisao in ("+GeraListaDivisao(empresas, divisaoDe.ToString(), divisaoAte.ToString())+") ";
        }

        //if (divisaoAte != null)
        //{
        //    if (divisaoAte.Value > 0)
        //        sqlWhere += " and lc.cod_divisao <= " + divisaoAte.Value + " ";
        //}

        if (linhaNegocioDe != null && linhaNegocioAte != null)
        {
            if (linhaNegocioDe.Value > 0)
                sqlWhere += @" and lc.COD_LINHA_NEGOCIO in ("+GeraListaLinhaNegocio(empresas, linhaNegocioDe.ToString(), linhaNegocioAte.ToString())+") ";
        }

        //if (linhaNegocioAte != null)
        //{
        //    if (linhaNegocioAte.Value > 0)
        //        sqlWhere += " and lc.cod_linha_negocio <= " + linhaNegocioAte.Value + " ";
        //}

        if (clienteDe != null && clienteAte != null)
        {
            if (clienteDe.Value > 0)
                sqlWhere += @" and lc.COD_EMPRESA in ("+GeraListaEmpresa(empresas,clienteDe.ToString(), clienteAte.ToString())+") ";
        }

        //if (clienteAte != null)
        //{
        //    if (clienteAte.Value > 0)
        //        sqlWhere += " and lc.cod_cliente <= " + clienteAte.Value + " ";
        //}

        if (jobDe != null && jobAte != null)
        {
            if (jobDe.Value > 0)
                sqlWhere += @" and lc.cod_job in ("+GeraListaJob(empresas,jobDe.ToString(),jobAte.ToString())+") ";
        }

        //if (jobAte != null)
        //{
        //    if (jobAte.Value > 0)
        //        sqlWhere += " and lc.cod_job <= " + jobAte.Value + " ";
        //}

        if (terDe != null && terAte != null)
        {
            if (terDe.Value > 0)
                sqlWhere += @" and lc.COD_TERCEIRO in (" + GeraListaEmpresa(empresas, terDe.ToString(), terAte.ToString()) + ") ";
        }

        string sql = "select * from ( " +
                    " select 0 as lote, 0 as seq_lote, cc.cod_conta, 'LB0' as lote_baixa, '' as numero_documento, 'D' as deb_cred,  " +
                    " cc.descricao, convert(datetime,'" + periodoAnterior.ToString("yyyyMMdd") + "') as data,'' as historico, " +
                    " 'Saldo Inicial' as nome_razao_social,'' as divisao,  '' as job,  " +
                    "sum(case when isnull(deb_cred,'D') = 'D' then isnull(lc.valor,0) else -isnull(lc.valor,0) end) as valor, " +
                    " '' as terceiro, '' modulo  from "+tabela+" lc right join cad_contas cc  on " +
                    " lc.cod_empresa in (" + empresas + @") and cc.cod_empresa in (" + empresas + @")  AND CC.COD_EMPRESA = LC.COD_EMPRESA  " +
                    " and lc.data <= '" + periodoAnterior.ToString("yyyyMMdd") + "'  and lc.pendente = 'False'    " +
                    " and lc.cod_conta = cc.cod_conta  where lc.cod_conta >= '" + contaDe + "' and lc.cod_conta <= '" + contaAte + "' and cc.analitica=1  " +
                    " " + sqlWhere + "    " +
                    " group by cc.cod_conta, cc.descricao   " +
                    " union   " +
                    " select lote, seq_lote, lc.cod_conta,  " +
                    " (case when lc.seq_baixa = 0 then 'L' + convert(varchar(9), lc.lote) else 'B' + convert(varchar(9), lc.seq_baixa) end) as lote_baixa,  " +
                    " lc.numero_documento,lc.deb_cred, " +
                    " cc.descricao,lc.data,lc.historico,ce1.nome_razao_social,cd.descricao as divisao,  " +
                    " cj.descricao as job,    " +
                    " sum(case when deb_cred = 'D' then lc.valor else -lc.valor end) as valor,  " +
                    " isnull(ce2.nome_razao_social,'Nenhum') as terceiro, modulo  " +
                    " from " + tabela + " lc left join cad_empresas ce2  " +
                    " on lc.cod_terceiro = ce2.cod_empresa and ce2.cod_empresa_pai in (" + empresas + @"),cad_contas cc, " +
                    " cad_empresas ce,cad_divisoes cd,cad_jobs cj,cad_empresas ce1   " +
                    " where   lc.cod_empresa in (" + empresas + @") AND LC.COD_EMPRESA = CC.COD_EMPRESA and cc.cod_empresa in (" + empresas + @") and lc.cod_Empresa = ce.cod_empresa and CD.COD_EMPRESA = LC.COD_EMPRESA AND CJ.COD_EMPRESA = LC.COD_EMPRESA AND cd.cod_empresa in (" + empresas + @") and cj.cod_empresa in (" + empresas + @") " +
                    " and lc.cod_conta >= '" + contaDe + "' and lc.cod_conta <= '" + contaAte + "' " +
                    " " + sqlWhere + "   " +
                    " and lc.data >= '" + periodoDe.ToString("yyyyMMdd") + "'  and lc.data <= '" + periodoAte.ToString("yyyyMMdd") + "'  and lc.pendente = 'False'   " +
                    " and lc.cod_conta = cc.cod_conta " +
                    " and lc.cod_divisao = cd.cod_divisao  and lc.cod_job = cj.cod_job   " +
                    " and cj.cod_cliente = ce1.cod_empresa   " + moedaWhere +
                    " group by lote, seq_lote, lc.cod_conta,  " +
                    " (case when lc.seq_baixa = 0 then 'L' + convert(varchar(9), lc.lote) else 'B' + convert(varchar(9), lc.seq_baixa) end), " +
                    " lc.numero_documento,lc.deb_cred, " +
                    " cc.descricao,lc.data,lc.historico,ce1.nome_razao_social,cd.descricao, cj.descricao, " +
                    " isnull(ce2.nome_razao_social,'Nenhum'), modulo " +
                    " ) x order by x.cod_conta,x.data,x.lote_baixa";

        _conn.fill(sql, ref tb);
    }

    public void diario(DateTime periodoInicio, DateTime periodoTermino, ref DataTable tb)
    {
        string sql = "select lc.data, lc.lote,lc.deb_cred,lc.cod_conta + ' - ' +cc.descricao,lc.numero_documento, " +
                    " lc.historico, sum(lc.valor)  " +
                    " from lanctos_contab lc, cad_contas cc " +
                    " where lc.pendente='False' " +
                    " and lc.cod_conta = cc.cod_conta " +
                    " and lc.data >= '" + periodoInicio + "'" +
                    " and lc.data <= '" + periodoTermino + "'" +
                    " group by lc.data,lc.lote,lc.seq_lote,lc.deb_cred, " +
                    " lc.cod_conta + ' - ' +cc.descricao, lc.numero_documento, " +
                    " lc.historico " +
                    " order by lc.data,lc.lote,lc.cod_conta + ' - ' +cc.descricao";

        _conn.fill(sql, ref tb);
    }
}
