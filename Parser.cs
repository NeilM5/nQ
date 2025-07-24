public abstract class Node { }

public class DeclareNode : Node
{
    public string Name;
    public DeclareNode(string name) => Name = name;
}

public class EnqueueNode : Node
{
    public List<int> Values;
    public string Name;
    public EnqueueNode(List<int> values, string name)
    {
        Values = values;
        Name = name;
    }
}

public class DequeueNode : Node
{
    public string Name;
    public DequeueNode(string name) => Name = name;
}

public class OperationNode : Node
{
    public string Operator;
    public OperationNode(string op) => Operator = op;
}

public class PushNode : Node
{
    public string Name;
    public PushNode(string name) => Name = name;
}

public class Parser
{
    private List<Token> tokens;
    private int pos;
    private Token current => pos < tokens.Count ? tokens[pos] : new Token(Type.EOF, "");

    private HashSet<string> declaredQueues;

    public Parser(List<Token> tokens, HashSet<string>? declaredQueues = null)
    {
        this.tokens = tokens;
        this.pos = 0;
        this.declaredQueues = declaredQueues ?? new HashSet<string> { "q", "q." };
    }

    private void Expect(Type type)
    {
        if (current._type == type)
        {
            pos++;
        }
        else throw new Exception($"expected {type}");
    }

    public List<Node> Parse()
    {
        var nodes = new List<Node>();
        while (current._type != Type.EOF)
        {
            // Variable Assignment
            if (current._type == Type.AT) nodes.Add(ParseDeclare());

            // Data Handling/Enqueue
            else if (current._type == Type.LBRACKET) nodes.Add(ParseEnqueue());

            // Dequeue
            else if (current._type == Type.DEQ) nodes.Add(ParseDequeue());

            // Operations
            else if (current._type == Type.SLASH) nodes.Add(ParseOperation());

            // Push Data
            else if (current._type == Type.PUSH) nodes.Add(ParsePush());

            else throw new Exception($"unexpected token: {current}");
        }

        return nodes;
    }

    private DeclareNode ParseDeclare()
    {
        Expect(Type.AT);

        var name = current._value;

        if (declaredQueues.Contains(name)) throw new Exception($"queue {name} is already declared or reserved");

        declaredQueues.Add(name);

        Expect(Type.IDENTIFIER);
        Expect(Type.SEMICOLON);

        return new DeclareNode(name);
    }

    private EnqueueNode ParseEnqueue()
    {
        Expect(Type.LBRACKET);
        var values = new List<int>();

        while (current._type == Type.NUM)
        {
            values.Add(int.Parse(current._value));

            Expect(Type.NUM);

            if (current._type == Type.COMMA)
            {
                Expect(Type.COMMA);
            }
        }

        Expect(Type.RBRACKET);
        Expect(Type.ENQ);

        string name = current._value;
        if (!declaredQueues.Contains(name))
            throw new Exception($"cannot enqueue an undeclared queue: {name}");

        Expect(Type.IDENTIFIER);
        Expect(Type.SEMICOLON);

        return new EnqueueNode(values, name);
    }

    private DequeueNode ParseDequeue()
    {
        Expect(Type.DEQ);
        string name = current._value;

        if (!declaredQueues.Contains(current._value))
            throw new Exception($"cannot dequeue from an undeclared queue: {name}");

        Expect(Type.IDENTIFIER);
        
        if (current._type == Type.SEMICOLON) Expect(Type.SEMICOLON);

        return new DequeueNode(name);
    }

    private OperationNode ParseOperation()
    {
        Expect(Type.SLASH);
        string op = current._value;

        Expect(Type.OP);

        if (current._type == Type.SEMICOLON) Expect(Type.SEMICOLON);

        return new OperationNode(op);
    }

    private PushNode ParsePush()
    {
        Expect(Type.PUSH);
        string name = current._value;

        if (!declaredQueues.Contains(current._value))
            throw new Exception($"cannot push an undeclared queue: {name}");

        Expect(Type.IDENTIFIER);
        Expect(Type.SEMICOLON);

        return new PushNode(name);
    }
}