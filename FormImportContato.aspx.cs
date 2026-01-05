using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class FormImportContato : BaseForm
{
   
    public FormImportContato()
        : base("IMPORTAR")
    {
       
    }
    
    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);
        if (!Page.IsPostBack)
        {
            //string mascara_cpf = "$(\"#" + dataTextBox.ClientID + "\").mask(\"99/99/9999\");";
            //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), mascara_cpf, true);

            jobsDAO job = new jobsDAO(_conn);
            DataTable table = new DataTable("table");
            job.lista(ref table, "DESCRICAO");
            DDLjob.DataSource = table;
            DDLjob.DataValueField = "COD_JOB";
            DDLjob.DataTextField = "DESCRICAO_COMPLETO";
            DDLjob.DataBind();
            DDLjob.Items.Insert(0, new ListItem("selecione", "0")); 

            contasDAO contas = new contasDAO(_conn);
            DataTable tb = new DataTable("tb");
            contas.lista(ref tb, 1);
            DDLcontadinheiro.DataSource = tb;
            DDLcontadinheiro.DataValueField = "COD_CONTA";
            DDLcontadinheiro.DataTextField = "DESCRICAO_COMPLETO";
            DDLcontadinheiro.DataBind();
            DDLcontadinheiro.Items.Insert(0, new ListItem("selecione", "0"));

            DDLcontacartao.DataSource = tb;
            DDLcontacartao.DataValueField = "COD_CONTA";
            DDLcontacartao.DataTextField = "DESCRICAO_COMPLETO";
            DDLcontacartao.DataBind();
            DDLcontacartao.Items.Insert(0, new ListItem("selecione", "0"));

            DDLcontareceita.DataSource = tb;
            DDLcontareceita.DataValueField = "COD_CONTA";
            DDLcontareceita.DataTextField = "DESCRICAO_COMPLETO";
            DDLcontareceita.DataBind();
            DDLcontareceita.Items.Insert(0, new ListItem("selecione", "0"));

            txterros.Visible = false;
        }
    }
 
    protected void Button1_Click(object sender, EventArgs e)
    {
        Import imp = new Import(_conn);
       
        string data_import = DateTime.Now.ToString("ddMMyyyyHmmss");
        string destino = Server.MapPath("Temp\\") +data_import+FileUpload1.FileName.ToString();
        FileUpload1.SaveAs(destino);
        imp.historico = txthistorico.Text; 
        imp.caminho = destino;
        imp.nomeplanilha = FileUpload1.FileName.ToString();
        imp.contachequeboleto = DDLcontadinheiro.SelectedValue.ToString();
        imp.contacartao = DDLcontacartao.SelectedValue.ToString();
        imp.contareceita = DDLcontareceita.SelectedValue.ToString();
        imp.job = Convert.ToInt32(DDLjob.SelectedValue.ToString());
        imp._descricaocontachequeboleto = DDLcontadinheiro.SelectedItem.Text.ToString();
        imp._descricaocontacartao = DDLcontacartao.SelectedItem.Text.ToString();
        imp._descricaocontareceita = DDLcontareceita.SelectedItem.Text.ToString();
        imp.date = DateTime.Now;

        List<string>erros =  imp.importar();

        if (erros.Count != 0)
        {
            txterros.Visible = true;
            string menssagem = "";
            for (int x = 0; x < erros.Count; x++)
            {
            menssagem += erros[x]+"\r\n";
            }
            txterros.Text = menssagem;
        }
        else
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('Todos contratos foram importados');", true);
       
        
    }
}