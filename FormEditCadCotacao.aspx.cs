using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormEditCadCotacao : BaseEditCadForm
{
    int codCotacao;

    string end;

    cotacaoDAO cotacaoDAO;
    moedaDAL moedaDAL;

    public FormEditCadCotacao()
        : base("COTACAO")
    {
        cotacaoDAO = new cotacaoDAO(_conn);
        moedaDAL = new moedaDAL(_conn);
    }

    protected override void Page_PreLoad(object sender, EventArgs e)
    {
        base.Page_PreLoad(sender, e);
        if (_cadastro)
        {
            _codigoTarefa = "CAD";
            Title += "Cadastro de Cotação";
        }
        else
        {
            _codigoTarefa = "ALT";
            Title += "Edição de Cotação";
        }
    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        if(!Page.IsPostBack)
        {
            comboMoeda.DataSource = moedaDAL.loadTotal();
            comboMoeda.DataTextField = "DESCRICAO";
            comboMoeda.DataValueField = "COD_MOEDA";
            comboMoeda.DataBind();
            comboMoeda.Items.Insert(0, new ListItem("Selecione..", "0"));

            comboMoedaBC.DataSource = comboMoeda.DataSource;
            comboMoedaBC.DataTextField = "DESCRICAO";
            comboMoedaBC.DataValueField = "COD_MOEDA";
            comboMoedaBC.DataBind();
            comboMoedaBC.Items.Insert(0, new ListItem("Selecione..", "0"));
        }

        if (_cadastro)
        {
            botaoSalvar.Text = "Cadastrar";
            subTitulo.Text = "Cadastro de Cotação";
        }
        else
        {
            botaoSalvar.Text = "Alterar";
            subTitulo.Text = "Alterar Cotação";
            if (!Page.IsPostBack)
            {
                codCotacao = Convert.ToInt32(Request.QueryString["id"]);
                SCotacao tipo = cotacaoDAO.load(codCotacao);
                if (tipo != null)
                {
                    comboMoeda.SelectedValue = tipo.codMoeda.ToString();
                    txtValor.Text = tipo.valor.ToString();
                    dtData.Text = tipo.data.ToString("dd/MM/yyyy");
                }
            }
        }
    }

    protected override void botaoSalvar_Click(object sender, EventArgs e)
    {
        base.botaoSalvar_Click(sender, e);

        if (comboMoeda.SelectedValue != "0" || txtValor.Text != "" || dtData.Text != "")
        {
            if (_cadastro)
            {
                if (cotacaoDAO.novo(Convert.ToInt32(comboMoeda.SelectedValue), txtValor.Text, Convert.ToDateTime(dtData.Text)))
                {
                    Response.Redirect("FormGridCotacao.aspx");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Já existe essa Cotação!');", true);
                }
            }
            else
            {
                codCotacao = 0;
                int.TryParse(Request.QueryString["id"], out codCotacao);

                if (cotacaoDAO.editar(codCotacao, Convert.ToInt32(comboMoeda.SelectedValue), txtValor.Text, Convert.ToDateTime(dtData.Text)))
                {
                    Response.Redirect("FormGridCotacao.aspx");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Já existe essa Cotação!');", true);
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Preencha todos os campos!');", true);
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (UploadArquivo())
        {
            //Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            //Workbook xlWorkbook = xlApp.Workbooks.Open(end, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            //_Worksheet xlWorksheet = (Worksheet)xlWorkbook.Worksheets.get_Item(1);
            //Range xlRange = xlWorksheet.UsedRange;

            //int rowCount = xlRange.Rows.Count;
            try
            {
                List<CotacaoItem> listCotacaoItem = new List<CotacaoItem>();
                string line = "";
                StreamReader rds = new StreamReader(end);

                while ((line = rds.ReadLine()) != null)
                {
                    string[] split = line.Split(';');
                    CotacaoItem item = new CotacaoItem();
                    item.codMoeda = Convert.ToInt32(comboMoedaBC.SelectedValue);
                    string Data = (split[0].Length == 7 ? "0" : "") + (split[0]);
                    item.data = DateTime.ParseExact(Data.Substring(4, 4) + Data.Substring(2, 2) + Data.Substring(0, 2) + " 00:00:00,000", "yyyyMMdd 00:00:00,000", System.Globalization.CultureInfo.InvariantCulture);
                    item.valorMoeda = Convert.ToDecimal(split[5]);
                    listCotacaoItem.Add(item);
                }

                cotacaoDAO.deletePeriodo(listCotacaoItem[0].data, listCotacaoItem[(listCotacaoItem.Count - 1)].data, Convert.ToInt32(comboMoedaBC.SelectedValue));

                foreach (CotacaoItem item in listCotacaoItem)
                {
                    cotacaoDAO.novo(item.codMoeda, item.valorMoeda.ToString("F5"), item.data);
                }

                Response.Redirect("FormGridCotacao.aspx");
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Erro ao importar o arquivo!');", true);
            }
        }
        else 
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alertaErro", "alert('Erro! Verifique a extensão do arquivo!');", true);
        }
    }

    public bool UploadArquivo() 
    {
        end = HttpContext.Current.Server.MapPath("Temp\\") + DateTime.Now.ToString("yyyyMMdd") + fileBC.FileName;
        
        try 
        {
            if (fileBC.FileName.IndexOf(".csv") == -1) return false;
            
            if (File.Exists(end)) File.Delete(end);

            fileBC.SaveAs(end);
        }
        catch (Exception we)
        {
            throw new Exception(we.Message);
        }

        return true;
    }
}