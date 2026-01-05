using System;
using System.Web.UI;

public partial class FormDadosResponsavel : BaseForm
{
    public FormDadosResponsavel()
        : base("RESPONSAVEL") 
    {

    }

    protected override void Page_Load(object sender, EventArgs e)
    {
        base.Page_Load(sender, e);

        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(),
            "$('#" + textCpf.ClientID + "').unmask();" +
            "$('#" + textCnpjEscritorio.ClientID + "').unmask();" +
            "$('#" + textCep.ClientID + "').unmask();" +
            "$('#" + textTelefone.ClientID + "').unmask();" +
            "$('#" + textCelular.ClientID + "').unmask();" +
            "$('#" + textCpf.ClientID + "').mask('999.999.999-99');" +
            "$('#" + textCnpjEscritorio.ClientID + "').mask('99.999.999/9999-99');" +
            "$('#" + textCep.ClientID + "').mask('99999-999');" +
            "$('#" + textTelefone.ClientID + "').mask('(99) 9999-9999');" +
            "$('#" + textCelular.ClientID + "').mask('(99) 99999-9999');" +
            "$('#" + dtValidadeCrc.ClientID + "').mask('99/99/9999');", true);

        if (!Page.IsPostBack)
            subTitulo.Text = "Dados do Responsável";

        if (!Page.IsPostBack)
        {
            responsavelDAO responsavelDAO = new responsavelDAO(_conn);
            SResponsavel responsavel = responsavelDAO.load(SessionView.EmpresaSession);
            
            if (responsavel != null)
            {
                textNome.Text = responsavel.nome;
                textCpf.Text = responsavel.cpf;
                textCrc.Text = responsavel.crc;
                textCnpjEscritorio.Text = responsavel.cnpjEscritorio;
                textEndereco.Text = responsavel.endereco;
                textNumero.Text = responsavel.numero;
                textComplemento.Text = responsavel.complemento;
                textCep.Text = responsavel.cep;
                textBairro.Text = responsavel.bairro;
                textTelefone.Text = responsavel.telefone;
                textCelular.Text = responsavel.celular;
                textEmail.Text = responsavel.email;
                textIdentQualif.Text = responsavel.ident_qualif;
                textCodAssi.Text = responsavel.cod_assin;

                if (responsavel.codigoMunicipio != 0)
                    textCodigoMunicipio.Text = responsavel.codigoMunicipio.ToString();

                textUfCrc.Text = responsavel.uf_crc;
                textUFSequencial.Text = responsavel.num_seq_crc;
                dtValidadeCrc.Text = (responsavel.dt_crc.ToString("dd/MM/yyyy") == "01/01/0001" ? DateTime.Now.ToString("dd/MM/yyyy") : responsavel.dt_crc.ToString("dd/MM/yyyy"));
            }
        }
    }

    protected void botaoSalvar_Click(object sender, EventArgs e)
    {
        responsavelDAO responsavelDAO = new responsavelDAO(_conn);
        SResponsavel responsavel = responsavelDAO.load(SessionView.EmpresaSession);
        bool cadastrar = false;

        if (responsavel == null)
        {
            cadastrar = true;
            responsavel = new SResponsavel();
        }

        responsavel.codEmpresa = SessionView.EmpresaSession;
        responsavel.nome = textNome.Text;
        responsavel.cpf = textCpf.Text;
        responsavel.crc = textCrc.Text;
        responsavel.cnpjEscritorio = textCnpjEscritorio.Text;
        responsavel.endereco = textEndereco.Text;
        responsavel.numero = textNumero.Text;
        responsavel.complemento = textComplemento.Text;
        responsavel.cep = textCep.Text;
        responsavel.bairro = textBairro.Text;
        responsavel.telefone = textTelefone.Text;
        responsavel.celular = textCelular.Text;
        responsavel.email = textEmail.Text;
        responsavel.ident_qualif = textIdentQualif.Text;
        responsavel.cod_assin = textCodAssi.Text;

        int codMunicipio;
        int.TryParse(textCodigoMunicipio.Text, out codMunicipio);
        responsavel.codigoMunicipio = codMunicipio;

        responsavel.uf_crc = textUfCrc.Text;
        responsavel.num_seq_crc = textUFSequencial.Text;

        if (!string.IsNullOrEmpty(dtValidadeCrc.Text))
            responsavel.dt_crc  = Convert.ToDateTime(dtValidadeCrc.Text);
        else
            responsavel.dt_crc = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));

        if (cadastrar)
            responsavelDAO.insert(responsavel);
        else
            responsavelDAO.update(responsavel);

        Page.Response.Redirect(Page.Request.Url.ToString(), true); //Recarrega a página.
    }
}