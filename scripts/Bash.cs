using Godot;
using System;
using System.IO;

public class Bash
{

    private System.Diagnostics.Process process;
    public string USR_DIR;
    public string CUR_DIR;
    //  Nomes das variáveis em caixa-alta para que se assemelhe
    //  às variáveis PATH de sistemas like-unix

    public Bash(string USR_DIR)
    {
        process = new System.Diagnostics.Process();
        process.EnableRaisingEvents = false;
        process.StartInfo.RedirectStandardOutput = true;
        // process.StartInfo.RedirectStandardError = true;
        process.StartInfo.FileName = "/bin/bash";

        this.USR_DIR = USR_DIR;
        CUR_DIR = USR_DIR;
    }


    private string Shell(string command)
    /*
    Função responsável por executar os comandos. Foi criada afim de simplificar 
    as entradas e saídas para que, assim, possa ser melhor utilizada ao longo do código
    */
    {
        process.StartInfo.WorkingDirectory = CUR_DIR;
        process.StartInfo.Arguments = " -c " + command;
        process.Start();
        process.WaitForExit();
        string output = process.StandardOutput.ReadToEnd();
        return output;
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
                return Shell(command);
            case "pwd":
                return Shell("pwd");
            case "echo":
                // Por enquanto, o echo servirá como um simples print
                // até que possa ser útil para adicionar informações
                // em arquivos
                return Shell(command);
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
                    process.StartInfo.WorkingDirectory = CUR_DIR;
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