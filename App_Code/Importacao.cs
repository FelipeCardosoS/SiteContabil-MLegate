using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.IO;


public class Importacao
{
    private ETipoImportacao _tipo;
    private ContaContabil _conta;
    private GrupoContabil _grupoContabil;

    private List<string> erros;
    private List<string> mimes = new List<string>();

    private string _caminho = "Temp/";

	public Importacao(Conexao c, ETipoImportacao tipo)
	{
        _conta = new ContaContabil(c);
        _grupoContabil = new GrupoContabil(c);
        _tipo = tipo;

        mimes.Add("application/excel");
        mimes.Add("application/vnd.ms-excel");
        mimes.Add("application/x-excel");
        mimes.Add("application/x-msexcel");
	}

    private bool verificaFormato(string formato)
    {
        bool existe = false;

        for (int i = 0; i < mimes.Count; i++)
        {
            if (mimes[i] == formato)
            {
                existe = true;
                break;
            }
        }

        return existe;
    }

    public List<string> iniciar(HttpPostedFile arq)
    {
        erros = new List<string>();

        if (_tipo == ETipoImportacao.CONTA_CONTABIL)
        {
            if (arq.ContentLength > 0)
            {
                if (verificaFormato(arq.ContentType))
                {
                    string newName = "";
                    Microsoft.Office.Interop.Excel.Application appExcel = null;
                    Microsoft.Office.Interop.Excel.Workbook excelWb = null;

                    string codigo = "";
                    string descricao = "";
                    string tipo = "";
                    string tempTipo = "";
                    string sintetica = "";
                    string grupo = "";
                    string tempGrupo = "";
                    string natureza = "";
                    string tempNatureza = "";

                    int linha = 1;

                    try
                    {
                        newName = DateTime.Now.ToString("yyyyMMddHmmss") + "." + arq.FileName.Split('.')[1];
                        arq.SaveAs(HttpContext.Current.Server.MapPath(_caminho + "/" + newName));

                        object oMissing = System.Reflection.Missing.Value;
                        appExcel = new Microsoft.Office.Interop.Excel.Application();
                        Microsoft.Office.Interop.Excel.Sheets excelSS;
                        Microsoft.Office.Interop.Excel.Worksheet excelWs;
                        excelWb = appExcel.Workbooks.Open(HttpContext.Current.Server.MapPath(_caminho + "/" + newName), 0, true, 5, "",
                            "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, null, null);
                        excelSS = excelWb.Worksheets;
                        excelWs = (Microsoft.Office.Interop.Excel.Worksheet)excelSS.get_Item(1);

                        bool faz = true;
                        while (faz)
                        {
                            if (linha == 1)
                            {
                                if (excelWs.get_Range(excelWs.Cells[1, 1], excelWs.Cells[1, 1]).Text.ToString().Trim() != "Codigo")
                                    erros.Add("Código não existe ou está em lugar incorreto no cabeçalho.");

                                if (excelWs.get_Range(excelWs.Cells[1, 2], excelWs.Cells[1, 2]).Text.ToString().Trim() != "Descricao")
                                    erros.Add("Descrição não existe ou está em lugar incorreto no cabeçalho.");

                                if (excelWs.get_Range(excelWs.Cells[1, 3], excelWs.Cells[1, 3]).Text.ToString().Trim() != "Tipo")
                                    erros.Add("Tipo não existe ou está em lugar incorreto no cabeçalho.");

                                if (excelWs.get_Range(excelWs.Cells[1, 4], excelWs.Cells[1, 4]).Text.ToString().Trim() != "Sintetica")
                                    erros.Add("Sintética não existe ou está em lugar incorreto no cabeçalho.");

                                if (excelWs.get_Range(excelWs.Cells[1, 5], excelWs.Cells[1, 5]).Text.ToString().Trim() != "Grupo")
                                    erros.Add("Grupo não existe ou está em lugar incorreto no cabeçalho.");

                                if (excelWs.get_Range(excelWs.Cells[1, 6], excelWs.Cells[1, 6]).Text.ToString().Trim() != "Natureza")
                                    erros.Add("Natureza não existe ou está em lugar incorreto no cabeçalho.");

                                if (erros.Count > 0)
                                {
                                    faz = false;
                                    break;
                                }
                            }
                            else
                            {
                                codigo = excelWs.get_Range(excelWs.Cells[linha, 1], excelWs.Cells[linha, 1]).Text.ToString();
                                descricao = excelWs.get_Range(excelWs.Cells[linha, 2], excelWs.Cells[linha, 2]).Text.ToString();
                                tipo = excelWs.get_Range(excelWs.Cells[linha, 3], excelWs.Cells[linha, 3]).Text.ToString();
                                sintetica = excelWs.get_Range(excelWs.Cells[linha, 4], excelWs.Cells[linha, 4]).Text.ToString();
                                grupo = excelWs.get_Range(excelWs.Cells[linha, 5], excelWs.Cells[linha, 5]).Text.ToString();
                                natureza = excelWs.get_Range(excelWs.Cells[linha, 6], excelWs.Cells[linha, 6]).Text.ToString();

                                if(codigo == "")
                                {
                                    faz = false;
                                    break;
                                }

                                if (tipo == "Sintetica")
                                    tempTipo = "0";
                                else
                                    tempTipo = "1";

                                if (natureza == "Devedora")
                                    tempNatureza = "D";
                                else
                                    tempNatureza = "C";

                                tempGrupo = _grupoContabil.getCodigo(grupo).ToString();

                                if (!_conta.existe(codigo))
                                {
                                    _conta.codigo = codigo;
                                    _conta.descricao = descricao;
                                    _conta.analitica = Convert.ToInt32(tempTipo);
                                    _conta.contaSintetica = sintetica;
                                    _conta.grupoContabil = Convert.ToInt32(tempGrupo);
                                    _conta.debitoCredito = Convert.ToChar(tempNatureza);
                                    _conta.tipo = "C";
                                    _conta.novo();
                                }
                                else
                                {
                                    _conta.codigo = codigo;
                                    _conta.load();

                                    if ((_conta.descricao != descricao
                                        || _conta.analitica != Convert.ToInt32(tempTipo)
                                        || _conta.contaSintetica != sintetica
                                        || _conta.grupoContabil != Convert.ToInt32(tempGrupo)
                                        || _conta.debitoCredito != Convert.ToChar(tempNatureza)))
                                    {
                                        _conta.descricao = descricao;
                                        _conta.analitica = Convert.ToInt32(tempTipo);
                                        _conta.contaSintetica = sintetica;
                                        _conta.grupoContabil = Convert.ToInt32(tempGrupo);
                                        _conta.debitoCredito = Convert.ToChar(tempNatureza);
                                        _conta.alterar();
                                    }
                                }
                            }

                            linha++;
                        }

                        if (appExcel != null)
                        {
                            if (excelWb != null)
                            {
                                excelWb.Close(null, null, null);
                            }
                            appExcel.Workbooks.Close();
                            appExcel.Quit();
                        }

                        FileInfo arqErro = new FileInfo(HttpContext.Current.Server.MapPath(_caminho + "/" + newName));
                        if (arqErro.Exists)
                            arqErro.Delete();

                    }
                    catch
                    {
                        if (appExcel != null)
                        {
                            if (excelWb != null)
                            {
                                excelWb.Close(null, null, null);
                            }
                            appExcel.Workbooks.Close();
                            appExcel.Quit();
                        }

                        FileInfo arqErro = new FileInfo(HttpContext.Current.Server.MapPath(_caminho + "/" + newName));
                        if (arqErro.Exists)
                            arqErro.Delete();
                    }
                }
                else
                {
                    erros.Add("Formato do arquivo é inválido.");
                }
            }
            else
            {
                erros.Add("Arquivo não possue conteúdo.");
            }
        }

        return erros;
    }
}
