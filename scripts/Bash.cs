using Godot;
using System;
using System.IO;

public class Bash
{

    public string USR_DIR;
    public string CUR_DIR;
    //  Nomes das variáveis em caixa-alta para que se assemelhe
    //  às variáveis PATH de sistemas like-unix

    public Bash(string USR_DIR)
    {
        this.USR_DIR = USR_DIR;
        CUR_DIR = USR_DIR;
    }

    private string[] ClearCommand(string command)
    /*
    Esta função deve realizar sanitização dos dados do campo de entrada de texto,
    evitando que o jogador burle as regras do nosso controle de jogo
    */
    {
        string[] vectorCommand = command.Split(" ");
        return vectorCommand;
    }

    public string ExecuteCommand(string command)
    /*
    Função responsável por tratar a entrada e permitir que apenas os comandos designados 
    sejam executadas e da maneira esperada, mantendo controle sobre o usuário
    */
    {
        Docker docker = new Docker();
        string[] vectorCommand = ClearCommand(command);
        switch (vectorCommand[0])
        {
            case "whoami":
                return docker.RunDockerCommand("whoami", "maquina", CUR_DIR);
            case "ls":
                return docker.RunDockerCommand("ls", "maquina", CUR_DIR);
            case "pwd":
                return CUR_DIR;
            case "echo":
                return docker.RunDockerCommand("echo " + vectorCommand[1], "maquina", CUR_DIR);
            case "cd":
                string targetDir;

                if (vectorCommand.Length == 1)
                    // Trata 'cd' sem argumentos (vai para home)
                    targetDir = USR_DIR;
                else
                    // Junta os argumentos restantes (permite espaços no caminho)
                    targetDir = string.Join(" ", vectorCommand, 1, vectorCommand.Length - 1);

                // Resolve caminhos relativos/absolutos
                string newPath;
                if (Path.IsPathRooted(targetDir))
                    newPath = targetDir;
                else
                    newPath = Path.Combine(CUR_DIR, targetDir);

                // Normaliza o caminho (remove .., ., etc)
                newPath = Path.GetFullPath(newPath);

                // Verifica se é diretório válido
                if (Directory.Exists(newPath))
                {
                    CUR_DIR = newPath;
                    return "";
                }
                else
                {
                    return $"cd: {targetDir}: Diretório não encontrado";
                }

            default:
                return "Comando inválido ou inexistente!";
        }
    }
}