using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

public class contasDAO
{
    private Conexao _conn;

	public contasDAO(Conexao c)
	{
        _conn = c;
	}

    public void insert(int cod_empresa, string cod_conta, int cod_grupo_contabil, string descricao, char debito_credito,
        string cod_conta_sintetica, int analitica, string cod_tipo_conta, int cod_job_default, int cod_job, string tipoImposto,
        bool geraCredito, bool retido, bool recibo, string codClassificacao, string codContaRef, string codContaRefEcf, int codMoedaBalanco, int codMoedaMovimento, int cta, string cod_Dre, string Cod_Balanco)
    {
        string sql = "INSERT INTO CAD_CONTAS(COD_CONTA,COD_GRUPO_CONTABIL,DESCRICAO,DEBITO_CREDITO,";
        sql += "COD_CONTA_SINTETICA, ANALITICA, COD_TIPO_CONTA,COD_EMPRESA, COD_JOB,COD_JOB_DEFAULT, TIPO_IMPOSTO,GERA_CREDITO,RETIDO,RECIBO,COD_CLASSIFICACAO,COD_CONTA_REF,COD_CONTA_REF_ECF,COD_MOEDA_BALANCO,COD_MOEDA_MOVIMENTO,CTA, COD_DRE, COD_BALANCO)";
        sql += "VALUES";
        sql += "('" + cod_conta + "'," + cod_grupo_contabil + ",'" + descricao + "','" + debito_credito + "','" + cod_conta_sintetica + "',";
        sql += "" + analitica + ",'" + cod_tipo_conta + "'," + cod_empresa + "," + cod_job + "," + cod_job_default + ",'" +
            tipoImposto + "'," + Convert.ToInt32(geraCredito) + "," + Convert.ToInt32(retido) + "," + Convert.ToInt32(recibo) + ",'" + codClassificacao + "','" + codContaRef + "','" + codContaRefEcf + "'," + codMoedaBalanco + "," + codMoedaMovimento + "," + cta + ",'" + cod_Dre + "','" + Cod_Balanco + "')";

        _conn.execute(sql);
    }

    public void update(int cod_empresa, string cod_conta, int cod_grupo_contabil, string descricao, char debito_credito,
        string cod_conta_sintetica, int analitica, string cod_tipo_conta, int cod_job_default, int cod_job, string tipoImposto,
        bool geraCredito, bool retido, bool recibo, string codClassificacao, string codContaRef, string codContaRefEcf, int codMoedaBalanco, int codMoedaMovimento, int cta, string Cod_Dre, string Cod_Balanco)
    {
        string sql = "UPDATE CAD_CONTAS SET COD_GRUPO_CONTABIL=" + cod_grupo_contabil + ", DESCRICAO='" + descricao + "',DEBITO_CREDITO='" + debito_credito + "', ";
        sql += " COD_CONTA_SINTETICA='" + cod_conta_sintetica + "', ANALITICA=" + analitica + ", COD_TIPO_CONTA='" + cod_tipo_conta + "', COD_JOB=" + cod_job + ",COD_JOB_DEFAULT=" + cod_job_default + ", ";
        sql += " TIPO_IMPOSTO='" + tipoImposto + "',GERA_CREDITO=" + Convert.ToInt32(geraCredito) + ",RETIDO=" + Convert.ToInt32(retido) + ", RECIBO=" + Convert.ToInt32(recibo) + ", COD_CLASSIFICACAO='" + codClassificacao + "', COD_CONTA_REF='" + codContaRef + "', COD_CONTA_REF_ECF='" + codContaRefEcf + "', COD_MOEDA_BALANCO=" + codMoedaBalanco + ",COD_MOEDA_MOVIMENTO=" + codMoedaMovimento + ", CTA=" + cta + " , cod_dre = '" + Cod_Dre + "', cod_Balanco = '" + Cod_Balanco + "' ";
        sql += " WHERE COD_CONTA='" + cod_conta + "' AND COD_EMPRESA=" + cod_empresa + "";

        _conn.execute(sql);
    }

    public void delete(int cod_empresa, string cod_conta)
    {
        string sql = "DELETE FROM CAD_CONTAS ";
        sql += " WHERE COD_CONTA='" + cod_conta + "' AND COD_EMPRESA=" + cod_empresa + " ";
        _conn.execute(sql);
    }

    public DataTable loadPlanosContas() 
    {
        string sql = "SELECT COD_EMPRESA FROM CAD_EMPRESAS WHERE COD_EMPRESA_PLANO_CONTAS = " + HttpContext.Current.Session["empresa"];
        return _conn.dataTable(sql, "contaPlanosContas");
    }

    public DataTable loadPlanosContasImpostos()
    {
        string sql = "SELECT COD_CONTA FROM CAD_CONTAS WHERE cod_tipo_conta in ('BI', 'I') and COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        return _conn.dataTable(sql, "contaPlanosContas");
    }

    public DataTable load(string cod_conta)
    {
        string sql = "SELECT * FROM CAD_CONTAS ";
        sql += " WHERE COD_CONTA='" + cod_conta + "' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";
        
        return _conn.dataTable(sql, "conta");
    }

    public bool existe(string cod_conta)
    {
        string sql = "SELECT COUNT(COD_CONTA) FROM CAD_CONTAS ";
        sql += " WHERE COD_CONTA='" + cod_conta + "' AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        int result = Convert.ToInt32(_conn.scalar(sql));
        if (result == 0)
            return false;
        else
            return true;
    }

    public bool existeGeraCredito(List<string> contas)
    {
        string txt = "";
        for (int i = 0; i < contas.Count; i++)
        {
            txt += "'"+contas[i] + "',";
        }
        txt = txt.Substring(0, txt.Length - 1);

        string sql = "SELECT isnull(COUNT(COD_CONTA),0) FROM CAD_CONTAS ";
        sql += " WHERE COD_CONTA in (" + txt + ") AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " and GERA_CREDITO=1";

        int total = Convert.ToInt32(_conn.scalar(sql));
        return (total > 0);
    }

    public void lista_sinteticas(ref DataTable tb)
    {
        string sql = "SELECT *, COD_CONTA + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO FROM CAD_CONTAS WHERE ANALITICA=0 AND COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + "";

        _conn.fill(sql, ref tb);
    }

    public List<Hashtable> lista()
    {
        string sql = "SELECT *, COD_CONTA + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO FROM CAD_CONTAS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY COD_CONTA + ' - ' + DESCRICAO";

        return _conn.reader(sql);
    }

    public List<Hashtable> lista(string tipo)
    {
        string sql = "SELECT *, COD_CONTA + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO FROM CAD_CONTAS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"];

        sql += " AND ANALITICA=1 ";
        if (tipo != "")
        {
            sql += " AND COD_TIPO_CONTA='" + tipo + "'";
        }

        sql += " ORDER BY COD_CONTA + ' - ' + DESCRICAO";

        return _conn.reader(sql);
    }

    public List<Hashtable> listaDiferenteDe(string tipo)
    {
        string sql = "SELECT *, COD_CONTA + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO FROM CAD_CONTAS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"];

        sql += " AND ANALITICA=1 ";
        if (tipo != "")
        {
            sql += " AND COD_TIPO_CONTA <> '" + tipo + "'";
        }

        sql += " ORDER BY COD_CONTA + ' - ' + DESCRICAO";

        return _conn.reader(sql);
    }

    public DataTable listaSpedContabil(int empresa)
    {
        //string sql = "select cad_contas.COD_CONTA, CAD_CONTAS.COD_CONTA_SINTETICA, CAD_CONTAS.DESCRICAO,LEN(cad_contas.cod_conta) as nivel, " +
        //            " (case when CAD_CONTAS.ANALITICA=1 then 'A' else 'S' end) as analitica_sintetica, " +
        //            " CAD_GRUPOS_CONTABEIS.DESCRICAO as grupo, cod_conta_ref, COD_DRE, Cod_Balanco from CAD_CONTAS, CAD_GRUPOS_CONTABEIS " +
        //            " where CAD_CONTAS.COD_EMPRESA=" + empresa + " AND CAD_CONTAS.COD_GRUPO_CONTABIL = CAD_GRUPOS_CONTABEIS.COD_GRUPO_CONTABIL " +
        //            " AND CAD_CONTAS.COD_EMPRESA = CAD_GRUPOS_CONTABEIS.COD_EMPRESA";

        string sql = @"WITH PAI (Cod_conta, COD_CONTA_SINTETICA, DESCRICAO, cod_empresa, nivel,analitica_sintetica, grupo, cod_conta_ref, COD_DRE, Cod_Balanco) AS 
                               (SELECT  CAD_CONTAS.cod_conta, COD_CONTA_SINTETICA, CAD_CONTAS.DESCRICAO, CAD_CONTAS.COD_EMPRESA, 1 as nivel, 
                               (case when CAD_CONTAS.ANALITICA=1 then 'A' else 'S' end) as analitica_sintetica,  CAD_GRUPOS_CONTABEIS.DESCRICAO as grupo, cod_conta_ref, COD_DRE, Cod_Balanco 
                                FROM CAD_CONTAS, CAD_GRUPOS_CONTABEIS  
                                WHERE COD_CONTA_SINTETICA = '' and CAD_CONTAS.COD_EMPRESA = " + empresa + @"
                                    and cad_contas.COD_EMPRESA = CAD_GRUPOS_CONTABEIS.COD_EMPRESA
		                            and cad_contas.COD_GRUPO_CONTABIL = CAD_GRUPOS_CONTABEIS.COD_GRUPO_CONTABIL
                                    UNION ALL
                                SELECT filho.cod_conta, filho.COD_CONTA_SINTETICA, filho.DESCRICAO, filho.cod_empresa, PAI.nivel+1 as nivel,
	                            (case when Filho.ANALITICA=1 then 'A' else 'S' end) as analitica_sintetica,  CAD_GRUPOS_CONTABEIS.DESCRICAO as grupo, filho.cod_conta_ref, filho.COD_DRE, filho.Cod_Balanco 
                                FROM CAD_CONTAS as Filho, PAI, CAD_GRUPOS_CONTABEIS  
                                WHERE PAI.COD_CONTA = filho.COD_CONTA_SINTETICA and filho.COD_EMPRESA = pai.cod_empresa and filho.COD_EMPRESA = PAI.cod_empresa
	                            and Filho.COD_EMPRESA = CAD_GRUPOS_CONTABEIS.COD_EMPRESA
	                            and Filho.COD_GRUPO_CONTABIL = CAD_GRUPOS_CONTABEIS.COD_GRUPO_CONTABIL)
                            SELECT cod_conta, COD_CONTA_SINTETICA, DESCRICAO, nivel,analitica_sintetica, grupo, cod_conta_ref, COD_DRE, Cod_Balanco
                            FROM PAI
                            order by cod_conta";

        return _conn.dataTable(sql, "contas_sped_contabil");
    }

    public DataTable listaSpedI052DRE(int empresa, string codigo)
    {
        string sql = "select codigo,descricao,analitica,nivel, ordem " +
                    "  from cad_Estrutura_Dre " +
                    " where COD_EMPRESA=" + empresa + " AND codigo = '" + codigo.Trim() + "'";

        return _conn.dataTable(sql, "contas_sped_contabil");
    }

    public DataTable listaSpedI052Balanco(int empresa, string codigo)
    {
        string sql = "select codigo,descricao,analitica,nivel, ordem, Ativo_Passivo " +
                    "  from Cad_Estrutura_Balanco " +
                    " where COD_EMPRESA=" + empresa + " AND codigo = '" + codigo.Trim() + "'";

        return _conn.dataTable(sql, "contas_sped_contabil");
    }

    public DataTable MontaBalanco(int empresa, DateTime DataInicio, DateTime DataFim)
    {
        //string sql = @"select bc.codigo, bc.descricao, bc.ordem, bc.nivel, bc.ativo_passivo, 
        //                               sum((case when lc.deb_cred = 'C' then valor else -valor end)) as Movimento,
        //                               sum((case when lc.data < '" + DataInicio.ToString("yyyyMMdd") + @"' then (case when lc.deb_cred = 'C' then valor else -valor end) else 0 end)) as Anterior
        //                        from cad_Estrutura_Balanco bc,
        //                             cad_contas cc,
	       //                          lanctos_contab lc
        //                        where bc.codigo = left(cc.cod_balanco,len(bc.codigo))
        //                          and cc.cod_conta = lc.cod_Conta
        //                          and lc.pendente = 'false'
        //                          and bc.cod_empresa = cc.cod_empresa
        //                          and cc.cod_Empresa = lc.cod_empresa
        //                          and lc.cod_empresa = " + empresa + @"
        //                          and lc.data <= '" + DataFim.ToString("yyyyMMdd") + @"'
        //                        group by bc.codigo, bc.descricao, bc.ordem, bc.ativo_passivo, bc.nivel
        //                        order by bc.ordem";

        string sql = @"select *, (select isnull(max(codigo),'') 
                                 from cad_Estrutura_Balanco 
	                             where cod_empresa = x.cod_empresa and x.codigo like  rtrim(ltrim(codigo)) + '%' and codigo <> x.codigo)  as superior from
                            (select bc.cod_empresa, bc.codigo, bc.descricao, bc.ordem, bc.nivel, bc.ativo_passivo, 
                                       sum((case when lc.deb_cred = 'C' then valor else -valor end)) as Movimento,
                                       sum((case when lc.data < '" + DataInicio.ToString("yyyyMMdd") + @"' then (case when lc.deb_cred = 'C' then valor else -valor end) else 0 end)) as Anterior,
                                      (case when bc.codigo = cc.cod_balanco then 'D' else 'T' end) as IND_COD_AGL
                                from cad_Estrutura_Balanco bc,
                                     cad_contas cc,
	                                 lanctos_contab lc
                                where bc.codigo = left(cc.cod_balanco,len(bc.codigo))
                                  and cc.cod_conta = lc.cod_Conta
                                  and lc.pendente = 'false'
                                  and bc.cod_empresa = cc.cod_empresa
                                  and cc.cod_Empresa = lc.cod_empresa
                                  and lc.cod_empresa = " + empresa + @"
                                  and lc.data <= '" + DataFim.ToString("yyyyMMdd") + @"'
                                group by bc.cod_empresa, bc.codigo, bc.descricao, bc.ordem, bc.ativo_passivo, bc.nivel, (case when bc.codigo = cc.cod_balanco then 'D' else 'T' end) ) x
                                order by ordem";

        return _conn.dataTable(sql, "Balanco");
    }

    public DataTable MontaDRE(int empresa, DateTime DataInicio, DateTime DataFim, DateTime DateFimDemonstracao, int periodo)
    {
        DateTime DataFimAnt = DataInicio.AddDays(-1);
        int Dif = (DataFim.Year * 12 + DataFim.Month) - (DataInicio.Year * 12 + DataInicio.Month);
        DateTime DataInicioAnt = DataFimAnt.AddMonths(-periodo+1);
        
        DataInicioAnt = DataInicioAnt.AddDays(-(DataInicioAnt.Day - 1));

        DataInicioAnt = Convert.ToDateTime(DataFimAnt.Year + "/01/01");

        string sql = @"select *, (select isnull(max(codigo),'') 
                             from cad_Estrutura_dre
	                         where cod_empresa = x.cod_empresa and x.codigo like  rtrim(ltrim(codigo)) + '%' and codigo <> x.codigo)  as superior from
                        (select dr.cod_empresa, dr.codigo, dr.descricao, dr.ordem, dr.analitica, dr.nivel,
                                       sum((case when lc.data between '" + DataInicio.ToString("yyyyMMdd") + "' and '" + DateFimDemonstracao.ToString("yyyyMMdd") + @"' then (case when lc.deb_cred = 'C' then valor else -valor end) else 0 end)) as Movimento,
	                                   sum((case when lc.data between '" + DataInicioAnt.ToString("yyyyMMdd") + "' and '" + DataFimAnt.ToString("yyyyMMdd") + @"' then (case when lc.deb_cred = 'C' then valor else -valor end) else 0 end)) as Anterior,
                                       (case when dr.codigo = cc.cod_dre then 'D' else 'T' end) as IND_COD_AGL
                                from cad_Estrutura_Dre dr,
                                     cad_contas cc,
	                                 lanctos_contab lc
                                where dr.codigo = left(cc.cod_dre,len(dr.codigo))
                                  and cc.cod_conta = lc.cod_Conta
                                  and lc.pendente = 'false'
                                  and lc.tipo_lancto = 'N'
                                  and dr.cod_empresa = cc.cod_empresa
                                  and cc.cod_Empresa = lc.cod_empresa
                                  and lc.cod_empresa = " + empresa + @"
                                  and lc.data between '" + DataInicioAnt.ToString("yyyyMMdd") + "' and '" + DateFimDemonstracao.ToString("yyyyMMdd") + @"'
                                group by dr.codigo, dr.cod_empresa, dr.descricao, dr.ordem, dr.analitica, dr.nivel, (case when dr.codigo = cc.cod_dre then 'D' else 'T' end) ) x
                                order by ordem";

        //string sql = @"select dr.codigo, dr.descricao, dr.ordem, dr.analitica, dr.nivel,
        //                               sum((case when lc.data between '" + DataInicio.ToString("yyyyMMdd") + "' and '" + DateFimDemonstracao.ToString("yyyyMMdd") + @"' then (case when lc.deb_cred = 'C' then valor else -valor end) else 0 end)) as Movimento,
	       //                            sum((case when lc.data between '" + DataInicioAnt.ToString("yyyyMMdd") + "' and '" + DataFimAnt.ToString("yyyyMMdd") + @"' then (case when lc.deb_cred = 'C' then valor else -valor end) else 0 end)) as Anterior
        //                        from cad_Estrutura_Dre dr,
        //                             cad_contas cc,
	       //                          lanctos_contab lc
        //                        where dr.codigo = left(cc.cod_dre,len(dr.codigo))
        //                          and cc.cod_conta = lc.cod_Conta
        //                          and lc.pendente = 'false'
        //                          and lc.tipo_lancto = 'N'
        //                          and dr.cod_empresa = cc.cod_empresa
        //                          and cc.cod_Empresa = lc.cod_empresa
        //                          and lc.cod_empresa = " + empresa + @"
        //                          and lc.data between '" + DataInicioAnt.ToString("yyyyMMdd") + "' and '" + DateFimDemonstracao.ToString("yyyyMMdd") + @"'
        //                        group by dr.codigo, dr.descricao, dr.ordem, dr.analitica, dr.nivel
        //                        order by dr.ordem";

        return _conn.dataTable(sql, "DRE");
    }

    public int planodecontas(int empresa) 
    {
        return Convert.ToInt32(_conn.scalar("SELECT TOP 1 COD_EMPRESA_PLANO_CONTAS FROM CAD_EMPRESAS WHERE COD_EMPRESA = "+empresa));
    }

    public void lista(ref DataTable tb)
    {
        //string sql = "SELECT CAD_CONTAS.*, CAD_CONTAS.COD_CONTA + ' - ' + CAD_CONTAS.DESCRICAO AS DESCRICAO_COMPLETO, CAD_GRUPOS_CONTABEIS.DESCRICAO AS GRUPO_CONTABIL FROM CAD_CONTAS,CAD_GRUPOS_CONTABEIS WHERE CAD_CONTAS.COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " AND CAD_CONTAS.COD_GRUPO_CONTABIL = CAD_GRUPOS_CONTABEIS.COD_GRUPO_CONTABIL and CAD_GRUPOS_CONTABEIS.COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + " ORDER BY CAD_CONTAS.COD_CONTA + ' - ' + CAD_CONTAS.DESCRICAO";

        string StrSql = @"with pai (nivel, Cod_conta, cod_empresa, cod_grupo_contabil, descricao, debito_credito, cod_conta_sintetica, analitica, cod_tipo_conta, cod_job, cod_job_default,
                                    gera_credito, tipo_imposto, retido, recibo, cod_classificacao, cod_conta_ref, cod_conta_ref_ecf, cod_moeda_movimento, cod_moeda_balanco, cta, tempo, cod_dre, 
                                    cod_balanco, descricao_completo, grupo_contabil) as
                                    (SELECT 1 as nivel, CAD_CONTAS.*, CAD_CONTAS.COD_CONTA + ' - ' + CAD_CONTAS.DESCRICAO AS DESCRICAO_COMPLETO, CAD_GRUPOS_CONTABEIS.DESCRICAO AS GRUPO_CONTABIL 
	                                    FROM CAD_CONTAS,CAD_GRUPOS_CONTABEIS 
	                                    WHERE COD_CONTA_SINTETICA = '' and CAD_CONTAS.COD_EMPRESA=" + HttpContext.Current.Session["empresa"] + @" AND CAD_CONTAS.COD_GRUPO_CONTABIL = CAD_GRUPOS_CONTABEIS.COD_GRUPO_CONTABIL and CAD_GRUPOS_CONTABEIS.COD_EMPRESA=CAD_CONTAS.COD_EMPRESA

                                        union all
	                                    SELECT pai.nivel +1 as nivel, CAD_CONTAS.*, CAD_CONTAS.COD_CONTA + ' - ' + CAD_CONTAS.DESCRICAO AS DESCRICAO_COMPLETO, CAD_GRUPOS_CONTABEIS.DESCRICAO AS GRUPO_CONTABIL 
	                                    FROM CAD_CONTAS,CAD_GRUPOS_CONTABEIS, pai
	                                    WHERE CAD_CONTAS.COD_CONTA_SINTETICA = pai.Cod_conta and CAD_CONTAS.COD_EMPRESA=pai.cod_empresa AND CAD_CONTAS.COD_GRUPO_CONTABIL = CAD_GRUPOS_CONTABEIS.COD_GRUPO_CONTABIL and CAD_GRUPOS_CONTABEIS.COD_EMPRESA=pai.cod_empresa 
	                                    )
                                    select * from pai
                                    order by Cod_conta";

        _conn.fill(StrSql, ref tb);
    }

    public void lista(ref DataTable tb, int analitica )
    {
        string sql = "SELECT *, COD_CONTA + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO FROM CAD_CONTAS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"];

        sql += " AND ANALITICA = " + analitica;

        sql += " ORDER BY COD_CONTA + ' - ' + DESCRICAO";
        _conn.fill(sql, ref tb);
    }
    public void lista(ref DataTable tb, int analitica, string tipoConta)
    {
        string sql = "SELECT *, COD_CONTA + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO FROM CAD_CONTAS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"];

        sql += " AND ANALITICA = " + analitica;
        sql += " AND COD_TIPO_CONTA = '" + tipoConta + "'";

        sql += " ORDER BY COD_CONTA + ' - ' + DESCRICAO";
        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, int analitica, int grupoContabil)
    {
        string sql = "SELECT *, COD_CONTA + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO FROM CAD_CONTAS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"];

        sql += " AND ANALITICA = " + analitica;
        sql += " and cod_grupo_contabil = " + grupoContabil;

        sql += " ORDER BY COD_CONTA + ' - ' + DESCRICAO";
        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, int analitica, bool PL)
    {
        string sql = "SELECT *, COD_CONTA + ' - ' + DESCRICAO AS DESCRICAO_COMPLETO FROM CAD_CONTAS WHERE COD_EMPRESA=" + HttpContext.Current.Session["empresa"];

        sql += " AND ANALITICA = " + analitica;
        sql += " and cod_grupo_contabil in (select cod_grupo_contabil from CAD_GRUPOS_CONTABEIS where Pl = 1)";

        sql += " ORDER BY COD_CONTA + ' - ' + DESCRICAO";
        _conn.fill(sql, ref tb);
    }

    public void lista(ref DataTable tb, Nullable<int>grupo, string codigo,string descricao,Nullable<char>debCred,
        Nullable<int> analitica, string tipo, int paginaAtual, string ordenacao)
    {
        string tmpOrdenacao = "";
        if (ordenacao != "")
            tmpOrdenacao = ordenacao;
        else
            tmpOrdenacao = "CAD_CONTAS.COD_CONTA";

        string sql = "select * from (SELECT  ROW_NUMBER() OVER (ORDER BY " + tmpOrdenacao + " ASC)  ";
        sql += " AS Row, cad_contas.*, cad_tipo_conta.descricao as descricao_tipo_conta , "+
               " case when cad_contas.analitica = 0 then 'Sintética' "+
               "     else 'Analitica' end as descricao_analitica_sintetica, " +
               " cad_grupos_contabeis.descricao as descricao_grupo_contabil  ";
        sql += "    FROM CAD_CONTAS,cad_tipo_conta, cad_grupos_contabeis WHERE cad_contas.cod_tipo_conta = cad_tipo_conta.cod_tipo_conta "+
               " and cad_contas.cod_grupo_contabil = cad_grupos_contabeis.cod_grupo_contabil ";

        sql += " and CAD_CONTAS.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        if (grupo != null)
            sql += " AND CAD_CONTAS.COD_GRUPO_CONTABIL=" + grupo.Value + "";

        if (codigo != null)
            sql += " AND CAD_CONTAS.COD_CONTA like '%" + codigo + "%'";

        if (descricao != null)
            sql += " AND CAD_CONTAS.DESCRICAO like '%" + descricao + "%'";

        if (debCred != null)
            sql += " AND CAD_CONTAS.DEBITO_CREDITO='" + debCred.Value + "'";

        if (analitica != null)
            sql += " AND CAD_CONTAS.ANALITICA=" + analitica.Value + "";

        if (tipo != null)
            sql += " AND CAD_CONTAS.COD_TIPO_CONTA='" + tipo + "'";

        sql += "    ) as vw where 1=1 ";

        //PAGINACAO
        sql += " AND vw.row <= " + (((paginaAtual - 1) * 50) + 50) + " AND vw.row >=" + ((paginaAtual - 1) * 50);

        _conn.fill(sql, ref tb);
    }

    public int totalRegistros(Nullable<int>grupo, string codigo,string descricao,Nullable<char>debCred,
        Nullable<int> analitica, string tipo)
    {
        string sql = "select count(COD_CONTA) from CAD_CONTAS WHERE 1=1 ";

        sql += " and COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "";

        if (grupo != null)
            sql += " AND COD_GRUPO_CONTABIL=" + grupo.Value + "";

        if (codigo != null)
            sql += " AND COD_CONTA like '" + codigo + "%'";

        if (descricao != null)
            sql += " AND DESCRICAO like '" + descricao + "%'";

        if (debCred != null)
            sql += " AND DEBITO_CREDITO='" + debCred.Value + "'";

        if (analitica != null)
            sql += " AND ANALITICA=" + analitica.Value + "";

        if (tipo != null)
            sql += " AND COD_TIPO_CONTA='" + tipo + "'";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void listaContasAnaliticas(ref DataTable tb)
    {
        string sql = "SELECT COD_CONTA + ' - ' + DESCRICAO as DESCRICAO, COD_CONTA FROM CAD_CONTAS WHERE COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " AND ANALITICA = 1 ORDER BY COD_CONTA";
        _conn.fill(sql, ref tb);
    }

    public void zeraCTA() 
    {
        string sql = "UPDATE CAD_CONTAS SET CTA = 0 where COD_EMPRESA =" + HttpContext.Current.Session["empresa"];
        _conn.execute(sql);
    }

    public int loadMoedaMovimento(string codConta) 
    {
        string sql = "SELECT COD_MOEDA_MOVIMENTO FROM CAD_CONTAS WHERE COD_CONTA = '"+codConta+"' ";
        return Convert.ToInt32(_conn.scalar(sql));
    }

    public DataTable loadContaCTA() 
    {
        string sql = "SELECT CAD_CONTAS.COD_CONTA as COD_CONTA, CAD_CONTAS.DESCRICAO as DESCRICAO_CONTA , CAD_JOBS.COD_JOB as COD_JOB ,CAD_JOBS.DESCRICAO as DESCRICAO_JOB ,CAD_LINHA_NEGOCIOS.COD_LINHA_NEGOCIO as COD_LINHA_NEGOCIO, CAD_LINHA_NEGOCIOS.DESCRICAO as DESCRICAO_LINHA_NEGOCIOS, CAD_DIVISOES.COD_DIVISAO as COD_DIVISAO, CAD_DIVISOES.DESCRICAO as DESCRICAO_DIVISAO, CAD_EMPRESAS.COD_EMPRESA as COD_CLIENTE, CAD_EMPRESAS.NOME_RAZAO_SOCIAL as DESCRICAO_CLIENTE "
        + "FROM CAD_CONTAS, CAD_JOBS, CAD_LINHA_NEGOCIOS, CAD_DIVISOES, CAD_EMPRESAS WHERE CAD_CONTAS.COD_JOB = CAD_JOBS.COD_JOB AND CAD_JOBS.COD_DIVISAO = CAD_DIVISOES.COD_DIVISAO AND CAD_JOBS.COD_LINHA_NEGOCIO = CAD_LINHA_NEGOCIOS.COD_LINHA_NEGOCIO AND CAD_JOBS.COD_CLIENTE = CAD_EMPRESAS.COD_EMPRESA AND CTA = 'True' AND CAD_CONTAS.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " AND CAD_JOBS.COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + "  ";
        return _conn.dataTable(sql, "LoadConta");
    }

    public string loadContaCTADescricao() 
    {
        string sql = "SELECT DESCRICAO FROM CAD_CONTAS WHERE CTA = 'True' AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"] + " ";
        return Convert.ToString(_conn.scalar(sql));
    }
}