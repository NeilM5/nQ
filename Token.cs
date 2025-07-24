public enum Type
{
    LBRACKET,
    RBRACKET,
    LPAREN,
    RPAREN,
    COMMA,
    SEMICOLON,
    AT,
    ENQ,
    DEQ,
    IDENTIFIER,
    NUM,
    PUSH,
    SLASH,
    OP,
    EOF
}

public class Token
{
    public Type _type;
    public string _value;
    public Token(Type type, string value)
    {
        _type = type;
        _value = value;
    }

    public override string ToString()
    {
        return $"{_type}: {_value}";
    }
}