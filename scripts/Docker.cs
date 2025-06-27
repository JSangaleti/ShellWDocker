using Godot;
using System;
using System.Diagnostics;

public class Docker
{
    private ProcessStartInfo psi;
    private Process proc;

    public Docker()
    {
        proc = new Process();
    }

    private string[] RunShell(string command)
    /*
    Executa os comandos no Docker por meio do Shell da máquina do usuário
    */
    {
        string[] output = new string[2];
        psi = new ProcessStartInfo("bash", $"-c \"{command}\"")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        proc.StartInfo = psi;

        proc.Start();

        output[0] = proc.StandardOutput.ReadToEnd();
        output[1] = proc.StandardError.ReadToEnd();

        proc.WaitForExit();

        return output;
    }

    public string[] StartMachine(string machine)
    {
        /* TODO
        retirar os trycatch e implementar verificações, 
        deixando os trycatch() para tratamento de erros
        */
        try
        {
            // Tenta criar os contêineres das imagens Docker
            string command = $"docker run --name {machine} -dit bash";
            return RunShell(command);
        }
        catch (System.Exception)
        {
            // Caso já existam, apenas inicialize
            string command = $"docker start {machine}";
            return RunShell(command);
        }
    }

    public string[] StopMachine(string machine)
    {
        return [];
    }

    public string RunDockerCommand(string command, string machineName, string path)
    {
        // Coloca o comando num formato de execução Docker
        string fullCmd = $"docker exec {machineName} bash -c \"{command}\"";

        try
        {
            string[] output = RunShell(fullCmd);

            return string.IsNullOrWhiteSpace(output[0]) ? output[1] : output[0];
        }
        catch (Exception e)
        {
            return $"Erro: {e.Message}";
        }
    }
}
