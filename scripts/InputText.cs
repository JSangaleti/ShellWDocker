using Godot;

public partial class InputText : TextEdit
{
    private RichTextLabel outputText;
    private string prompt;
    Bash bash;

    public void Reset()
    {
        prompt = bash.CUR_DIR + " - >> ";
        Text = "";
        InsertTextAtCaret(prompt);
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            if (eventKey.Pressed && eventKey.Keycode == Key.Enter)
            {
                AcceptEvent();

                string command = GetLine(GetCaretLine()).Replace(prompt, "").Trim();
                if (command != "")
                    outputText.AppendText(prompt + command + "\n" + bash.ExecuteCommand(command) + "\n");
                if (command == "clear")
                    outputText.Text = "";

                Reset();

            }
            if (eventKey.Pressed && eventKey.Keycode == Key.Backspace)
            {
                _Backspace(0);
            }
        }

    }

    public override void _Backspace(int caretIndex)
    {
        int line = GetCaretLine();
        int column = GetCaretColumn();

        if (column > prompt.Length)
            RemoveText(line, column - 1, line, column);
    }

    public override void _Ready()
    {
        bash = new Bash("/home");
        outputText = (RichTextLabel)GetNode("../OutputText");

        Reset();
    }


}
