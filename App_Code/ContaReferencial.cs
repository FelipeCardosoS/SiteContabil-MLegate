using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

/// <summary>
/// Summary description for ContaReferencial
/// </summary>
public class ContaReferencial
{
	public ContaReferencial()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public void importar(string arquivo, char separador, Conexao conn)
    {
        contasRefDAO contaRefDAO = new contasRefDAO(conn);
        FileInfo info = new FileInfo(arquivo);
        if (!info.Exists)
            throw new ApplicationException("Arquivo não encontrado");

        using (StreamReader leitor = new StreamReader(arquivo,System.Text.Encoding.Default))
        {
            while (leitor.Peek() != -1)
            {
                string linha = leitor.ReadLine();
                string[] arr = linha.Split(separador);
                if (arr.Length > 1)
                {
                    string codigo = arr[0];
                    string descricao = arr[1];
                    string dataIni = arrumaData(arr[2]);
                    string dataFim = arrumaData(arr[3]);
                    DateTime? iniValidade = (dataIni == "" ? null : (DateTime?)Convert.ToDateTime(dataIni));
                    DateTime? fimValidade = (dataFim == "" ? null : (DateTime?)Convert.ToDateTime(dataFim));
                    string analiticaSintetica = arr[4];
                    bool inserir = true;
                    if (fimValidade.HasValue)
                    {
                        if (fimValidade.Value <= DateTime.Now)
                        {
                            inserir = false;
                        }
                    }
                    if(inserir)
                        contaRefDAO.insert(codigo, descricao, iniValidade, fimValidade, analiticaSintetica);
                }
            }
        }
    }

    private string arrumaData(string valor)
    {
        string retorno = "";
        if (valor.Length == 8)
        {
            retorno = valor.Substring(0, 2) + "/" + valor.Substring(2, 2) + "/" + valor.Substring(4, 4);
        }
        return retorno;
    }
}