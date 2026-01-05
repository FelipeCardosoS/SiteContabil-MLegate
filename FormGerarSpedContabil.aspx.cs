using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sped;
using System.IO;

public partial class FormGerarSpedContabil : BaseForm
{
    public FormGerarSpedContabil()
        : base("SPED_CONTABIL") { }

    public Conexao c;

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(),
            "$('#" + textInicio.ClientID + "').unmask();" +
            "$('#" + textTermino.ClientID + "').unmask();" +
            "$('#" + textTermino.ClientID + "').mask('99/99/9999');" +
            "$('#" + textInicio.ClientID + "').mask('99/99/9999');", true);

        c = new Conexao();
        c.open();

        //atualizaBotao();

        if (!Page.IsPostBack)
        {
            subTitulo.Text = "Gerar Sped Contábil 8.0";
        }
    }

    protected void botaoGerar_Click(object sender, EventArgs e)
    {
        //try
        //{
        string Tipodemonstracao = "Anual";
        if (DTrime.Checked)
        {
            Tipodemonstracao = "Trimestral";
        }
        if (DMensal.Checked)
        {
            Tipodemonstracao = "Mensal";
        }
        
            Contabil cont = new Contabil(Convert.ToDateTime(textInicio.Text), Convert.ToDateTime(textTermino.Text), "G", 3, Convert.ToDateTime("01-01-2012"), Convert.ToInt32(HttpContext.Current.Session["empresa"]), c, Tipodemonstracao, checkLucroPresumido.Checked, checkDetCentroCusto.Checked);
            string nomeArquivo = "SPEEDS - " + DateTime.Now.ToString("dd - MM") + " - Nome " + HttpContext.Current.Session["nome_empresa"].ToString().Replace("/", "") + "_cor.txt";
            System.IO.File.Copy(HttpContext.Current.Request.PhysicalApplicationPath + "Speds/" + nomeArquivo, HttpContext.Current.Request.PhysicalApplicationPath + "Speds/" + nomeArquivo.Replace("_cor", ""), true);
            System.IO.File.Delete(HttpContext.Current.Request.PhysicalApplicationPath + "Speds/" + nomeArquivo);
            textoLiteral.Text = "<a href=\"Speds/" + nomeArquivo.Replace("_cor", "")+"\">DOWNLOAD ARQUIVO - SPED CONTÁBIL</a>";
        //}
        //catch (Exception ee)
        //{
        //    textoLiteral.Text = ee.Message;
        //}
    }
    //protected void btnFile_Click(object sender, EventArgs e)
    //{
    //    if (FileUpload.HasFile)
    //        try
    //        {
    //            if (verificaFormato(Path.GetExtension(FileUpload.FileName)))
    //            {
    //                FileUpload.SaveAs(HttpContext.Current.Server.MapPath("") + "\\Dre\\" + HttpContext.Current.Session["empresa"] + Path.GetExtension(FileUpload.FileName));
    //                ltUpload.Text = "<center class=\"msgSuc\"><b>Upload feito com sucesso!</b><br />Nome do Arquivo: " +
    //                    FileUpload.PostedFile.FileName +
    //                    "<br />Tamanho do Arquivo: " +
    //                    FileUpload.PostedFile.ContentLength +
    //                    "<br />Tipo de Arquivo: " +
    //                    FileUpload.PostedFile.ContentType + "</center>";}
    //            else
    //            {
    //                ltUpload.Text = "<center class=\"msgError\">Formato inválido do arquivo!<br /> Formatos permitidos: \".xlsx\", \".xlsm\" e \".xls\".<br />Formato do arquivo enviado: \"" + Path.GetExtension(FileUpload.FileName) + "\".</center>";
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            ltUpload.Text = "<center class=\"msgError\">Error: " + ex.Message.ToString() + "</center>";
    //        }
    //    else
    //    {
    //        ltUpload.Text = "<center class=\"msgError\">Selecione um arquivo para fazer o Upload!</center>";
    //    }

    //    atualizaBotao();
    //}

    //public bool verificaFormato(string formato) 
    //{
    //    switch (formato){
    //        case ".xlsx":
    //        {
    //            return true;
    //            break;
    //        }
    //        case ".xlsm":
    //        {
    //            return true;
    //            break;
    //        }
    //        case ".xls":
    //        {
    //            return true;
    //            break;
    //        }
    //        default :{ 
    //            return false; 
    //        }
    //    }
    //}

    //public void atualizaBotao() 
    //{
    //    if (verificaArquivo())
    //    {
    //        botaoGerar.Enabled = true;
    //        botaoGerar.Text = "Gerar SpedContábil";
    //        msgAlertExist.Visible = true;
    //    }
    //    else
    //    {
    //        botaoGerar.Enabled = false;
    //        botaoGerar.Text = "Faça o upload da planilha do DRE!";
    //        msgAlertExist.Visible = false;
    //    }
    //}

    //public bool verificaArquivo() 
    //{
    //    bool objeto = false;

    //    if (File.Exists(HttpContext.Current.Server.MapPath("") + "\\Dre\\" + HttpContext.Current.Session["empresa"] + ".xlsx"))
    //        objeto = true;

    //    if (File.Exists(HttpContext.Current.Server.MapPath("") + "\\Dre\\" + HttpContext.Current.Session["empresa"] + ".xlsm"))
    //        objeto = true;

    //    if (File.Exists(HttpContext.Current.Server.MapPath("") + "\\Dre\\" + HttpContext.Current.Session["empresa"] + ".xls"))
    //        objeto = true;

    //    return objeto;
    //}
}