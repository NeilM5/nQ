class Program
{
    static void Main()
    {
        var interpreter = new Interpreter();
        var declaredQueues = new HashSet<string> { "q", "q." };

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();

            if (input == null || input.Trim().ToLower() == "exit") break;

            try
            {
                // Lexer
                var lexer = new Lexer(input);
                var tokens = lexer.Tokenize();

                // Parser
                var parser = new Parser(tokens, declaredQueues);
                var ast = parser.Parse();

                // Interpreter
                interpreter.Execute(ast);

                while (interpreter.printQueue.Count > 0)
                {
                    Console.WriteLine(interpreter.printQueue.Dequeue());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"error: {e.Message}");
            }
        }
    }
}