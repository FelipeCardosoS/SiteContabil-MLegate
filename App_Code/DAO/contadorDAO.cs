using System;
using System.Data;

public class contadorDAO
{
    private Conexao _conn;

	public contadorDAO(Conexao conn)
	{
        _conn = conn;
	}

    public void insert(SContador contador)
    {
        string sql = "INSERT INTO CAD_CONTADOR (COD_EMPRESA, NOME, CPF, CRC, CNPJ_ESCRITORIO, CEP, ENDERECO, NUMERO, COMPLEMENTO, BAIRRO, TELEFONE, FAX, EMAIL, COD_MUNICIPIO, IDENT_QUALIF, COD_ASSIN, UF_CRC, NUM_SEQ_CRC, DT_CRC) " +
                     "VALUES (" + contador.codEmpresa + ", '" + contador.nome.Replace("'", "''") + "', '" + contador.cpf + "', '" + contador.crc.Replace("'", "''") + "', '" + contador.cnpjEscritorio + "', '" + contador.cep + "', " +
                     "'" + contador.endereco.Replace("'", "''") + "', '" + contador.numero.Replace("'", "''") + "', '" + contador.complemento.Replace("'", "''") + "', '" + contador.bairro.Replace("'", "''") + "', " +
                     "'" + contador.telefone + "', '" + contador.celular + "', '" + contador.email.Replace("'", "''") + "', '" + contador.codigoMunicipio + "', '" +
                     contador.ident_qualif.Replace("'", "''") + "', '" + contador.cod_assin.Replace("'", "''") + "', '" + contador.uf_crc + "', '" + contador.num_seq_crc.Replace("'", "''") + "', '" + contador.dt_crc.ToString("yyyyMMdd") + "')";
        
        _conn.execute(sql);
    }

    public void update(SContador contador)
    {
        string sql = "UPDATE CAD_CONTADOR SET NOME = '" + contador.nome.Replace("'", "''") + "', CPF = '" + contador.cpf + "', CRC = '" + contador.crc.Replace("'", "''") + "', CNPJ_ESCRITORIO = '" + contador.cnpjEscritorio + "', " +
                     "CEP = '" + contador.cep + "', ENDERECO = '" + contador.endereco.Replace("'", "''") + "', NUMERO = '" + contador.numero.Replace("'", "''") + "', COMPLEMENTO = '" + contador.complemento.Replace("'", "''") + "', " +
                     "BAIRRO = '" + contador.bairro.Replace("'", "''") + "', TELEFONE = '" + contador.telefone + "', FAX = '" + contador.celular + "', EMAIL = '" + contador.email.Replace("'", "''") + "', " +
                     "COD_MUNICIPIO = " + contador.codigoMunicipio + ", IDENT_QUALIF = '" + contador.ident_qualif.Replace("'", "''") + "', COD_ASSIN = '" + contador.cod_assin.Replace("'", "''") + "', " +
                     "UF_CRC = '" + contador.uf_crc + "', NUM_SEQ_CRC = '" + contador.num_seq_crc.Replace("'", "''") + "', DT_CRC = '" + contador.dt_crc.ToString("yyyyMMdd") + "' " +
                     "WHERE COD_EMPRESA = " + contador.codEmpresa;

        _conn.execute(sql);
    }

    public SContador load(int codEmpresa)
    {
        string sql = "SELECT * FROM CAD_CONTADOR WHERE COD_EMPRESA = " + codEmpresa;
        DataTable tb = _conn.dataTable(sql, "dados");
        
        SContador SContador = null;
        
        if (tb.Rows.Count > 0)
            SContador = createObject(tb.Rows[0]);
        
        return SContador;
    }

    private SContador createObject(DataRow row)
    {
        SContador SContador = new SContador();
        SContador.nome = row["NOME"].ToString();
        SContador.cpf = row["CPF"].ToString();
        SContador.crc = row["CRC"].ToString();
        
        if (row["CNPJ_ESCRITORIO"] != DBNull.Value)
            SContador.cnpjEscritorio = row["CNPJ_ESCRITORIO"].ToString();
        if (row["ENDERECO"] != DBNull.Value)
            SContador.endereco = row["ENDERECO"].ToString();
        if (row["NUMERO"] != DBNull.Value)
            SContador.numero = row["NUMERO"].ToString();
        if (row["COMPLEMENTO"] != DBNull.Value)
            SContador.complemento = row["COMPLEMENTO"].ToString();
        if (row["CEP"] != DBNull.Value)
            SContador.cep = row["CEP"].ToString();
        if (row["BAIRRO"] != DBNull.Value)
            SContador.bairro = row["BAIRRO"].ToString();
        if (row["TELEFONE"] != DBNull.Value)
            SContador.telefone = row["TELEFONE"].ToString();
        if (row["FAX"] != DBNull.Value)
            SContador.celular = row["FAX"].ToString();
        if (row["EMAIL"] != DBNull.Value)
            SContador.email = row["EMAIL"].ToString();
        if (row["IDENT_QUALIF"] != DBNull.Value)
            SContador.ident_qualif = row["IDENT_QUALIF"].ToString();
        if (row["COD_ASSIN"] != DBNull.Value)
            SContador.cod_assin = row["COD_ASSIN"].ToString();
        if (row["COD_MUNICIPIO"] != DBNull.Value)
            SContador.codigoMunicipio = Convert.ToInt32(row["COD_MUNICIPIO"]);
        if (row["UF_CRC"] != DBNull.Value)
            SContador.uf_crc = row["UF_CRC"].ToString();
        if (row["NUM_SEQ_CRC"] != DBNull.Value)
            SContador.num_seq_crc = row["NUM_SEQ_CRC"].ToString();
        if (row["DT_CRC"] != DBNull.Value)
            SContador.dt_crc = Convert.ToDateTime(row["DT_CRC"]);

        return SContador;
    }
}