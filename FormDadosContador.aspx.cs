using System;
using System.Web.UI;

public partial class FormDadosContador : BaseForm
{
    public FormDadosContador()
        : base("CONTADOR") 
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
            subTitulo.Text = "Dados do Contador";

        if (!Page.IsPostBack)
        {
            contadorDAO contadorDAO = new contadorDAO(_conn);
            SContador contador = contadorDAO.load(SessionView.EmpresaSession);
            
            if (contador != null)
            {
                textNome.Text = contador.nome;
                textCpf.Text = contador.cpf;
                textCrc.Text = contador.crc;
                textCnpjEscritorio.Text = contador.cnpjEscritorio;
                textEndereco.Text = contador.endereco;
                textNumero.Text = contador.numero;
                textComplemento.Text = contador.complemento;
                textCep.Text = contador.cep;
                textBairro.Text = contador.bairro;
                textTelefone.Text = contador.telefone;
                textCelular.Text = contador.celular;
                textEmail.Text = contador.email;
                textIdentQualif.Text = contador.ident_qualif;
                textCodAssi.Text = contador.cod_assin;

                if (contador.codigoMunicipio != 0)
                    textCodigoMunicipio.Text = contador.codigoMunicipio.ToString();
                
                textUfCrc.Text = contador.uf_crc;
                textUFSequencial.Text = contador.num_seq_crc;
                dtValidadeCrc.Text = (contador.dt_crc.ToString("dd/MM/yyyy") == "01/01/0001" ? DateTime.Now.ToString("dd/MM/yyyy") : contador.dt_crc.ToString("dd/MM/yyyy"));
            }
        }
    }

    protected void botaoSalvar_Click(object sender, EventArgs e)
    {
        contadorDAO contadorDAO = new contadorDAO(_conn);
        SContador contador = contadorDAO.load(SessionView.EmpresaSession);
        bool cadastrar = false;

        if (contador == null)
        {
            cadastrar = true;
            contador = new SContador();
        }

        contador.codEmpresa = SessionView.EmpresaSession;
        contador.nome = textNome.Text;
        contador.cpf = textCpf.Text;
        contador.crc = textCrc.Text;
        contador.cnpjEscritorio = textCnpjEscritorio.Text;
        contador.endereco = textEndereco.Text;
        contador.numero = textNumero.Text;
        contador.complemento = textComplemento.Text;
        contador.cep = textCep.Text;
        contador.bairro = textBairro.Text;
        contador.telefone = textTelefone.Text;
        contador.celular = textCelular.Text;
        contador.email = textEmail.Text;
        contador.ident_qualif = textIdentQualif.Text;
        contador.cod_assin = textCodAssi.Text;
        
        int codMunicipio;
        int.TryParse(textCodigoMunicipio.Text, out codMunicipio);
        contador.codigoMunicipio = codMunicipio;
        
        contador.uf_crc = textUfCrc.Text;
        contador.num_seq_crc = textUFSequencial.Text;
        
        if (!string.IsNullOrEmpty(dtValidadeCrc.Text))
            contador.dt_crc = Convert.ToDateTime(dtValidadeCrc.Text);
        else
            contador.dt_crc = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy"));
        
        if (cadastrar)
            contadorDAO.insert(contador);
        else
            contadorDAO.update(contador);

        Page.Response.Redirect(Page.Request.Url.ToString(), true); //Recarrega a página.
    }
}