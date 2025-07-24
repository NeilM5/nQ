using System.Globalization;

public class Lexer
{
    private string text;
    private int pos;
    private char current;

    public Lexer(string input)
    {
        text = input;
        current = text.Length > 0 ? text[pos] : '\0';
    }

    private void Advance()
    {
        pos++;
        current = pos < text.Length ? text[pos] : '\0';
    }

    private void SkipWhiteSpace()
    {
        while (char.IsWhiteSpace(current))
        {
            Advance();
        }
    }

    private string Number()
    {
        string result = "";
        while (char.IsDigit(current))
        {
            result += current;
            Advance();
        }

        return result;
    }

    private string Identifier()
    {
        string result = "";
        while (char.IsLetterOrDigit(current) || current == '.')
        {
            result += current;
            Advance();
        }

        return result;
    }

    private string Operator()
    {
        string result = "";
        while (char.IsLetter(current) || "+-*/".Contains(current))
        {
            result += current;
            Advance();
        }

        return result;
    }

    public List<Token> Tokenize()
    {
        var tokens = new List<Token>();

        while (current != '\0')
        {
            if (char.IsWhiteSpace(current))
            {
                SkipWhiteSpace();
                continue;
            }

            if (char.IsDigit(current))
            {
                tokens.Add(new Token(Type.NUM, Number()));
                continue;
            }

            switch (current)
            {
                case '[':
                    tokens.Add(new Token(Type.LBRACKET, "["));
                    Advance();
                    break;
                case ']':
                    tokens.Add(new Token(Type.RBRACKET, "]"));
                    Advance();
                    break;
                case '(':
                    tokens.Add(new Token(Type.LPAREN, "("));
                    Advance();
                    break;
                case ')':
                    tokens.Add(new Token(Type.RPAREN, ")"));
                    Advance();
                    break;
                case ',':
                    tokens.Add(new Token(Type.COMMA, ","));
                    Advance();
                    break;
                case ';':
                    tokens.Add(new Token(Type.SEMICOLON, ";"));
                    Advance();
                    break;
                case '>':
                    tokens.Add(new Token(Type.PUSH, ">"));
                    Advance();
                    break;
                case '/':
                    tokens.Add(new Token(Type.SLASH, "/"));
                    Advance();

                    string op = Operator();

                    if (op.Length > 0)
                    {
                        tokens.Add(new Token(Type.OP, op));
                    }
                    else throw new Exception($"invalid operator: /{op}");

                    break;

                case '+':
                case '-':
                case '*':
                    tokens.Add(new Token(Type.OP, current.ToString()));
                    Advance();
                    break;

                case '@':
                    tokens.Add(new Token(Type.AT, "@"));
                    Advance();

                    string id = Identifier();
                    if (string.IsNullOrEmpty(id)) throw new Exception("expected identifier after @");

                    tokens.Add(new Token(Type.IDENTIFIER, id));

                    break;

                case 'n':
                    tokens.Add(new Token(Type.ENQ, "n"));
                    Advance();
                    break;
                case 'd':
                    tokens.Add(new Token(Type.DEQ, "d"));
                    Advance();
                    break;
                default:
                    if (char.IsLetter(current))
                    {
                        string ident = Identifier();
                        tokens.Add(new Token(Type.IDENTIFIER, ident));
                    }
                    else throw new Exception($"invalid character: {current}");
                    break;
            }
        }

        tokens.Add(new Token(Type.EOF, ""));

        return tokens;
    }
}