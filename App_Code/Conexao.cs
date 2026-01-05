using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public class Conexao
{
    private SqlConnection _conn;

    public Conexao()
    {
        if (_conn == null)
            _conn = new SqlConnection((string)ConfigurationManager.ConnectionStrings["strConexao"].ConnectionString);
    }

    public Conexao(string strConn)
    {
        if (_conn == null)
            _conn = new SqlConnection(strConn);
    }

    public void open()
    {
        if (_conn.State == ConnectionState.Closed)
        {
            _conn.Open();

            using (SqlCommand cmd = new SqlCommand("SET DATEFORMAT dmy", _conn))
                cmd.ExecuteNonQuery();
        }
    }

    public void close()
    {
        if (_conn.State == ConnectionState.Open)
            _conn.Close();
    }

    public virtual void execute(string sql)
    {
        open();

        using (SqlCommand cmd = new SqlCommand(sql, _conn))
        {
            cmd.CommandTimeout = 5;    
            cmd.ExecuteNonQuery();
        }

        close();
    }

    public virtual int executeReturnRows(string sql)
    {
        int rows = 0;
        open();

        using (SqlCommand cmd = new SqlCommand(sql, _conn))
        {
            cmd.CommandTimeout = 0;
            rows = cmd.ExecuteNonQuery();
        }

        close();
        return rows;
    }

    public virtual object scalar(string sql)
    {
        open();

        using (SqlCommand cmd = new SqlCommand(sql, _conn))
        {
            cmd.CommandTimeout = 0;
            object result = null;
            result = cmd.ExecuteScalar();
            close();
            return result;
        }
    }

    public virtual DataTable dataTable(string sql, string nome)
    {
        open();

        using (SqlCommand cmd = new SqlCommand(sql, _conn))
        {
            cmd.CommandTimeout = 0;

            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataTable tb = new DataTable(nome);

                try
                {
                    adapter.SelectCommand.CommandTimeout = 0;
                    adapter.Fill(tb);
                }
                catch (Exception ex)
                {
                    System.Web.HttpContext.Current.Response.Write(ex.Message);
                }
                finally
                {
                    close();
                }
                return tb;
            }
        }
    }

    public virtual DataSet dataSet(string sql)
    {
        open();

        using (SqlCommand cmd = new SqlCommand(sql, _conn))
        {
            cmd.CommandTimeout = 0;

            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();

                try
                {
                    adapter.SelectCommand.CommandTimeout = 0;
                    adapter.Fill(ds);
                }
                catch (Exception ex)
                {
                    System.Web.HttpContext.Current.Response.Write(ex.Message);
                }
                finally
                {
                    close();
                }
                return ds;
            }
        }
    }

    public virtual void fill(string sql, ref DataTable tb)
    {
        open();

        using (SqlCommand cmd = new SqlCommand(sql, _conn))
        {
            cmd.CommandTimeout = 0;

            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                try
                {
                    adapter.SelectCommand.CommandTimeout = 0;
                    adapter.Fill(tb);
                }
                catch (Exception ex)
                {
                    System.Web.HttpContext.Current.Response.Write(ex.Message);
                }
                finally
                {
                    close();
                }
            }
        }
    }

    public virtual DataSet dataSet(string sql, string tabela)
    {
        open();

        using (SqlCommand cmd = new SqlCommand(sql, _conn))
        {
            cmd.CommandTimeout = 0;

            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();

                try
                {
                    adapter.SelectCommand.CommandTimeout = 0;
                    adapter.Fill(ds, tabela);
                }
                catch (Exception ex)
                {
                    System.Web.HttpContext.Current.Response.Write(ex.Message);
                }
                finally
                {
                    close();
                }
                return ds;
            }
        }
    }

    public virtual List<Hashtable> reader(string sql)
    {
        open();

        using (SqlCommand cmd = new SqlCommand(sql, _conn))
        {
            cmd.CommandTimeout = 0;
            List<Hashtable> arr = new List<Hashtable>();

            try
            {
                SqlDataReader leitor = cmd.ExecuteReader();
                int colunas = leitor.FieldCount;

                while (leitor.Read())
                {
                    Hashtable ht = new Hashtable();

                    for (int i = 0; i < colunas; i++)
                        ht.Add(leitor.GetName(i), leitor[leitor.GetName(i)]);
                    
                    arr.Add(ht);
                }
                leitor.Close();
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Response.Write(ex.Message);
            }
            finally
            {
                close();
            }
            return arr;
        }
    }
}