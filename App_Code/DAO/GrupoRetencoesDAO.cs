//Projeto interrompido em 2018. Sem previsão de retomada.
//Menu (Banco de Dados): "CAD_MODULOS" e "CAD_TAREFAS" já possuem estrutura (FATURAMENTO_CADASTROS_GRUPO_RETENCOES).
//Engloba 5 Arquivos: "FormGrupoRetencoes.js", "FormGrupoRetencoes.aspx", "FormGrupoRetencoes.aspx.cs", "GrupoRetencao.cs" e "GrupoRetencoesDAO.cs".

using System;
using System.Data;
using System.Web;

public class GrupoRetencoesDAO
{
    private Conexao _conn;

	public GrupoRetencoesDAO(Conexao c)
	{
        _conn = c;
	}

    public void lista_Grupo_Retencoes(ref DataTable tb, int cod_emitente)
    {
        string sql = "SELECT COD_GRUPO_RETENCAO, NOME, ATIVO FROM CAD_GRUPO_RETENCOES";
        sql += " WHERE COD_EMITENTE = " + cod_emitente + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        sql += " ORDER BY COD_GRUPO_RETENCAO";
        _conn.fill(sql, ref tb);
    }

    public int novo(string nome, string aliquota, string apresentacao, int Cod_Retencoes_Sys)
    {
        string sql = "INSERT INTO CAD_RETENCOES(COD_EMPRESA, NOME, ALIQUOTA, APRESENTACAO, Cod_Retencoes_Sys) VALUES("
            + HttpContext.Current.Session["empresa"] + ", '" + nome.Replace("'", "''") + "', " + Convert.ToDouble(aliquota.Replace(".", ",")).ToString().Replace(",", ".") + ", '" + apresentacao.Replace("'", "''") + "', "+ Cod_Retencoes_Sys
            + "); SELECT SCOPE_IDENTITY()";

        return Convert.ToInt32(_conn.scalar(sql));
    }

    public void alterar(int cod_retencao, string nome, string aliquota, string apresentacao, int Cod_Retencoes_Sys)
    {
        string sql = "UPDATE CAD_RETENCOES SET NOME = '" + nome.Replace("'", "''") + "', ALIQUOTA = " + Convert.ToDouble(aliquota.Replace(".", ",")).ToString().Replace(",", ".") 
            + ", APRESENTACAO = '" + apresentacao.Replace("'", "''") + ", Cod_retencao_Sys = " + Cod_Retencoes_Sys
            + "' WHERE COD_RETENCAO = " + cod_retencao + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];

        _conn.execute(sql);
    }

    public void deletar(int cod_retencao)
    {
        string sql = "DELETE FROM CAD_RETENCOES WHERE COD_RETENCAO = " + cod_retencao + " AND COD_EMPRESA = " + HttpContext.Current.Session["empresa"];
        _conn.execute(sql);
    }

    public void insert_Emitentes_Selecionados(int cod_retencao, int cod_emitente)
    {
        string sql = "INSERT INTO CAD_RETENCOES_EMITENTE(COD_RETENCAO, COD_EMITENTE) VALUES(" + cod_retencao + ", " + cod_emitente + ")";
        _conn.execute(sql);
    }

    public void delete_Emitentes_Deselecionados(int cod_retencao, int cod_emitente)
    {
        string sql = "DELETE FROM CAD_RETENCOES_EMITENTE WHERE COD_RETENCAO = " + cod_retencao + " AND COD_EMITENTE = " + cod_emitente;
        _conn.execute(sql);
    }
}