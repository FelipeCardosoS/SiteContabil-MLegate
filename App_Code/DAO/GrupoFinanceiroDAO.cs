using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de _
/// </summary>
public class GrupoFinanceiroDAO
{
    private Conexao _conn;
    public GrupoFinanceiroDAO(Conexao conn)
    {
        _conn = conn;
    }

    public void list(ref DataTable table, int paginaAtual, string descricao, string ordenacao)
    {
        descricao = string.IsNullOrEmpty(descricao) ? descricao : string.Concat(" AND GRUPO_FINANCEIRO LIKE '%", descricao, "%'");
        string sql = string.Concat("SELECT COD_EMPRESA, GRUPO_FINANCEIRO, DESCRICAO FROM CAD_GRUPOS_FINANCEIROS WHERE COD_EMPRESA = ", HttpContext.Current.Session["empresa"], descricao, " ORDER BY GRUPO_FINANCEIRO ", ordenacao);
        _conn.fill(sql, ref table);

        //List<GrupoEconomico> listaGrupos = tableGrupos.AsEnumerable().Select(o => new GrupoEconomico
        //{
        //    codEmpresa = Convert.ToInt32(o["cod_empresa"]),
        //    codigoGrupoEconomico = Convert.ToInt32(o["cod_grupo_economico"]),
        //    descricao = Convert.ToString(o["descricao"])
        //}).ToList();

        //return tableGrupos;
    }

    public List<GrupoFinanceiro> list()
    {
        string sql = string.Concat("SELECT COD_EMPRESA, GRUPO_FINANCEIRO, DESCRICAO FROM CAD_GRUPOS_FINANCEIROS WHERE COD_EMPRESA = ", HttpContext.Current.Session["empresa"], " ORDER BY GRUPO_FINANCEIRO");
        DataTable tableGrupos = _conn.dataTable(sql, "grupo_financeiro");

        List<GrupoFinanceiro> listaGrupos = tableGrupos.AsEnumerable().Select(o => new GrupoFinanceiro
        {
            codEmpresa = Convert.ToInt32(o["cod_empresa"]),
            nome = Convert.ToString(o["grupo_financeiro"]),
            descricao = Convert.ToString(o["descricao"])
        }).ToList();

        return listaGrupos;
    }

    public void insert(GrupoFinanceiro grupoFinanceiro)
    {
        string sql = string.Concat("INSERT INTO CAD_GRUPOS_FINANCEIROS (COD_EMPRESA, GRUPO_FINANCEIRO, DESCRICAO) VALUES (", HttpContext.Current.Session["empresa"], ", '", grupoFinanceiro.nome, "', '" + grupoFinanceiro.descricao + "')");
        _conn.execute(sql);
    }

    public void update(GrupoFinanceiro grupoFinanceiro, string nomeGrupoFinanceiro)
    {
        string sql = string.Concat("UPDATE CAD_EMPRESAS SET TIPO_CONTA_ENTRADA = '", grupoFinanceiro.nome, "' WHERE TIPO_CONTA_ENTRADA = '", nomeGrupoFinanceiro, "' AND COD_EMPRESA_PAI = ", HttpContext.Current.Session["empresa"], "; ");
        sql += string.Concat("UPDATE CAD_EMPRESAS SET TIPO_CONTA_SAIDA = '", grupoFinanceiro.nome, "' WHERE TIPO_CONTA_SAIDA = '", nomeGrupoFinanceiro, "' AND COD_EMPRESA_PAI = ", HttpContext.Current.Session["empresa"], "; ");
        
        sql += string.Concat("UPDATE CAD_GRUPOS_FINANCEIROS SET GRUPO_FINANCEIRO = '", grupoFinanceiro.nome, "', DESCRICAO = '" + grupoFinanceiro.descricao + "' WHERE GRUPO_FINANCEIRO = '", nomeGrupoFinanceiro, "' AND COD_EMPRESA = ", HttpContext.Current.Session["empresa"]);
        _conn.execute(sql);
    }

    public void delete(string nomeGrupoFinanceiro)
    {
        string sql = string.Concat("UPDATE CAD_EMPRESAS SET TIPO_CONTA_ENTRADA = NULL WHERE TIPO_CONTA_ENTRADA = '", nomeGrupoFinanceiro, "' AND COD_EMPRESA_PAI = ", HttpContext.Current.Session["empresa"], "; ");
        sql += string.Concat("UPDATE CAD_EMPRESAS SET TIPO_CONTA_SAIDA = NULL WHERE TIPO_CONTA_SAIDA = '", nomeGrupoFinanceiro, "' AND COD_EMPRESA_PAI = ", HttpContext.Current.Session["empresa"], "; ");

        sql += string.Concat("DELETE FROM CAD_GRUPOS_FINANCEIROS WHERE GRUPO_FINANCEIRO = ", nomeGrupoFinanceiro, " AND COD_EMPRESA = ", HttpContext.Current.Session["empresa"]);
        _conn.execute(sql);
    }

    public GrupoFinanceiro load(string nomeGrupoFinanceiro)
    {
        string sql = string.Concat("SELECT DESCRICAO FROM CAD_GRUPOS_FINANCEIROS WHERE GRUPO_FINANCEIRO = '", nomeGrupoFinanceiro, "' AND COD_EMPRESA = ", HttpContext.Current.Session["empresa"]);
        DataTable resp = _conn.dataTable(sql, "grupo_financeiro");
        GrupoFinanceiro grupoFinanceiro = new GrupoFinanceiro(_conn);
        if(resp.Rows.Count > 0)
        {
            grupoFinanceiro = new GrupoFinanceiro(_conn)
            {
                codEmpresa = Convert.ToInt32(HttpContext.Current.Session["empresa"]),
                nome = nomeGrupoFinanceiro,
                descricao = Convert.ToString(resp.Rows[0]["descricao"])
            };
        }

        return grupoFinanceiro;
    }

    public int totalRegistros(string descricao)
    {
        descricao = string.IsNullOrEmpty(descricao) ? descricao : string.Concat(" AND GRUPO_FINANCEIRO LIKE '%", descricao, "%'");
        string sql = string.Concat("SELECT COUNT(GRUPO_FINANCEIRO) FROM CAD_GRUPOS_FINANCEIROS WHERE COD_EMPRESA = ", HttpContext.Current.Session["empresa"], descricao);

        return Convert.ToInt32(_conn.scalar(sql));
    }
}