using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Descrição resumida de _
/// </summary>
public class GrupoEconomicoDAO
{
    private Conexao _conn;
    public GrupoEconomicoDAO(Conexao conn)
    {
        _conn = conn;
    }

    public void list(ref DataTable table, int paginaAtual, string descricao, string ordenacao)
    {
        descricao = string.IsNullOrEmpty(descricao) ? descricao : string.Concat(" AND DESCRICAO LIKE '%", descricao, "%'");
        string sql = string.Concat("SELECT COD_GRUPO_ECONOMICO, COD_EMPRESA, DESCRICAO FROM CAD_GRUPOS_ECONOMICOS WHERE COD_EMPRESA = ", HttpContext.Current.Session["empresa"], descricao, " ORDER BY DESCRICAO ", ordenacao);
        _conn.fill(sql, ref table);

        //List<GrupoEconomico> listaGrupos = tableGrupos.AsEnumerable().Select(o => new GrupoEconomico
        //{
        //    codEmpresa = Convert.ToInt32(o["cod_empresa"]),
        //    codigoGrupoEconomico = Convert.ToInt32(o["cod_grupo_economico"]),
        //    descricao = Convert.ToString(o["descricao"])
        //}).ToList();

        //return tableGrupos;
    }

    public List<GrupoEconomico> list()
    {
        string sql = string.Concat("SELECT COD_GRUPO_ECONOMICO, COD_EMPRESA, DESCRICAO FROM CAD_GRUPOS_ECONOMICOS WHERE COD_GRUPO_ECONOMICO > 0 AND COD_EMPRESA = ", HttpContext.Current.Session["empresa"], " ORDER BY DESCRICAO");
        DataTable tableGrupos = _conn.dataTable(sql, "grupo_economico");

        List<GrupoEconomico> listaGrupos = tableGrupos.AsEnumerable().Select(o => new GrupoEconomico
        {
            codEmpresa = Convert.ToInt32(o["cod_empresa"]),
            codigoGrupoEconomico = Convert.ToInt32(o["cod_grupo_economico"]),
            descricao = Convert.ToString(o["descricao"])
        }).ToList();

        return listaGrupos;
    }

    public void insert(GrupoEconomico grupoEconomico)
    {
        string sql = string.Concat("INSERT INTO CAD_GRUPOS_ECONOMICOS (COD_EMPRESA, DESCRICAO) VALUES (", HttpContext.Current.Session["empresa"], ", '", grupoEconomico.descricao, "')");
        _conn.execute(sql);
    }

    public void update(GrupoEconomico grupoEconomico)
    {
        string sql = string.Concat("UPDATE CAD_GRUPOS_ECONOMICOS SET DESCRICAO = '", grupoEconomico.descricao, "' WHERE COD_GRUPO_ECONOMICO = ", grupoEconomico.codigoGrupoEconomico, " AND COD_EMPRESA = ", HttpContext.Current.Session["empresa"]);
        _conn.execute(sql);
    }

    public void delete(int codGrupoEconomico)
    {
        string sql = string.Concat("DELETE FROM CAD_GRUPOS_ECONOMICOS WHERE COD_GRUPO_ECONOMICO = ", codGrupoEconomico, " AND COD_EMPRESA = ", HttpContext.Current.Session["empresa"]);
        _conn.execute(sql);
    }

    public GrupoEconomico load(int codGrupoEconomico)
    {
        string sql = string.Concat("SELECT DESCRICAO FROM CAD_GRUPOS_ECONOMICOS WHERE COD_GRUPO_ECONOMICO = ", codGrupoEconomico, " AND COD_EMPRESA = ", HttpContext.Current.Session["empresa"]);
        DataTable resp = _conn.dataTable(sql, "grupo_economico");
        GrupoEconomico grupoEconomico = new GrupoEconomico(_conn);
        if(resp.Rows.Count > 0)
        {
            grupoEconomico = new GrupoEconomico(_conn)
            {
                codigoGrupoEconomico = codGrupoEconomico,
                codEmpresa = Convert.ToInt32(HttpContext.Current.Session["empresa"]),
                descricao = Convert.ToString(resp.Rows[0]["descricao"])
            };
        }

        return grupoEconomico;
    }

    public int totalRegistros(string descricao)
    {
        descricao = string.IsNullOrEmpty(descricao) ? descricao : string.Concat(" AND DESCRICAO LIKE '%", descricao, "%'");
        string sql = string.Concat("SELECT COUNT(COD_GRUPO_ECONOMICO) FROM CAD_GRUPOS_ECONOMICOS WHERE COD_EMPRESA = ", HttpContext.Current.Session["empresa"], descricao);

        return Convert.ToInt32(_conn.scalar(sql));
    }
}