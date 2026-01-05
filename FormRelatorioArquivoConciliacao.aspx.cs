using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Security;
using System.Web;
using System.Web.UI;

public partial class FormRelatorioArquivoConciliacao : BaseForm
{
    private readonly EmissaoNF_Funcoes EmissaoNF_Funcoes;
    private readonly ArquivoConciliacaoDAO ArquivoConciliacaoDAO;

    public FormRelatorioArquivoConciliacao()
        : base("DEFAULT")
    {
        EmissaoNF_Funcoes = new EmissaoNF_Funcoes(_conn);
        ArquivoConciliacaoDAO = new ArquivoConciliacaoDAO(_conn);
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        subTitulo.Text = "Arquivo para Conciliação";
    }

    protected void btnGerar_Click(object sender, EventArgs e)
    {
        //Validação do Período: Data Inicial e Data Final.
        List<string> Mensagem_Sucesso_Erro = new List<string>();
        string Mensagem_Erro;

        if (string.IsNullOrEmpty(dateDe.Value) || string.IsNullOrEmpty(dateAte.Value))
            Mensagem_Sucesso_Erro.Add("O preenchimento do período é obrigatório.");
        else
        {
            Mensagem_Erro = EmissaoNF_Funcoes.Verifica_Data(Convert.ToDateTime(dateDe.Value).ToString("dd/MM/yyyy"), "Período Inicial");
            if (Mensagem_Erro != "") //Se o Período Inicial estiver no formato errado, será diferente de "vazio"
                Mensagem_Sucesso_Erro.Add(Mensagem_Erro); //Mensagem de erro atribuída pela função "Verifica_Data"

            Mensagem_Erro = EmissaoNF_Funcoes.Verifica_Data(Convert.ToDateTime(dateAte.Value).ToString("dd/MM/yyyy"), "Período Final");
            if (Mensagem_Erro != "") //Se o Período Final estiver no formato errado, será diferente de "vazio"
                Mensagem_Sucesso_Erro.Add(Mensagem_Erro); //Mensagem de erro atribuída pela função "Verifica_Data"
        }

        if (Mensagem_Sucesso_Erro.Count == 0 && Convert.ToDateTime(dateDe.Value) > Convert.ToDateTime(dateAte.Value))
            Mensagem_Sucesso_Erro.Add("Informe uma Data Final igual ou posterior a Data Inicial.");

        if (Mensagem_Sucesso_Erro.Count >= 1)
            errosFormulario(Mensagem_Sucesso_Erro);
        else //Validação OK
        {
            try
            {
                DataTable dtArquivoConciliacao = ArquivoConciliacaoDAO.ArquivoConciliacao(Convert.ToDateTime(dateDe.Value).ToString("yyyyMMdd"), Convert.ToDateTime(dateAte.Value).ToString("yyyyMMdd"));
                ExportToCSVFile(dtArquivoConciliacao);

                ScriptManager.RegisterStartupScript(this, GetType(), "Sistema", "alert('Operação realizada com sucesso.');", true);
            }
            catch (SecurityException ex)
            {
                Mensagem_Sucesso_Erro.Add("O usuário não possui permissão para executar esta tarefa!" + "\\n\\n" +
                                          "Não foi possível gerar o arquivo." + "\\n\\n" +
                                          "Solicite suporte à equipe de TI." + "\\n\\n" +
                                          "Mensagem: " + ex.Message);
                errosFormulario(Mensagem_Sucesso_Erro);
            }
            catch (Exception ex)
            {
                Mensagem_Sucesso_Erro.Add("!!! Erro Inesperado !!!" + "\\n\\n" +
                                          "Não foi possível gerar o arquivo." + "\\n\\n" +
                                          "Solicite suporte à equipe de TI." + "\\n\\n" +
                                          "Mensagem: " + ex.Message);
                errosFormulario(Mensagem_Sucesso_Erro);
            }
            finally
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Sistema", "alert('Operação realizada com sucesso.');", true);
            }
        }
    }

    //Caso ocorra erro de compilação neste método, reinstale o "CsvHelper (Josh Close)" em "NuGet Packages".
    public void ExportToCSVFile(DataTable dtArquivoConciliacao)
    {
        string FileName = "Arquivo para Conciliação.csv";
        string FilePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Temp\" + FileName;

        using (var textWriter = File.CreateText(FilePath))
        using (var csv = new CsvWriter(textWriter, CultureInfo.InvariantCulture))
        {
            csv.Configuration.Delimiter = ";";

            //Write Columns
            foreach (DataColumn column in dtArquivoConciliacao.Columns)
                csv.WriteField(column.ColumnName);

            csv.NextRecord();

            //Write Row Values
            foreach (DataRow row in dtArquivoConciliacao.Rows)
            {
                for (var i = 0; i < dtArquivoConciliacao.Columns.Count; i++)
                    csv.WriteField(row[i]);

                csv.NextRecord();
            }
        }

        HttpResponse Response = HttpContext.Current.Response;
        Response.ClearContent();
        Response.Clear();
        Response.ContentType = "text/plain";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
        Response.TransmitFile(FilePath);
        Response.Flush();
        Response.End(); //End() é necessário, pois o ASP.NET envia mais dados antes de enviar o Response. 
                        //End() força que nada seja adicionado e que o Response seja enviado da forma que está.
                        //Sem End(), o arquivo é corrompido com o HTML da página atual. (Usar Application.CompleteRequest() também corrompe o arquivo)
    }
}