using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

class MyException : Exception
{
    private string _mensagem;
    private string _detalhe;
    private string _arquivoLog = Environment.CurrentDirectory + "/log_erros.txt";

    public MyException(string mensagem, string detalhe)
    {
        _mensagem = mensagem;
        _detalhe = detalhe;
    }

    public void trata()
    {
    }
}
