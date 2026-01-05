using System.Data;

public class Emitente : Empresa
{
	public Emitente(Conexao c)
        :base(c)
	{
        _tipo = "EMITENTE";
	}

    public void lista_Emitentes(ref DataTable tb)
    {
        empresaDAO.lista_Emitentes(ref tb);
    }
}