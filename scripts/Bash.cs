using Godot;

public class Bash
{
    public string USR_DIR;
    public string CUR_DIR;
    //  Nomes das variáveis em caixa-alta para que se assemelhe
    //  às variáveis PATH de sistemas like-unix

    public Bash(string USR_DIR)
    {
        this.USR_DIR = USR_DIR;
        this.CUR_DIR = USR_DIR;
    }


    private void Shell()
    /*
    Função responsável por executar os comandos. Foi criada afim de simplificar 
    as entradas e saídas para que, assim, possa ser melhor utilizada ao longo do código
    */
    {

    }

    public string ExecuteCommand(string command)
    /*
    Função responsável por tratar a entrada e permitir que apenas os comandos designados 
    sejam executadas e da maneira esperada, mantendo controle sobre o usuário
    */
    {
        string[] vectorCommand = command.Split(" ");
        switch (vectorCommand[0])
        {
            case "ls":
                command += " " + CUR_DIR;
                return null;

            default:
                return "Comando inválido ou nexistente!";
        }
    }
}