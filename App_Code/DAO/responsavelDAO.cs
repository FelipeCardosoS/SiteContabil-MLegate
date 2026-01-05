using System;
using System.Data;

public class responsavelDAO
{
    private Conexao _conn;

    public responsavelDAO(Conexao conn)
	{
        _conn = conn;
	}

    public void insert(SResponsavel responsavel)
    {
        string sql = "INSERT INTO CAD_RESPONSAVEL (COD_EMPRESA, NOME, CPF, CRC, CNPJ_ESCRITORIO, CEP, ENDERECO, NUMERO, COMPLEMENTO, BAIRRO, TELEFONE, FAX, EMAIL, COD_MUNICIPIO, IDENT_QUALIF, COD_ASSIN, UF_CRC, NUM_SEQ_CRC, DT_CRC) " +
                     "VALUES (" + responsavel.codEmpresa + ", '" + responsavel.nome.Replace("'", "''") + "', '" + responsavel.cpf + "', '" + responsavel.crc.Replace("'", "''") + "', '" + responsavel.cnpjEscritorio + "', '" + responsavel.cep + "', " +
                     "'" + responsavel.endereco.Replace("'", "''") + "', '" + responsavel.numero.Replace("'", "''") + "', '" + responsavel.complemento.Replace("'", "''") + "', '" + responsavel.bairro.Replace("'", "''") + "', " +
                     "'" + responsavel.telefone + "', '" + responsavel.celular + "', '" + responsavel.email.Replace("'", "''") + "', '" + responsavel.codigoMunicipio + "', '" +
                     responsavel.ident_qualif.Replace("'", "''") + "', '" + responsavel.cod_assin.Replace("'", "''") + "', '" + responsavel.uf_crc + "', '" + responsavel.num_seq_crc.Replace("'", "''") + "', '" + responsavel.dt_crc.ToString("yyyyMMdd") + "')";

        _conn.execute(sql);
    }

    public void update(SResponsavel responsavel)
    {
        string sql = "UPDATE CAD_RESPONSAVEL SET NOME = '" + responsavel.nome.Replace("'", "''") + "', CPF = '" + responsavel.cpf + "', CRC = '" + responsavel.crc.Replace("'", "''") + "', CNPJ_ESCRITORIO = '" + responsavel.cnpjEscritorio + "', " +
                     "CEP = '" + responsavel.cep + "', ENDERECO = '" + responsavel.endereco.Replace("'", "''") + "', NUMERO = '" + responsavel.numero.Replace("'", "''") + "', COMPLEMENTO = '" + responsavel.complemento.Replace("'", "''") + "', " +
                     "BAIRRO = '" + responsavel.bairro.Replace("'", "''") + "', TELEFONE = '" + responsavel.telefone + "', FAX = '" + responsavel.celular + "', EMAIL = '" + responsavel.email.Replace("'", "''") + "', " +
                     "COD_MUNICIPIO = " + responsavel.codigoMunicipio + ", IDENT_QUALIF = '" + responsavel.ident_qualif.Replace("'", "''") + "', COD_ASSIN = '" + responsavel.cod_assin.Replace("'", "''") + "', " +
                     "UF_CRC = '" + responsavel.uf_crc + "', NUM_SEQ_CRC = '" + responsavel.num_seq_crc.Replace("'", "''") + "', DT_CRC = '" + responsavel.dt_crc.ToString("yyyyMMdd") + "' " +
                     "WHERE COD_EMPRESA = " + responsavel.codEmpresa;

        _conn.execute(sql);
    }

    public SResponsavel load(int codEmpresa)
    {
        string sql = "SELECT * FROM CAD_RESPONSAVEL WHERE COD_EMPRESA = " + codEmpresa;
        DataTable tb = _conn.dataTable(sql, "dados");
        
        SResponsavel SResponsavel = null;
        
        if (tb.Rows.Count > 0)
            SResponsavel = createObject(tb.Rows[0]);
        
        return SResponsavel;
    }

    private SResponsavel createObject(DataRow row)
    {
        SResponsavel SResponsavel = new SResponsavel();
        SResponsavel.nome = row["NOME"].ToString();
        SResponsavel.cpf = row["CPF"].ToString();
        SResponsavel.crc = row["CRC"].ToString();
        
        if (row["CNPJ_ESCRITORIO"] != DBNull.Value)
            SResponsavel.cnpjEscritorio = row["CNPJ_ESCRITORIO"].ToString();
        if (row["ENDERECO"] != DBNull.Value)
            SResponsavel.endereco = row["ENDERECO"].ToString();
        if (row["NUMERO"] != DBNull.Value)
            SResponsavel.numero = row["NUMERO"].ToString();
        if (row["COMPLEMENTO"] != DBNull.Value)
            SResponsavel.complemento = row["COMPLEMENTO"].ToString();
        if (row["CEP"] != DBNull.Value)
            SResponsavel.cep = row["CEP"].ToString();
        if (row["BAIRRO"] != DBNull.Value)
            SResponsavel.bairro = row["BAIRRO"].ToString();
        if (row["TELEFONE"] != DBNull.Value)
            SResponsavel.telefone = row["TELEFONE"].ToString();
        if (row["FAX"] != DBNull.Value)
            SResponsavel.celular = row["FAX"].ToString();
        if (row["EMAIL"] != DBNull.Value)
            SResponsavel.email = row["EMAIL"].ToString();
        if (row["IDENT_QUALIF"] != DBNull.Value)
            SResponsavel.ident_qualif = row["IDENT_QUALIF"].ToString();
        if (row["COD_ASSIN"] != DBNull.Value)
            SResponsavel.cod_assin = row["COD_ASSIN"].ToString();
        if (row["COD_MUNICIPIO"] != DBNull.Value)
            SResponsavel.codigoMunicipio = Convert.ToInt32(row["COD_MUNICIPIO"]);
        if (row["UF_CRC"] != DBNull.Value)
            SResponsavel.uf_crc = row["UF_CRC"].ToString();
        if (row["NUM_SEQ_CRC"] != DBNull.Value)
            SResponsavel.num_seq_crc = row["NUM_SEQ_CRC"].ToString();
        if (row["DT_CRC"] != DBNull.Value)
            SResponsavel.dt_crc = Convert.ToDateTime(row["DT_CRC"]);

        return SResponsavel;
    }
}